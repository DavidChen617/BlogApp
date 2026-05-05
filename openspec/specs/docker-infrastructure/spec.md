### Requirement: Docker Compose 提供本地開發環境

系統 SHALL 提供 `docker-compose.yml`，包含 PostgreSQL 與 Kafka（含 KRaft 模式，不需要 Zookeeper）服務。

#### Scenario: 啟動 Docker Compose 後服務可連線
- **WHEN** 執行 docker compose up
- **THEN** PostgreSQL 在 localhost:5432 可連線，Kafka 在 localhost:9092 可連線

---

### Requirement: Migration 以獨立 Docker job 執行

系統 SHALL 提供 Migration Dockerfile，以一次性 container 執行 `dotnet ef database update`，完成後自動退出。

#### Scenario: Migration job 在 API 啟動前完成
- **WHEN** Docker Compose 啟動
- **THEN** migration job 執行完畢且 exit code 為 0 後，API container 才啟動

#### Scenario: Migration 冪等性
- **WHEN** Migration job 執行多次（如重新部署）
- **THEN** 已套用的 migration 不重複執行，不拋出錯誤
