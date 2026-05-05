## Context

Domain 與 Application 層已完整建立（Post/Comment Aggregate、CQRS Handlers、Domain Events）。Infrastructure 層是最後一塊拼圖，負責實作所有 Domain 介面（IPostRepository、ICommentRepository、IUnitOfWork、IOutboxWriter）並接通 PostgreSQL 與 Kafka。

## Goals / Non-Goals

**Goals:**
- 實作所有 Domain 定義的介面，讓 Application 層可以實際運行
- 建立 PostgreSQL + Kafka 的本地開發環境（Docker Compose）
- 確保 Outbox Pattern 的原子性：Domain 操作與事件寫入在同一個 DB Transaction
- Migration 以 Docker job 方式執行，符合 Kubernetes 部署實踐

**Non-Goals:**
- API Endpoint 實作不在此 change 範圍
- 身份驗證（Identity Context）不在此 change 範圍
- 生產環境的 Kafka 高可用配置不在此 change 範圍

## Decisions

### 1. IUnitOfWork 放 Domain 層，不放 Infrastructure

**決策**：`IUnitOfWork` 與 `ITransactionUnitOfWork` 介面定義在 `Domain.Common`，`EfUnitOfWork` 實作在 Infrastructure。

**理由**：Application Handler 依賴 IUnitOfWork，Application 層只能依賴 Domain 層。若介面放 Infrastructure，Application → Infrastructure 的依賴方向就反了。

```
Domain.Common/
  IUnitOfWork.cs              ← SaveChangesAsync
  ITransactionUnitOfWork.cs   ← Begin/Commit/Rollback（繼承 IUnitOfWork）

Infrastructure/
  EfUnitOfWork.cs             ← 同時實作兩個介面，封裝 BlogDbContext
```

### 2. PostId / CommentId 使用 EF Value Converter

**決策**：在 `IEntityTypeConfiguration` 裡設定 Value Converter，Domain 物件不做任何修改。

**理由**：Value Object 是 Domain 的核心設計，EF Core 的對應方式不應污染 Domain。Value Converter 讓 Domain 與 ORM 完全解耦。

```csharp
builder.Property(p => p.Id)
    .HasConversion(id => id.Value, v => PostId.From(v));
```

### 3. Repository 完全獨立，不用 Generic Base

**決策**：`PostRepository` 和 `CommentRepository` 各自獨立實作，不繼承共用 Base。

**理由**：每個 Aggregate 的查詢需求不同（Post 有分頁、AuthorId 查詢；Comment 有 DeleteByPostId）。Generic Base 的彈性不足，且 blog 只有兩個 Repository，重複程式碼量可接受。

### 4. Kafka Topic 命名

**決策**：採用動詞過去式格式 `{entity}-{event}`，全小寫 kebab-case。

```
post-created
post-published
post-deleted
author-name-changed
```

**理由**：語意清楚，符合常見 Event-Driven 命名慣例。

### 5. Kafka Consumer 使用 IEventDispatcher 分派

**決策**：`KafkaMessageSubscriber` 是 `IHostedService`，消費訊息後呼叫 `IEventDispatcher.DispatchAsync()`，由 CoreMesh 的 EventDispatcher 找到對應的 `IEventHandler<T>` 執行。

**理由**：不需要在 Consumer 裡寫 switch/if 判斷事件類型，CoreMesh 負責 type-based dispatch。新增 EventHandler 只需要新增實作，不需要修改 Consumer。

```
KafkaMessageSubscriber
  → IEventDispatcher.DispatchAsync(envelope)
    → EventDispatcher 根據 EventType 找到 handler
      → PostDeletedEventHandler.HandleAsync(event)
        → ICommentRepository.DeleteByPostIdAsync()
        → IUnitOfWork.SaveChangesAsync()
```

### 6. Migration 使用 Docker job

**決策**：建立獨立的 `Migration.Dockerfile`，Docker Compose 以 `depends_on` 確保 migration job 完成後才啟動 API。

**理由**：符合 Kubernetes initContainer 模式，migration 與應用程式生命週期解耦，避免多個 Pod 同時啟動時重複執行 migration。

### 7. Application Handler 補 IUnitOfWork

**決策**：所有 Command Handler 補注入 `IUnitOfWork`，在 Handler 最後呼叫 `SaveChangesAsync()`。EventHandler 同樣補注入。

**理由**：Handler 明確控制 Transaction 邊界，符合「一個 Command = 一個 Transaction」原則。

## Risks / Trade-offs

- [Kafka Consumer 冪等性] 同一個事件可能被消費多次 → `IEvent.Id` 做冪等性 key，CoreMesh Outbox 的 `IOutboxStore` 支援標記已處理
- [Migration Docker job] 若 migration 失敗，API 無法啟動 → 確保 migration script 可重入（idempotent）
- [Value Converter 效能] 每次查詢都有轉換開銷 → 微乎其微，可忽略
