# Instructions

- Following Playwright test failed.
- Explain why, be concise, respect Playwright best practices.
- Provide a snippet of code with the fix, if possible.

# Test info

- Name: auth.spec.js >> Phân hệ Đăng Ký & Đăng Nhập (Authentication Test Suite) >> REG_02: Chặn đăng ký trùng Tên đăng nhập
- Location: tests\auth.spec.js:35:3

# Error details

```
Error: page.goto: net::ERR_CONNECTION_REFUSED at https://localhost:44357/Account/Dang_ky
Call log:
  - navigating to "https://localhost:44357/Account/Dang_ky", waiting until "load"

```

# Test source

```ts
  1  | const { test, expect } = require('@playwright/test');
  2  | 
  3  | test.describe('Phân hệ Đăng Ký & Đăng Nhập (Authentication Test Suite)', () => {
  4  | 
  5  |   test('REG_01: Đăng ký thành công và tự động trim khoảng trắng', async ({ page }) => {
  6  |     await page.goto('/Account/Dang_ky');
  7  |     
  8  |     const uniqueId = Date.now() + Math.floor(Math.random() * 1000);
  9  |     const rawUsername = `  auto_client_${uniqueId}  `;
  10 |     const email = `autoclient_${uniqueId}@example.com`;
  11 | 
  12 |     // Giả lập nhập tên đăng nhập có khoảng trắng ở đầu và cuối
  13 |     const usernameInput = page.locator('input[name="TenDangNhap"]');
  14 |     await usernameInput.fill(rawUsername);
  15 |     
  16 |     await page.fill('input[name="TenHienThi"]', 'Khách hàng Tự động');
  17 |     await page.fill('input[name="Email"]', email);
  18 |     await page.fill('input[name="MatKhau"]', 'SecurePass123!');
  19 |     await page.fill('input[name="XacNhanMatKhau"]', 'SecurePass123!');
  20 |     
  21 |     await page.click('button[type="submit"]');
  22 | 
  23 |     // Chờ hệ thống xử lý và chuyển hướng về màn hình đăng nhập
  24 |     await expect(page).toHaveURL(/\/Account\/Dang_nhap/);
  25 | 
  26 |     // Chạy thêm bước kiểm tra đăng nhập bằng tên đã trim để kiểm chứng tính năng Trim hoạt động
  27 |     await page.fill('input[name="TenDangNhap"]', `auto_client_${uniqueId}`); // Nhập bản đã trim
  28 |     await page.fill('input[name="MatKhau"]', 'SecurePass123!');
  29 |     await page.click('button[type="submit"]');
  30 |     
  31 |     // Đăng nhập thành công -> về trang chủ hoặc dashboard của user
  32 |     await expect(page).toHaveURL(/\/Home\/Trang_chu|Checkout\/ThanhToan|Account\/Orders|\//);
  33 |   });
  34 | 
  35 |   test('REG_02: Chặn đăng ký trùng Tên đăng nhập', async ({ page }) => {
> 36 |     await page.goto('/Account/Dang_ky');
     |                ^ Error: page.goto: net::ERR_CONNECTION_REFUSED at https://localhost:44357/Account/Dang_ky
  37 |     
  38 |     // Nhập tên ĐN đã tồn tại
  39 |     await page.fill('input[name="TenDangNhap"]', 'admin');
  40 |     await page.fill('input[name="TenHienThi"]', 'Quản trị viên');
  41 |     await page.fill('input[name="Email"]', 'admin_new@example.com');
  42 |     await page.fill('input[name="MatKhau"]', 'AdminPass123!');
  43 |     await page.fill('input[name="XacNhanMatKhau"]', 'AdminPass123!');
  44 |     
  45 |     await page.click('button[type="submit"]');
  46 | 
  47 |     // Phải hiển thị lỗi validation trên giao diện hoặc báo trùng lặp
  48 |     const errorMsg = page.locator('.validation-summary-errors, .alert-danger, [data-valmsg-for="TenDangNhap"]');
  49 |     await expect(errorMsg).toBeVisible();
  50 |   });
  51 | 
  52 |   test('REG_03: Chặn đăng ký với Email sai định dạng', async ({ page }) => {
  53 |     await page.goto('/Account/Dang_ky');
  54 |     
  55 |     const emailInput = page.locator('input[name="Email"]');
  56 |     await emailInput.fill('autouser01gmail.com'); // Thiếu @
  57 |     await emailInput.blur();
  58 | 
  59 |     // Front-end validation phải bắt lỗi lập tức
  60 |     await expect(emailInput).toHaveClass(/is-invalid/);
  61 |     const errorSpan = page.locator("[data-valmsg-for='Email']");
  62 |     await expect(errorSpan).toContainText('Email không hợp lệ');
  63 |   });
  64 | 
  65 |   test('LOG_01: Đăng nhập thất bại với mật khẩu sai', async ({ page }) => {
  66 |     await page.goto('/Account/Dang_nhap');
  67 |     
  68 |     await page.fill('input[name="TenDangNhap"]', 'admin');
  69 |     await page.fill('input[name="MatKhau"]', 'WrongPassword123');
  70 |     await page.click('button[type="submit"]');
  71 | 
  72 |     // Xác nhận vẫn ở trang đăng nhập và hiển thị thông báo lỗi
  73 |     await expect(page).toHaveURL(/\/Account\/Dang_nhap/);
  74 |     const errorSummary = page.locator('.validation-summary-errors, .alert-danger, .validation-summary-errors li');
  75 |     await expect(errorSummary).toBeVisible();
  76 |   });
  77 | 
  78 |   test('LOG_02: Kiểm tra chức năng Ẩn/Hiện mật khẩu', async ({ page }) => {
  79 |     await page.goto('/Account/Dang_nhap');
  80 |     
  81 |     const passwordInput = page.locator('input[name="MatKhau"]');
  82 |     await passwordInput.fill('AdminPass123!');
  83 | 
  84 |     // Xác nhận mặc định mật khẩu là ẩn (type="password")
  85 |     await expect(passwordInput).toHaveAttribute('type', 'password');
  86 | 
  87 |     // Tìm nút Toggle Ẩn/Hiện (nếu có biểu tượng con mắt) và click
  88 |     const toggleEye = page.locator('.toggle-password, .fa-eye, #showPasswordBtn');
  89 |     if (await toggleEye.count() > 0) {
  90 |       await toggleEye.click();
  91 |       // Xác nhận mật khẩu được hiển thị rõ ràng (type="text")
  92 |       await expect(passwordInput).toHaveAttribute('type', 'text');
  93 |     }
  94 |   });
  95 | });
  96 | 
```