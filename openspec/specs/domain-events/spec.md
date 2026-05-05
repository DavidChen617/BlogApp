### Requirement: Domain Event 實作 IEvent

所有 Domain Event SHALL 實作 `CoreMesh.Outbox.Abstractions.IEvent`，包含唯一 Id 與發生時間。

#### Scenario: Domain Event 具備必要欄位
- **WHEN** 系統產生任何 Domain Event
- **THEN** Event 具備唯一的 Guid Id 與 UTC 時間戳記 OccurredAtUtc

---

### Requirement: Application Handler 原子性寫入 Outbox

系統 SHALL 確保 Aggregate 儲存與 Domain Event 寫入 Outbox 在同一個 Transaction 內完成。

#### Scenario: 成功寫入
- **WHEN** Application Handler 執行 Command，儲存 Aggregate 並呼叫 IOutboxWriter.AddAsync()
- **THEN** 兩者在同一個 DB Transaction 提交，確保原子性

#### Scenario: Transaction 失敗時兩者皆回滾
- **WHEN** SaveChangesAsync() 拋出例外
- **THEN** Aggregate 變更與 Outbox 訊息皆未持久化

---

### Requirement: PostDeletedEvent 觸發留言清除

系統 SHALL 在文章刪除時發出 PostDeletedEvent，供 Comment 訂閱處理。

#### Scenario: 文章刪除產生事件
- **WHEN** DeletePostCommand 執行成功
- **THEN** PostDeletedEvent 寫入 Outbox，包含被刪除的 PostId

---

### Requirement: AuthorNameChangedEvent 同步 Post 快照

系統 SHALL 訂閱來自 Identity Context 的 AuthorNameChangedEvent，更新 Post 中儲存的 AuthorName 快照。

#### Scenario: 收到 AuthorNameChangedEvent
- **WHEN** Kafka 收到 AuthorNameChangedEvent，包含 AuthorId 與新的 AuthorName
- **THEN** 系統更新所有該 AuthorId 對應 Post 的 AuthorName 欄位
