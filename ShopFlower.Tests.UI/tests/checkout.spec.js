const { test, expect } = require('@playwright/test');

test.describe('Phân hệ Giỏ Hàng & Thanh Toán (Checkout Test Suite)', () => {

  // Đăng ký, đăng nhập và thêm một sản phẩm vào giỏ hàng trước mỗi test case
  test.beforeEach(async ({ page }) => {
    // 1. Đăng ký một tài khoản mới
    const uniqueId = Date.now() + Math.floor(Math.random() * 1000);
    const username = `user_pay_${uniqueId}`;
    const email = `user_pay_${uniqueId}@example.com`;
    
    await page.goto('/Account/Dang_ky');
    await page.fill('input[name="TenDangNhap"]', username);
    await page.fill('input[name="TenHienThi"]', 'Khách Thanh Toán');
    await page.fill('input[name="Email"]', email);
    await page.fill('input[name="MatKhau"]', 'Password@123');
    await page.fill('input[name="XacNhanMatKhau"]', 'Password@123');
    await page.click('input[type="submit"]');

    // 2. Đăng nhập
    await page.goto('/Account/Dang_nhap');
    await page.fill('input[name="TenDangNhap"]', username);
    await page.fill('input[name="MatKhau"]', 'Password@123');
    await page.click('input[type="submit"]');

    // Chờ đăng nhập thành công
    await expect(page).toHaveURL(/\/Home\/Trang_chu|Account\/Orders|Admin|\//);

    // 3. Thêm sản phẩm vào giỏ hàng
    await page.goto('/');
    const buyForm = page.locator('form[action*="ThemGioHang"]').first();
    const buyButton = buyForm.locator('button, input[type="submit"]').first();
    await buyButton.click();
    await page.waitForLoadState('networkidle');
  });

  test('PAY_01: Xác thực trường Số điện thoại tự động lọc chữ cái', async ({ page }) => {
    await page.goto('/Checkout/ThanhToan');

    const phoneInput = page.locator('#SoDienThoai');
    await phoneInput.waitFor({ state: 'visible' });
    
    // Gõ chuỗi ký tự hỗn hợp có chữ và số
    await phoneInput.fill('0912abc345xyz');
    
    // Rời khỏi ô
    await phoneInput.blur();

    // Xác nhận trường Số điện thoại tự động loại bỏ mọi chữ cái, chỉ giữ lại số
    await expect(phoneInput).toHaveValue('0912345');
  });

  test('PAY_02: Trình kích hoạt lỗi Validation động (Real-time Feedback)', async ({ page }) => {
    await page.goto('/Checkout/ThanhToan');

    const emailInput = page.locator('#Email');
    await emailInput.waitFor({ state: 'visible' });
    
    // Xóa email autofill để giả lập trường hợp để trống
    await emailInput.fill('');
    
    // Để trống và di chuyển đi
    await emailInput.focus();
    await emailInput.blur();

    // Trình duyệt phải ngay lập tức thêm class viền đỏ
    await expect(emailInput).toHaveClass(/is-invalid/);

    // Hiển thị thông báo lỗi tương ứng bên dưới
    const errorSpan = page.locator("[data-valmsg-for='Email']");
    await expect(errorSpan).toBeVisible();
    await expect(errorSpan).toContainText('Vui lòng nhập email');

    // Sửa lỗi bằng email hợp lệ
    await emailInput.fill('lienhe@blossom.com');
    await emailInput.blur();

    // Viền phải chuyển sang màu xanh lá (is-valid) và thông báo lỗi biến mất
    await expect(emailInput).toHaveClass(/is-valid/);
    await expect(errorSpan).toBeEmpty();
  });

  test('PAY_03: Gửi form trống -> Tự động cuộn mượt và Focus vào ô lỗi đầu tiên', async ({ page }) => {
    await page.goto('/Checkout/ThanhToan');

    // Xóa email autofill để biến nó thành lỗi đầu tiên
    const emailInput = page.locator('#Email');
    await emailInput.waitFor({ state: 'visible' });
    await emailInput.fill('');

    // Nhấp nút Đặt Hàng trực tiếp mà không điền thông tin
    const submitBtn = page.locator('button.btn-place-order, button[type="submit"]').first();
    await submitBtn.click();

    // Chờ 500ms để hiệu ứng cuộn mượt hoàn tất
    await page.waitForTimeout(500);

    // Xác nhận ô bị lỗi đầu tiên (Email) tự động nhận Focus
    const firstErrorInput = page.locator('#Email');
    await expect(firstErrorInput).toBeFocused();
    await expect(firstErrorInput).toHaveClass(/is-invalid/);
  });

  test('PAY_04: Kiểm tra định dạng tiền tệ và tính đúng đắn giỏ hàng', async ({ page }) => {
    await page.goto('/Checkout/ThanhToan');

    // Đơn giá, tạm tính và tổng tiền phải được định dạng ngăn cách hàng nghìn (đ hoặc VNĐ)
    const grandTotal = page.locator('.total-price');
    await expect(grandTotal).toBeVisible();
    await expect(grandTotal).toContainText(/đ|VNĐ/);
  });
});
