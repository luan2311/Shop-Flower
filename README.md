# ShopFlower Testing

Tài liệu này mô tả dự án ShopFlower dưới góc nhìn kiểm thử. Mục tiêu của brand **Testing** là cung cấp một bộ hướng dẫn rõ ràng để cài đặt, chạy ứng dụng và thực thi các lớp kiểm thử tự động cho website bán hoa ShopFlower.

## Tổng Quan

ShopFlower là ứng dụng ASP.NET MVC phục vụ các nghiệp vụ bán hoa trực tuyến: xem danh mục sản phẩm, tìm kiếm, quản lý giỏ hàng, thanh toán, tài khoản người dùng, tin tức, liên hệ và khu vực quản trị.

Phần Testing tập trung kiểm tra các lớp sau:

- Unit test cho logic C# quan trọng.
- Security test cho các rủi ro như SQL Injection và XSS.
- API test bằng SoapUI.
- UI/E2E test bằng Playwright trên nhiều trình duyệt.
- Kiểm thử thủ công cho các luồng nghiệp vụ chính.

## Quick Start Cho Testing

Sau khi đã cấu hình database và chạy được website bằng IIS Express, có thể kiểm tra nhanh bằng các lệnh sau:

```bash
dotnet test ShopFlower.Tests/ShopFlower.Tests.csproj
dotnet test ShopFlower.Tests.Security/ShopFlower.Tests.Security.csproj
```

Chạy Playwright UI tests:

```bash
cd ShopFlower.Tests.UI
npm install
npx playwright install
npm test
```

Nếu cần test API bằng SoapUI, mở file `ShopFlower-soapui-project.xml` trong SoapUI hoặc chạy bằng `testrunner.bat` của SoapUI.

## Công Nghệ Sử Dụng

- ASP.NET MVC trên .NET Framework 4.8.
- Entity Framework Database First với file `.edmx`.
- SQL Server hoặc SQL Server Express.
- MSTest cho C# unit/security tests.
- SoapUI cho kiểm thử API.
- Playwright cho kiểm thử giao diện tự động.
- Visual Studio 2019/2022 hoặc môi trường tương thích .NET Framework 4.8.

## Cấu Trúc Chính

```text
Shop-Flower/
├── ShopFlower/                    # Ứng dụng ASP.NET MVC
├── ShopFlower.Tests/              # Unit tests C#
├── ShopFlower.Tests.Security/     # Security tests
├── ShopFlower.Tests.UI/           # Playwright E2E tests
├── Agent/                         # Kế hoạch và tài liệu testing
├── ShopFlower-soapui-project.xml  # File cấu hình SoapUI hiện có trong repo
└── ShopFlower.sln                 # Solution Visual Studio
```

## Yêu Cầu Cài Đặt

Trước khi chạy dự án, cần chuẩn bị:

- Visual Studio có workload ASP.NET and web development.
- .NET Framework 4.8 Developer Pack.
- SQL Server hoặc SQL Server Express.
- Node.js LTS để chạy Playwright.
- SoapUI 5.7.x nếu muốn chạy API test.
- Git và NuGet package restore.

## Cài Đặt Và Chạy Ứng Dụng

1. Clone repository:

```bash
git clone https://github.com/luan2311/Shop-Flower.git
cd Shop-Flower
```

2. Mở `ShopFlower.sln` bằng Visual Studio.

3. Restore NuGet packages cho toàn bộ solution.

4. Chuẩn bị database SQL Server theo dữ liệu được nhóm cung cấp. Sau đó cập nhật connection string trong `ShopFlower/Web.config`.

5. Đặt project `ShopFlower` làm Startup Project.

6. Build solution, sau đó chạy bằng IIS Express.

Ứng dụng thường chạy ở một địa chỉ dạng:

```text
https://localhost:<port>
```

Port cụ thể phụ thuộc cấu hình IIS Express trên máy đang chạy.

## Phạm Vi Kiểm Thử

### 1. Tài Khoản

- Đăng ký tài khoản hợp lệ.
- Chặn username trùng.
- Kiểm tra định dạng email.
- Đăng nhập thành công/thất bại.
- Kiểm tra tài khoản bị khóa hoặc không có quyền truy cập.
- Kiểm tra hash mật khẩu.

### 2. Sản Phẩm Và Giỏ Hàng

- Hiển thị danh sách và chi tiết sản phẩm.
- Tìm kiếm sản phẩm.
- Thêm sản phẩm vào giỏ hàng.
- Cập nhật số lượng.
- Xóa sản phẩm khỏi giỏ hàng.
- Tính tổng tiền theo thời gian thực.

### 3. Thanh Toán

- Kiểm tra form thông tin đặt hàng.
- Chặn dữ liệu thiếu hoặc sai định dạng.
- Kiểm tra số điện thoại.
- Kiểm tra luồng đặt hàng.
- Kiểm tra giao diện lỗi và focus vào trường lỗi đầu tiên.

### 4. Quản Trị

- Phân quyền truy cập khu vực admin.
- Quản lý sản phẩm.
- Quản lý tin tức.
- Quản lý tài khoản.
- Quản lý đơn hàng và liên hệ.

### 5. Bảo Mật

- SQL Injection.
- Cross-Site Scripting.
- Kiểm tra thông báo lỗi không làm lộ thông tin nhạy cảm.
- Kiểm tra truy cập trái phép vào admin.

## Chạy Unit Test C#

Chạy test logic C#:

```bash
dotnet test ShopFlower.Tests/ShopFlower.Tests.csproj
```

Chạy security tests:

```bash
dotnet test ShopFlower.Tests.Security/ShopFlower.Tests.Security.csproj
```

Các test hiện có tập trung vào:

- Hashing mật khẩu.
- SQL Injection.
- XSS.
- Một số logic backend có thể kiểm thử độc lập.

## Chạy UI/E2E Test Bằng Playwright

Đi tới thư mục test UI:

```bash
cd ShopFlower.Tests.UI
npm install
npx playwright install
npm test
```

Các file test chính:

- `tests/auth.spec.js`: luồng đăng ký, đăng nhập.
- `tests/checkout.spec.js`: giỏ hàng và thanh toán.
- `tests/admin.spec.js`: khu vực quản trị.
- `tests/extended-flows.spec.js`: các luồng mở rộng.

Để chạy ở chế độ có giao diện trình duyệt:

```bash
npm run test:headed
```

Để mở Playwright UI mode:

```bash
npm run test:ui
```

## Chạy API Test Bằng SoapUI

Repository hiện có file cấu hình SoapUI:

```text
ShopFlower-soapui-project.xml
```

Có thể chạy theo một trong hai cách:

- Mở SoapUI, chọn Import Project, sau đó chọn `ShopFlower-soapui-project.xml`.
- Dùng `testrunner.bat` của SoapUI và truyền đường dẫn tới file project.

Ví dụ trên Windows:

```bat
"C:\Program Files\SmartBear\SoapUI-5.7.0\bin\testrunner.bat" -r -a -j -f"SoapUI_Reports" "ShopFlower-soapui-project.xml"
```

Báo cáo SoapUI sẽ được xuất ra thư mục:

```text
SoapUI_Reports/
```

## Chạy Toàn Bộ Bộ Kiểm Thử

Nếu muốn chạy toàn bộ test thủ công theo thứ tự, dùng các lệnh:

```bash
dotnet test ShopFlower.Tests/ShopFlower.Tests.csproj
dotnet test ShopFlower.Tests.Security/ShopFlower.Tests.Security.csproj
cd ShopFlower.Tests.UI
npm test
```

Thứ tự kiểm thử khuyến nghị:

1. C# unit tests.
2. C# security tests.
3. SoapUI API tests.
4. Playwright UI/E2E tests.

Trước khi chạy tổng hợp, cần bảo đảm:

- Ứng dụng web đang chạy.
- Database đã kết nối đúng.
- Node dependencies đã được cài hoặc script có quyền chạy `npm install`.
- SoapUI đã được cài nếu cần chạy API test.

## Báo Cáo Kiểm Thử

Các báo cáo/kết quả test thường nằm tại:

```text
ShopFlower.Tests.UI/playwright-report/
ShopFlower.Tests.UI/test-results/
ShopFlower.Tests.Security/Report.Security.md
SoapUI_Reports/
TestResults/
```

Khi ghi nhận lỗi, nên lưu kèm:

- Tên test case.
- Môi trường chạy.
- Dữ liệu đầu vào.
- Kết quả mong đợi.
- Kết quả thực tế.
- Screenshot hoặc video nếu là lỗi UI.
- Log hoặc stack trace nếu là lỗi backend.

## Quy Ước Testing

- Test case nên đặt tên theo phân hệ: `AUTH`, `CART`, `CHECKOUT`, `ADMIN`, `SECURITY`.
- Mỗi test chỉ nên kiểm tra một hành vi chính.
- Dữ liệu test cần rõ ràng và có thể tái lập.
- Không dùng dữ liệu production cho kiểm thử tự động.
- Khi phát hiện lỗi bảo mật, ghi nhận mức độ ảnh hưởng và bước tái hiện tối thiểu.

## Checklist Nghiệm Thu Nhanh

- Build solution thành công.
- Trang chủ mở được trên IIS Express.
- Kết nối database thành công.
- Đăng ký và đăng nhập hoạt động.
- Thêm sản phẩm vào giỏ hàng hoạt động.
- Thanh toán kiểm tra dữ liệu bắt buộc.
- Admin không cho user thường truy cập.
- Unit tests chạy được.
- Security tests chạy được.
- Playwright tests chạy được trên ít nhất một trình duyệt.
- Báo cáo test được xuất ra sau khi chạy.

## Ghi Chú Cho Nhóm Testing

README này ưu tiên phục vụ việc kiểm thử và nghiệm thu. Nếu cần triển khai production, cần bổ sung riêng tài liệu về cấu hình server, bảo mật connection string, publish IIS, backup database và quản lý secrets.
