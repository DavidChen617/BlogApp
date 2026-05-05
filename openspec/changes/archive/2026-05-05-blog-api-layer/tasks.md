## 1. 套件與 DI 設定

- [x] 1.1 Api.csproj 加入 `CoreMesh.Dispatching` 套件參考
- [x] 1.2 Api.csproj 加入 `CoreMesh.Endpoints` 套件參考
- [x] 1.3 Api.csproj 加入 `CoreMesh.Validation` 套件參考
- [x] 1.4 將 `ApplicationDependency` 改寫為 static extension method `AddApplication()`，呼叫 `AddDispatching` + `AddValidatable`（掃描 Application assembly）
- [x] 1.5 建立 `BlogExceptionHandler : IExceptionHandler`（KeyNotFoundException → 404、InvalidOperationException → 422、其他 → 500）
- [x] 1.6 更新 `Program.cs`：加入 `AddApplication()`、`AddEndpoints()`、`app.UseExceptionHandler()`、`app.MapEndpoints()`

## 2. Posts Group 與 Endpoints

- [x] 2.1 建立 `Endpoints/Posts/PostsGroup.cs`（`IGroupEndpoint`，GroupPrefix = "/api/posts"，WithTags("Posts")）
- [x] 2.2 建立 `Endpoints/Posts/CreatePostEndpoint.cs`（`POST /`，注入 ISender + IValidator，驗證後 dispatch CreatePostCommand，回傳 201）
- [x] 2.3 建立 `Endpoints/Posts/PublishPostEndpoint.cs`（`POST /{id}/publish`，dispatch PublishPostCommand，回傳 204）
- [x] 2.4 建立 `Endpoints/Posts/UpdatePostEndpoint.cs`（`PUT /{id}`，注入 ISender + IValidator，驗證後 dispatch UpdatePostCommand，回傳 204）
- [x] 2.5 建立 `Endpoints/Posts/DeletePostEndpoint.cs`（`DELETE /{id}`，dispatch DeletePostCommand，回傳 204）
- [x] 2.6 建立 `Endpoints/Posts/GetPostEndpoint.cs`（`GET /{id}`，dispatch GetPostByIdQuery，回傳 200 + PostDto）
- [x] 2.7 建立 `Endpoints/Posts/ListPostsEndpoint.cs`（`GET /`，dispatch GetPostsQuery，支援 ?page=&pageSize= query string，回傳 200 + PagedResult）

## 3. Comments Group 與 Endpoints

- [x] 3.1 建立 `Endpoints/Comments/CommentsGroup.cs`（`IGroupEndpoint`，GroupPrefix = "/api/comments"，WithTags("Comments")）
- [x] 3.2 建立 `Endpoints/Comments/CreateCommentEndpoint.cs`（`POST /`，注入 ISender + IValidator，驗證後 dispatch CreateCommentCommand，回傳 201）
- [x] 3.3 建立 `Endpoints/Comments/DeleteCommentEndpoint.cs`（`DELETE /{id}`，dispatch DeleteCommentCommand，回傳 204）
