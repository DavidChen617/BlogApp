## ADDED Requirements

### Requirement: 新增留言端點

系統 SHALL 提供 `POST /api/comments` HTTP 端點，將請求轉發至 `CreateCommentCommand`。

#### Scenario: 成功新增留言
- **WHEN** 用戶端送出有效的 postId、authorId、body，且對應文章狀態為 Published
- **THEN** 系統回傳 HTTP 201，body 為空

#### Scenario: 留言內容驗證失敗
- **WHEN** 用戶端送出空白 body
- **THEN** 系統回傳 HTTP 400，body 為 ValidationProblem，包含欄位錯誤訊息

#### Scenario: 對草稿文章新增留言
- **WHEN** 對應文章狀態為 Draft
- **THEN** 系統回傳 HTTP 422

#### Scenario: 文章不存在
- **WHEN** 指定的 PostId 不存在
- **THEN** 系統回傳 HTTP 404

---

### Requirement: 刪除留言端點

系統 SHALL 提供 `DELETE /api/comments/{id}` HTTP 端點，將請求轉發至 `DeleteCommentCommand`。

#### Scenario: 成功刪除留言
- **WHEN** 留言存在
- **THEN** 系統回傳 HTTP 204

#### Scenario: 留言不存在
- **WHEN** 指定的 CommentId 不存在
- **THEN** 系統回傳 HTTP 404
