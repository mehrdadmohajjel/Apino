// wwwroot/js/branchadmin/paymentReport.js

// متغیر برای نگهداری صفحه جاری
let currentPage = 1;

$(document).ready(function () {
    // 1. تنظیمات و فعال‌سازی دیت‌پیکر شمسی
    jalaliDatepicker.startWatch({
        minDate: "attr",
        maxDate: "attr",
        time: false,
        hasSecond: false
    });

    // 2. بارگذاری اولیه داده‌ها (صفحه 1)
    loadPayments(1);

    // 3. رویداد کلیک دکمه فیلتر
    $('#btnFilter').click(function (e) {
        e.preventDefault(); // جلوگیری از رفتار پیش‌فرض فرم
        loadPayments(1);
    });
});

/**
 * تابع اصلی برای دریافت داده‌ها از سرور
 * @param {number} page - شماره صفحه
 */
function loadPayments(page) {
    currentPage = page;

    // گرفتن مقادیر فیلترها
    var fromDate = $('#fromDate').val();
    var toDate = $('#toDate').val();

    // نمایش لودینگ و پاک کردن جدول
    $('#loader').show();
    $('#paymentsTable tbody').empty();
    $('#paginationContainer').empty(); // مخفی کردن پیجینگ قدیمی در حین لود
    $('#emptyMessage').hide(); // مخفی کردن پیام نبود رکورد

    $.ajax({
        // *** نکته مهم: آدرس باید شامل Area باشد ***
        url: '/BranchAdmin/PaymentReports/GetList',
        type: 'GET',
        data: {
            page: page,
            fromDate: fromDate,
            toDate: toDate
        },
        success: function (response) {
            $('#loader').hide();

            // بررسی اینکه آیا لیستی وجود دارد یا خیر
            if (!response.items || response.items.length === 0) {
                renderEmptyMessage();
                return;
            }

            renderTable(response.items);
            renderPagination(response.totalPages, response.currentPage);
        },
        error: function (xhr, status, error) {
            $('#loader').hide();
            console.error("Error loading payments:", error);
            console.log("Response:", xhr.responseText);

            // نمایش پیام خطا به کاربر
            var tbody = $('#paymentsTable tbody');
            tbody.html('<tr><td colspan="6" class="text-center text-danger">خطا در برقراری ارتباط با سرور. لطفا مجددا تلاش کنید.</td></tr>');
        }
    });
}

/**
 * رندر کردن سطرهای جدول
 * @param {Array} items - آرایه داده‌های دریافتی
 */
function renderTable(items) {
    var tbody = $('#paymentsTable tbody');
    tbody.empty();

    $.each(items, function (index, item) {
        // جدا کردن سه رقم سه رقم مبلغ
        var formattedAmount = item.amount ? item.amount.toLocaleString() : "0";

        var row = `<tr>
                <td>${item.id}</td>
                <td>${item.userFullName}</td>
                <td>${formattedAmount}</td>
                <td><span class="badge bg-info text-dark" style="direction: ltr;">${item.paymentDate}</span></td>
                <td>${getStatusBadge(item.status)}</td>
                <td>${item.refId || '-'}</td>
            </tr>`;
        tbody.append(row);
    });
}

/**
 * نمایش پیام در صورت نبود داده
 */
function renderEmptyMessage() {
    var tbody = $('#paymentsTable tbody');
    tbody.html('<tr><td colspan="6" class="text-center text-muted fw-bold p-4">رکوردی با این مشخصات یافت نشد</td></tr>');
}

/**
 * ساخت دکمه‌های صفحه‌بندی
 * @param {number} totalPages - کل صفحات
 * @param {number} current - صفحه جاری
 */
function renderPagination(totalPages, current) {
    var container = $('#paginationContainer');
    container.empty();

    if (totalPages <= 1) return;

    var html = '<ul class="pagination">';

    // دکمه قبلی
    var prevDisabled = (current === 1) ? 'disabled' : '';
    // دقت کنید onclick باید به تابع loadPayments اشاره کند
    html += `<li class="page-item ${prevDisabled}">
                <button class="page-link" onclick="loadPayments(${current - 1})" ${prevDisabled}>قبلی</button>
             </li>`;

    // دکمه‌های شماره صفحه (نمایش هوشمند: فقط چند صفحه اطراف صفحه جاری)
    // برای سادگی فعلا همه را نمایش می‌دهیم، اما برای تعداد زیاد بهتر است محدود شود
    for (var i = 1; i <= totalPages; i++) {
        var activeClass = (i === current) ? 'active' : '';
        html += `<li class="page-item ${activeClass}">
                    <button class="page-link" onclick="loadPayments(${i})">${i}</button>
                 </li>`;
    }

    // دکمه بعدی
    var nextDisabled = (current === totalPages) ? 'disabled' : '';
    html += `<li class="page-item ${nextDisabled}">
                <button class="page-link" onclick="loadPayments(${current + 1})" ${nextDisabled}>بعدی</button>
             </li>`;

    html += '</ul>';
    container.html(html);
}

/**
 * تابع کمکی برای استایل وضعیت (اختیاری)
 */
function getStatusBadge(status) {
    // فرض بر این است که وضعیت به صورت متن یا عدد می‌آید
    // می‌توانید بر اساس منطق خود رنگ‌ها را تغییر دهید
    if (status === 'موفق' || status === true)
        return '<span class="badge bg-success">موفق</span>';

    return `<span class="badge bg-secondary">${status}</span>`;
}
