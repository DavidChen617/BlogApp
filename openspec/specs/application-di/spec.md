## ADDED Requirements

### Requirement: Application 層 DI 註冊

系統 SHALL 提供 `ApplicationDependency.AddApplication()` 擴充方法，完成 Application 層所有依賴的 DI 注冊。

#### Scenario: 呼叫 AddApplication 後 Dispatcher 可用
- **WHEN** `builder.Services.AddApplication()` 被呼叫
- **THEN** IoC 容器中可解析 `ISender`、`IDispatcher`，且所有 `IRequestHandler` 實作均已注冊

#### Scenario: 呼叫 AddApplication 後 Validator 可用
- **WHEN** `builder.Services.AddApplication()` 被呼叫
- **THEN** IoC 容器中可解析 `IValidator`，且 Application 層所有 `IValidatable<T>` 均已掃描

---

### Requirement: Program.cs 串接所有 DI 與路由

系統 SHALL 在 `Program.cs` 依序呼叫 AddApplication、AddInfrastructure、AddEndpoints，並在 build 後呼叫 MapEndpoints。

#### Scenario: 應用程式啟動不拋出例外
- **WHEN** 應用程式以正確的 appsettings（PostgreSQL / Kafka 連線）啟動
- **THEN** 所有服務可正常解析，路由對應成功，應用程式進入 running 狀態
