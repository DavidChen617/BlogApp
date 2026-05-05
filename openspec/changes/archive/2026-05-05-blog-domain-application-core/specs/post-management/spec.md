## ADDED Requirements

### Requirement: 建立文章

系統 SHALL 允許使用者建立一篇新文章，初始狀態為草稿（Draft）。

#### Scenario: 成功建立文章
- **WHEN** 使用者提供有效的標題、內容與 AuthorId
- **THEN** 系統建立一篇狀態為 Draft 的文章，並產生 PostCreatedEvent

#### Scenario: 標題為空時拒絕建立
- **WHEN** 使用者提供空白標題
- **THEN** 系統回傳驗證錯誤，不建立文章

#### Scenario: 內容為空時拒絕建立
- **WHEN** 使用者提供空白內容
- **THEN** 系統回傳驗證錯誤，不建立文章

---

### Requirement: 發布文章

系統 SHALL 允許將草稿文章發布，狀態由 Draft 變為 Published。

#### Scenario: 成功發布草稿文章
- **WHEN** 文章狀態為 Draft，使用者發出發布指令
- **THEN** 文章狀態變為 Published，並產生 PostPublishedEvent

#### Scenario: 已發布文章不可重複發布
- **WHEN** 文章狀態已為 Published，使用者再次發出發布指令
- **THEN** 系統回傳業務錯誤，狀態不變

---

### Requirement: 修改文章

系統 SHALL 允許修改 Draft 狀態文章的標題與內容。

#### Scenario: 成功修改草稿文章
- **WHEN** 文章狀態為 Draft，使用者提供新的標題與內容
- **THEN** 文章標題與內容更新成功

#### Scenario: 已發布文章不可修改
- **WHEN** 文章狀態為 Published，使用者嘗試修改內容
- **THEN** 系統回傳業務錯誤，內容不變

---

### Requirement: 刪除文章

系統 SHALL 允許刪除文章，並通知其他 Context 清除相關資料。

#### Scenario: 成功刪除文章
- **WHEN** 文章存在，使用者發出刪除指令
- **THEN** 文章被標記為刪除，並產生 PostDeletedEvent

---

### Requirement: 查詢文章

系統 SHALL 支援依 PostId 查詢單篇文章，以及查詢分頁文章列表。

#### Scenario: 查詢存在的文章
- **WHEN** 使用者提供有效的 PostId
- **THEN** 系統回傳對應的文章資料

#### Scenario: 查詢不存在的文章
- **WHEN** 使用者提供不存在的 PostId
- **THEN** 系統回傳 NotFound 結果
