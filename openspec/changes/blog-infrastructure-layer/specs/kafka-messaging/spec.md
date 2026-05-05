## ADDED Requirements

### Requirement: KafkaEventPublisher 發布事件至對應 Topic

系統 SHALL 實作 `KafkaEventPublisher : IEventPublisher`，依據 OutboxMessage 的 EventType 發布至對應的 Kafka Topic。

#### Scenario: 事件發布至正確 Topic
- **WHEN** OutboxDispatcher 呼叫 PublishAsync(message)，EventType 為 PostCreatedEvent
- **THEN** 訊息發布至 Kafka Topic `post-created`

#### Scenario: Topic 命名對應規則
- **WHEN** 系統啟動
- **THEN** 以下 Topic 存在：post-created、post-published、post-deleted、author-name-changed

---

### Requirement: KafkaMessageSubscriber 消費事件並 Dispatch 至 EventHandler

系統 SHALL 實作 `KafkaMessageSubscriber : IHostedService`，持續消費 Kafka Topic 訊息，將訊息轉為 EventEnvelope 後呼叫 `IEventDispatcher.DispatchAsync()`。

#### Scenario: 消費訊息並觸發對應 EventHandler
- **WHEN** Kafka Topic `post-deleted` 收到訊息
- **THEN** PostDeletedEventHandler.HandleAsync() 被呼叫，對應留言被刪除

#### Scenario: EventHandler 執行後持久化
- **WHEN** PostDeletedEventHandler 執行完畢
- **THEN** IUnitOfWork.SaveChangesAsync() 被呼叫，變更寫入 DB

#### Scenario: 消費失敗不影響其他訊息
- **WHEN** 某訊息的 EventHandler 拋出例外
- **THEN** 該訊息標記為失敗，Consumer 繼續處理下一筆訊息
