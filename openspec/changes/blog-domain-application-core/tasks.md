## 1. 專案依賴設定

- [x] 1.1 Domain.csproj 加入 CoreMesh.Outbox.Abstractions 套件參考
- [x] 1.2 Application.csproj 加入 CoreMesh.Dispatching、CoreMesh.Validation.Abstractions 套件參考
- [x] 1.3 建立測試專案 Domain.Tests 與 Application.Tests

## 2. Domain 層：共用基底

- [x] 2.1 建立 `Entity<TId>` 基底類別（包含 Id、DomainEvents 集合、AddDomainEvent 方法）
- [x] 2.2 建立 `AggregateRoot<TId>` 繼承 Entity，提供 ClearDomainEvents()
- [x] 2.3 建立 `ValueObject` 基底類別（結構相等性比較）

## 3. Domain 層：Post Aggregate

- [x] 3.1 建立 `PostId` Value Object
- [x] 3.2 建立 `PostStatus` enum（Draft, Published）
- [x] 3.3 建立 `Post` Aggregate Root，包含 Title、Content、AuthorId、AuthorName、Status
- [x] 3.4 實作 `Post.Create()` 靜態工廠方法，初始狀態為 Draft，產生 PostCreatedEvent
- [x] 3.5 實作 `Post.Publish()`，Draft → Published，產生 PostPublishedEvent；已發布時拋出業務例外
- [x] 3.6 實作 `Post.Update()`，僅允許 Draft 狀態修改，否則拋出業務例外
- [x] 3.7 實作 `Post.Delete()`，產生 PostDeletedEvent
- [x] 3.8 定義 `IPostRepository`（Add、GetByIdAsync、Update、Delete、GetPagedAsync）

## 4. Domain 層：Comment Aggregate

- [x] 4.1 建立 `CommentId` Value Object
- [x] 4.2 建立 `Comment` Aggregate Root，包含 PostId、AuthorId、Body、CreatedAt
- [x] 4.3 實作 `Comment.Create()` 靜態工廠方法
- [x] 4.4 定義 `ICommentRepository`（Add、GetByIdAsync、Delete、DeleteByPostIdAsync）

## 5. Domain 層：Domain Events

- [x] 5.1 建立 `PostCreatedEvent : IEvent`（PostId、AuthorId、Title）
- [x] 5.2 建立 `PostPublishedEvent : IEvent`（PostId）
- [x] 5.3 建立 `PostDeletedEvent : IEvent`（PostId）
- [x] 5.4 建立 `AuthorNameChangedEvent : IEvent`（AuthorId、NewAuthorName）— 供訂閱用

## 6. Application 層：Post Command Handlers

- [x] 6.1 建立 `CreatePostCommand : IRequest, IValidatable`（Title、Content、AuthorId、AuthorName）
- [x] 6.2 建立 `CreatePostHandler : IRequestHandler<CreatePostCommand>`
- [x] 6.3 建立 `PublishPostCommand : IRequest`（PostId）
- [x] 6.4 建立 `PublishPostHandler : IRequestHandler<PublishPostCommand>`
- [x] 6.5 建立 `UpdatePostCommand : IRequest, IValidatable`（PostId、Title、Content）
- [x] 6.6 建立 `UpdatePostHandler : IRequestHandler<UpdatePostCommand>`
- [x] 6.7 建立 `DeletePostCommand : IRequest`（PostId）
- [x] 6.8 建立 `DeletePostHandler : IRequestHandler<DeletePostCommand>`

## 7. Application 層：Post Query Handlers

- [x] 7.1 建立 `GetPostByIdQuery : IRequest<PostDto>`（PostId）
- [x] 7.2 建立 `GetPostByIdHandler : IRequestHandler<GetPostByIdQuery, PostDto>`
- [x] 7.3 建立 `GetPostsQuery : IRequest<PagedResult<PostDto>>`（Page、PageSize）
- [x] 7.4 建立 `GetPostsHandler : IRequestHandler<GetPostsQuery, PagedResult<PostDto>>`
- [x] 7.5 建立 `PostDto` record

## 8. Application 層：Comment Command Handlers

- [x] 8.1 建立 `CreateCommentCommand : IRequest, IValidatable`（PostId、AuthorId、Body）
- [x] 8.2 建立 `CreateCommentHandler`，驗證 Post 為 Published 狀態後建立留言
- [x] 8.3 建立 `DeleteCommentCommand : IRequest`（CommentId）
- [x] 8.4 建立 `DeleteCommentHandler`

## 9. Application 層：Event Handlers

- [x] 9.1 建立 `PostDeletedEventHandler`，處理 PostDeletedEvent → 呼叫 ICommentRepository.DeleteByPostIdAsync
- [x] 9.2 建立 `AuthorNameChangedEventHandler`，處理 AuthorNameChangedEvent → 更新所有相關 Post 的 AuthorName

## 10. 單元測試

- [x] 10.1 測試 Post.Create() — 驗證初始狀態與事件產生
- [x] 10.2 測試 Post.Publish() — 正常發布與重複發布的例外
- [x] 10.3 測試 Post.Update() — Draft 可修改，Published 拋出例外
- [x] 10.4 測試 Post.Delete() — 驗證 PostDeletedEvent 產生
- [x] 10.5 測試 Comment.Create() — 驗證基本建立
- [x] 10.6 測試 CreatePostHandler — 驗證 Repository.Add 與 OutboxWriter.AddAsync 被呼叫
- [x] 10.7 測試 PublishPostHandler — 驗證 Post.Publish() 與事件寫入
- [x] 10.8 測試 CreateCommentHandler — 驗證 Post 狀態檢查邏輯
