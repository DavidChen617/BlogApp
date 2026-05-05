## Why

Domain 與 Application 層已完成，但缺少真實的持久化與訊息傳遞支撐。Infrastructure 層需要實作 EF Core Repository、Outbox（與 PostgreSQL 整合）、Kafka Publisher/Subscriber，以及補齊 Application Handler 缺少的 IUnitOfWork 注入，讓整個 DDD 架構可以端對端運行。

## What Changes

- 建立 `BlogDbContext`（PostgreSQL，含 Post、Comment、OutboxMessage 三張表）
- 實作 `IUnitOfWork` 與 `ITransactionUnitOfWork`，封裝 DbContext 的 SaveChanges / Transaction 控制
- 實作 `PostRepository` 與 `CommentRepository`（完全獨立，PostId/CommentId 使用 Value Converter）
- 實作 `EfCoreOutboxWriter` 與 `EfCoreOutboxStore`，整合 CoreMesh.Outbox
- 實作 `KafkaEventPublisher` 與 `KafkaMessageSubscriber`（背景服務，消費後 dispatch 至 IEventHandler）
- 建立 Docker Compose（PostgreSQL + Kafka）及 Migration Docker job
- 補齊所有 Command Handler 注入 `IUnitOfWork` 並呼叫 `SaveChangesAsync()`
- 補齊 EventHandler（PostDeletedEventHandler、AuthorNameChangedEventHandler）注入 `IUnitOfWork`

## Capabilities

### New Capabilities

- `persistence`: EF Core 持久化，含 BlogDbContext、Repository 實作、Value Converter、IEntityTypeConfiguration
- `unit-of-work`: IUnitOfWork / ITransactionUnitOfWork 介面定義與 EfUnitOfWork 實作
- `outbox-infrastructure`: EfCoreOutboxWriter、EfCoreOutboxStore，與 PostgreSQL 整合
- `kafka-messaging`: KafkaEventPublisher、KafkaMessageSubscriber 背景服務，Topic 命名規範
- `docker-infrastructure`: Docker Compose（PostgreSQL + Kafka）與 Migration Docker job

### Modified Capabilities

- `post-management`: Command Handler 補注入 IUnitOfWork，呼叫 SaveChangesAsync
- `comment-management`: Command Handler 補注入 IUnitOfWork，呼叫 SaveChangesAsync
- `domain-events`: EventHandler 補注入 IUnitOfWork，Kafka Consumer 觸發後確保持久化

## Impact

- `src/Infrastructure/`: 全面新增實作檔案
- `src/Application/`: 所有 Command Handler 與 EventHandler 加入 IUnitOfWork 注入
- `src/Domain/`: 新增 IUnitOfWork / ITransactionUnitOfWork 介面（放 Domain 層）
- `docker-compose.yml`: 新增
- 依賴新增：`Npgsql.EntityFrameworkCore.PostgreSQL`、`Confluent.Kafka`、`CoreMesh.Outbox`
