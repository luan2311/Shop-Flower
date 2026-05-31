# Kế Hoạch Phân Phối Và Thực Thi Kiểm Thử Chi Tiết (Siêu Cấp) - ShopFlower

Tài liệu này cung cấp sơ đồ phân phối công việc chi tiết ở mức độ cực kỳ cao dành cho **5 thành viên** để triển khai bộ Test Plan toàn diện cho dự án ShopFlower. Kế hoạch này được thiết kế để vận hành trơn chu như một quy trình Agile/Scrum thực thụ trong vòng **3 ngày**.

---

## 1. Ma Trận Trách Nhiệm RACI (Responsibility Assignment Matrix)

Để đảm bảo không có bất kỳ ca kiểm thử nào bị bỏ sót hay trùng lặp, dưới đây là ma trận phân bổ trách nhiệm chi tiết cho từng Test Case ID đã được thiết lập:

*   **R (Responsible):** Người trực tiếp thực hiện công việc (Code, Test).
*   **A (Accountable):** Người chịu trách nhiệm tối cao về kết quả và duyệt sản phẩm.
*   **C (Consulted):** Người được tham vấn chuyên môn (Hỗ trợ, cung cấp dữ liệu).
*   **I (Informed):** Người được thông báo kết quả sau khi hoàn thành.

| Mã Test Case | Tên Ca Kiểm Thử | T.Viên 1 (Auto Lead) | T.Viên 2 (Backend) | T.Viên 3 (API QA) | T.Viên 4 (Security) | T.Viên 5 (Manual QA) |
| :--- | :--- | :---: | :---: | :---: | :---: | :---: |
| **REG_01 - 05** | Phân hệ Đăng Ký (Register) | **R** | **C** | **R** | **I** | **A** |
| **LOG_01 - 05** | Phân hệ Đăng Nhập (Login) | **R** | **C** | **R** | **I** | **A** |
| **DET_01 - 04** | Phân hệ Chi Tiết Sản Phẩm | **R** | **I** | **I** | **I** | **R** / **A** |
| **PAY_01 - 03** | Phân hệ Giỏ Hàng & Thanh Toán | **R** | **C** | **R** | **I** | **R** / **A** |
| **ADM_01 - 04** | Phân hệ Quản Trị (Admin CRUD) | **R** | **C** | **R** | **I** | **R** / **A** |
| **PRF_01 / NWS_01**| Phân hệ Cá Nhân & Tin Tức | **I** | **I** | **R** | **I** | **R** / **A** |
| **SEC_01** | Kiểm thử SQL Injection & XSS | **I** | **C** | **I** | **R** | **A** |

---

## 2. Bản Phân Công Công Việc Chi Tiết Từng Giờ (Phase-by-Phase Task Allocation)

### 🥇 THÀNH VIÊN 1: QA AUTOMATION LEAD (Chuyên Gia Playwright Giao Diện)

*   **Đầu vào yêu cầu (Inputs):** Thông tin cổng mạng IIS Express (cổng mặc định `44357`), mã nguồn view [ThanhToan.cshtml](file:///C:/code/CNPM/Nhom6_ST5-Ca2/Shop-Flower/ShopFlower/Views/Checkout/ThanhToan.cshtml), tài khoản test hợp lệ do Thành viên 2 cung cấp.
*   **Đầu ra bàn giao (Outputs):** Dự án Node.js [ShopFlower.Tests.UI](file:///C:/code/CNPM/Nhom6_ST5-Ca2/Shop-Flower/ShopFlower.Tests.UI), tệp cấu hình `playwright.config.js`, và 3 file test `.spec.js`.

#### 📅 Lịch trình thực hiện chi tiết:
*   **NGÀY 1:**
    *   *Sáng (08:00 - 12:00):* Khởi tạo dự án Node.js bằng lệnh `npm init -y` tại thư mục `ShopFlower.Tests.UI`. Cài đặt các thư viện `@playwright/test` và thực thi lệnh tải trình duyệt `npx playwright install`.
    *   *Chiều (13:30 - 17:30):* Thiết lập cấu hình tệp `playwright.config.js` hỗ trợ cross-browser song song (Chromium, Firefox, WebKit) và giả lập khung hình điện thoại (iPhone 14 Pro, Pixel 7). Bật cấu hình `ignoreHTTPSErrors: true` để tránh lỗi chứng chỉ SSL cục bộ.
*   **NGÀY 2:**
    *   *Sáng (08:00 - 12:00):* Viết kịch bản tự động kiểm thử đăng ký/đăng nhập (`auth.spec.js`), kiểm tra các ca biên như độ dài mật khẩu tối thiểu và tự động trim khoảng trắng.
    *   *Chiều (13:30 - 17:30):* Viết kịch bản kiểm thử trang Thanh toán (`checkout.spec.js`). Viết code mô phỏng điền form lỗi -> xác thực class `is-invalid` được thêm vào input và trang tự cuộn focus vào ô lỗi. Viết code test lọc ký tự chữ ô Số điện thoại.
*   **NGÀY 3:**
    *   *Sáng (08:00 - 12:00):* Viết kịch bản kiểm thử phân quyền Admin (`admin.spec.js`), kiểm tra việc chặn User thường truy cập vào trang quản trị và chặn tạo sản phẩm có giá `<= 0`.
    *   *Chiều (13:30 - 17:30):* Chạy toàn bộ các ca kiểm thử tự động trên mọi trình duyệt, chụp ảnh màn hình các ca lỗi (nếu có) và xuất báo cáo `playwright-report` bàn giao cho Thành viên 5.

---

### 💻 THÀNH VIÊN 2: BACKEND DEVELOPER (Chuyên Gia Unit Test C#)

*   **Đầu vào yêu cầu (Inputs):** Mã nguồn các API Controllers (`AccountController.cs`, `HoaDonController.cs`, `LienHeController.cs`) và chuỗi kết nối Database local.
*   **Đầu ra bàn giao (Outputs):** Dự án [ShopFlower.Tests](file:///C:/code/CNPM/Nhom6_ST5-Ca2/Shop-Flower/ShopFlower.Tests) biên dịch thành công 100%, chứa các tệp kiểm thử `PasswordHashingTests.cs`, `LienHeControllerTests.cs`, `HoaDonControllerTests.cs`.

#### 📅 Lịch trình thực hiện chi tiết:
*   **NGÀY 1:**
    *   *Sáng (08:00 - 12:00):* Liên kết dự án `ShopFlower.Tests.csproj` vào file solution chung `ShopFlower.sln` thông qua Visual Studio hoặc lệnh CLI. Thực hiện cài đặt các thư viện test (`MSTest.TestFramework`, `MSTest.TestAdapter`, `Moq`) qua NuGet.
    *   *Chiều (13:30 - 17:30):* Mở rộng lớp `PasswordHashingTests.cs` để phủ kín các trường hợp kiểm thử băm mật khẩu PBKDF2 (10,000 iterations) và cơ chế đối sánh tự động fallback SHA256 cũ khi người dùng đăng nhập lần đầu.
*   **NGÀY 2:**
    *   *Sáng (08:00 - 12:00):* Viết các ca kiểm thử tích hợp cho API Liên hệ (`LienHeControllerTests.cs`). Sử dụng thư viện `Moq` giả lập DbContext để verify Stored Procedure `sp_ThemLienHe` được gọi đúng tham số khi nhận dữ liệu từ API.
    *   *Chiều (13:30 - 17:30):* Viết kịch bản kiểm thử đơn hàng (`HoaDonControllerTests.cs`), mô phỏng nghiệp vụ tạo hóa đơn an toàn qua `POST api/HoaDon`. Xác minh cơ chế tự động Rollback (hủy bỏ) toàn bộ transaction khi một chi tiết hoa (`CTHD`) bị lỗi định dạng hoặc hết hàng.
*   **NGÀY 3:**
    *   *Sáng (08:00 - 12:00):* Viết ca kiểm thử ngưng bán sản phẩm (`SanPhamControllerTests.cs`) để kiểm tra logic soft-delete (khi gửi request PUT, số lượng tồn kho tự động cập nhật về 0 và trạng thái chuyển thành "Hết hàng").
    *   *Chiều (13:30 - 17:30):* Chạy kiểm thử toàn bộ dự án C# bằng lệnh `dotnet test ShopFlower.Tests\ShopFlower.Tests.csproj --no-build` và chuyển giao file kết quả cho Thành viên 5.

---

### 🔌 THÀNH VIÊN 3: API QA SPECIALIST (Chuyên Gia API SoapUI)

*   **Đầu vào yêu cầu (Inputs):** Tài liệu đặc tả API, danh sách Endpoint của 6 API Controllers, và công cụ SoapUI cài đặt trên máy.
*   **Đầu ra bàn giao (Outputs):** Tệp cấu hình dự án SoapUI [ShopFlower_SoapUI_Project.xml](file:///C:/code/CNPM/Nhom6_ST5-Ca2/Shop-Flower/ShopFlower_SoapUI_Project.xml), script chạy nhanh [run_soapui_tests.bat](file:///C:/code/CNPM/Nhom6_ST5-Ca2/Shop-Flower/run_soapui_tests.bat).

#### 📅 Lịch trình thực hiện chi tiết:
*   **NGÀY 1:**
    *   *Sáng (08:00 - 12:00):* Khởi tạo dự án REST trong SoapUI, thiết lập Endpoint gốc `https://localhost:44357`. Khai báo đầy đủ các Resource cho 6 Controllers.
    *   *Chiều (13:30 - 17:30):* Xây dựng TestSuite "E2E Checkout Workflow TestSuite". Thiết lập Step 1 (Register) và Step 2 (Login). Viết các Assertions cơ bản kiểm tra mã trạng thái HTTP `200 OK`.
*   **NGÀY 2:**
    *   *Sáng (08:00 - 12:00):* Cấu hình **Property Transfer** nâng cao. Viết script trích xuất giá trị `$.profile.MaTK` từ kết quả đăng nhập của Step 2 và lưu thành biến toàn cục của TestCase để truyền tự động vào thuộc tính đặt hàng ở Step 4.
    *   *Chiều (13:30 - 17:30):* Hoàn thành Step 4 (Place Order) và Step 5 (Order History). Viết script trích xuất mã hóa đơn `MaHD` trả về từ Step 4 truyền vào API cập nhật trạng thái hóa đơn của Admin (Step 6).
*   **NGÀY 3:**
    *   *Sáng (08:00 - 12:00):* Thêm các Assertions nâng cao về hiệu năng (SLA Response Time < 500ms cho mọi request) và Assertions JSON Schema để đối sánh cấu trúc dữ liệu trả về từ DB.
    *   *Chiều (13:30 - 17:30):* Tối ưu hóa tệp tin `run_soapui_tests.bat` để tự động quét tìm thư mục cài đặt SoapUI và tạo lệnh tự động lưu trữ báo cáo định dạng JUnit vào thư mục `SoapUI_Reports`.

---

### 🛡️ THÀNH VIÊN 4: SECURITY QA TESTER (Chuyên Gia Bảo Mật Hệ Thống)

*   **Đầu vào yêu cầu (Inputs):** Các ô nhập liệu nhạy cảm trên giao diện web, các API endpoints tiếp nhận dữ liệu chuỗi (String).
*   **Đầu ra bàn giao (Outputs):** Báo cáo đánh giá lỗ hổng bảo mật (Security Penetest Summary Report) và danh sách khuyến nghị bảo mật.

#### 📅 Lịch trình thực hiện chi tiết:
*   **NGÀY 1:**
    *   *Sáng (08:00 - 12:00):* Nghiên cứu cấu trúc CSDL và các trường đầu vào. Chuẩn bị danh sách các Payload tấn công SQL Injection thông dụng (ví dụ: `' OR '1'='1`, `admin' --`, `' UNION SELECT NULL, NULL...`).
    *   *Chiều (13:30 - 17:30):* Tiến hành kiểm thử SQL Injection thủ công và tự động trên ô nhập liệu Đăng nhập và Tìm kiếm hoa. Xác nhận hệ thống trả về mã lỗi `400 Bad Request` hoặc không trả về kết quả rác, không bị sập (lỗi 500) làm lộ cấu trúc bảng.
*   **NGÀY 2:**
    *   *Sáng (08:00 - 12:00):* Soạn thảo các Payload tấn công Cross-Site Scripting (XSS) dạng Stored XSS và Reflected XSS (ví dụ: `<script>alert('xss')</script>`, `<img src=x onerror=alert(1)>`).
    *   *Chiều (13:30 - 17:30):* Tiến hành chèn các Payload XSS vào Form Liên hệ (`POST api/LienHe`) và phần bình luận/ghi chú đơn hàng. Truy cập vào giao diện quản trị để kiểm tra xem trình duyệt có thực thi đoạn mã script độc hại đó không (nếu hệ thống đã được mã hóa HTML Encode tốt, mã script sẽ chỉ hiển thị dưới dạng văn bản thường và không gây hại).
*   **NGÀY 3:**
    *   *Sáng (08:00 - 12:00):* Tổng hợp các phát hiện lỗi bảo mật, phân tích mức độ nghiêm trọng (High, Medium, Low) dựa trên chuẩn OWASP Top 10.
    *   *Chiều (13:30 - 17:30):* Viết báo cáo bàn giao kỹ thuật cho lập trình viên Backend để thực hiện vá lỗi (nếu có lỗ hổng) và chuyển giao tài liệu cho Thành viên 5.

---

### 📝 THÀNH VIÊN 5: MANUAL QA & DATA AUDITOR (Kiểm Thử Thủ Công & Nghiệm Thu)

*   **Đầu vào yêu cầu (Inputs):** Các báo cáo kết quả tự động từ Thành viên 1, 2, 3, 4; quyền truy cập trực tiếp vào DB SQL Server.
*   **Đầu ra bàn giao (Outputs):** Tài liệu Báo cáo Nghiệm thu Kiểm thử toàn diện (Test Execution Summary) để bàn giao cho khách hàng.

#### 📅 Lịch trình thực hiện chi tiết:
*   **NGÀY 1:**
    *   *Sáng (08:00 - 12:00):* Chuẩn bị tệp dữ liệu kiểm thử (Test Data Sheet) chứa các giá trị biên phục vụ kiểm thử thủ công như các mức số lượng tồn kho (n = 0, n = 1, n = 9999).
    *   *Chiều (13:30 - 17:30):* Tiến hành kiểm thử thủ công giao diện (Manual UAT) trên các tính năng như hiển thị phóng to (Zoom) ảnh sản phẩm, kiểm tra xem ảnh thay thế alt có hoạt động đúng khi link ảnh bị hỏng không.
*   **NGÀY 2:**
    *   *Sáng (08:00 - 12:00):* Kiểm thử thủ công các ca kiểm thử biên số lượng đặt mua tại trang Chi tiết sản phẩm. Nhập các giá trị rác hoặc số âm trực tiếp vào ô để xem Front-end chặn lỗi thế nào.
    *   *Chiều (13:30 - 17:30):* Thực hiện đối soát cơ sở dữ liệu (Database Auditing). Sau khi các thành viên chạy test tự động tạo đơn hàng mới, truy cập trực tiếp CSDL SQL Server kiểm tra tính đồng bộ của bảng `HOADON` và `CTHD` (đối chiếu xem tổng tiền hóa đơn có bằng tổng giá trị của các chi tiết sản phẩm cộng lại không).
*   **NGÀY 3:**
    *   *Sáng (08:00 - 12:00):* Tạo và cấu hình tệp tin chạy tự động toàn bộ Suite kiểm thử [run_all_tests.bat](file:///C:/code/CNPM/Nhom6_ST5-Ca2/Shop-Flower/run_all_tests.bat) kết nối cả C#, SoapUI và Playwright.
    *   *Chiều (13:30 - 17:30):* Kích hoạt chạy Master Script, thu thập báo cáo từ các bài test tự động và bảo mật, biên soạn tài liệu **Báo cáo nghiệm thu kiểm thử tổng thể** hoàn chỉnh và đóng gói dự án để bàn giao.

---

## 3. Các Điểm Liên Kết & Bàn Giao Dữ Liệu Giữa Các Thành Viên (Integration Hand-offs)

Để các thành viên không bị gián đoạn công việc, quy trình bàn giao dữ liệu được quy định nghiêm ngặt như sau:

```
 [Thành viên 2] ──(Tài khoản & SP ID)──> [Thành viên 1 & 3] ──(Đơn hàng ảo tạo ra)──> [Thành viên 5]
 (C# Backend)                              (Playwright & SoapUI)                         (Đối soát DB)
```

1.  **Bàn giao Dữ liệu cấu hình (Đầu Ngày 1):** Thành viên 2 (Backend) phải cung cấp cổng IIS Express hoạt động và danh sách các mã sản phẩm hợp lệ trong DB (ví dụ: `SP001`, `SP002`) cho Thành viên 1 (Playwright) và Thành viên 3 (SoapUI) để họ đưa vào kịch bản test tự động.
2.  **Bàn giao Tiêu chí mã hóa mật khẩu (Chiều Ngày 1):** Thành viên 2 xác nhận với Thành viên 1 và 3 về độ dài và độ phức tạp tối thiểu của mật khẩu tài khoản test để tránh việc kịch bản test tự động đăng ký bị Front-end chặn.
3.  **Bàn giao Mã đơn hàng đối soát (Chiều Ngày 2 & 3):** Sau khi Thành viên 1 và 3 chạy test tự động tạo đơn hàng ảo thành công, họ phải xuất ra các mã hóa đơn tương ứng (ví dụ: `HD001`, `HD002`) để Thành viên 5 (Manual QA) truy vấn trực tiếp vào SQL Server nhằm đối soát tính toàn vẹn dữ liệu.

---

## 4. Quản Trị Rủi Ro Và Phương Án Dự Phòng (Risk & Mitigation Checklist)

| Tình Huống Rủi Ro | Mức Độ | Ảnh Hưởng | Phương Án Khắc Phục Kịp Thời (Mitigation) |
| :--- | :---: | :--- | :--- |
| **Cơ sở dữ liệu Local bị mất kết nối** | **Cao** | Chặn toàn bộ các bài test API, Unit Test và đặt hàng. | *Thành viên 2* chịu trách nhiệm backup sẵn file `.bak` của DB `QL_SHOPFLOWER` để phục hồi lại chỉ trong 5 phút. |
| **Playwright bị lỗi không tìm thấy trình duyệt** | **Trung bình**| Không chạy được kiểm thử UI tự động. | Chạy lệnh `npx playwright install --with-deps` để tự động tải và sửa chữa các gói thư viện hệ thống bị thiếu. |
| **Trùng lặp dữ liệu khi chạy test nhiều lần** | **Trung bình**| Lỗi Unique Constraint khi chạy lại ca test đăng ký. | *Thành viên 3* cấu hình SoapUI tự động tạo chuỗi ngẫu nhiên (Random String) đính kèm vào tên đăng nhập (ví dụ: `soapui_user_${=System.currentTimeMillis()}`). |
| **Cổng kết nối IIS Express 44357 bị chặn** | **Trung bình**| Chặn toàn bộ luồng kiểm thử giao diện và API. | *Thành viên 1* thực hiện kiểm tra bằng lệnh `netstat -ano \| findstr 44357` và tắt tiến trình đang chiếm dụng cổng, hoặc thay đổi cổng kết nối trong `Web.config`. |
