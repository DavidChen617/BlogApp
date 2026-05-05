## ADDED Requirements

### Requirement: 驗證錯誤回應格式

系統 SHALL 在請求欄位驗證失敗時，回傳 HTTP 400 並使用 ProblemDetails 格式包含欄位錯誤。

#### Scenario: 驗證失敗回應
- **WHEN** Endpoint 呼叫 IValidator.Validate() 回傳 IsValid = false
- **THEN** 系統回傳 HTTP 400，Content-Type 為 application/problem+json，body 包含各欄位錯誤訊息

---

### Requirement: 資源不存在錯誤回應

系統 SHALL 在資源不存在時回傳 HTTP 404。

#### Scenario: KeyNotFoundException 映射
- **WHEN** Application 層拋出 KeyNotFoundException
- **THEN** BlogExceptionHandler 攔截並回傳 HTTP 404，body 包含 detail 訊息

---

### Requirement: 業務規則違反錯誤回應

系統 SHALL 在業務規則被違反時回傳 HTTP 422。

#### Scenario: InvalidOperationException 映射
- **WHEN** Application 層拋出 InvalidOperationException
- **THEN** BlogExceptionHandler 攔截並回傳 HTTP 422，body 包含 detail 訊息

---

### Requirement: 未處理例外錯誤回應

系統 SHALL 在發生未預期例外時回傳 HTTP 500，不洩漏 stack trace。

#### Scenario: 未預期例外映射
- **WHEN** 任何非預期例外逸出 Application 層
- **THEN** BlogExceptionHandler 回傳 HTTP 500，body 為通用錯誤訊息（不含 exception 細節）
