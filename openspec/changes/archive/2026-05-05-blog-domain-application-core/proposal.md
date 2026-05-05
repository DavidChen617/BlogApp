## Why

這個專案的核心目標是打造一個符合 DDD 設計原則的 .NET Blog API，以 CoreMesh 套件作為基礎建設。目前專案四個層次（Api / Application / Domain / Infrastructure）皆為空殼，需要先建立 Domain 與 Application 的核心骨架，讓整個架構有清晰的依賴方向與業務邊界。

## What Changes

- 建立 `Domain` 層的 Aggregate、Entity、Value Object、Repository 介面與 Domain Event 基底
- 建立 `Application` 層的 CQRS Command/Query（使用 CoreMesh.Dispatching）
- 建立 `Post` 與 `Comment` 兩個 Aggregate Root
- 定義 `IPostRepository` 與 `ICommentRepository` 介面（放 Domain 層）
- 定義跨 Context 的 Domain Event（實作 `IEvent`，交由 Outbox 發到 Kafka）
- 建立 Post 相關的 CRUD Command/Query Handler
- 建立單元測試覆蓋 Domain 邏輯與 Application Handler

## Capabilities

### New Capabilities

- `post-management`: Post Aggregate 的建立、發布、修改、刪除，以及對應的 Command/Query Handler
- `comment-management`: Comment Aggregate 的新增、刪除，以及對應的 Command/Query Handler
- `domain-events`: Domain Event 定義與 IOutboxWriter 整合，支援跨 Context 的最終一致性

### Modified Capabilities

## Impact

- `src/Domain`: 新增 Post、Comment Aggregate、Value Object、Repository 介面、Domain Event
- `src/Application`: 新增 Post/Comment 的 Command、Query、Handler（使用 IDispatcher）
- `tests/`: 新增 Domain 單元測試與 Application Handler 測試
- 依賴新增：`CoreMesh.Dispatching`、`CoreMesh.Outbox.Abstractions`
