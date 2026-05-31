# Instructions

- Following Playwright test failed.
- Explain why, be concise, respect Playwright best practices.
- Provide a snippet of code with the fix, if possible.

# Test info

- Name: checkout.spec.js >> Phân hệ Giỏ Hàng & Thanh Toán (Checkout Test Suite) >> PAY_02: Trình kích hoạt lỗi Validation động (Real-time Feedback)
- Location: tests\checkout.spec.js:53:3

# Error details

```
Error: page.goto: net::ERR_CONNECTION_REFUSED at https://localhost:44357/Account/Dang_ky
Call log:
  - navigating to "https://localhost:44357/Account/Dang_ky", waiting until "load"

```

# Test source

```ts
  1   | const { test, expect } = require('@playwright/test');
  2   | 
  3   | test.describe('Phân hệ Giỏ Hàng & Thanh Toán (Checkout Test Suite)', () => {
  4   | 
  5   |   // Đăng ký, đăng nhập và thêm một sản phẩm vào giỏ hàng trước mỗi test case
  6   |   test.beforeEach(async ({ page }) => {
  7   |     // 1. Đăng ký một tài khoản mới
  8   |     const uniqueId = Date.now() + Math.floor(Math.random() * 1000);
  9   |     const username = `user_pay_${uniqueId}`;
  10  |     const email = `user_pay_${uniqueId}@example.com`;
  11  |     
> 12  |     await page.goto('/Account/Dang_ky');
      |                ^ Error: page.goto: net::ERR_CONNECTION_REFUSED at https://localhost:44357/Account/Dang_ky
  13  |     await page.fill('input[name="TenDangNhap"]', username);
  14  |     await page.fill('input[name="TenHienThi"]', 'Khách Thanh Toán');
  15  |     await page.fill('input[name="Email"]', email);
  16  |     await page.fill('input[name="MatKhau"]', 'Password@123');
  17  |     await page.fill('input[name="XacNhanMatKhau"]', 'Password@123');
  18  |     await page.click('input[type="submit"]');
  19  | 
  20  |     // 2. Đăng nhập
  21  |     await page.goto('/Account/Dang_nhap');
  22  |     await page.fill('input[name="TenDangNhap"]', username);
  23  |     await page.fill('input[name="MatKhau"]', 'Password@123');
  24  |     await page.click('input[type="submit"]');
  25  | 
  26  |     // Chờ đăng nhập thành công
  27  |     await expect(page).toHaveURL(/\/Home\/Trang_chu|Account\/Orders|Admin|\//);
  28  | 
  29  |     // 3. Thêm sản phẩm vào giỏ hàng
  30  |     await page.goto('/');
  31  |     const buyForm = page.locator('form[action*="ThemGioHang"]').first();
  32  |     const buyButton = buyForm.locator('button, input[type="submit"]').first();
  33  |     await buyButton.click();
  34  |     await page.waitForLoadState('networkidle');
  35  |   });
  36  | 
  37  |   test('PAY_01: Xác thực trường Số điện thoại tự động lọc chữ cái', async ({ page }) => {
  38  |     await page.goto('/Checkout/ThanhToan');
  39  | 
  40  |     const phoneInput = page.locator('#SoDienThoai');
  41  |     await phoneInput.waitFor({ state: 'visible' });
  42  |     
  43  |     // Gõ chuỗi ký tự hỗn hợp có chữ và số
  44  |     await phoneInput.fill('0912abc345xyz');
  45  |     
  46  |     // Rời khỏi ô
  47  |     await phoneInput.blur();
  48  | 
  49  |     // Xác nhận trường Số điện thoại tự động loại bỏ mọi chữ cái, chỉ giữ lại số
  50  |     await expect(phoneInput).toHaveValue('0912345');
  51  |   });
  52  | 
  53  |   test('PAY_02: Trình kích hoạt lỗi Validation động (Real-time Feedback)', async ({ page }) => {
  54  |     await page.goto('/Checkout/ThanhToan');
  55  | 
  56  |     const emailInput = page.locator('#Email');
  57  |     await emailInput.waitFor({ state: 'visible' });
  58  |     
  59  |     // Xóa email autofill để giả lập trường hợp để trống
  60  |     await emailInput.fill('');
  61  |     
  62  |     // Để trống và di chuyển đi
  63  |     await emailInput.focus();
  64  |     await emailInput.blur();
  65  | 
  66  |     // Trình duyệt phải ngay lập tức thêm class viền đỏ
  67  |     await expect(emailInput).toHaveClass(/is-invalid/);
  68  | 
  69  |     // Hiển thị thông báo lỗi tương ứng bên dưới
  70  |     const errorSpan = page.locator("[data-valmsg-for='Email']");
  71  |     await expect(errorSpan).toBeVisible();
  72  |     await expect(errorSpan).toContainText('Vui lòng nhập email');
  73  | 
  74  |     // Sửa lỗi bằng email hợp lệ
  75  |     await emailInput.fill('lienhe@blossom.com');
  76  |     await emailInput.blur();
  77  | 
  78  |     // Viền phải chuyển sang màu xanh lá (is-valid) và thông báo lỗi biến mất
  79  |     await expect(emailInput).toHaveClass(/is-valid/);
  80  |     await expect(errorSpan).toBeEmpty();
  81  |   });
  82  | 
  83  |   test('PAY_03: Gửi form trống -> Tự động cuộn mượt và Focus vào ô lỗi đầu tiên', async ({ page }) => {
  84  |     await page.goto('/Checkout/ThanhToan');
  85  | 
  86  |     // Xóa email autofill để biến nó thành lỗi đầu tiên
  87  |     const emailInput = page.locator('#Email');
  88  |     await emailInput.waitFor({ state: 'visible' });
  89  |     await emailInput.fill('');
  90  | 
  91  |     // Nhấp nút Đặt Hàng trực tiếp mà không điền thông tin
  92  |     const submitBtn = page.locator('button.btn-place-order, button[type="submit"]').first();
  93  |     await submitBtn.click();
  94  | 
  95  |     // Chờ 500ms để hiệu ứng cuộn mượt hoàn tất
  96  |     await page.waitForTimeout(500);
  97  | 
  98  |     // Xác nhận ô bị lỗi đầu tiên (Email) tự động nhận Focus
  99  |     const firstErrorInput = page.locator('#Email');
  100 |     await expect(firstErrorInput).toBeFocused();
  101 |     await expect(firstErrorInput).toHaveClass(/is-invalid/);
  102 |   });
  103 | 
  104 |   test('PAY_04: Kiểm tra định dạng tiền tệ và tính đúng đắn giỏ hàng', async ({ page }) => {
  105 |     await page.goto('/Checkout/ThanhToan');
  106 | 
  107 |     // Đơn giá, tạm tính và tổng tiền phải được định dạng ngăn cách hàng nghìn (đ hoặc VNĐ)
  108 |     const grandTotal = page.locator('.total-price');
  109 |     await expect(grandTotal).toBeVisible();
  110 |     await expect(grandTotal).toContainText(/đ|VNĐ/);
  111 |   });
  112 | });
```