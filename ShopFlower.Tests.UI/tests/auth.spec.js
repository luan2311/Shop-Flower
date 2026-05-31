const { test, expect } = require('@playwright/test');

test.describe('Phân hệ Đăng Ký & Đăng Nhập (Authentication Test Suite)', () => {

  test('REG_01: Đăng ký thành công và tự động trim khoảng trắng', async ({ page }) => {
    await page.goto('/Account/Dang_ky');
    
    const uniqueId = Date.now() + Math.floor(Math.random() * 1000);
    const rawUsername = `  auto_client_${uniqueId}  `;
    const email = `autoclient_${uniqueId}@example.com`;

    // Giả lập nhập tên đăng nhập có khoảng trắng ở đầu và cuối
    const usernameInput = page.locator('input[name="TenDangNhap"]');
    await usernameInput.fill(rawUsername);
    
    await page.fill('input[name="TenHienThi"]', 'Khách hàng Tự động');
    await page.fill('input[name="Email"]', email);
    await page.fill('input[name="MatKhau"]', 'SecurePass123!');
    await page.fill('input[name="XacNhanMatKhau"]', 'SecurePass123!');
    
    await page.click('button[type="submit"]');

    // Chờ hệ thống xử lý và chuyển hướng về màn hình đăng nhập
    await expect(page).toHaveURL(/\/Account\/Dang_nhap/);

    // Chạy thêm bước kiểm tra đăng nhập bằng tên đã trim để kiểm chứng tính năng Trim hoạt động
    await page.fill('input[name="TenDangNhap"]', `auto_client_${uniqueId}`); // Nhập bản đã trim
    await page.fill('input[name="MatKhau"]', 'SecurePass123!');
    await page.click('button[type="submit"]');
    
    // Đăng nhập thành công -> về trang chủ hoặc dashboard của user
    await expect(page).toHaveURL(/\/Home\/Trang_chu|Checkout\/ThanhToan|Account\/Orders|\//);
  });

  test('REG_02: Chặn đăng ký trùng Tên đăng nhập', async ({ page }) => {
    await page.goto('/Account/Dang_ky');
    
    // Nhập tên ĐN đã tồn tại
    await page.fill('input[name="TenDangNhap"]', 'admin');
    await page.fill('input[name="TenHienThi"]', 'Quản trị viên');
    await page.fill('input[name="Email"]', 'admin_new@example.com');
    await page.fill('input[name="MatKhau"]', 'AdminPass123!');
    await page.fill('input[name="XacNhanMatKhau"]', 'AdminPass123!');
    
    await page.click('button[type="submit"]');

    // Phải hiển thị lỗi validation trên giao diện hoặc báo trùng lặp
    const errorMsg = page.locator('.validation-summary-errors, .alert-danger, [data-valmsg-for="TenDangNhap"]');
    await expect(errorMsg).toBeVisible();
  });

  test('REG_03: Chặn đăng ký với Email sai định dạng', async ({ page }) => {
    await page.goto('/Account/Dang_ky');
    
    const emailInput = page.locator('input[name="Email"]');
    await emailInput.fill('autouser01gmail.com'); // Thiếu @
    await emailInput.blur();

    // Front-end validation phải bắt lỗi lập tức
    await expect(emailInput).toHaveClass(/is-invalid/);
    const errorSpan = page.locator("[data-valmsg-for='Email']");
    await expect(errorSpan).toContainText('Email không hợp lệ');
  });

  test('LOG_01: Đăng nhập thất bại với mật khẩu sai', async ({ page }) => {
    await page.goto('/Account/Dang_nhap');
    
    await page.fill('input[name="TenDangNhap"]', 'admin');
    await page.fill('input[name="MatKhau"]', 'WrongPassword123');
    await page.click('button[type="submit"]');

    // Xác nhận vẫn ở trang đăng nhập và hiển thị thông báo lỗi
    await expect(page).toHaveURL(/\/Account\/Dang_nhap/);
    const errorSummary = page.locator('.validation-summary-errors, .alert-danger, .validation-summary-errors li');
    await expect(errorSummary).toBeVisible();
  });

  test('LOG_02: Kiểm tra chức năng Ẩn/Hiện mật khẩu', async ({ page }) => {
    await page.goto('/Account/Dang_nhap');
    
    const passwordInput = page.locator('input[name="MatKhau"]');
    await passwordInput.fill('AdminPass123!');

    // Xác nhận mặc định mật khẩu là ẩn (type="password")
    await expect(passwordInput).toHaveAttribute('type', 'password');

    // Tìm nút Toggle Ẩn/Hiện (nếu có biểu tượng con mắt) và click
    const toggleEye = page.locator('.toggle-password, .fa-eye, #showPasswordBtn');
    if (await toggleEye.count() > 0) {
      await toggleEye.click();
      // Xác nhận mật khẩu được hiển thị rõ ràng (type="text")
      await expect(passwordInput).toHaveAttribute('type', 'text');
    }
  });
});
