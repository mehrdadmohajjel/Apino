console.log('shop.js loaded');

// Base Params
const params = new URLSearchParams(window.location.search);
const branchId = params.get('branchId');
console.log(branchId);
if (!branchId) console.warn('branchId is missing from URL');

let currentCategory = null;

// ==============================
// Update Cart Badge
function updateCartBadge(newCount) {

    const badge = $('#cart-count');
    const oldCount = parseInt(badge.text()) || 0;

    if (newCount <= 0) {
        badge.fadeOut(150);
        localStorage.setItem('cartCount', 0);
        return;
    }

    badge.text(newCount).fadeIn(150);

    // 🔔 فقط اگر عدد تغییر کرده → انیمیشن
    if (newCount !== oldCount) {
        badge.removeClass('bump'); // reset
        void badge[0].offsetWidth; // force reflow
        badge.addClass('bump');
    }

    localStorage.setItem('cartCount', newCount);
}


// Load initial cart count from server or localStorage
let cartCount = parseInt(localStorage.getItem('cartCount')) || 0;
updateCartBadge(cartCount);

// ==============================
// ================= Change Category =================
function changeCategory(categoryId) {
    if (currentCategory === categoryId) return;
    currentCategory = categoryId;

    if (!branchId) return Toast.fire({ icon: 'error', title: 'شناسه شعبه مشخص نیست' });

    Swal.fire({ title: 'در حال بارگذاری...', allowOutsideClick: false, didOpen: () => Swal.showLoading() });

    $.ajax({
        url: '/shop', // حتماً کنترلر را برای partial view تنظیم کنید
        type: 'GET',
        data: { branchId: branchId, categoryId: categoryId || null }
    })
        .done(html => {
            Swal.close();

            const $newProducts = $(html).find('#product-list');
            if ($newProducts.length) {
                $('#product-list').fadeOut(100, function () {
                    $(this).html($newProducts.html()).fadeIn(150);
                });
            } else {
                console.error('Partial #product-list not found');
                Toast.fire({ icon: 'error', title: 'خطا در بارگذاری محصولات' });
            }

            const newUrl = categoryId ? `/shop?branchId=${branchId}&categoryId=${categoryId}` : `/shop?branchId=${branchId}`;
            history.pushState({}, '', newUrl);
        })
        .fail(() => {
            Swal.close();
            Toast.fire({ icon: 'error', title: 'خطا در دریافت محصولات' });
        });
}

// ================= Category - Desktop =================
$(document).on('click', '.category-btn', function () {
    const categoryId = String($(this).data('category') || '');

    $('.category-btn').removeClass('active');
    $(this).addClass('active');

    $('.mobile-category-select').val(categoryId);

    changeCategory(categoryId);
});

// ================= Category - Mobile =================
$(document).on('change', '.mobile-category-select', function () {
    const categoryId = String(this.value || '');

    console.log('Mobile category changed:', categoryId);

    // sync desktop buttons
    $('.category-btn').removeClass('active');
    $(`.category-btn[data-category="${categoryId}"]`).addClass('active');

    changeCategory(categoryId);
});

// ==============================
// ================= Add To Cart =================
$(document).on('click', '.add-to-cart-btn', function () {

    if ($(this).prop('disabled')) return;
    $(this).prop('disabled', true);

    const productId = $(this).data('id');
    const btnBranchId = $(this).data('branch-id'); // 👈 اسم متفاوت
    const stock = parseInt($(this).data('stock'));
    const payAtPlace = $(this).data('payatplace') == 1;

    setTimeout(() => $(this).prop('disabled', false), 1500);

    if (!productId || !btnBranchId) {
        Toast.fire({ icon: 'error', title: 'اطلاعات محصول ناقص است' });
        return;
    }

    if (stock <= 0) {
        Toast.fire({ icon: 'warning', title: 'این محصول ناموجود است' });
        return;
    }

    const token = localStorage.getItem('accessToken');

    if (token) {
        addToCart(productId, btnBranchId, payAtPlace);
    } else {
        addToGuestCart({
            productId,
            branchId: btnBranchId,
            quantity: 1,
            payAtPlace
        });
        openLoginModal();
    }
});

function addToGuestCart(item) {
    let cart = JSON.parse(localStorage.getItem('guestCart')) || [];

    const existing = cart.find(x =>
        x.productId === item.productId &&
        x.branchId === item.branchId
    );

    if (existing) {
        existing.quantity += item.quantity || 1; // ⚠️ حتما همان نام quantity
    } else {
        cart.push({
            productId: item.productId,
            branchId: item.branchId,
            quantity: item.quantity || 1,
            payAtPlace: item.payAtPlace
        });
    }

    localStorage.setItem('guestCart', JSON.stringify(cart));
    // ✅ badge
    const totalQty = cart.reduce((sum, x) => sum + x.quantity, 0);
    updateCartBadge(totalQty);
    localStorage.setItem('cartCount', totalQty);

    console.log('Guest cart updated:', cart);
}

// ================= Add To Cart AJAX =================
function addToCart(productId, branchId, payAtPlace) {

    Swal.fire({
        title: 'در حال افزودن به سبد...',
        allowOutsideClick: false,
        didOpen: () => Swal.showLoading()
    });

    $.ajax({
        url: '/cart/add',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ productId, branchId, payAtPlace })
    })
        .done(function (res) {
            Swal.close();

            // 🔢 آپدیت badge سبد خرید
            if (res && typeof res.count !== 'undefined') {
                updateCartBadge(res.count);
                localStorage.setItem('cartCount', res.count);
            }

            Toast.fire({
                icon: 'success',
                title: 'محصول به سبد اضافه شد'
            });
        })
        .fail(function (xhr) {
            Swal.close();

            let msg = 'خطای نامشخص';
            if (xhr.responseJSON?.message) {
                msg = xhr.responseJSON.message;
            } else if (xhr.responseText) {
                msg = xhr.responseText;
            }

            Toast.fire({
                icon: 'error',
                title: msg
            });
        });
}

// Browser back/forward
window.addEventListener('popstate', () => {
    location.reload();
});

//Merge After Otp
function onOtpVerified() {

    const guestCart = JSON.parse(localStorage.getItem('guestCart'));

    if (!guestCart || guestCart.length === 0) {
        redirectToCart();
        return;
    }

    $.ajax({
        url: '/cart/merge',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(guestCart),
        headers: {
            Authorization: 'Bearer ' + localStorage.getItem('accessToken')
        }
    })
        .done(() => {
            localStorage.removeItem('guestCart');
            if (res.count !== undefined) {
                updateCartBadge(res.count);
                localStorage.setItem('cartCount', res.count);
            }
            redirectToCart();
        })
        .fail(() => {
            Toast.fire({ icon: 'error', title: 'خطا در انتقال سبد خرید' });
        });
}

function redirectToCart() {
    window.location.href = '/cart';
}
