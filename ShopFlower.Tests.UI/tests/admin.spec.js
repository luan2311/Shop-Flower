const { test, expect } = require('@playwright/test');

test.describe('Phân hệ Quản Trị Hệ Thống (Admin & Authorization Test Suite)', () => {

  test('ADM_01: Chặn truy cập trang Admin đối với tài khoản vai trò User', async ({ page }) => {
    // 1. Đăng ký một tài khoản mới để đảm bảo tài khoản tồn tại và có vai trò User
    const uniqueId = Date.now() + Math.floor(Math.random() * 1000);
    const username = `user_${uniqueId}`;
    const email = `user_${uniqueId}@example.com`;
    
    await page.goto('/Account/Dang_ky');
    await page.fill('input[name="TenDangNhap"]', username);
    await page.fill('input[name="TenHienThi"]', 'Khách Hàng Thường');
    await page.fill('input[name="Email"]', email);
    await page.fill('input[name="MatKhau"]', 'Password@123');
    await page.fill('input[name="XacNhanMatKhau"]', 'Password@123');
    await page.click('input[type="submit"]');
    await page.waitForURL(/\/Account\/Dang_nhap/);

    // 2. Đăng nhập bằng tài khoản vừa tạo
    await page.goto('/Account/Dang_nhap');
    await page.fill('input[name="TenDangNhap"]', username);
    await page.fill('input[name="MatKhau"]', 'Password@123');
    await page.click('input[type="submit"]');

    // Chờ đăng nhập thành công và không còn ở trang đăng nhập nữa
    await expect(page).not.toHaveURL(/\/Account\/Dang_nhap/);

    // 3. Cố tình truy cập trang Admin Dashboard chuyên dụng (yêu cầu quyền Admin)
    await page.goto('/Admin/Dashboard/Dashboard');

    // 4. Hệ thống phải chặn và hiển thị trang báo lỗi không có quyền (Unauthorized)
    await expect(page).not.toHaveURL(/\/Admin\/Dashboard\/Dashboard/);
    const unauthMessage = page.locator('.container h2, .unauthorized-text, .alert-warning');
    await expect(unauthMessage.first()).toContainText(/Không có quyền|Từ chối truy cập|Unauthorized|Lỗi/);
  });

  test('ADM_02: CRUD Sản phẩm - Chặn tạo sản phẩm giá bán âm hoặc bằng 0', async ({ page }) => {
    // 1. Giả lập đăng nhập quyền Admin
    await page.goto('/Account/Dang_nhap');
    await page.fill('input[name="TenDangNhap"]', 'admin');
    await page.fill('input[name="MatKhau"]', '12345');
    await page.click('input[type="submit"]');

    // Chờ đăng nhập thành công
    await expect(page).not.toHaveURL(/\/Account\/Dang_nhap/);

    // 2. Truy cập trang thêm sản phẩm
    await page.goto('/Admin/Dashboard/CreateSanPham');

    // 3. Nhập giá bán bằng 0 hoặc số âm
    await page.fill('input[name="TenSP"]', 'Bó hoa tươi mới');
    await page.fill('input[name="GiaBan"]', '-150000'); // Giá âm
    
    // 4. Click nút submit
    await page.click('input[type="submit"]');

    // 5. Xác nhận trình duyệt ngăn cản form gửi đi (vẫn ở trang Create) và trường GiaBan bị đánh dấu lỗi
    await expect(page).toHaveURL(/\/Admin\/Dashboard\/CreateSanPham/);
    
    const isInvalid = await page.$eval('input[name="GiaBan"]', el => el.validity.valid === false);
    expect(isInvalid).toBe(true);
  });

  test('ADM_03: Duyệt hóa đơn và đồng bộ dữ liệu', async ({ page }) => {
    // 1. Đăng ký tài khoản khách hàng để mua hàng
    const uniqueId = Date.now() + Math.floor(Math.random() * 1000);
    const username = `buyer_${uniqueId}`;
    const email = `buyer_${uniqueId}@example.com`;

    await page.goto('/Account/Dang_ky');
    await page.fill('input[name="TenDangNhap"]', username);
    await page.fill('input[name="TenHienThi"]', 'Khách Đặt Hàng');
    await page.fill('input[name="Email"]', email);
    await page.fill('input[name="MatKhau"]', 'Password@123');
    await page.fill('input[name="XacNhanMatKhau"]', 'Password@123');
    await page.click('input[type="submit"]');
    await page.waitForURL(/\/Account\/Dang_nhap/);

    // Đăng nhập
    await page.goto('/Account/Dang_nhap');
    await page.fill('input[name="TenDangNhap"]', username);
    await page.fill('input[name="MatKhau"]', 'Password@123');
    await page.click('input[type="submit"]');
    await expect(page).not.toHaveURL(/\/Account\/Dang_nhap/);

    // 2. Thêm sản phẩm vào giỏ hàng
    await page.goto('/');
    const buyForm = page.locator('form[action*="ThemGioHang"]').first();
    const buyButton = buyForm.locator('button, input[type="submit"]').first();
    await buyButton.click();
    await page.waitForLoadState('networkidle');

    // 3. Tiến hành thanh toán
    await page.goto('/Checkout/ThanhToan');
    
    // Email đã được autofill do đã đăng nhập, ta điền các trường còn lại
    await page.fill('#HoTenNguoiNhan', 'Người Mua Thử Nghiệm');
    await page.fill('#SoDienThoai', '0912345678');
    await page.fill('#DiaChiGiaoHang', '123 Phố Lán');
    
    // Điền autocomplete
    await page.fill('#TinhThanh', 'TP Hồ Chí Minh');
    await page.fill('#QuanHuyen', 'Quận 7');
    await page.fill('#PhuongXa', 'Tân Phong');
    
    // Chọn phương thức COD
    await page.click('#cod');

    // Submit Đặt Hàng
    const placeOrderBtn = page.locator('button.btn-place-order, button[type="submit"]').first();
    await placeOrderBtn.click();
    await page.waitForLoadState('networkidle');

    // Đăng xuất tài khoản khách hàng bằng cách xóa cookies
    await page.context().clearCookies();
    await page.goto('/Account/Dang_nhap');

    // 4. Đăng nhập quyền Admin
    await page.fill('input[name="TenDangNhap"]', 'admin');
    await page.fill('input[name="MatKhau"]', '12345');
    await page.click('input[type="submit"]');
    await expect(page).not.toHaveURL(/\/Account\/Dang_nhap/);

    // 5. Vào trang danh sách hóa đơn
    await page.goto('/Admin/Dashboard/QL_HoaDon');
    
    // 6. Click nút "Chi tiết" hóa đơn đầu tiên (chắc chắn có ít nhất 1 hóa đơn vừa tạo)
    const detailsLink = page.locator('a:has-text("Chi tiết")').first();
    await detailsLink.click();

    // 7. Xác nhận trạng thái hóa đơn hiển thị chuẩn
    await expect(page).toHaveURL(/\/Admin\/Dashboard\/DetailsHoaDon/);
    const orderStatusLabel = page.locator('h2, .order-status, #TrangThai, .card-title, h3').first();
    await expect(orderStatusLabel).toBeVisible();
  });
});
