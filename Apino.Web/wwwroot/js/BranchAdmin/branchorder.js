$(document).ready(function () {
    loadOrders();

    // دکمه رفرش
    $('#btnRefresh').click(function () {
        loadOrders();
    });

    // رفرش خودکار هر 60 ثانیه
    setInterval(loadOrders, 60000);
});

// --- Main Functions ---

function loadOrders() {
    const tableBody = $('#ordersTable tbody');
    const spinner = $('#loadingSpinner');
    const emptyMsg = $('#emptyMessage');

    // فقط اگر جدول خالی است اسپینر نشان بده (تا در رفرش خودکار پرش نداشته باشیم)
    if (tableBody.children().length === 0) {
        spinner.removeClass('d-none');
        emptyMsg.addClass('d-none');
    }

    $.ajax({
        url: '/BranchAdmin/Orders/list',
        type: 'GET',
        success: function (data) {
            tableBody.empty();
            spinner.addClass('d-none');

            if (!data || data.length === 0) {
                emptyMsg.removeClass('d-none');
                return;
            }
            emptyMsg.addClass('d-none');

            data.forEach(order => {
                const row = renderOrderRow(order);
                tableBody.append(row);
            });
        },
        error: function () {
            spinner.addClass('d-none');
            showToast('خطا در دریافت اطلاعات سفارشات', 'danger');
        }
    });
}

function renderOrderRow(order) {
    // تعیین استایل وضعیت فعلی
    let statusBadge = '';
    switch (order.statusId) {
        case 2: statusBadge = '<span class="badge bg-primary">پرداخت شده</span>'; break;
        case 3: statusBadge = '<span class="badge bg-info text-dark">تایید شده</span>'; break;
        case 4: statusBadge = '<span class="badge bg-warning text-dark">در حال آماده‌سازی</span>'; break;
        case 5: statusBadge = '<span class="badge bg-success">آماده تحویل</span>'; break;
        case 8: statusBadge = '<span class="badge bg-dark">تحویل شده</span>'; break;
        default: statusBadge = '<span class="badge bg-secondary">' + order.statusTitle + '</span>';
    }

    // تعیین دکمه عملیات
    let actionBtn = '';
    if (!order.isFinished && order.nextStatusId > 0) {
        // از data attributes برای پاس دادن اطلاعات به تابع کلیک استفاده می‌کنیم
        actionBtn = `
            <button class="btn btn-sm btn-${order.nextStatusColor} shadow-sm px-3" 
                    onclick="changeOrderStatus(${order.id}, '${order.nextStatusTitle}')">
                ${order.nextStatusTitle} <i class="fas fa-chevron-left ms-1 small"></i>
            </button>
        `;
    } else if (order.isFinished) {
        actionBtn = '<span class="text-success small"><i class="fas fa-check-double me-1"></i>تکمیل شده</span>';
    }

    return `
        <tr>
            <td class="ps-4 fw-bold">#${order.orderNumber}</td>
            <td>
                <div class="d-flex flex-column">
                    <span class="fw-bold small text-dark">${order.customerName}</span>
                    <span class="text-muted" style="font-size: 0.75rem">${order.customerMobile}</span>
                </div>
            </td>
            <td>${Number(order.totalAmount).toLocaleString()}</td>
            <td class="text-muted small">${order.createDate}</td>
            <td class="text-center">${statusBadge}</td>
            <td class="text-end pe-4">${actionBtn}</td>
        </tr>
    `;
}

let isProcessing = false;

function changeOrderStatus(orderId, nextTitle) {
    if (isProcessing) return;

    // تاییدیه ساده مرورگر (جایگزین Alert)
    if (!confirm(`آیا از تغییر وضعیت به «${nextTitle}» اطمینان دارید؟`)) return;

    isProcessing = true;

    // ارسال درخواست به سرور
    $.ajax({
        url: '/BranchAdmin/Orders/change-status',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(orderId), // ارسال ID به عنوان Body
        success: function (response) {
            showToast(response.message || 'وضعیت سفارش با موفقیت تغییر کرد', 'success');
            loadOrders(); // ریلود جدول
        },
        error: function (xhr) {
            const errorMsg = xhr.responseText || 'خطا در انجام عملیات';
            showToast(errorMsg, 'danger');
        },
        complete: function () {
            isProcessing = false;
        }
    });
}

// --- Toast Notification System (Bootstrap 5) ---

function showToast(message, type = 'primary') {
    const toastContainer = document.getElementById('toastContainer');

    // رنگ‌ها بر اساس تایپ
    let bgClass = 'bg-white';
    let textClass = 'text-dark';
    let icon = '';

    if (type === 'success') {
        bgClass = 'bg-success';
        textClass = 'text-white';
        icon = '<i class="fas fa-check-circle me-2"></i>';
    } else if (type === 'danger') {
        bgClass = 'bg-danger';
        textClass = 'text-white';
        icon = '<i class="fas fa-exclamation-circle me-2"></i>';
    } else if (type === 'warning') {
        bgClass = 'bg-warning';
        textClass = 'text-dark';
    }

    const toastHtml = `
        <div class="toast align-items-center ${textClass} ${bgClass} border-0 mb-2" role="alert" aria-live="assertive" aria-atomic="true">
            <div class="d-flex">
                <div class="toast-body fs-6">
                    ${icon} ${message}
                </div>
                <button type="button" class="btn-close ${type === 'success' || type === 'danger' ? 'btn-close-white' : ''} me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
            </div>
        </div>
    `;

    // اضافه کردن به صفحه
    const tempDiv = document.createElement('div');
    tempDiv.innerHTML = toastHtml;
    const toastElement = tempDiv.firstElementChild;
    toastContainer.appendChild(toastElement);

    // فعال‌سازی با Bootstrap API
    const bsToast = new bootstrap.Toast(toastElement, { delay: 4000 });
    bsToast.show();

    // حذف از DOM بعد از مخفی شدن
    toastElement.addEventListener('hidden.bs.toast', function () {
        toastElement.remove();
    });
}
