$(document).ready(function () {

    // 1️⃣ مخفی کردن سکشن branch-products هنگام بارگذاری صفحه
    $("#branch-products").hide();

    // کلیک روی شعبه
    $('.branch-link').on('click', function () {
        var branchId = $(this).data('id');

        // نمایش سکشن branch-products با انیمیشن
        $("#branch-products").slideDown();

        // لود دسته‌بندی‌ها
        $.ajax({
            url: '/Branches/GetCategories?branchId=' + branchId,
            type: 'GET',
            success: function (categories) {
                var html = '';

                if (!categories || categories.length === 0) {
                    html = '<div class="col-12"><p>هیچ دسته‌بندی برای این شعبه وجود ندارد.</p></div>';
                } else {
                    categories.forEach(function (cat) {
                        html += '<div class="col-lg-4 col-md-6 col-sm-6 category-box" data-id="' + cat.Id + '">';
                        html += '<div class="single-features-box">';
                        html += '<div class="category-image">';
                        html += '<img src="/images/Category/' + cat.ImageUrl + '" alt="' + cat.Title + '" class="img-fluid"/>';
                        html += '</div>';
                        html += '<h3>' + cat.Title + '</h3>';
                        html += '<p>' + cat.Description + '</p>';
                        html += '<div class="products-container" style="display:none;"></div>'; // محصولات زیر هر دسته‌بندی مخفی
                        html += '</div></div>';
                    });
                }

                $('#branch-products .row').html(html);

                // اسکرول به سکشن branch-products
                $('html, body').animate({
                    scrollTop: $('#branch-products').offset().top - 100
                }, 500);
            },
            error: function () {
                alert('خطا در بارگذاری دسته‌بندی‌ها!');
            }
        });
    });

    // کلیک روی دسته‌بندی‌ها برای نمایش محصولات
    $('#branch-products').on('click', '.category-box', function () {
        var categoryId = $(this).data('id');
        var container = $(this).find('.products-container');

        $.ajax({
            url: '/Branches/GetProducts?categoryId=' + categoryId,
            type: 'GET',
            success: function (products) {
                var html = '';

                if (!products || products.length === 0) {
                    html = '<p>هیچ محصولی در این دسته‌بندی موجود نیست.</p>';
                } else {
                    products.forEach(function (p) {
                        html += '<div class="single-product">';
                        html += '<img src="/images/Category/' + p.ImageUrl + '" alt="' + p.Title + '" class="img-fluid" />';
                        html += '<h5>' + p.Title + '</h5>';
                        html += '<p>' + p.Description + '</p>';
                        html += '<span>قیمت: ' + p.Price + ' تومان</span>';
                        html += '</div>';
                    });
                }

                container.html(html);

                // باز و بسته شدن محصولات با انیمیشن
                container.slideToggle();
            },
            error: function () {
                alert('خطا در بارگذاری محصولات!');
            }
        });
    });

});
