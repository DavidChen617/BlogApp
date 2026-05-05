## ADDED Requirements

### Requirement: 建立文章端點

系統 SHALL 提供 `POST /api/posts` HTTP 端點，將請求轉發至 `CreatePostCommand`。

#### Scenario: 成功建立文章
- **WHEN** 用戶端送出有效的 title、content、authorId、authorName
- **THEN** 系統回傳 HTTP 201，body 為空

#### Scenario: 請求欄位驗證失敗
- **WHEN** 用戶端送出空白 title 或空白 content
- **THEN** 系統回傳 HTTP 400，body 為 ValidationProblem，包含欄位錯誤訊息

---

### Requirement: 發布文章端點

系統 SHALL 提供 `POST /api/posts/{id}/publish` HTTP 端點，將請求轉發至 `PublishPostCommand`。

#### Scenario: 成功發布草稿文章
- **WHEN** 文章存在且狀態為 Draft
- **THEN** 系統回傳 HTTP 204

#### Scenario: 文章不存在
- **WHEN** 指定的 PostId 不存在
- **THEN** 系統回傳 HTTP 404

#### Scenario: 已發布文章重複發布
- **WHEN** 文章狀態已為 Published
- **THEN** 系統回傳 HTTP 422

---

### Requirement: 修改文章端點

系統 SHALL 提供 `PUT /api/posts/{id}` HTTP 端點，將請求轉發至 `UpdatePostCommand`。

#### Scenario: 成功修改草稿文章
- **WHEN** 文章存在且狀態為 Draft，用戶端送出有效 title、content
- **THEN** 系統回傳 HTTP 204

#### Scenario: 文章不存在
- **WHEN** 指定的 PostId 不存在
- **THEN** 系統回傳 HTTP 404

#### Scenario: 已發布文章修改
- **WHEN** 文章狀態為 Published
- **THEN** 系統回傳 HTTP 422

---

### Requirement: 刪除文章端點

系統 SHALL 提供 `DELETE /api/posts/{id}` HTTP 端點，將請求轉發至 `DeletePostCommand`。

#### Scenario: 成功刪除文章
- **WHEN** 文章存在
- **THEN** 系統回傳 HTTP 204

#### Scenario: 文章不存在
- **WHEN** 指定的 PostId 不存在
- **THEN** 系統回傳 HTTP 404

---

### Requirement: 查詢單篇文章端點

系統 SHALL 提供 `GET /api/posts/{id}` HTTP 端點，將請求轉發至 `GetPostByIdQuery`。

#### Scenario: 查詢存在的文章
- **WHEN** 指定的 PostId 存在
- **THEN** 系統回傳 HTTP 200，body 為 PostDto（含 id、title、content、authorId、authorName、status、createdAtUtc）

#### Scenario: 查詢不存在的文章
- **WHEN** 指定的 PostId 不存在
- **THEN** 系統回傳 HTTP 404

---

### Requirement: 查詢分頁文章清單端點

系統 SHALL 提供 `GET /api/posts` HTTP 端點，將請求轉發至 `GetPostsQuery`。

#### Scenario: 查詢文章清單（預設分頁）
- **WHEN** 用戶端不帶 query string
- **THEN** 系統回傳 HTTP 200，body 為 PagedResult<PostDto>，page=1、pageSize=20

#### Scenario: 查詢文章清單（自訂分頁）
- **WHEN** 用戶端帶 ?page=2&pageSize=5
- **THEN** 系統回傳 HTTP 200，body 為 PagedResult<PostDto>，page=2、pageSize=5
