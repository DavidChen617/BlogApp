## Context

Domain、Application、Infrastructure 三層已完成。Application 層的 Handlers 使用 exception 語意回報錯誤（`KeyNotFoundException` 表示資源不存在、`InvalidOperationException` 表示業務規則違反）。Api 專案目前只有空殼的 Program.cs，`ApplicationDependency` 為空 stub。

現有的 CoreMesh 生態：
- `CoreMesh.Dispatching`：`AddDispatching(assembly)` 掃描並注冊 Handlers；`ISender.Send()` dispatch requests
- `CoreMesh.Endpoints`：`IGroupEndpoint` 定義路由群組；`IGroupedEndpoint<TGroup>` 定義群組內的 endpoint；`AddEndpoints()` + `MapEndpoints()` 完成 DI 與路由對應
- `CoreMesh.Validation`：`AddValidatable(assembly)` 注冊 `IValidator`；`IValidator.Validate()` 回傳 `ValidationResult`（含 `IsValid` 與 `Errors`）

## Goals / Non-Goals

**Goals:**
- 為 Post 與 Comment 資源建立 HTTP REST Endpoints
- 完成 ApplicationDependency（Handler 掃描）與 Api 的 DI 串接
- 在 Endpoint 層做顯式驗證（Validation），對應 HTTP 400
- 以 `IExceptionHandler` middleware 統一處理 exception → HTTP 映射

**Non-Goals:**
- 不引入 CoreMesh.Result；Application Handlers 維持現有 exception-driven 設計
- 不實作 JWT 驗證 / Authorization
- 不實作 Query Comment 端點（Domain 層無 Query handler for Comments）

## Decisions

### D1：Endpoint 組織方式

使用 `IGroupEndpoint` + `IGroupedEndpoint<TGroup>` 模式，按資源分群：

```
Endpoints/
  Posts/
    PostsGroup.cs           ← IGroupEndpoint，prefix = "/api/posts"
    CreatePostEndpoint.cs   ← IGroupedEndpoint<PostsGroup>
    PublishPostEndpoint.cs
    UpdatePostEndpoint.cs
    DeletePostEndpoint.cs
    GetPostEndpoint.cs
    ListPostsEndpoint.cs
  Comments/
    CommentsGroup.cs        ← IGroupEndpoint，prefix = "/api/comments"
    CreateCommentEndpoint.cs
    DeleteCommentEndpoint.cs
```

路由設計：

| Method | Route | Handler |
|--------|-------|---------|
| GET | /api/posts | ListPosts（分頁） |
| GET | /api/posts/{id} | GetPost |
| POST | /api/posts | CreatePost |
| PUT | /api/posts/{id} | UpdatePost |
| DELETE | /api/posts/{id} | DeletePost |
| POST | /api/posts/{id}/publish | PublishPost |
| POST | /api/comments | CreateComment（postId 放 body） |
| DELETE | /api/comments/{id} | DeleteComment |

**為何不用單檔多 class：** 小 Blog 資源目前端點少，但拆開每個 endpoint 一個檔案可獨立測試、未來新增時不需要動既有程式碼。

### D2：驗證策略

在 Endpoint 的 handler delegate 內部顯式呼叫 `IValidator.Validate(command)`：

```csharp
var result = validator.Validate(command);
if (!result.IsValid)
    return Results.ValidationProblem(result.Errors);
```

**為何不用 middleware/pipeline 自動驗證：** CoreMesh.Dispatching 無 pipeline。Middleware 方案需要 body rewind，複雜度高。顯式驗證讓每個 endpoint 自我完備，一眼看得出驗證邏輯。

### D3：Exception → HTTP 映射

建立 `BlogExceptionHandler : IExceptionHandler`，集中映射：

| Exception | HTTP |
|-----------|------|
| `KeyNotFoundException` | 404 Not Found |
| `InvalidOperationException` | 422 Unprocessable Entity |

**為何不在每個 endpoint try-catch：** 集中在 middleware 避免重複程式碼，且 Application 層已有明確 exception 語意，不需要改動。

### D4：ApplicationDependency 改為 extension method

將現有空 stub 改寫為：

```csharp
public static IServiceCollection AddApplication(this IServiceCollection services)
{
    var assembly = typeof(CreatePostHandler).Assembly;
    services.AddDispatching(assembly);
    services.AddValidatable(assembly);
    return services;
}
```

呼叫改由 `Program.cs` 加上 `builder.Services.AddApplication()`。

## Risks / Trade-offs

- **exception 當控制流：** `KeyNotFoundException` / `InvalidOperationException` 做 not-found / business-error 語意清晰但非 best practice；若未來要更精細的錯誤碼（error code），需改 Application handlers 回傳 Result 型別。目前先維持簡單。
- **`Publish` sub-resource action：** `POST /api/posts/{id}/publish` 不是標準 REST；但對 DDD command 語意最直覺，且不需要 PATCH partial update 的複雜度。

## Migration Plan

1. 加入 NuGet 套件（Api.csproj）
2. 補齊 ApplicationDependency.AddApplication()
3. 建立 BlogExceptionHandler
4. 建立 PostsGroup + Post Endpoints（7 個 endpoint 類別）
5. 建立 CommentsGroup + Comment Endpoints（2 個 endpoint 類別）
6. 更新 Program.cs（AddApplication / AddEndpoints / MapEndpoints / UseExceptionHandler）

## Open Questions

無
