let currentPage = 1;

$(document).ready(function () {
    // ابتدا دراپ‌داون‌ها لود شوند تا فیلترها پر شوند
    loadDropdowns().then(() => {
        loadProducts(currentPage);
    });

    $('#btn-add').click(() => {
        clearForm();
        $('#modalTitle').text('افزودن محصول جدید');
        $('#productModal').modal('show');
    });

    $('#btn-save').click(saveProduct);

    // رویداد تغییر فیلترها: هر وقت عوض شد، برو به صفحه ۱ و دوباره لود کن
    $('#filter-category, #filter-serviceType').change(function () {
        loadProducts(1);
    });
});

// --- بخش دراپ‌داون‌ها ---
let dropdownCache = null;

function loadDropdowns() {
    return new Promise((resolve, reject) => {
        if (dropdownCache) {
            populateAllSelects(dropdownCache);
            resolve();
            return;
        }

        $.get('/BranchAdmin/Products/form-data', function (data) {
            dropdownCache = data;
            populateAllSelects(data);
            resolve();
        }).fail(reject);
    });
}
function populateAllSelects(data) {
    // لیست تمام سلکت‌هایی که باید پر شوند (هم فیلتر هم فرم)
    const targets = [
        { select: $('#p-category'), data: data.categories, defaultText: 'انتخاب کنید...' },
        { select: $('#filter-category'), data: data.categories, defaultText: 'همه دسته‌بندی‌ها' },
        { select: $('#p-serviceType'), data: data.serviceTypes, defaultText: 'انتخاب کنید...' },
        { select: $('#filter-serviceType'), data: data.serviceTypes, defaultText: 'همه خدمات' }
    ];

    targets.forEach(t => {
        const el = t.select;
        // فقط اگر خالی هستند پر کن (به جز گزینه اول)
        if (el.children('option').length <= 1) {
            el.empty(); // اطمینان از خالی بودن
            el.append(`<option value="">${t.defaultText}</option>`);

            t.data.forEach(item => {
                // برای category فیلد categoryTitle و برای serviceType فیلد title
                const text = item.categoryTitle || item.title;
                el.append(`<option value="${item.id}">${text}</option>`);
            });
        }
    });
}
// --- بخش بارگذاری محصولات و صفحه‌بندی ---
function loadProducts(page) {
    currentPage = page;
    const tbody = $('#tblProducts tbody');
    tbody.html('<tr><td colspan="7" class="text-center text-muted py-3"><i class="fa fa-spinner fa-spin"></i> در حال بارگذاری...</td></tr>');

    // خواندن مقادیر فیلتر
    const catId = $('#filter-category').val();
    const srvId = $('#filter-serviceType').val();

    // ساخت URL با پارامترهای فیلتر
    let url = `/BranchAdmin/Products/list?page=${page}`;
    if (catId) url += `&categoryId=${catId}`;
    if (srvId) url += `&serviceTypeId=${srvId}`;

    $.get(url, function (response) {
        tbody.empty();

        const data = response.items;
        const totalPages = response.totalPages;

        if (!data || data.length === 0) {
            tbody.append('<tr><td colspan="7" class="text-center py-4 text-muted">هیچ محصولی با این مشخصات یافت نشد</td></tr>');
            $('#pagination-container').empty();
            return;
        }

        data.forEach(item => {
            let imgUrl = item.imageName ? `/images/products/${item.imageName}` : '/images/placeholder.png';
            let statusBadge = item.isActive
                ? '<span class="badge bg-success bg-opacity-10 text-success px-3 rounded-pill">فعال</span>'
                : '<span class="badge bg-danger bg-opacity-10 text-danger px-3 rounded-pill">غیرفعال</span>';

            tbody.append(`
                <tr>
                    <td class="ps-4">
                        <img src="${imgUrl}" class="rounded-3 shadow-sm bg-white border" 
                             style="width: 50px; height: 50px; object-fit: contain;">
                    </td>
                    <td>
                        <div class="fw-bold text-dark">${item.title}</div>
                        <div class="d-md-none text-muted small" style="font-size:0.75rem">
                           ${item.categoryTitle}
                        </div>
                    </td>
                    <td class="d-none d-md-table-cell text-muted small">${item.categoryTitle}</td>
                    <td class="d-none d-lg-table-cell text-muted"><span class="badge bg-light text-dark border fw-normal">${item.serviceTypeTitle}</span></td>
                    <td>
                        <div class="d-flex flex-column">
                            <span class="fw-bold fs-6">${parseInt(item.price).toLocaleString()} <span class="small text-muted fw-normal" style="font-size:0.7rem">تومان</span></span>
                            <span class="small text-muted" style="font-size:0.75rem">موجودی: ${item.stock}</span>
                        </div>
                    </td>
                    <td class="text-center">${statusBadge}</td>
                    <td class="text-end pe-4">
                        <div class="d-flex justify-content-end gap-2">
                            <button class="btn btn-sm btn-outline-primary" onclick='editProduct(${JSON.stringify(item)})' title="ویرایش">
                                <i class="fa fa-pen"></i>
                            </button>
                            <button class="btn btn-sm btn-outline-danger" onclick="toggleProduct(${item.id})" title="تغییر وضعیت">
                                <i class="fa fa-power-off"></i>
                            </button>
                        </div>
                    </td>
                </tr>
            `);
        });

        renderPagination(response.currentPage, response.totalPages);
    });
}
function renderPagination(current, total) {
    const container = $('#pagination-container');
    container.empty();

    if (total <= 1) return;

    // دکمه قبلی
    let prevClass = current === 1 ? 'disabled' : '';
    container.append(`
        <li class="page-item ${prevClass}">
            <a class="page-link" href="#" onclick="loadProducts(${current - 1}); return false;">&laquo;</a>
        </li>
    `);

    // محاسبه بازه نمایش (مثلا اگر صفحه 10 هستیم: 8، 9، 10، 11، 12 را نشان بده)
    let startPage = Math.max(1, current - 2);
    let endPage = Math.min(total, current + 2);

    if (startPage > 1) {
        container.append(`<li class="page-item"><a class="page-link" href="#" onclick="loadProducts(1); return false;">1</a></li>`);
        if (startPage > 2) container.append(`<li class="page-item disabled"><span class="page-link">...</span></li>`);
    }

    for (let i = startPage; i <= endPage; i++) {
        let activeClass = i === current ? 'active' : '';
        container.append(`
            <li class="page-item ${activeClass}">
                <a class="page-link" href="#" onclick="loadProducts(${i}); return false;">${i}</a>
            </li>
        `);
    }

    if (endPage < total) {
        if (endPage < total - 1) container.append(`<li class="page-item disabled"><span class="page-link">...</span></li>`);
        container.append(`<li class="page-item"><a class="page-link" href="#" onclick="loadProducts(${total}); return false;">${total}</a></li>`);
    }

    // دکمه بعدی
    let nextClass = current === total ? 'disabled' : '';
    container.append(`
        <li class="page-item ${nextClass}">
            <a class="page-link" href="#" onclick="loadProducts(${current + 1}); return false;">&raquo;</a>
        </li>
    `);
}

// --- بخش ذخیره سازی ---
function saveProduct() {
    if (!$('#p-title').val() || !$('#p-price').val() || !$('#p-category').val() || !$('#p-serviceType').val()) {
        alert('لطفا فیلدهای اجباری (عنوان، قیمت، دسته و نوع خدمت) را پر کنید.');
        return;
    }

    const formData = new FormData();
    const id = $('#p-id').val();
    formData.append('Id', id);
    formData.append('Title', $('#p-title').val());
    formData.append('ProductCategoryId', $('#p-category').val());
    formData.append('ServiceTypeId', $('#p-serviceType').val());
    formData.append('Price', $('#p-price').val());
    formData.append('Stock', $('#p-stock').val());
    formData.append('Description', $('#p-desc').val());
    formData.append('IsActive', $('#p-active').is(':checked'));

    const fileInput = $('#p-image')[0];
    if (fileInput.files.length > 0) {
        formData.append('Image', fileInput.files[0]);
    }

    const btn = $('#btn-save');
    const originalText = btn.html();
    btn.prop('disabled', true).html('<i class="fa fa-spinner fa-spin"></i> ...');

    $.ajax({
        url: '/BranchAdmin/Products/save',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false
    }).done(() => {
        $('#productModal').modal('hide');
        // اگر آیتم جدید بود به صفحه 1 برو، اگر ویرایش بود همان صفحه را رفرش کن
        if (id == 0) loadProducts(1);
        else loadProducts(currentPage);
    }).fail((xhr) => {
        console.error(xhr);
        alert('خطا در ذخیره سازی.');
    }).always(() => {
        btn.prop('disabled', false).html(originalText);
    });
}

// --- بخش ویرایش ---
async function editProduct(item) {
    await loadDropdowns();

    $('#modalTitle').text('ویرایش محصول');
    $('#p-id').val(item.id);
    $('#p-title').val(item.title);
    $('#p-price').val(item.price);
    $('#p-stock').val(item.stock);
    $('#p-desc').val(item.description);
    $('#p-active').prop('checked', item.isActive);
    $('#p-category').val(item.productCategoryId);
    $('#p-serviceType').val(item.serviceTypeId);

    if (item.imageName) {
        $('#p-img-preview').attr('src', `/images/products/${item.imageName}`);
    } else {
        $('#p-img-preview').attr('src', '/images/placeholder.png');
    }
    $('#p-image').val(''); // پاک کردن اینپوت فایل

    $('#productModal').modal('show');
}

// --- بخش تغییر وضعیت و پاکسازی ---
function toggleProduct(id) {
    if (!confirm("آیا از تغییر وضعیت این محصول اطمینان دارید؟")) return;

    $.ajax({
        url: '/BranchAdmin/Products/toggle',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(id)
    }).done(() => {
        loadProducts(currentPage); // ماندن در همان صفحه
    });
}

function clearForm() {
    $('#p-id').val(0);
    $('#p-title').val('');
    $('#p-category').val('');
    $('#p-serviceType').val('');
    $('#p-price').val('');
    $('#p-stock').val(100);
    $('#p-desc').val('');
    $('#p-active').prop('checked', true);
    $('#p-image').val('');
    $('#p-img-preview').attr('src', '/images/placeholder.png');
}
