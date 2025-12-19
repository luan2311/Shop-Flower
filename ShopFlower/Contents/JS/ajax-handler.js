// ========================================
// AJAX Handler for Shop Flower
// ========================================

(function ($) {
    'use strict';

    // ========================================
    // 1. CART AJAX FUNCTIONS
    // ========================================

    /**
     * Thêm sản phẩm vào giỏ hàng qua AJAX
     */
    window.addToCartAjax = function (productId, returnUrl) {
        if (!productId) {
            console.error('Product ID is required');
            return;
        }

        $.ajax({
            url: '/Cart/ThemGioHang',
            type: 'POST',
            data: {
                ms: productId,
                strURL: returnUrl || window.location.href
            },
            success: function (response) {
                if (response.success) {
                    // Cập nhật số lượng giỏ hàng trong header
                    updateCartCount(response.count);

                    // Hiển thị thông báo thành công
                    showNotification('Đã thêm sản phẩm vào giỏ hàng!', 'success');
                } else {
                    showNotification('Không thể thêm sản phẩm vào giỏ hàng', 'error');
                }
            },
            error: function (xhr, status, error) {
                console.error('Error adding to cart:', error);
                showNotification('Đã xảy ra lỗi. Vui lòng thử lại!', 'error');
            }
        });
    };

    /**
     * Cập nhật số lượng sản phẩm trong giỏ hàng
     */
    window.updateCartQuantityAjax = function (productId, quantity) {
        if (!productId || quantity < 1) {
            return;
        }

        $.ajax({
            url: '/Cart/CapNhatGioHang',
            type: 'POST',
            data: {
                MaSP: productId,
                txtSoLuong: quantity
            },
            success: function (response) {
                if (response.success) {
                    // Cập nhật giá tiền của sản phẩm
                    var productRow = $('.ajaxcart__row[data-product-id="' + productId + '"]');
                    if (productRow.length) {
                        productRow.find('.item-total-price').text(response.itemTotalPrice + '₫');
                    }

                    // Cập nhật tổng tiền giỏ hàng
                    $('.cart-subtotal-price').text(response.cartSubtotal + '₫');

                    // Cập nhật số lượng trong header
                    updateCartCount(response.cartCount);

                    showNotification('Đã cập nhật giỏ hàng', 'success');
                } else {
                    showNotification(response.message || 'Không thể cập nhật giỏ hàng', 'error');
                }
            },
            error: function () {
                showNotification('Đã xảy ra lỗi khi cập nhật giỏ hàng', 'error');
            }
        });
    };

    /**
     * Xóa sản phẩm khỏi giỏ hàng qua AJAX
     */
    window.removeFromCartAjax = function (productId) {
        if (!productId) return;

        if (!confirm('Bạn có chắc muốn xóa sản phẩm này khỏi giỏ hàng?')) {
            return;
        }

        $.ajax({
            url: '/Cart/XoaGioHang',
            type: 'POST',
            data: { MaSP: productId },
            success: function (response) {
                if (response.success) {
                    // Xóa dòng sản phẩm khỏi giao diện
                    var productRow = $('.ajaxcart__row[data-product-id="' + productId + '"]');
                    productRow.fadeOut(300, function () {
                        $(this).remove();

                        // Kiểm tra nếu giỏ hàng trống
                        if ($('.ajaxcart__row').length === 0) {
                            window.location.href = '/Cart/empty_cart';
                        }
                    });

                    // Cập nhật tổng tiền
                    if (response.cartSubtotal !== undefined) {
                        $('.cart-subtotal-price').text(response.cartSubtotal + '₫');
                    }

                    // Cập nhật số lượng trong header
                    updateCartCount(response.cartCount);

                    showNotification('Đã xóa sản phẩm khỏi giỏ hàng', 'success');
                } else {
                    showNotification(response.message || 'Không thể xóa sản phẩm', 'error');
                }
            },
            error: function () {
                showNotification('Đã xảy ra lỗi', 'error');
            }
        });
    };

    // ========================================
    // 2. WISHLIST AJAX FUNCTIONS
    // ========================================

    /**
     * Thêm sản phẩm vào danh sách yêu thích
     */
    window.addToWishlistAjax = function (productId, returnUrl) {
        if (!productId) return;

        $.ajax({
            url: '/Wishlist/ThemVaoYeuThich',
            type: 'GET',
            data: {
                ms: productId,
                strURL: returnUrl || window.location.href
            },
            success: function (response) {
                if (response.success) {
                    updateWishlistCount(response.count);
                    showNotification('Đã thêm vào danh sách yêu thích!', 'success');
                } else {
                    showNotification('Không thể thêm vào danh sách yêu thích', 'error');
                }
            },
            error: function () {
                showNotification('Đã xảy ra lỗi', 'error');
            }
        });
    };

    /**
     * Xóa sản phẩm khỏi wishlist
     */
    window.removeFromWishlistAjax = function (productId) {
        if (!productId) return;

        if (!confirm('Bạn có chắc muốn xóa sản phẩm này khỏi danh sách yêu thích?')) {
            return;
        }

        $.ajax({
            url: '/Wishlist/XoaSanPhamYeuThich',
            type: 'POST',
            data: { MaSP: productId },
            success: function (response) {
                if (response.success) {
                    // Xóa sản phẩm khỏi giao diện
                    var productItem = $('[data-wishlist-id="' + productId + '"]');
                    productItem.fadeOut(300, function () {
                        $(this).remove();

                        // Kiểm tra nếu wishlist trống
                        if ($('[data-wishlist-id]').length === 0) {
                            window.location.href = '/Wishlist/empty_wishlist';
                        }
                    });

                    updateWishlistCount(response.count);
                    showNotification('Đã xóa khỏi danh sách yêu thích', 'success');
                } else {
                    showNotification(response.message || 'Không thể xóa sản phẩm', 'error');
                }
            },
            error: function () {
                showNotification('Đã xảy ra lỗi', 'error');
            }
        });
    };

    // ========================================
    // 3. SEARCH AJAX FUNCTION
    // ========================================

    /**
     * Tìm kiếm sản phẩm qua AJAX
     */
    window.searchProductsAjax = function (keyword, resultContainerId) {
        if (!keyword || keyword.length < 2) {
            return;
        }

        $.ajax({
            url: '/SanPham/SearchAjax',
            type: 'GET',
            data: { keyword: keyword },
            success: function (response) {
                if (response.success && response.products) {
                    displaySearchResults(response.products, resultContainerId);
                } else {
                    displaySearchResults([], resultContainerId);
                }
            },
            error: function () {
                console.error('Error searching products');
            }
        });
    };

    /**
     * Hiển thị kết quả tìm kiếm
     */
    function displaySearchResults(products, containerId) {
        var container = $('#' + containerId);
        if (!container.length) return;

        if (products.length === 0) {
            container.html('<p class="text-center">Không tìm thấy sản phẩm nào</p>');
            return;
        }

        var html = '<div class="search-results">';
        products.forEach(function (product) {
            html += '<div class="search-result-item">';
            html += '<img src="/Images/' + product.AnhSP + '" alt="' + product.TenSP + '" />';
            html += '<div class="product-info">';
            html += '<h4>' + product.TenSP + '</h4>';
            html += '<p>' + formatCurrency(product.GiaBan) + 'đ</p>';
            html += '</div>';
            html += '</div>';
        });
        html += '</div>';

        container.html(html);
    }

    // ========================================
    // 4. HELPER FUNCTIONS
    // ========================================

    /**
     * Cập nhật số lượng giỏ hàng trong header
     */
    function updateCartCount(count) {
        $('.count_item_pr').text(count || 0);
        $('.cart-count').text(count || 0);
    }

    /**
   * Cập nhật số lượng wishlist trong header
     */
    function updateWishlistCount(count) {
        $('.wishlist-count').text(count || 0);
    }

    /**
   * Hiển thị thông báo
     */
    function showNotification(message, type) {
        type = type || 'info';

        // Tạo notification element nếu chưa có
        var notificationId = 'ajax-notification';
        var $notification = $('#' + notificationId);

        if (!$notification.length) {
            $('body').append('<div id="' + notificationId + '" class="ajax-notification"></div>');
            $notification = $('#' + notificationId);
        }

        // Set class theo loại thông báo
        $notification.removeClass('success error info warning')
            .addClass(type)
            .text(message)
            .fadeIn(300);

        // Tự động ẩn sau 3 giây
        setTimeout(function () {
            $notification.fadeOut(300);
        }, 3000);
    }

    /**
     * Format số tiền
     */
    function formatCurrency(amount) {
        return new Intl.NumberFormat('vi-VN').format(amount);
    }

    // ========================================
    // 5. AUTO-INITIALIZATION
    // ========================================

    $(document).ready(function () {

        // Auto-attach AJAX handlers to elements with data-ajax attributes

        // Add to cart buttons
        $(document).on('click', '[data-ajax-add-cart]', function (e) {
            e.preventDefault();
            var productId = $(this).data('product-id') || $(this).data('ajax-add-cart');
            var returnUrl = $(this).data('return-url') || window.location.href;
            addToCartAjax(productId, returnUrl);
        });

        // Add to wishlist buttons
        $(document).on('click', '[data-ajax-add-wishlist]', function (e) {
            e.preventDefault();
            var productId = $(this).data('product-id') || $(this).data('ajax-add-wishlist');
            var returnUrl = $(this).data('return-url') || window.location.href;
            addToWishlistAjax(productId, returnUrl);
        });

        // Remove from cart buttons
        $(document).on('click', '[data-ajax-remove-cart]', function (e) {
            e.preventDefault();
            var productId = $(this).data('product-id') || $(this).data('ajax-remove-cart');
            removeFromCartAjax(productId);
        });

        // Remove from wishlist buttons
        $(document).on('click', '[data-ajax-remove-wishlist]', function (e) {
            e.preventDefault();
            var productId = $(this).data('product-id') || $(this).data('ajax-remove-wishlist');
            removeFromWishlistAjax(productId);
        });

        // Cart quantity input change (debounced)
        var quantityUpdateTimeout;
        $(document).on('input', '.cart-quantity-input', function () {
            clearTimeout(quantityUpdateTimeout);
            var $input = $(this);
            var productId = $input.data('product-id');
            var quantity = parseInt($input.val());

            quantityUpdateTimeout = setTimeout(function () {
                if (quantity > 0) {
                    updateCartQuantityAjax(productId, quantity);
                }
            }, 500); // Đợi 500ms sau khi user ngừng nhập
        });

        // Search input (debounced)
        var searchTimeout;
        $(document).on('input', '[data-ajax-search]', function () {
            clearTimeout(searchTimeout);
            var keyword = $(this).val();
            var resultContainer = $(this).data('result-container');

            searchTimeout = setTimeout(function () {
                if (keyword.length >= 2) {
                    searchProductsAjax(keyword, resultContainer);
                }
            }, 300);
        });
    });

})(jQuery);
