#Factory Machine Management API

這是一個基於 **ASP.NET Core 8** 開發的工廠設備監控後端系統，旨在實現機台狀態監控與維護紀錄的數位化管理。

## 技術棧 (Tech Stack)
- **Framework:** ASP.NET Core 8 Web API
- **ORM:** Entity Framework Core (Code First)
- **Database:** SQL Server
- **Documentation:** Swagger UI with XML Documentation
- **Version Control:** Git

## 專案亮點
- **架構優化 (Service Layer)**: 實作服務層模式，達成邏輯與控制器的解耦，提升代碼可讀性與測試性。
- **解決循環參考 (DTO Pattern)**: 透過 Data Transfer Object (DTO) 解決實體模型間的循環參考問題，確保 JSON 輸出精簡且安全。
- **標準化 API 文件**: 整合 XML 註解與 `ProducesResponseType`，提供具備強型別描述的 Swagger 文件。

## 功能清單 (CRUD)
- [x] 機台清單查詢 (包含狀態過濾)
- [x] 新增/修改/刪除機台資訊
- [x] 機台維修紀錄掛載與查詢

## 執行截圖
