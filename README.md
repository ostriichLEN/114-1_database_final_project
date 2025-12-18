# 樂團成員管理系統 資料庫管理系統 (Band Management Database System)

這是一個基於 **ASP.NET Core MVC (.NET 8.0)** 開發的網頁版資料庫管理系統，用於管理數個樂團、樂手與藝人資料。本專案為 114-1 學年度資料庫設計課程期末專案。

## DEMO LINK
> 僅供 12/18 課堂 demo 使用，完畢後默認連結失效。
- [web server](https://tonja-unsharped-karl.ngrok-free.dev/)

## 專案功能 (Features)

### 1\. 系統權限與安全

  * **登入系統**：透過 Cookie Authentication 實作，未登入使用者無法存取任何資料頁面。
  * **權限控管**：受 ASP.NET Core Identity 保護。

### 2\. 資料管理模組

  * **樂團管理 (Bands)**：
      * 列出所有樂團（包含成立年份、目前成員數）。
      * 新增、修改、刪除樂團資料。
      * **搜尋**: 可根據使用者輸入字串進行樂團搜索。
      * **進階搜尋**：可查詢「成團超過 N 年」且「成員數至少 M 人」的樂團。
  * **藝人管理 (Actors)**：
      * 列出藝人基本資料（姓名、生日、所屬國家）。
      * 自動計算並顯示藝人目前的**年齡**。
      * 新增、修改、刪除藝人資料。
      * **搜尋**: 可根據使用者輸入字串進行藝人姓氏、名字、所屬國家搜索。
      * **進階搜尋**：可篩選出「年齡大於 N 歲」的藝人。
  * **樂手管理 (Characters)**：
      * 列出樂手詳細資料（身高、生日、負責(樂團中的)位置）。
      * 顯示關聯的樂團名稱與藝人姓名（非僅顯示 ID）。
      * 新增、修改、刪除樂手資料。
      * **搜尋**: 可根據使用者輸入字串進行樂手姓氏、名稱搜尋
      * **進階搜尋**：可依照「樂團位置」（如 Vocal, Guitar, Bass 等）篩選角色。

## 技術堆疊 (Tech Stack)

  * **框架**：ASP.NET Core 8.0 MVC
  * **語言**：C\#, JavaScript
  * **資料庫**：Microsoft SQL Server
  * **ORM**：Entity Framework Core (Database-First / Scaffolding)
  * **前端**：Bootstrap 5, Razor Views
  * **開發工具**：Visual Studio 2022

## 安裝與執行 (Installation & Setup)

### 1\. 環境需求

  * .NET 8.0 SDK
  * Microsoft SQL Server (LocalDB 或 Express)

### 2\. 資料庫設定

本專案預設連線至本地端的 `character_1` 資料庫。
請確保 SQL Server 中已建立該資料庫，並執行初始化 SQL 腳本（`init.sql`）。

**連線字串設定** (`appsettings.json`)：

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=character_1;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

*如不使用 `localhost`，請將 `Server=` 修改為 SQL Server 實例名稱（例如 `(localdb)\mssqllocaldb`）。*

### 3\. 執行專案

使用 Visual Studio 開啟 `114-1_database_final_project.sln`，按下 **F5** 或 **Ctrl+F5** 執行。

或者在終端機輸入：

```bash
dotnet run
```

專案預設執行於：`https://localhost:7215`

## 使用說明 (Usage)

### 系統登入

- 採用 ASP.NET Core Identity 安全性功能支援。
- 透過註冊按鈕進行註冊，並使用已註冊的帳號、密碼於登入介面中完成登入後可進行資料庫編輯。

### 網頁導覽

  * **首頁**：系統入口。
  * **樂手管理**：管理所有樂手資料，支援依位置篩選。
  * **藝人管理**：管理藝人資料，支援依年齡篩選。
  * **樂團列表**：管理樂團資料，支援依創團年分與人數篩選。

## 資料庫關聯圖 (ER Diagram 簡述)

  * **Band (1)** \<---\> **(N) Character** : 一個樂團有多名樂手。
  * **VoiceActor (1)** \<---\> **(N) Character** : 一位藝人可兼任多個樂團 (一個藝人可以經營多個樂手)（雖通常為一對一，但設計上保留彈性）。

## 授權 (License)

本專案採用 [MIT License](LICENSE) 授權。
