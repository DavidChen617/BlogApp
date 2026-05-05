## Context

專案為 .NET 10 Blog API，使用 DDD 架構分為 Api / Application / Domain / Infrastructure 四層。基礎建設使用自製的 CoreMesh 套件（Dispatching、Endpoints、Outbox）。目前四層皆為空殼，需要建立 Domain 與 Application 的核心骨架。

## Goals / Non-Goals

**Goals:**
- 建立清晰的 DDD 分層依賴方向：Api → Application → Domain ← Infrastructure
- 定義 Post 與 Comment 兩個 Aggregate Root 及其業務規則
- 以 CoreMesh.Dispatching 實作 CQRS Command/Query 模式
- 以 CoreMesh.Outbox.Abstractions 定義 Domain Event，支援 Outbox → Kafka 流程
- Repository 介面放 Domain，實作放 Infrastructure（此 change 只定義介面）
- 建立單元測試覆蓋 Domain 邏輯

**Non-Goals:**
- Infrastructure 實作（EF Core、Kafka）不在此 change 範圍
- Identity / Author Bounded Context 不在此 change 範圍
- API Endpoint 不在此 change 範圍

## Decisions

### 1. Post 與 Comment 是同一 Bounded Context 的獨立 Aggregate

**決策**：Post 與 Comment 各自是 Aggregate Root，透過 `PostId` 關聯，不互相持有物件參考。

**理由**：新增留言與修改文章標題不需要在同一個 Transaction 裡，強一致性需求不存在。Post 刪除時透過 Domain Event 通知 Comment 清除，採最終一致性。

**替代方案考慮**：把 Comment 塞入 Post Aggregate — 拒絕，因為 Post 有大量留言時會有嚴重的載入效能問題。

### 2. Repository 介面放 Domain 層

**決策**：`IPostRepository`、`ICommentRepository` 定義在 Domain 層。

**理由**：Domain Service 需要透過 Repository 執行業務邏輯查詢。若介面放 Application，則 Domain 無法使用，違反依賴方向。Infrastructure 實作 Domain 定義的介面，符合 Dependency Inversion Principle。

### 3. Domain Event 統一走 IOutboxWriter → Kafka

**決策**：所有 Domain Event 實作 `IEvent`（CoreMesh.Outbox.Abstractions），由 Application Handler 呼叫 `IOutboxWriter.AddAsync()` 寫入 Outbox，不使用 IDispatcher.Publish() 做 in-process 事件。

**理由**：統一一條事件路徑，減少維護複雜度。Outbox Pattern 確保 Domain 操作與事件發布的原子性（同一 DB Transaction）。

**替代方案考慮**：同 Context 事件走 in-process IPublisher，跨 Context 走 Outbox — 拒絕，兩套機制增加認知負擔。

### 4. Application Handler 負責 Outbox 寫入

**決策**：Application Handler 在儲存 Aggregate 後，收集 Aggregate 上的 Domain Events，呼叫 `IOutboxWriter.AddAsync()`。

**理由**：Domain 物件只負責產生事件（放在自身集合中），不依賴 Infrastructure。Application Layer 協調 Repository 儲存與 Outbox 寫入，兩者在同一 Transaction 內完成。

```
CreatePostHandler:
  1. post = Post.Create(...)          // Domain 產生事件
  2. repository.Add(post)             // 準備儲存
  3. outboxWriter.AddAsync(event)     // 準備寫入 Outbox
  4. unitOfWork.SaveChangesAsync()    // 一次 commit，原子性
```

### 5. CQRS 用 CoreMesh.Dispatching，不另外抽象

**決策**：Command 實作 `IRequest` / `IRequest<TResponse>`，Handler 實作 `IRequestHandler`，直接使用 CoreMesh.Dispatching 的介面。

**理由**：CoreMesh.Dispatching 已提供完整的 Dispatcher 抽象，不需要再包一層自製 ICommandBus。

## Risks / Trade-offs

- [最終一致性] Post 刪除後，Comment 短暫仍存在 → 可接受，Blog 情境對即時一致性需求低
- [Outbox 依賴] Application Handler 需要 IOutboxWriter，測試時需要 mock → 透過介面注入可測
- [單 Aggregate 範圍] 此 change 不含 Infrastructure 實作，Domain 測試需使用 in-memory fake repository
