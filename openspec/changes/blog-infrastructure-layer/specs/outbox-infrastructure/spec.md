## ADDED Requirements

### Requirement: EfCoreOutboxWriter 在同一 Transaction 內寫入 OutboxMessage

系統 SHALL 實作 `EfCoreOutboxWriter : IOutboxWriter`，呼叫 `AddAsync()` 時僅將 OutboxMessage 加入 EF Change Tracker，不立即提交，由 IUnitOfWork.SaveChangesAsync() 統一提交。

#### Scenario: OutboxMessage 與 Domain 變更同一 Transaction 提交
- **WHEN** Handler 同時執行 repository.Add(post) 與 outboxWriter.AddAsync(event) 後呼叫 SaveChangesAsync
- **THEN** Post 與 OutboxMessage 同時出現在 DB，或同時不出現（原子性）

---

### Requirement: EfCoreOutboxStore 追蹤訊息處理狀態

系統 SHALL 實作 `EfCoreOutboxStore : IOutboxStore`，提供讀取待處理訊息與標記已處理的能力，供 OutboxDispatcher 背景服務使用。

#### Scenario: 讀取未處理的 OutboxMessage
- **WHEN** OutboxDispatcher 輪詢 OutboxStore
- **THEN** 只回傳尚未標記為已處理的 OutboxMessage

#### Scenario: 標記訊息為已處理
- **WHEN** Kafka 發布成功後呼叫 MarkAsProcessedAsync
- **THEN** 該訊息不再出現在待處理列表
