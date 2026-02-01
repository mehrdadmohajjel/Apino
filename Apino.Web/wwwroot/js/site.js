$(document).ready(function () {
    loadCartCount();
    loadNotifications();

    const $branchProducts = $("#branch-products");
    const $branchProductsRow = $("#branch-products .row");
    const $branchProductsTitle = $("#branch-products-title");

    // مخفی کردن سکشن branch-products هنگام بارگذاری
    $branchProducts.hide();

    // ==============================
    // کلیک روی شعبه
    // ==============================
    $('.branch-link').on('click', function () {
        const $this = $(this);
        const branchId = $this.data('id');
        const branchTitle = $this.find('h3').text();

        if (!branchId) {
            console.error('Branch ID موجود نیست!');
            return;
        }

        // کلاس active
        $('.branch-link').removeClass('active');
        $this.addClass('active');

        // عنوان سکشن
        $branchProductsTitle.text('خدمات ارائه شده در شعبه ' + branchTitle);

        // نمایش سکشن با انیمیشن
        $branchProducts.slideDown();

        // پاک کردن دسته‌بندی‌های قبلی
        $branchProductsRow.empty();

        // لود دسته‌بندی‌ها
        $.ajax({
            url: `/Branches/GetCategories?branchId=${branchId}`,
            type: 'GET',
            success: function (categories) {

                if (!categories || categories.length === 0) {
                    $branchProductsRow.html('<div class="col-12"><p>هیچ دسته‌بندی برای این شعبه وجود ندارد.</p></div>');
                    return;
                }

                // ساخت HTML دسته‌بندی‌ها با data صحیح
                const html = categories.map(cat => `
                    <div class="col-lg-4 col-md-6 col-sm-6 category-box" 
                         data-category-id="${cat.id}" 
                         data-branch-id="${branchId}">
                        <div class="single-features-box">
                            <div class="category-image">
                                <img src="/images/Category/${cat.iconName}" 
                                     alt="${cat.categoryTitle}" class="img-fluid"/>
                            </div>
                            <h3>${cat.categoryTitle}</h3>
                            <p>${cat.description || ''}</p>
                            <div class="products-container" style="display:none;"></div>
                        </div>
                    </div>
                `).join('');

                $branchProductsRow.html(html);

                // اسکرول به سکشن branch-products
                $('html, body').animate({
                    scrollTop: $branchProducts.offset().top - 100
                }, 500);
            },
            error: function (xhr, status, error) {
                console.error('خطا در بارگذاری دسته‌بندی‌ها:', error, xhr.responseText);
                alert('خطا در بارگذاری دسته‌بندی‌ها!');
            }
        });
    });

    // ==============================
    // کلیک روی دسته‌بندی‌ها
    // ==============================
    $branchProducts.on('click', '.category-box', function () {
        const $this = $(this);
        const categoryId = $this.data('category-id');
        const branchId = $this.data('branch-id');

        if (!categoryId || !branchId) {
            console.warn('اطلاعات دسته‌بندی یا شعبه موجود نیست!', $this[0].dataset);
            alert('اطلاعات دسته‌بندی یا شعبه موجود نیست!');
            return;
        }

        // هدایت به صفحه shop با پارامترهای صحیح
        window.location.href = `/shop?branchId=${branchId}&categoryId=${categoryId}`;
    });

});
$(function () {
    const token = localStorage.getItem('accessToken');
    if (!token) return;

    $.ajax({
        url: '/cart/count',
        type: 'GET',
        success: res => {
            updateCartBadge(res.count);
            localStorage.setItem('cartCount', res.count);
        }
    });
});
function updateCartBadge(newCount) {

    const badge = $('#cart-count');

    newCount = parseInt(newCount) || 0;
    const oldCount = parseInt(badge.text()) || 0;

    if (newCount <= 0) {
        badge.text('0').hide();
        localStorage.setItem('cartCount', 0);
        return;
    }

    badge.text(newCount).fadeIn(150);

    // انیمیشن فقط در صورت تغییر عدد
    if (newCount !== oldCount) {
        badge.removeClass('bump');
        void badge[0].offsetWidth;
        badge.addClass('bump');
    }

    localStorage.setItem('cartCount', newCount);
}


async function loadCartCount() {

    const badge = $('#cart-count');
    const token = localStorage.getItem('accessToken');

    if (!token) {
        badge.hide();
        return;
    }

    try {
        const res = await fetch('/cart/count', {
            headers: {
                Authorization: 'Bearer ' + token
            }
        });

        if (!res.ok) {
            badge.hide();
            return;
        }

        const data = await res.json();
        const count = parseInt(data.count) || 0;

        updateCartBadge(count);

    } catch (e) {
        console.error('Cart count error:', e);
        badge.hide();
    }
}



$('#cart-link').on('click', function () {

    const token = localStorage.getItem('accessToken');

    if (!token) {
        // کاربر لاگین نیست
        openLoginModal(); // OTP
        return;
    }

    // لاگین است → برو به cart
    window.location.href = '/cart';
});

$('.notification-btn').on('click', function () {
    loadNotifications();
});

$(document).on('click', '#go-to-notifications', function () {
    window.location.href = '/notifications';
});
/* ==============================
   Load Notifications
============================== */
function loadNotifications() {

    $.get('/notification/latest')
        .done(res => {

            const dropdown = $('.notification-dropdown');
            dropdown.empty();

            // Badge
            if (res.count > 0) {
                $('#notif-count').text(res.count).fadeIn(150);
            } else {
                $('#notif-count').fadeOut(100);
            }

            if (!res.items || res.items.length === 0) {
                dropdown.append(`
                    <li class="text-center text-muted p-3">
                        اعلانی وجود ندارد
                    </li>
                `);
                return;
            }

            res.items.forEach(n => {
                dropdown.append(`
                    <li class="dropdown-item notification-item ${n.isRead ? '' : 'unread'}"
                        data-id="${n.id}">
                        <div class="fw-bold">${n.title}</div>
                        <div class="small text-muted">${n.message}</div>
                        <div class="text-end text-secondary small mt-1">${n.persianDate}</div>
                    </li>
                `);
            });

            dropdown.append(`
                <li><hr class="dropdown-divider"></li>
                <li>
                    <a href="javascript:void(0)"
                       id="go-to-notifications"
                       class="dropdown-item text-center text-primary">
                        مشاهده همه اعلان‌ها
                    </a>
                </li>
            `);
        })
        .fail(err => {
            console.error(err);
            $('.notification-dropdown').html(`
                <li class="text-center text-danger p-3">
                    خطا در دریافت اعلان‌ها
                </li>
            `);
        });
}

//====================================
//  خواندن آیتم با کلیک روش
//======================================
$(document).on('click', '.notification-item', function () {

    const id = $(this).data('id');
    const el = $(this);

    $.post('/notification/mark-read', { id })
        .done(() => {
            el.removeClass('unread');
            loadNotifications(); // refresh badge
        });
});
$('#mark-all-read').on('click', function () {

    $.post('/notification/mark-all')
        .done(() => {
            $('.notification-item').removeClass('unread');
            $('#notif-count').fadeOut();
            Toast.fire({ icon: 'success', title: 'همه اعلان‌ها خوانده شدند' });
        });
});

//$('#branch-products').on('click', '.category-box', function () {
//    var categoryId = $(this).data('id');
//    var container = $(this).find('.products-container');

//    $.ajax({
//        url: '/Branches/GetProducts?categoryId=' + categoryId,
//        type: 'GET',
//        success: function (products) {
//            var html = '';

//            if (!products || products.length === 0) {
//                html = '<p>هیچ محصولی در این دسته‌بندی موجود نیست.</p>';
//            } else {
//                products.forEach(function (p) {
//                    html += '<div class="single-product">';
//                    html += '<img src="/images/branch/' + p.ImageName + '" alt="' + p.Title + '" class="img-fluid" />';
//                    html += '<h5>' + p.Title + '</h5>';
//                    html += '<p>' + p.Description + '</p>';
//                    html += '<span>قیمت: ' + p.Price.toLocaleString() + ' تومان</span>';
//                    html += '</div>';
//                });
//            }

//            container.html(html);
//            container.slideToggle();
//        },
//        error: function () {
//            alert('خطا در بارگذاری محصولات!');
//        }
//    });
//});