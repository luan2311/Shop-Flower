# Instructions

- Following Playwright test failed.
- Explain why, be concise, respect Playwright best practices.
- Provide a snippet of code with the fix, if possible.

# Test info

- Name: extended-flows.spec.js >> Bo sung UI flows cho Gio hang, Tim kiem, Wishlist, Auth va Admin >> TC_CART_01: Cap nhat so luong gio hang va tinh lai tong tien real-time
- Location: tests\extended-flows.spec.js:55:3

# Error details

```
Error: expect(locator).toBeVisible() failed

Locator:  locator('.ajaxcart__row').first()
Expected: visible
Received: hidden
Timeout:  5000ms

Call log:
  - Expect "toBeVisible" with timeout 5000ms
  - waiting for locator('.ajaxcart__row').first()
    13 × locator resolved to <div class="ajaxcart__row" data-product-id="PVN276    ">…</div>
       - unexpected value "hidden"

```

```yaml
- banner:
  - button "Menu":
    - img
  - link "Góc Hoa Xinh":
    - /url: /
    - img "Góc Hoa Xinh"
  - search:
    - textbox "Tìm sản phẩm..."
    - button "Tìm kiếm":
      - img
  - link "Tài khoản":
    - /url: /Admin
    - text: cart_rt_1780253093746
  - link "Giỏ hàng":
    - /url: /Cart/Cart
    - img
    - text: "1"
- heading "Giỏ hàng của bạn" [level=1]
- contentinfo:
  - link "Góc Hoa Xinh":
    - /url: /
    - img "Góc Hoa Xinh"
  - text: "Địa chỉ: 140 Lê Trọng Tấn, Quận Tân Phú Điện thoại: 0898449950 Email: 2001230486@huit.edu.vn"
  - heading "Menu" [level=4]
  - heading "Chính sách" [level=4]
  - heading "Thành Viên thực hiện" [level=4]
  - link "Lên đầu trang":
    - /url: "#"
```

# Test source

```ts
  1   | const { test, expect } = require('@playwright/test');
  2   | 
  3   | const userPassword = 'Password@123';
  4   | 
  5   | function uniqueUser(prefix) {
  6   |   const uniqueId = Date.now() + Math.floor(Math.random() * 1000);
  7   |   return {
  8   |     username: `${prefix}_${uniqueId}`,
  9   |     email: `${prefix}_${uniqueId}@example.com`,
  10  |   };
  11  | }
  12  | 
  13  | async function registerUser(page, prefix = 'user_flow') {
  14  |   const user = uniqueUser(prefix);
  15  | 
  16  |   await page.goto('/Account/Dang_ky');
  17  |   await page.fill('input[name="TenDangNhap"]', user.username);
  18  |   await page.fill('input[name="TenHienThi"]', 'Khach Hang UI');
  19  |   await page.fill('input[name="Email"]', user.email);
  20  |   await page.fill('input[name="MatKhau"]', userPassword);
  21  |   await page.fill('input[name="XacNhanMatKhau"]', userPassword);
  22  |   await page.click('button[type="submit"], input[type="submit"]');
  23  |   await expect(page).toHaveURL(/\/Account\/Dang_nhap/);
  24  | 
  25  |   return user;
  26  | }
  27  | 
  28  | async function login(page, username, password = userPassword) {
  29  |   await page.goto('/Account/Dang_nhap');
  30  |   await page.fill('input[name="TenDangNhap"]', username);
  31  |   await page.fill('input[name="MatKhau"]', password);
  32  |   await page.click('button[type="submit"], input[type="submit"]');
  33  |   await expect(page).not.toHaveURL(/\/Account\/Dang_nhap/);
  34  | }
  35  | 
  36  | async function loginAdmin(page) {
  37  |   await login(page, 'admin', '12345');
  38  | }
  39  | 
  40  | async function addFirstAvailableProductToCart(page) {
  41  |   await page.goto('/');
  42  |   const buyForm = page.locator('form[action*="ThemGioHang"]').filter({
  43  |     has: page.locator('button:not([disabled]), input[type="submit"]:not([disabled])'),
  44  |   }).first();
  45  |   await expect(buyForm).toBeVisible();
  46  |   await buyForm.locator('button:not([disabled]), input[type="submit"]:not([disabled])').first().click();
  47  |   await page.waitForLoadState('networkidle');
  48  | }
  49  | 
  50  | function currencyNumber(text) {
  51  |   return Number((text || '').replace(/[^\d]/g, ''));
  52  | }
  53  | 
  54  | test.describe('Bo sung UI flows cho Gio hang, Tim kiem, Wishlist, Auth va Admin', () => {
  55  |   test('TC_CART_01: Cap nhat so luong gio hang va tinh lai tong tien real-time', async ({ page }) => {
  56  |     const user = await registerUser(page, 'cart_rt');
  57  |     await login(page, user.username);
  58  |     await addFirstAvailableProductToCart(page);
  59  | 
  60  |     await page.goto('/Cart/Cart');
  61  |     const row = page.locator('.ajaxcart__row').first();
> 62  |     await expect(row).toBeVisible();
      |                       ^ Error: expect(locator).toBeVisible() failed
  63  | 
  64  |     const quantityInput = row.locator('.cart-quantity-input');
  65  |     const itemTotal = row.locator('.item-total-price');
  66  |     const cartTotal = page.locator('.cart-subtotal-price');
  67  | 
  68  |     const oldItemTotal = currencyNumber(await itemTotal.innerText());
  69  |     const oldCartTotal = currencyNumber(await cartTotal.innerText());
  70  | 
  71  |     await quantityInput.fill('2');
  72  |     await expect.poll(async () => currencyNumber(await itemTotal.innerText())).toBeGreaterThan(oldItemTotal);
  73  |     await expect.poll(async () => currencyNumber(await cartTotal.innerText())).toBeGreaterThanOrEqual(oldCartTotal);
  74  |     await expect(page).toHaveURL(/\/Cart\/Cart/);
  75  | 
  76  |     await quantityInput.fill('-1');
  77  |     await quantityInput.dispatchEvent('input');
  78  |     await expect.poll(async () => Number(await quantityInput.inputValue())).toBeGreaterThanOrEqual(1);
  79  |   });
  80  | 
  81  |   test('TC_SEARCH_01: Tim kiem tu khoa vo nghia hien thi thong bao khong co ket qua', async ({ page }) => {
  82  |     const keyword = 'hoa_hong_xanh_sieu_nhan_123';
  83  | 
  84  |     await page.goto(`/SanPham/Search?query=${encodeURIComponent(keyword)}`);
  85  | 
  86  |     await expect(page.locator('body')).toContainText(/Không tìm thấy|Khong tim thay/i);
  87  |     await expect(page.locator('body')).toContainText(keyword);
  88  |     await expect(page.locator('.item_product_main')).toHaveCount(0);
  89  |   });
  90  | 
  91  |   test('TC_WISHLIST_01: Them san pham vao Wishlist va cap nhat so dem tren header', async ({ page }) => {
  92  |     const user = await registerUser(page, 'wish_flow');
  93  |     await login(page, user.username);
  94  | 
  95  |     await page.goto('/');
  96  |     const productCard = page.locator('form[action*="ThemGioHang"]').filter({
  97  |       has: page.locator('.setWishlist'),
  98  |     }).first();
  99  |     await expect(productCard).toBeVisible();
  100 | 
  101 |     const productName = (await productCard.locator('.product-name a').first().innerText()).trim();
  102 |     await productCard.locator('.setWishlist').first().click();
  103 |     await page.waitForLoadState('networkidle');
  104 | 
  105 |     await page.goto('/Wishlist/Wishlist');
  106 |     await expect(page.locator('.favorite-card')).toContainText(productName);
  107 |     await expect(page.locator('.js-wishlist-count').first()).toHaveText('1');
  108 |   });
  109 | 
  110 |   test('TC_AUTH_FORGOT_PWD: Quen mat khau voi email ton tai hien thi thong bao thanh cong mau xanh', async ({ page }) => {
  111 |     const user = await registerUser(page, 'forgot_pwd');
  112 | 
  113 |     await page.goto('/Account/Dang_nhap?show_recover=true');
  114 |     await expect(page.locator('.h_recover')).toBeVisible();
  115 |     await page.fill('#recover-email', user.email);
  116 |     await page.click('.h_recover input[type="submit"]');
  117 | 
  118 |     await expect(page).toHaveURL(/\/Account\/Reset_mat_khau/);
  119 |     const successAlert = page.locator('.alert-success');
  120 |     await expect(successAlert).toBeVisible();
  121 |     await expect(successAlert).toContainText(/Xác thực email thành công|Xac thuc email thanh cong/i);
  122 |     await expect(successAlert).toHaveCSS('background-color', /rgb\((?:209,\s*231,\s*221|212,\s*237,\s*218)\)/);
  123 |   });
  124 | 
  125 |   test('TC_ADM_PRODUCT_INVENTORY: Admin dat ton kho ve 0 thi nut them gio hang bi disabled', async ({ page }) => {
  126 |     await loginAdmin(page);
  127 | 
  128 |     await page.goto('/Admin/Dashboard/QL_SanPham');
  129 |     const activeProductRow = page.locator('table tr').filter({
  130 |       has: page.locator('button:has-text("Sửa")'),
  131 |       hasNot: page.locator('.badge-danger'),
  132 |     }).first();
  133 |     await expect(activeProductRow).toBeVisible();
  134 | 
  135 |     const productName = (await activeProductRow.locator('td').nth(1).innerText()).replace(/Ngưng bán/g, '').trim();
  136 |     await activeProductRow.locator('button:has-text("Sửa")').click();
  137 |     await expect(page).toHaveURL(/\/Admin\/Dashboard\/EditSanPham/);
  138 | 
  139 |     await page.fill('input[name="SoLuongTon"]', '0');
  140 |     const statusSelect = page.locator('select[name="TinhTrang"]');
  141 |     if (await statusSelect.count()) {
  142 |       const outOfStockValue = await statusSelect.locator('option').evaluateAll(options => {
  143 |         const option = options.find(item => /Hết hàng|Het hang/i.test(item.textContent || ''));
  144 |         return option ? option.value : null;
  145 |       });
  146 |       if (outOfStockValue) {
  147 |         await statusSelect.selectOption(outOfStockValue);
  148 |       } else {
  149 |         await statusSelect.selectOption({ index: 1 });
  150 |       }
  151 |     }
  152 |     await page.click('input[type="submit"][value*="Lưu"], input[type="submit"]');
  153 |     await expect(page).toHaveURL(/\/Admin\/Dashboard\/QL_SanPham/);
  154 | 
  155 |     await page.context().clearCookies();
  156 |     await page.goto(`/SanPham/Search?query=${encodeURIComponent(productName)}`);
  157 |     const customerProductCard = page.locator('.item_product_main').filter({
  158 |       hasText: productName,
  159 |     }).first();
  160 |     await expect(customerProductCard).toBeVisible();
  161 | 
  162 |     const addToCartButton = customerProductCard.locator('button.add_to_cart, button[title*="Thêm"]').first();
```