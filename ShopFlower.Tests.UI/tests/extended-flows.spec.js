const { test, expect } = require('@playwright/test');

const userPassword = 'Password@123';

function uniqueUser(prefix) {
  const uniqueId = Date.now() + Math.floor(Math.random() * 1000);
  return {
    username: `${prefix}_${uniqueId}`,
    email: `${prefix}_${uniqueId}@example.com`,
  };
}

async function registerUser(page, prefix = 'user_flow') {
  const user = uniqueUser(prefix);

  await page.goto('/Account/Dang_ky');
  await page.fill('input[name="TenDangNhap"]', user.username);
  await page.fill('input[name="TenHienThi"]', 'Khach Hang UI');
  await page.fill('input[name="Email"]', user.email);
  await page.fill('input[name="MatKhau"]', userPassword);
  await page.fill('input[name="XacNhanMatKhau"]', userPassword);
  await page.click('button[type="submit"], input[type="submit"]');
  await expect(page).toHaveURL(/\/Account\/Dang_nhap/);

  return user;
}

async function login(page, username, password = userPassword) {
  await page.goto('/Account/Dang_nhap');
  await page.fill('input[name="TenDangNhap"]', username);
  await page.fill('input[name="MatKhau"]', password);
  await page.click('button[type="submit"], input[type="submit"]');
  await expect(page).not.toHaveURL(/\/Account\/Dang_nhap/);
}

async function loginAdmin(page) {
  await login(page, 'admin', '12345');
}

async function addFirstAvailableProductToCart(page) {
  await page.goto('/');
  const buyForm = page.locator('form[action*="ThemGioHang"]').filter({
    has: page.locator('button:not([disabled]), input[type="submit"]:not([disabled])'),
  }).first();
  await expect(buyForm).toBeVisible();
  await buyForm.locator('button:not([disabled]), input[type="submit"]:not([disabled])').first().click();
  await page.waitForLoadState('networkidle');
}

function currencyNumber(text) {
  return Number((text || '').replace(/[^\d]/g, ''));
}

test.describe('Bo sung UI flows cho Gio hang, Tim kiem, Wishlist, Auth va Admin', () => {
  test('TC_CART_01: Cap nhat so luong gio hang va tinh lai tong tien real-time', async ({ page }) => {
    const user = await registerUser(page, 'cart_rt');
    await login(page, user.username);
    await addFirstAvailableProductToCart(page);

    await page.goto('/Cart/Cart');
    const row = page.locator('.ajaxcart__row').first();
    await expect(row).toBeVisible();

    const quantityInput = row.locator('.cart-quantity-input');
    const itemTotal = row.locator('.item-total-price');
    const cartTotal = page.locator('.cart-subtotal-price');

    const oldItemTotal = currencyNumber(await itemTotal.innerText());
    const oldCartTotal = currencyNumber(await cartTotal.innerText());

    await quantityInput.fill('2');
    await expect.poll(async () => currencyNumber(await itemTotal.innerText())).toBeGreaterThan(oldItemTotal);
    await expect.poll(async () => currencyNumber(await cartTotal.innerText())).toBeGreaterThanOrEqual(oldCartTotal);
    await expect(page).toHaveURL(/\/Cart\/Cart/);

    await quantityInput.fill('-1');
    await quantityInput.dispatchEvent('input');
    await expect.poll(async () => Number(await quantityInput.inputValue())).toBeGreaterThanOrEqual(1);
  });

  test('TC_SEARCH_01: Tim kiem tu khoa vo nghia hien thi thong bao khong co ket qua', async ({ page }) => {
    const keyword = 'hoa_hong_xanh_sieu_nhan_123';

    await page.goto(`/SanPham/Search?query=${encodeURIComponent(keyword)}`);

    await expect(page.locator('body')).toContainText(/Không tìm thấy|Khong tim thay/i);
    await expect(page.locator('body')).toContainText(keyword);
    await expect(page.locator('.item_product_main')).toHaveCount(0);
  });

  test('TC_WISHLIST_01: Them san pham vao Wishlist va cap nhat so dem tren header', async ({ page }) => {
    const user = await registerUser(page, 'wish_flow');
    await login(page, user.username);

    await page.goto('/');
    const productCard = page.locator('form[action*="ThemGioHang"]').filter({
      has: page.locator('.setWishlist'),
    }).first();
    await expect(productCard).toBeVisible();

    const productName = (await productCard.locator('.product-name a').first().innerText()).trim();
    await productCard.locator('.setWishlist').first().click();
    await page.waitForLoadState('networkidle');

    await page.goto('/Wishlist/Wishlist');
    await expect(page.locator('.favorite-card')).toContainText(productName);
    await expect(page.locator('.js-wishlist-count').first()).toHaveText('1');
  });

  test('TC_AUTH_FORGOT_PWD: Quen mat khau voi email ton tai hien thi thong bao thanh cong mau xanh', async ({ page }) => {
    const user = await registerUser(page, 'forgot_pwd');

    await page.goto('/Account/Dang_nhap?show_recover=true');
    await expect(page.locator('.h_recover')).toBeVisible();
    await page.fill('#recover-email', user.email);
    await page.click('.h_recover input[type="submit"]');

    await expect(page).toHaveURL(/\/Account\/Reset_mat_khau/);
    const successAlert = page.locator('.alert-success');
    await expect(successAlert).toBeVisible();
    await expect(successAlert).toContainText(/Xác thực email thành công|Xac thuc email thanh cong/i);
    await expect(successAlert).toHaveCSS('background-color', /rgb\((?:209,\s*231,\s*221|212,\s*237,\s*218)\)/);
  });

  test('TC_ADM_PRODUCT_INVENTORY: Admin dat ton kho ve 0 thi nut them gio hang bi disabled', async ({ page }) => {
    await loginAdmin(page);

    await page.goto('/Admin/Dashboard/QL_SanPham');
    const activeProductRow = page.locator('table tr').filter({
      has: page.locator('button:has-text("Sửa")'),
      hasNot: page.locator('.badge-danger'),
    }).first();
    await expect(activeProductRow).toBeVisible();

    const productName = (await activeProductRow.locator('td').nth(1).innerText()).replace(/Ngưng bán/g, '').trim();
    await activeProductRow.locator('button:has-text("Sửa")').click();
    await expect(page).toHaveURL(/\/Admin\/Dashboard\/EditSanPham/);

    await page.fill('input[name="SoLuongTon"]', '0');
    const statusSelect = page.locator('select[name="TinhTrang"]');
    if (await statusSelect.count()) {
      const outOfStockValue = await statusSelect.locator('option').evaluateAll(options => {
        const option = options.find(item => /Hết hàng|Het hang/i.test(item.textContent || ''));
        return option ? option.value : null;
      });
      if (outOfStockValue) {
        await statusSelect.selectOption(outOfStockValue);
      } else {
        await statusSelect.selectOption({ index: 1 });
      }
    }
    await page.click('input[type="submit"][value*="Lưu"], input[type="submit"]');
    await expect(page).toHaveURL(/\/Admin\/Dashboard\/QL_SanPham/);

    await page.context().clearCookies();
    await page.goto(`/SanPham/Search?query=${encodeURIComponent(productName)}`);
    const customerProductCard = page.locator('.item_product_main').filter({
      hasText: productName,
    }).first();
    await expect(customerProductCard).toBeVisible();

    const addToCartButton = customerProductCard.locator('button.add_to_cart, button[title*="Thêm"]').first();
    await expect(addToCartButton).toBeDisabled();
    await expect(addToCartButton).toHaveAttribute('title', /ngừng kinh doanh|Hết hàng|Het hang/i);
  });

  test('TC_ADM_USER_BAN: Admin vo hieu hoa user va user khong the dang nhap lai', async ({ browser }) => {
    const userContext = await browser.newContext({ ignoreHTTPSErrors: true });
    const adminContext = await browser.newContext({ ignoreHTTPSErrors: true });
    const userPage = await userContext.newPage();
    const adminPage = await adminContext.newPage();

    try {
      const user = await registerUser(userPage, 'ban_flow');
      await login(userPage, user.username);

      await loginAdmin(adminPage);
      await adminPage.goto(`/Admin/Dashboard/QL_TaiKhoan?searchString=${encodeURIComponent(user.username)}`);
      const targetRow = adminPage.locator('table tr').filter({ hasText: user.username }).first();
      await expect(targetRow).toBeVisible();
      adminPage.once('dialog', dialog => dialog.accept());
      await targetRow.locator('button:has-text("Vô hiệu hóa")').click();
      await adminPage.waitForLoadState('networkidle');

      await userPage.goto('/Checkout/ThanhToan');
      await expect(userPage).toHaveURL(/\/Account\/Dang_nhap/);

      await userPage.fill('input[name="TenDangNhap"]', user.username);
      await userPage.fill('input[name="MatKhau"]', userPassword);
      await userPage.click('button[type="submit"], input[type="submit"]');

      await expect(userPage).toHaveURL(/\/Account\/Dang_nhap/);
      await expect(userPage.locator('.alert-danger, .validation-summary-errors').first()).toContainText(
        /Tài khoản.*(khóa|vô hiệu hóa)|Tai khoan.*(khoa|vo hieu hoa)/i
      );
    } finally {
      await userContext.close();
      await adminContext.close();
    }
  });
});
