## ADDED Requirements

### Requirement: IUnitOfWork 提供 SaveChangesAsync

系統 SHALL 在 Domain.Common 定義 `IUnitOfWork` 介面，提供 `SaveChangesAsync()`，供 Command Handler 在完成 Domain 操作後一次性持久化所有變更。

#### Scenario: Command Handler 呼叫 SaveChangesAsync 後資料持久化
- **WHEN** Handler 呼叫 repository.Add() 與 outboxWriter.AddAsync() 後執行 SaveChangesAsync
- **THEN** 兩者在同一個 DB Transaction 提交，DB 中可查詢到 Post 與 OutboxMessage

---

### Requirement: ITransactionUnitOfWork 提供明確的 Transaction 控制

系統 SHALL 在 Domain.Common 定義 `ITransactionUnitOfWork : IUnitOfWork`，提供 `BeginTransactionAsync()`、`CommitAsync()`、`RollbackAsync()`，供需要明確控制 Transaction 邊界的場景使用。

#### Scenario: 明確 Transaction 範圍內的操作原子性
- **WHEN** 呼叫 BeginTransactionAsync，執行多次操作後呼叫 CommitAsync
- **THEN** 所有操作在同一個 Transaction 提交

#### Scenario: RollbackAsync 取消所有未提交變更
- **WHEN** 呼叫 BeginTransactionAsync 後執行操作，再呼叫 RollbackAsync
- **THEN** 所有操作被回滾，DB 中不存在任何變更
