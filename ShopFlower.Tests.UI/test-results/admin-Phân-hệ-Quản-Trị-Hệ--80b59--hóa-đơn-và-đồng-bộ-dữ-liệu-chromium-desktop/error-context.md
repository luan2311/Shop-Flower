# Instructions

- Following Playwright test failed.
- Explain why, be concise, respect Playwright best practices.
- Provide a snippet of code with the fix, if possible.

# Test info

- Name: admin.spec.js >> Phân hệ Quản Trị Hệ Thống (Admin & Authorization Test Suite) >> ADM_03: Duyệt hóa đơn và đồng bộ dữ liệu
- Location: tests\admin.spec.js:65:3

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
  3   | test.describe('Phân hệ Quản Trị Hệ Thống (Admin & Authorization Test Suite)', () => {
  4   | 
  5   |   test('ADM_01: Chặn truy cập trang Admin đối với tài khoản vai trò User', async ({ page }) => {
  6   |     // 1. Đăng ký một tài khoản mới để đảm bảo tài khoản tồn tại và có vai trò User
  7   |     const uniqueId = Date.now() + Math.floor(Math.random() * 1000);
  8   |     const username = `user_${uniqueId}`;
  9   |     const email = `user_${uniqueId}@example.com`;
  10  |     
  11  |     await page.goto('/Account/Dang_ky');
  12  |     await page.fill('input[name="TenDangNhap"]', username);
  13  |     await page.fill('input[name="TenHienThi"]', 'Khách Hàng Thường');
  14  |     await page.fill('input[name="Email"]', email);
  15  |     await page.fill('input[name="MatKhau"]', 'Password@123');
  16  |     await page.fill('input[name="XacNhanMatKhau"]', 'Password@123');
  17  |     await page.click('input[type="submit"]');
  18  |     await page.waitForURL(/\/Account\/Dang_nhap/);
  19  | 
  20  |     // 2. Đăng nhập bằng tài khoản vừa tạo
  21  |     await page.goto('/Account/Dang_nhap');
  22  |     await page.fill('input[name="TenDangNhap"]', username);
  23  |     await page.fill('input[name="MatKhau"]', 'Password@123');
  24  |     await page.click('input[type="submit"]');
  25  | 
  26  |     // Chờ đăng nhập thành công và không còn ở trang đăng nhập nữa
  27  |     await expect(page).not.toHaveURL(/\/Account\/Dang_nhap/);
  28  | 
  29  |     // 3. Cố tình truy cập trang Admin Dashboard chuyên dụng (yêu cầu quyền Admin)
  30  |     await page.goto('/Admin/Dashboard/Dashboard');
  31  | 
  32  |     // 4. Hệ thống phải chặn và hiển thị trang báo lỗi không có quyền (Unauthorized)
  33  |     await expect(page).not.toHaveURL(/\/Admin\/Dashboard\/Dashboard/);
  34  |     const unauthMessage = page.locator('.container h2, .unauthorized-text, .alert-warning');
  35  |     await expect(unauthMessage.first()).toContainText(/Không có quyền|Từ chối truy cập|Unauthorized|Lỗi/);
  36  |   });
  37  | 
  38  |   test('ADM_02: CRUD Sản phẩm - Chặn tạo sản phẩm giá bán âm hoặc bằng 0', async ({ page }) => {
  39  |     // 1. Giả lập đăng nhập quyền Admin
  40  |     await page.goto('/Account/Dang_nhap');
  41  |     await page.fill('input[name="TenDangNhap"]', 'admin');
  42  |     await page.fill('input[name="MatKhau"]', '12345');
  43  |     await page.click('input[type="submit"]');
  44  | 
  45  |     // Chờ đăng nhập thành công
  46  |     await expect(page).not.toHaveURL(/\/Account\/Dang_nhap/);
  47  | 
  48  |     // 2. Truy cập trang thêm sản phẩm
  49  |     await page.goto('/Admin/Dashboard/CreateSanPham');
  50  | 
  51  |     // 3. Nhập giá bán bằng 0 hoặc số âm
  52  |     await page.fill('input[name="TenSP"]', 'Bó hoa tươi mới');
  53  |     await page.fill('input[name="GiaBan"]', '-150000'); // Giá âm
  54  |     
  55  |     // 4. Click nút submit
  56  |     await page.click('input[type="submit"]');
  57  | 
  58  |     // 5. Xác nhận trình duyệt ngăn cản form gửi đi (vẫn ở trang Create) và trường GiaBan bị đánh dấu lỗi
  59  |     await expect(page).toHaveURL(/\/Admin\/Dashboard\/CreateSanPham/);
  60  |     
  61  |     const isInvalid = await page.$eval('input[name="GiaBan"]', el => el.validity.valid === false);
  62  |     expect(isInvalid).toBe(true);
  63  |   });
  64  | 
  65  |   test('ADM_03: Duyệt hóa đơn và đồng bộ dữ liệu', async ({ page }) => {
  66  |     // 1. Đăng ký tài khoản khách hàng để mua hàng
  67  |     const uniqueId = Date.now() + Math.floor(Math.random() * 1000);
  68  |     const username = `buyer_${uniqueId}`;
  69  |     const email = `buyer_${uniqueId}@example.com`;
  70  | 
> 71  |     await page.goto('/Account/Dang_ky');
      |                ^ Error: page.goto: net::ERR_CONNECTION_REFUSED at https://localhost:44357/Account/Dang_ky
  72  |     await page.fill('input[name="TenDangNhap"]', username);
  73  |     await page.fill('input[name="TenHienThi"]', 'Khách Đặt Hàng');
  74  |     await page.fill('input[name="Email"]', email);
  75  |     await page.fill('input[name="MatKhau"]', 'Password@123');
  76  |     await page.fill('input[name="XacNhanMatKhau"]', 'Password@123');
  77  |     await page.click('input[type="submit"]');
  78  |     await page.waitForURL(/\/Account\/Dang_nhap/);
  79  | 
  80  |     // Đăng nhập
  81  |     await page.goto('/Account/Dang_nhap');
  82  |     await page.fill('input[name="TenDangNhap"]', username);
  83  |     await page.fill('input[name="MatKhau"]', 'Password@123');
  84  |     await page.click('input[type="submit"]');
  85  |     await expect(page).not.toHaveURL(/\/Account\/Dang_nhap/);
  86  | 
  87  |     // 2. Thêm sản phẩm vào giỏ hàng
  88  |     await page.goto('/');
  89  |     const buyForm = page.locator('form[action*="ThemGioHang"]').first();
  90  |     const buyButton = buyForm.locator('button, input[type="submit"]').first();
  91  |     await buyButton.click();
  92  |     await page.waitForLoadState('networkidle');
  93  | 
  94  |     // 3. Tiến hành thanh toán
  95  |     await page.goto('/Checkout/ThanhToan');
  96  |     
  97  |     // Email đã được autofill do đã đăng nhập, ta điền các trường còn lại
  98  |     await page.fill('#HoTenNguoiNhan', 'Người Mua Thử Nghiệm');
  99  |     await page.fill('#SoDienThoai', '0912345678');
  100 |     await page.fill('#DiaChiGiaoHang', '123 Phố Lán');
  101 |     
  102 |     // Điền autocomplete
  103 |     await page.fill('#TinhThanh', 'TP Hồ Chí Minh');
  104 |     await page.fill('#QuanHuyen', 'Quận 7');
  105 |     await page.fill('#PhuongXa', 'Tân Phong');
  106 |     
  107 |     // Chọn phương thức COD
  108 |     await page.click('#cod');
  109 | 
  110 |     // Submit Đặt Hàng
  111 |     const placeOrderBtn = page.locator('button.btn-place-order, button[type="submit"]').first();
  112 |     await placeOrderBtn.click();
  113 |     await page.waitForLoadState('networkidle');
  114 | 
  115 |     // Đăng xuất tài khoản khách hàng bằng cách xóa cookies
  116 |     await page.context().clearCookies();
  117 |     await page.goto('/Account/Dang_nhap');
  118 | 
  119 |     // 4. Đăng nhập quyền Admin
  120 |     await page.fill('input[name="TenDangNhap"]', 'admin');
  121 |     await page.fill('input[name="MatKhau"]', '12345');
  122 |     await page.click('input[type="submit"]');
  123 |     await expect(page).not.toHaveURL(/\/Account\/Dang_nhap/);
  124 | 
  125 |     // 5. Vào trang danh sách hóa đơn
  126 |     await page.goto('/Admin/Dashboard/QL_HoaDon');
  127 |     
  128 |     // 6. Click nút "Chi tiết" hóa đơn đầu tiên (chắc chắn có ít nhất 1 hóa đơn vừa tạo)
  129 |     const detailsLink = page.locator('a:has-text("Chi tiết")').first();
  130 |     await detailsLink.click();
  131 | 
  132 |     // 7. Xác nhận trạng thái hóa đơn hiển thị chuẩn
  133 |     await expect(page).toHaveURL(/\/Admin\/Dashboard\/DetailsHoaDon/);
  134 |     const orderStatusLabel = page.locator('h2, .order-status, #TrangThai, .card-title, h3').first();
  135 |     await expect(orderStatusLabel).toBeVisible();
  136 |   });
  137 | });
  138 | 
```