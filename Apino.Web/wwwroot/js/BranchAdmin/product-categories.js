$(document).ready(function () {
    // ۱. لود لیست به محض ورود
    loadCategories();

    // باز کردن مدال افزودن
    $('#btn-add').click(() => {
        clearForm();
        $('#catModal').modal('show');
    });

    // ذخیره اطلاعات
    $('#save-cat').click(saveCategory);
});

function loadCategories() {
    $.get('/BranchAdmin/Categories/list', function (data) {
        const tbody = $('#tblCategories tbody');
        tbody.empty();

        if (!data || data.length === 0) {
            tbody.append('<tr><td colspan="6" class="text-center py-3">هیچ رکوردی یافت نشد</td></tr>');
            return;
        }

        data.forEach(x => {
            // تصویر
            let imgUrl = x.iconName
                ? `/images/category/${x.iconName}`
                : '/images/placeholder.png'; // حتما یک عکس placeholder.png در پوشه images داشته باشید

            // وضعیت (به صورت بج یا آیکون برای اشغال فضای کمتر)
            let statusBadge = x.isActive
                ? '<span class="badge bg-success rounded-pill"><i class="fa fa-check"></i></span>'
                : '<span class="badge bg-danger rounded-pill"><i class="fa fa-times"></i></span>';

            // آیکون پرداخت در محل
            let payIcon = x.payAtPlace
                ? '<i class="fa fa-check-circle text-success fs-5"></i>'
                : '<i class="fa fa-times-circle text-muted fs-5"></i>';

            tbody.append(`
                <tr>
                    <td>
                        <img src="${imgUrl}" alt="${x.categoryTitle}" 
                             class="rounded-3 border" 
                             style="width: 40px; height: 40px; object-fit: cover;" />
                    </td>
                    
                    <td class="fw-bold text-dark">
                        ${x.categoryTitle}
                        <!-- نمایش اسلاگ به صورت زیرنویس فقط در موبایل -->
                        <div class="d-block d-md-none text-muted small" style="font-size: 0.75rem;">
                            ${x.slug ? x.slug.substring(0, 15) + '...' : ''}
                        </div>
                    </td>

                    <!-- این دو ستون در موبایل مخفی می‌شوند -->
                    <td class="d-none d-md-table-cell text-muted small">${x.slug || '-'}</td>
                    <td class="d-none d-md-table-cell text-center">${payIcon}</td>

                    <td class="text-center">${statusBadge}</td>
                    
                    <td class="text-end pe-2">
                        <div class="d-flex justify-content-end gap-1">
                            <button class="btn btn-sm btn-outline-primary" onclick='editCategory(${JSON.stringify(x)})' title="ویرایش">
                                <i class="fa fa-edit"></i>
                            </button>
                            <button class="btn btn-sm btn-outline-danger" onclick="deleteCategory(${x.id})" title="تغییر وضعیت">
                                <i class="fa fa-power-off"></i>
                            </button>
                        </div>
                    </td>
                </tr>
            `);
        });
    }).fail(function () {
        // هندلینگ خطا بدون alert مزاحم
        $('#tblCategories tbody').html('<tr><td colspan="6" class="text-center text-danger">خطا در بارگیری اطلاعات</td></tr>');
    });
}

// بقیه توابع (saveCategory, editCategory, ...) بدون تغییر باقی می‌مانند
// فقط مطمئن شوید توابع saveCategory و ... که در پاسخ قبلی دادم را دارید.

function saveCategory() {
    // ۲. استفاده از FormData برای ارسال فایل و متن با هم
    var formData = new FormData();

    formData.append('Id', $('#cat-id').val());
    formData.append('CategoryTitle', $('#cat-title').val());
    formData.append('PayAtPlace', $('#cat-pay').is(':checked'));
    formData.append('IsActive', $('#cat-active').is(':checked'));

    // دریافت فایل
    var fileInput = $('#cat-icon')[0];
    if (fileInput.files.length > 0) {
        formData.append('Icon', fileInput.files[0]);
    }

    $.ajax({
        url: '/BranchAdmin/Categories/Save', // آدرس اصلاح شده
        type: 'POST',
        data: formData,
        // این دو خط برای ارسال فایل با AJAX ضروری هستند:
        processData: false,
        contentType: false
    }).done(() => {
        $('#catModal').modal('hide');
        loadCategories(); // رفرش لیست
        clearForm();
    }).fail((xhr) => {
        console.error(xhr);
        alert('خطا در ذخیره‌سازی. لطفا ورودی‌ها را بررسی کنید.');
    });
}

// تابع کمکی برای پر کردن فرم جهت ویرایش
function editCategory(item) {
    $('#cat-id').val(item.id);
    $('#cat-title').val(item.categoryTitle);
    $('#cat-pay').prop('checked', item.payAtPlace);
    $('#cat-active').prop('checked', item.isActive);

    // نکته: فایل را نمیتوان در input file ست کرد (امنیت مرورگر)
    $('#cat-icon').val('');

    $('#catModal').modal('show');
}

function deleteCategory(id) {
    if (!confirm('آیا از تغییر وضعیت این آیتم اطمینان دارید؟')) return;

    $.ajax({
        url: '/BranchAdmin/Categories/Delete', // آدرس اصلاح شده
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(id)
    }).done(loadCategories);
}

function clearForm() {
    $('#cat-id').val('0');
    $('#cat-title').val('');
    $('#cat-icon').val('');
    $('#cat-pay').prop('checked', false);
    $('#cat-active').prop('checked', true);
}
