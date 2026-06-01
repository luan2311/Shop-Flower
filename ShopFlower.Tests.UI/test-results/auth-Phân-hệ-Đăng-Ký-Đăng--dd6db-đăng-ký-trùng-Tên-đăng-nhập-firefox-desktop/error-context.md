# Instructions

- Following Playwright test failed.
- Explain why, be concise, respect Playwright best practices.
- Provide a snippet of code with the fix, if possible.

# Test info

- Name: auth.spec.js >> Phân hệ Đăng Ký & Đăng Nhập (Authentication Test Suite) >> REG_02: Chặn đăng ký trùng Tên đăng nhập
- Location: tests\auth.spec.js:35:3

# Error details

```
Test timeout of 30000ms exceeded.
```

```
Error: page.click: Test timeout of 30000ms exceeded.
Call log:
  - waiting for locator('button[type="submit"]')
    - locator resolved to <button type="submit" tabindex="-1"></button>
  - attempting click action
    2 × waiting for element to be visible, enabled and stable
      - element is not stable
    - retrying click action
    - waiting 20ms
    - waiting for element to be visible, enabled and stable
    - element is not stable
  - retrying click action
    - waiting 100ms
    - waiting for element to be visible, enabled and stable
    - element is visible, enabled and stable
    - scrolling into view if needed
    - done scrolling
    - <div class="wpx">…</div> intercepts pointer events
  - retrying click action
    - waiting 100ms
    - waiting for element to be visible, enabled and stable
    - element is not stable
  15 × retrying click action
       - waiting 500ms
       - waiting for element to be visible, enabled and stable
       - element is visible, enabled and stable
       - scrolling into view if needed
       - done scrolling
       - <div class="wpx">…</div> intercepts pointer events
  - retrying click action
    - waiting 500ms
    - waiting for element to be visible, enabled and stable
    - element is not stable
  - retrying click action
    - waiting 500ms
    - waiting for element to be visible, enabled and stable
    - element is visible, enabled and stable
    - scrolling into view if needed
    - done scrolling
    - <div class="wpx">…</div> intercepts pointer events
  - retrying click action
    - waiting 500ms

```

# Page snapshot

```yaml
- generic [ref=e1]:
  - banner [ref=e2]:
    - generic [ref=e6]:
      - link "Góc Hoa Xinh" [ref=e8] [cursor=pointer]:
        - /url: /
        - img "Góc Hoa Xinh" [ref=e9]
      - search [ref=e12]:
        - textbox "Tìm sản phẩm..." [ref=e13]
        - button "Tìm kiếm" [ref=e14] [cursor=pointer]:
          - img [ref=e15]
      - generic [ref=e17]:
        - 'generic "Điện thoại: 0898449950" [ref=e19]':
          - img [ref=e20]
          - generic [ref=e25]:
            - generic [ref=e26]: Gọi mua hàng
            - text: "0898449950"
        - link "Tài khoản" [ref=e28] [cursor=pointer]:
          - /url: /Account/Dang_nhap
          - img [ref=e29]
          - generic [ref=e31]:
            - generic [ref=e32]: Thông tin
            - generic [ref=e33]:
              - text: Tài khoản
              - img [ref=e34]
        - link "Giỏ hàng" [ref=e37] [cursor=pointer]:
          - /url: /Cart/Cart
          - img [ref=e39]
          - generic [ref=e41]: "0"
          - text: Giỏ hàng
    - list [ref=e46]:
      - listitem [ref=e47]:
        - link "Trang chủ" [ref=e48] [cursor=pointer]:
          - /url: /
      - listitem [ref=e49]:
        - link "Tất cả sản phẩm" [ref=e50] [cursor=pointer]:
          - /url: /SanPham/tat_ca_san_pham
      - listitem [ref=e51]:
        - link "Bó Hoa Tươi" [ref=e52] [cursor=pointer]:
          - /url: /SanPham/bo_hoa_tuoi
      - listitem [ref=e53]:
        - link "Kệ Hoa chúc mừng" [ref=e54] [cursor=pointer]:
          - /url: /SanPham/ke_hoa_chuc_mung
          - text: Kệ Hoa chúc mừng
          - img [ref=e55]
      - listitem [ref=e56]:
        - link "Hoa Cưới" [ref=e57] [cursor=pointer]:
          - /url: /SanPham/hoa_cuoi
      - listitem [ref=e58]:
        - link "Hoa Sáp" [ref=e59] [cursor=pointer]:
          - /url: /SanPham/hoa_sap
      - listitem [ref=e60]:
        - link "Tin Tức" [ref=e61] [cursor=pointer]:
          - /url: /Home/Tin_tuc
      - listitem [ref=e62]:
        - link "Liên Hệ" [ref=e63] [cursor=pointer]:
          - /url: /Home/Lien_he
  - generic [ref=e72]:
    - heading "Đăng ký" [level=1] [ref=e73]
    - generic [ref=e75]:
      - group [ref=e76]:
        - textbox "Tên đăng nhập" [ref=e77]: admin
      - group [ref=e78]:
        - textbox "Email" [ref=e79]: admin_new@example.com
      - group [ref=e80]:
        - textbox "Tên hiển thị (không bắt buộc)" [ref=e81]: Quản trị viên
      - group [ref=e82]:
        - textbox "Mật khẩu" [ref=e83]: AdminPass123!
      - group [ref=e84]:
        - textbox "Xác nhận mật khẩu" [active] [ref=e85]: AdminPass123!
      - generic [ref=e86]:
        - button "ĐĂNG KÝ" [ref=e87] [cursor=pointer]
        - button [ref=e88] [cursor=pointer]
      - generic:
        - generic:
          - generic [ref=e89]: Đã có tài khoản?
          - link "Đăng nhập" [ref=e90] [cursor=pointer]:
            - /url: /Account/Dang_nhap
  - contentinfo [ref=e91]:
    - generic [ref=e94]:
      - generic [ref=e95]:
        - link "Góc Hoa Xinh" [ref=e97] [cursor=pointer]:
          - /url: /
          - img "Góc Hoa Xinh" [ref=e98]
        - generic [ref=e99]:
          - generic [ref=e101]: "Địa chỉ: 140 Lê Trọng Tấn, Quận Tân Phú"
          - generic [ref=e103]: "Điện thoại: 0898449950"
          - generic [ref=e105]: "Email: 2001230486@huit.edu.vn"
      - generic [ref=e106]:
        - heading "Menu" [level=4] [ref=e107]
        - list [ref=e108]:
          - listitem [ref=e109]:
            - link "Trang chủ" [ref=e110] [cursor=pointer]:
              - /url: /
          - listitem [ref=e111]:
            - link "Tất cả sản phẩm" [ref=e112] [cursor=pointer]:
              - /url: /SanPham/tat_ca_san_pham
          - listitem [ref=e113]:
            - link "Bó Hoa Tươi" [ref=e114] [cursor=pointer]:
              - /url: /SanPham/bo_hoa_tuoi
          - listitem [ref=e115]:
            - link "Kệ Hoa chúc mừng" [ref=e116] [cursor=pointer]:
              - /url: /SanPham/ke_hoa_chuc_mung
          - listitem [ref=e117]:
            - link "Hoa Cưới" [ref=e118] [cursor=pointer]:
              - /url: /SanPham/hoa_cuoi
          - listitem [ref=e119]:
            - link "Hoa Sáp" [ref=e120] [cursor=pointer]:
              - /url: /SanPham/hoa_sap
          - listitem [ref=e121]:
            - link "Tin Tức" [ref=e122] [cursor=pointer]:
              - /url: /Home/Tin_tuc
          - listitem [ref=e123]:
            - link "Liên Hệ" [ref=e124] [cursor=pointer]:
              - /url: /Home/Lien_he
      - generic [ref=e125]:
        - heading "Chính sách" [level=4] [ref=e126]
        - list [ref=e127]:
          - listitem [ref=e128]:
            - link "Hình Thức Thanh Toán" [ref=e129] [cursor=pointer]:
              - /url: /Chinh_Sach_/Hinh_thuc_thanh_toan
          - listitem [ref=e130]:
            - link "Chính sách giao hàng" [ref=e131] [cursor=pointer]:
              - /url: /Chinh_Sach_/Giao_hang
          - listitem [ref=e132]:
            - link "Chính sách bảo mật thông tin" [ref=e133] [cursor=pointer]:
              - /url: /Chinh_Sach_/Bao_mat
          - listitem [ref=e134]:
            - link "Chính sách bảo hành" [ref=e135] [cursor=pointer]:
              - /url: /Chinh_Sach_/Bao_hanh
          - listitem [ref=e136]:
            - link "Điều khoản sử dụng" [ref=e137] [cursor=pointer]:
              - /url: /Chinh_Sach_/Dieu_khoan_su_dung
      - generic [ref=e138]:
        - heading "Thành Viên thực hiện" [level=4] [ref=e139]
        - list [ref=e140]:
          - listitem [ref=e141]:
            - generic [ref=e142]: Phạm Nguyễn Minh Luân
          - listitem [ref=e143]:
            - generic [ref=e144]: Phùng Tuấn Huy
          - listitem [ref=e145]:
            - generic [ref=e146]: Trần Mạnh Toàn
          - listitem [ref=e147]:
            - generic [ref=e148]: Trần Nguyễn Bảo An
          - listitem [ref=e149]:
            - generic [ref=e150]: Phạm Văn Tú
    - link "Lên đầu trang" [ref=e151] [cursor=pointer]:
      - /url: "#"
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
  36 |     await page.goto('/Account/Dang_ky');
  37 |     
  38 |     // Nhập tên ĐN đã tồn tại
  39 |     await page.fill('input[name="TenDangNhap"]', 'admin');
  40 |     await page.fill('input[name="TenHienThi"]', 'Quản trị viên');
  41 |     await page.fill('input[name="Email"]', 'admin_new@example.com');
  42 |     await page.fill('input[name="MatKhau"]', 'AdminPass123!');
  43 |     await page.fill('input[name="XacNhanMatKhau"]', 'AdminPass123!');
  44 |     
> 45 |     await page.click('button[type="submit"]');
     |                ^ Error: page.click: Test timeout of 30000ms exceeded.
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