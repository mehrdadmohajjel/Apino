$(document).ready(function () {

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
function updateCartBadge(count) {
    $('#cart-count').text(count || 0);
}

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