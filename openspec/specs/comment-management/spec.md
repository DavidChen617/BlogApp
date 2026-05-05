### Requirement: 新增留言

系統 SHALL 允許使用者對已發布的文章新增留言。

#### Scenario: 成功新增留言
- **WHEN** 文章狀態為 Published，使用者提供有效的留言內容與 AuthorId
- **THEN** 系統建立留言，並以 PostId 關聯至對應文章

#### Scenario: 對草稿文章新增留言
- **WHEN** 文章狀態為 Draft，使用者嘗試新增留言
- **THEN** 系統回傳業務錯誤，不建立留言

#### Scenario: 留言內容為空時拒絕
- **WHEN** 使用者提供空白留言內容
- **THEN** 系統回傳驗證錯誤，不建立留言

---

### Requirement: 刪除留言

系統 SHALL 允許刪除留言。

#### Scenario: 成功刪除留言
- **WHEN** 留言存在，使用者發出刪除指令
- **THEN** 留言被移除

#### Scenario: 刪除不存在的留言
- **WHEN** 使用者提供不存在的 CommentId
- **THEN** 系統回傳 NotFound 結果

---

### Requirement: 文章刪除時清除留言

系統 SHALL 在收到 PostDeletedEvent 後，刪除該文章的所有留言。

#### Scenario: 接收到 PostDeletedEvent
- **WHEN** Kafka 收到 PostDeletedEvent，包含有效的 PostId
- **THEN** 系統刪除所有 PostId 對應的留言
