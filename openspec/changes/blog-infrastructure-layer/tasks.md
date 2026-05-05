## 1. Domain 層補充介面

- [x] 1.1 建立 `IUnitOfWork`（Domain.Common）：SaveChangesAsync()
- [x] 1.2 建立 `ITransactionUnitOfWork : IUnitOfWork`（Domain.Common）：BeginTransactionAsync / CommitAsync / RollbackAsync

## 2. Application Handler 補 IUnitOfWork

- [x] 2.1 `CreatePostHandler` 補注入 IUnitOfWork，末尾加 SaveChangesAsync()
- [x] 2.2 `PublishPostHandler` 補注入 IUnitOfWork，末尾加 SaveChangesAsync()
- [x] 2.3 `UpdatePostHandler` 補注入 IUnitOfWork，末尾加 SaveChangesAsync()
- [x] 2.4 `DeletePostHandler` 補注入 IUnitOfWork，末尾加 SaveChangesAsync()
- [x] 2.5 `CreateCommentHandler` 補注入 IUnitOfWork，末尾加 SaveChangesAsync()
- [x] 2.6 `DeleteCommentHandler` 補注入 IUnitOfWork，末尾加 SaveChangesAsync()
- [x] 2.7 `PostDeletedEventHandler` 補注入 IUnitOfWork，末尾加 SaveChangesAsync()
- [x] 2.8 `AuthorNameChangedEventHandler` 補注入 IUnitOfWork，末尾加 SaveChangesAsync()

## 3. Infrastructure 套件依賴

- [x] 3.1 Infrastructure.csproj 加入 `Npgsql.EntityFrameworkCore.PostgreSQL`
- [x] 3.2 Infrastructure.csproj 加入 `CoreMesh.Outbox`
- [x] 3.3 Infrastructure.csproj 加入 `Confluent.Kafka`

## 4. BlogDbContext 與 EF 設定

- [x] 4.1 建立 `BlogDbContext`（DbSet<Post>、DbSet<Comment>、DbSet<OutboxMessage>）
- [x] 4.2 建立 `PostConfiguration : IEntityTypeConfiguration<Post>`（PostId Value Converter、欄位對應）
- [x] 4.3 建立 `CommentConfiguration : IEntityTypeConfiguration<Comment>`（CommentId / PostId Value Converter）
- [x] 4.4 建立 `OutboxMessageConfiguration`（使用 CoreMesh Outbox sample 的設定為基礎）
- [x] 4.5 在 BlogDbContext.OnModelCreating 套用 ApplyConfigurationsFromAssembly

## 5. Unit of Work 實作

- [x] 5.1 建立 `EfUnitOfWork : IUnitOfWork, ITransactionUnitOfWork`，封裝 BlogDbContext
- [x] 5.2 SaveChangesAsync 委派給 DbContext.SaveChangesAsync
- [x] 5.3 BeginTransactionAsync / CommitAsync / RollbackAsync 使用 DbContext.Database.BeginTransactionAsync

## 6. Repository 實作

- [x] 6.1 建立 `PostRepository : IPostRepository`
- [x] 6.2 實作 Add / GetByIdAsync / Update / Delete / GetPagedAsync / GetByAuthorIdAsync
- [x] 6.3 建立 `CommentRepository : ICommentRepository`
- [x] 6.4 實作 Add / GetByIdAsync / Delete / DeleteByPostIdAsync

## 7. Outbox Infrastructure 實作

- [x] 7.1 建立 `EfCoreOutboxWriter : IOutboxWriter`（參考 CoreMesh sample）
- [x] 7.2 建立 `EfCoreOutboxStore : IOutboxStore`（參考 CoreMesh sample）

## 8. Kafka 實作

- [x] 8.1 建立 `KafkaEventPublisher : IEventPublisher`（依 EventType 對應 Topic）
- [x] 8.2 建立 Topic 對應表（post-created / post-published / post-deleted / author-name-changed）
- [x] 8.3 建立 `KafkaMessageSubscriber : IHostedService`，消費訊息後呼叫 IEventDispatcher.DispatchAsync()
- [x] 8.4 KafkaMessageSubscriber 在 DispatchAsync 後呼叫 IUnitOfWork.SaveChangesAsync()
- [x] 8.5 建立 `KafkaTopicInitializer : IHostedService`，應用程式啟動時確保 Topic 存在

## 9. DI 註冊

- [x] 9.1 建立 `InfrastructureDependency.AddInfrastructure()`，註冊所有 Infrastructure 服務
- [x] 9.2 註冊 BlogDbContext（PostgreSQL connection string from config）
- [x] 9.3 註冊 EfUnitOfWork（同時註冊 IUnitOfWork 與 ITransactionUnitOfWork）
- [x] 9.4 註冊 PostRepository / CommentRepository
- [x] 9.5 註冊 CoreMesh Outbox（AddCoreMeshOutbox，含 EfCoreOutboxStore / EfCoreOutboxWriter / KafkaEventPublisher / KafkaMessageSubscriber）
- [x] 9.6 註冊 Kafka IProducer 與 IConsumer（singleton，從 config 讀取 BootstrapServers）
- [x] 9.7 在 Api/Program.cs 呼叫 AddInfrastructure()

## 10. Migration 與 Docker

- [x] 10.1 執行 `dotnet ef migrations add Initialize` 產生初始 migration
- [x] 10.2 建立 `docker-compose.yml`（PostgreSQL + Kafka KRaft）
- [x] 10.3 建立 `Migration.Dockerfile`（執行 dotnet ef database update）
- [x] 10.4 docker-compose.yml 加入 migration job service，depends_on API container
- [x] 10.5 建立 `appsettings.Development.json` 補充 PostgreSQL / Kafka 連線設定
