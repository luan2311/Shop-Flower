# Instructions

- Following Playwright test failed.
- Explain why, be concise, respect Playwright best practices.
- Provide a snippet of code with the fix, if possible.

# Test info

- Name: checkout.spec.js >> Phân hệ Giỏ Hàng & Thanh Toán (Checkout Test Suite) >> PAY_03: Gửi form trống -> Tự động cuộn mượt và Focus vào ô lỗi đầu tiên
- Location: tests\checkout.spec.js:83:3

# Error details

```
Test timeout of 30000ms exceeded.
```

```
Error: locator.waitFor: Test timeout of 30000ms exceeded.
Call log:
  - waiting for locator('#Email') to be visible

```

# Page snapshot

```yaml
- generic [active] [ref=e1]:
  - banner [ref=e2]:
    - generic [ref=e6]:
      - link "Góc Hoa Xinh" [ref=e8]:
        - /url: /
        - img "Góc Hoa Xinh" [ref=e9]
      - search [ref=e12]:
        - textbox "Tìm sản phẩm..." [ref=e13]
        - generic: product
        - button "Tìm kiếm" [ref=e14] [cursor=pointer]:
          - img [ref=e15]
      - generic [ref=e17]:
        - 'generic "Điện thoại: 0898449950" [ref=e19]':
          - img [ref=e20]
          - generic [ref=e26]:
            - generic [ref=e27]: Gọi mua hàng
            - text: "0898449950"
        - link "Tài khoản" [ref=e29]:
          - /url: /Account/Dang_nhap
          - img [ref=e30]
          - generic [ref=e32]:
            - generic [ref=e33]: Thông tin
            - generic [ref=e34]:
              - text: Tài khoản
              - img [ref=e35]
        - link "Giỏ hàng" [ref=e38]:
          - /url: /Cart/Cart
          - img [ref=e40]
          - generic [ref=e42]: "1"
          - text: Giỏ hàng
    - list [ref=e47]:
      - listitem [ref=e48]:
        - link "Trang chủ" [ref=e49]:
          - /url: /
      - listitem [ref=e50]:
        - link "Tất cả sản phẩm" [ref=e51]:
          - /url: /SanPham/tat_ca_san_pham
      - listitem [ref=e52]:
        - link "Bó Hoa Tươi" [ref=e53]:
          - /url: /SanPham/bo_hoa_tuoi
      - listitem [ref=e54]:
        - link "Kệ Hoa chúc mừng" [ref=e55]:
          - /url: /SanPham/ke_hoa_chuc_mung
          - text: Kệ Hoa chúc mừng
          - img [ref=e56]
      - listitem [ref=e57]:
        - link "Hoa Cưới" [ref=e58]:
          - /url: /SanPham/hoa_cuoi
      - listitem [ref=e59]:
        - link "Hoa Sáp" [ref=e60]:
          - /url: /SanPham/hoa_sap
      - listitem [ref=e61]:
        - link "Tin Tức" [ref=e62]:
          - /url: /Home/Tin_tuc
      - listitem [ref=e63]:
        - link "Liên Hệ" [ref=e64]:
          - /url: /Home/Lien_he
  - generic [ref=e73]:
    - heading "Đăng nhập" [level=1] [ref=e74]
    - generic [ref=e76]:
      - generic: lvunA4OHWkTcR7LHuXs6PM0GyCw7zdz6vjiLq-3u6TYaiGQ11zFWlygEAiRG7ngi_D07cCoLY2XcNn4JCqe0nkfr7iOtfC4Dgivjr14wXFI1
      - generic: /Checkout/ThanhToan
      - generic [ref=e77]:
        - group [ref=e78]:
          - textbox "Tên đăng nhập" [ref=e79]
        - group [ref=e80]:
          - textbox "Mật khẩu" [ref=e81]
        - generic [ref=e82]:
          - button "ĐĂNG NHẬP" [ref=e83] [cursor=pointer]
          - button [ref=e84] [cursor=pointer]
        - generic:
          - generic:
            - generic [ref=e85] [cursor=pointer]: Quên mật khẩu?
            - link "Đăng ký tại đây" [ref=e86]:
              - /url: /Account/Dang_ky
  - contentinfo [ref=e87]:
    - generic [ref=e90]:
      - generic [ref=e91]:
        - link "Góc Hoa Xinh" [ref=e93]:
          - /url: /
          - img "Góc Hoa Xinh" [ref=e94]
        - generic [ref=e95]:
          - generic [ref=e97]: "Địa chỉ: 140 Lê Trọng Tấn, Quận Tân Phú"
          - generic [ref=e99]: "Điện thoại: 0898449950"
          - generic [ref=e101]: "Email: 2001230486@huit.edu.vn"
      - generic [ref=e102]:
        - heading "Menu" [level=4] [ref=e103]
        - list [ref=e104]:
          - listitem [ref=e105]:
            - link "Trang chủ" [ref=e106]:
              - /url: /
          - listitem [ref=e107]:
            - link "Tất cả sản phẩm" [ref=e108]:
              - /url: /SanPham/tat_ca_san_pham
          - listitem [ref=e109]:
            - link "Bó Hoa Tươi" [ref=e110]:
              - /url: /SanPham/bo_hoa_tuoi
          - listitem [ref=e111]:
            - link "Kệ Hoa chúc mừng" [ref=e112]:
              - /url: /SanPham/ke_hoa_chuc_mung
          - listitem [ref=e113]:
            - link "Hoa Cưới" [ref=e114]:
              - /url: /SanPham/hoa_cuoi
          - listitem [ref=e115]:
            - link "Hoa Sáp" [ref=e116]:
              - /url: /SanPham/hoa_sap
          - listitem [ref=e117]:
            - link "Tin Tức" [ref=e118]:
              - /url: /Home/Tin_tuc
          - listitem [ref=e119]:
            - link "Liên Hệ" [ref=e120]:
              - /url: /Home/Lien_he
      - generic [ref=e121]:
        - heading "Chính sách" [level=4] [ref=e122]
        - list [ref=e123]:
          - listitem [ref=e124]:
            - link "Hình Thức Thanh Toán" [ref=e125]:
              - /url: /Chinh_Sach_/Hinh_thuc_thanh_toan
          - listitem [ref=e126]:
            - link "Chính sách giao hàng" [ref=e127]:
              - /url: /Chinh_Sach_/Giao_hang
          - listitem [ref=e128]:
            - link "Chính sách bảo mật thông tin" [ref=e129]:
              - /url: /Chinh_Sach_/Bao_mat
          - listitem [ref=e130]:
            - link "Chính sách bảo hành" [ref=e131]:
              - /url: /Chinh_Sach_/Bao_hanh
          - listitem [ref=e132]:
            - link "Điều khoản sử dụng" [ref=e133]:
              - /url: /Chinh_Sach_/Dieu_khoan_su_dung
      - generic [ref=e134]:
        - heading "Thành Viên thực hiện" [level=4] [ref=e135]
        - list [ref=e136]:
          - listitem [ref=e137]:
            - generic [ref=e138]: Phạm Nguyễn Minh Luân
          - listitem [ref=e139]:
            - generic [ref=e140]: Phùng Tuấn Huy
          - listitem [ref=e141]:
            - generic [ref=e142]: Trần Mạnh Toàn
          - listitem [ref=e143]:
            - generic [ref=e144]: Trần Nguyễn Bảo An
          - listitem [ref=e145]:
            - generic [ref=e146]: Phạm Văn Tú
    - link "Lên đầu trang" [ref=e147]:
      - /url: "#"
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
  12  |     await page.goto('/Account/Dang_ky');
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
> 88  |     await emailInput.waitFor({ state: 'visible' });
      |                      ^ Error: locator.waitFor: Test timeout of 30000ms exceeded.
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
  113 | 
```