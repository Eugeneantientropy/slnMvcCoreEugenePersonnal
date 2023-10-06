# slnMvcCoreEugenePersonnal
https://portaly.cc/eugeneantientropy  //個人履歷網站(包含圖文版作品介紹)
Flash Bean 咖啡豆店商網站
個人作品是以ASP.NET CORE 的MVC框架撰寫的咖啡豆電商網站。

主要功能有商城系統、會員系統、發文系統、後臺管理系統，儀錶板系統，站內聊天室，並串接綠界金流API、Open AI API及Google SMTP寄信等外部服務。
並使用Azure雲端部屬網站及SQL資料庫。

以下為各大項系統功能簡介:
1.首頁
2.文章系統
3.會員系統
4.商城系統
5.後臺儀錶板管理系統
6.站內聊天室
7.Azure雲端部屬
8.資料庫
https://eugeneantientropy.azurewebsites.net/ //作品實體網站



首頁
• 使用Html/Css，Boostrap等技術實做，並使用RWD響應式網頁風格。
• 上方Header導覽列放置網站各項功能。
• 輪播列顯示商品廣告宣傳，並有連結商城之按鈕。
• 文章區塊顯示最新三筆文章，並提供150字文章內容供使用者讀閱，點擊ReadMore可至文章列表查看完整內容。
• 下方Footer放置網站導覽列及版權免責聲明等。

文章系統
• 使用Html/Css，Boostrap等技術排版實做前端網頁，並使用RWD響應式網頁風格。
• 使用Entity FrameWork 將資料庫文章撈出並於前端。
• 使用Razor将C#參數帶入HTML中，於前端頁面顯示。
• 使用sessionStorage將使用者端儲存網頁未至，頁面刷新或返回時能回到之前網頁位置。

文章系統-文章編輯(Open AI API 串接)
• 使用 TinyMCE 所見即所得(WYSIWYG)編輯器，提供更方便編輯文章環境。
• 串接Open AI API 提供文案撰寫者AI文案輔助器。
Open AI 使用可給定參數:
1.AI最大回覆字數
2.AI回覆的冒險值(調越高變化程度越大，越小則像公式機器人)
3.給定AI在擔任角色，訓練AI為該網站專屬客服等，使用方式非常多元。)
此API免費額度18鎂，故僅開放管理者於編輯文章使用。

會員系統
• 會員註冊透過前後端雙重驗證輸入資料是否符合規範(帳號有無重複、電子信箱有無包含@及密碼有無包含數字英文特殊符號等)。
• 密碼使用BCrypt加密後存入資料庫，網站管理人員無法透過資料庫直接看到用戶密碼，增加本網站資訊安全。
• 會員Info頁面能修改會員基本資料及查看購買過的歷史訂單，訂單出貨狀況。
• 登錄時使用Entity Framework至資料庫比對Email，密碼部分則是使用BCrypt.Verify，將輸入密碼加密後與資料庫加密後密碼比對驗證。
• 驗證成功將會員資料以Json格式放入Session及Model Class，方便其他頁面向該類別索取目前登錄會員資料。
• 驗證Session有無存入登入前最後查看網頁資料，如有登入後直接跳轉該網頁，加強無縫式用戶體驗。

會員系統-忘記密碼(串接Google SMTP寄信服務)
• 使用Gmail透過兩步驟驗證拿到應用程式專屬密碼。
• 提供Port587，Email，應用程式專屬密碼等資料串街SMTP寄信服務。
• 將用戶輸入Email比對資料庫，如成功並寄送隨機產出(至少包含大小寫英文、數字及特殊符號)之新密碼至該用戶信箱。

商城系統
• 頁面使用Entity Framework至資料庫撈取商品資料顯示於頁面，並透過資料欄Classfication提供使用者分類欄位進行篩選。
• 需驗證在有會員登入狀況下才能進入購買頁面。
• 商品詳細頁面透過專屬資料表顯示多張商品圖片及詳細商品規格，尚餘數量等。
• 如確定加入購物車，先驗證Session有無購物車商品資料，如無創建一個List放入Session中，如有用JsonSerializer.Deserialize方式解構出Json List資料再加入。
• Header的購物車Icon會依據Session購物數量顯示於右上角。
• 商品CRUD實做搜尋欄，能一次上傳多張照片並透過Guid隨機產生20碼檔案名稱，存入WebRootPath/指定圖檔路徑。

商城系統-結帳(串接第三方綠界金流)
• 透過Guid產生20碼專屬變數為該筆訂單專屬訂單存入資料庫，將ID帶入跳轉頁面。
• 經跳轉頁面帶入訂單專屬ID並帶入以下參數:
1.MerchantTradeNo(專屬ID)
2.MerchantTradeDate(結帳日期)
3.TotalAmount(訂單金額)
4.ItemName(商品名稱)
5.ReturnURL(Post付款結果至指定網址)
6.OrderResultURL(Post付款結果資訊至指定網址)
7.ClientBackURL(留在綠界之頁面，會顯示返回店面至指定網址，若OrderResultURL有設定優先排序以此優先)
• CheckMacValue為回傳檢查碼需在前後加入hashKey及HashIV並以SHA256加密處理後傳出，將數據提交至綠界平台。


後臺儀錶板管理系統
• 前端使用Chart.js模組，視覺化商品類型、出貨訂單、新進會員、營業額四項網站指標數據。
• 控制器使用Entity FrameWork方式取出顯示資料存入List。
• JavaScript發出AJAX至後端，獲取每個圖表數據。
• 圖表類型本站使用Pie、Line、Bar，此工具庫還提供Doughnut、Bubble等視覺化樣式，使用方式多元。

站內即時聊天室
• 使用SignalR 程式庫功能，實現即時通信功能。
• Signal使用多種通信協定WebSocket、Long Polling 或 Server-Sent Events (SSE)，能依瀏覽器支援能力自動尋找最適合的通訊方式 。
• 新增類別定義連線事件、離線事件及發送訊息事件。
• 前端以jQuery綁定事件和操作 DOM 元素。

Azure雲端佈置
• 使用PaaS（平台即服務）。

SQL資料庫
• 使用SQL關聯式資料庫。
• 資料庫有會員、管理員、商品、商品照片、訂單、訂單明細、文章等資料表。
