## Why

Domain、Application、Infrastructure 三層已完成，但尚未有任何 HTTP 入口點，外部無法操作 Blog 資源。需要在 Api 專案中實作 Minimal API Endpoints，將 HTTP 請求對應至 Application 層的 Commands/Queries。

## What Changes

- 為 Api 專案加入 `CoreMesh.Dispatching` 與 `CoreMesh.Endpoints` 套件
- 實作 Posts Endpoints（建立、發布、修改、刪除、查詢單篇、查詢分頁列表）
- 實作 Comments Endpoints（新增留言、刪除留言）
- 在 Program.cs 完成 DI 註冊（AddApplication / AddCoreMeshDispatching）與路由對應（MapEndpoints）
- 統一錯誤處理：驗證錯誤回 400，資源不存在回 404，業務規則錯誤回 422

## Capabilities

### New Capabilities
- `post-endpoints`: Post 資源的 HTTP REST Endpoints，對應 CreatePost / PublishPost / UpdatePost / DeletePost / GetPost / ListPosts
- `comment-endpoints`: Comment 資源的 HTTP REST Endpoints，對應 CreateComment / DeleteComment
- `api-error-handling`: 統一的 HTTP 錯誤回應規格（ValidationError 400、NotFound 404、BusinessError 422）
- `application-di`: Application 層 DI 註冊（CoreMesh.Dispatching handlers 掃描）

### Modified Capabilities

## Impact

- `src/Api/Api.csproj`：新增 CoreMesh.Dispatching、CoreMesh.Endpoints 套件參考
- `src/Api/Program.cs`：新增 AddApplication()、AddCoreMeshDispatching()、MapEndpoints()
- `src/Api/Endpoints/`：新增 PostEndpoints.cs、CommentEndpoints.cs
- `src/Application/ApplicationDependency.cs`：新增 AddApplication() 擴充方法，掃描 Handler 並注入 IUnitOfWork 依賴
