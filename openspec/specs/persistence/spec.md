### Requirement: BlogDbContext 包含所有必要的 DbSet

系統 SHALL 建立 `BlogDbContext` 繼承 `DbContext`，包含 Post、Comment、OutboxMessage 三個 DbSet。

#### Scenario: DbContext 正確對應 Domain 物件
- **WHEN** 應用程式啟動並連線至 PostgreSQL
- **THEN** Post、Comment、OutboxMessage 三張表存在且 schema 正確

---

### Requirement: PostId 與 CommentId 透過 Value Converter 對應

系統 SHALL 在 `IEntityTypeConfiguration` 中設定 Value Converter，將 PostId/CommentId 與 Guid 互轉，Domain 物件不做任何修改。

#### Scenario: Post 寫入與讀取保持 PostId 一致性
- **WHEN** PostRepository.Add(post) 後執行 SaveChanges，再以相同 PostId 查詢
- **THEN** 回傳的 Post 物件 Id 與原始 PostId 相等

---

### Requirement: Repository 實作符合 Domain 介面

系統 SHALL 實作 `PostRepository : IPostRepository` 與 `CommentRepository : ICommentRepository`，每個方法行為符合 Domain 介面的語意契約。

#### Scenario: Add 後 SaveChanges 可查詢到資料
- **WHEN** repository.Add(entity) 後呼叫 SaveChangesAsync
- **THEN** GetByIdAsync 可取得該 entity

#### Scenario: Delete 後 SaveChanges 無法查詢到資料
- **WHEN** repository.Delete(entity) 後呼叫 SaveChangesAsync
- **THEN** GetByIdAsync 回傳 null

#### Scenario: GetPagedAsync 回傳正確分頁結果
- **WHEN** 資料庫有 25 筆 Post，查詢 Page=2, PageSize=10
- **THEN** 回傳 10 筆資料，TotalCount 為 25
