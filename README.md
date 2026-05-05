# BlogApp

BlogApp 是一個以 Domain-Driven Design（DDD）實作的部落格應用程式，目標是用清楚的分層方式來管理文章與留言的商業邏輯。

這個專案的設計重點是把核心規則放在 Domain 層，讓 Application 負責協調流程，Infrastructure 負責資料庫與訊息整合，API 則對外提供 HTTP 介面。

## 這個專案是什麼

這是一個 Blog App 範例專案，包含：

- 文章管理
- 留言管理
- 發佈、更新、刪除等文章流程
- 事件驅動與 Outbox 整合

它用來示範如何用 DDD 思維整理一個中型後端系統，讓業務規則、資料存取、與外部整合彼此分離。

## 架構

專案採用分層式 DDD 架構：

- `Domain`：核心領域模型、值物件、實體、領域事件
- `Application`：用例協調、命令與查詢、驗證
- `Infrastructure`：EF Core、PostgreSQL、Kafka、Outbox、Repository 實作
- `Api`：HTTP Endpoints 與對外輸入輸出

## 技術棧

- .NET 10
- ASP.NET Core
- EF Core
- PostgreSQL
- Kafka
- CoreMesh.Dispatching
- CoreMesh.Endpoints
- CoreMesh.Validation
- CoreMesh.Outbox
- xUnit
- NSubstitute

## 部署與開發

專案已提供 Docker 與 Docker Compose 設定，可用來啟動 API、PostgreSQL、Kafka，以及資料庫 migration。

如果你要本地開發，通常會搭配：

- `docker compose up -d --build`
- `dotnet test`
- `dotnet build`

## 目標

這個專案主要是用來展示：

- 如何用 DDD 切分後端責任
- 如何把 API、Application、Domain、Infrastructure 分開
- 如何整合資料庫、訊息佇列、與 Outbox pattern
- 如何用容器化方式部署整套服務
