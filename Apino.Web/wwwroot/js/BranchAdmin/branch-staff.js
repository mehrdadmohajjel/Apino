$(document).ready(function () {
    loadStaff();

    // باز کردن مدال
    $('#btn-add').click(function () {
        $('#frmStaff')[0].reset();
        $('#staff-userId').val('');
        $('#selected-user').addClass('d-none');
        $('#user-search').val('').prop('disabled', false);
        $('#btn-save-staff').prop('disabled', true);

        // تنظیم تاریخ امروز به عنوان پیش‌فرض
        document.getElementById('staff-date').valueAsDate = new Date();

        $('#staffModal').modal('show');
    });

    // جستجوی زنده (Live Search)
    let searchTimeout;
    $('#user-search').on('input', function () {
        const term = $(this).val();
        clearTimeout(searchTimeout);

        if (term.length < 3) {
            $('#search-results').hide();
            return;
        }

        searchTimeout = setTimeout(function () {
            $.get(`/BranchAdmin/Staff/search-users?term=${term}`, function (data) {
                const list = $('#search-results');
                list.empty();

                if (data.length === 0) {
                    list.append('<div class="p-3 text-muted small text-center">کاربری یافت نشد</div>');
                } else {
                    data.forEach(user => {
                        let avatar = user.avatar ? `/images/avatars/${user.avatar}` : '/images/default-avatar.png';
                        list.append(`
                            <div class="search-item d-flex align-items-center" onclick="selectUser(${user.userId}, '${user.displayName}')">
                                <img src="${avatar}" class="rounded-circle me-2" width="30" height="30">
                                <span class="small text-dark">${user.displayName}</span>
                            </div>
                        `);
                    });
                }
                list.show();
            });
        }, 400); // تاخیر ۴۰۰ میلی‌ثانیه برای جلوگیری از درخواست‌های زیاد
    });

    // مخفی کردن لیست جستجو وقتی کلیک بیرون انجام شود
    $(document).click(function (e) {
        if (!$(e.target).closest('#user-search, #search-results').length) {
            $('#search-results').hide();
        }
    });

    // حذف انتخاب کاربر
    $('#btn-remove-selection').click(function () {
        $('#staff-userId').val('');
        $('#selected-user').addClass('d-none');
        $('#user-search').val('').prop('disabled', false).focus();
        $('#btn-save-staff').prop('disabled', true);
    });

    // ذخیره کارمند
    $('#btn-save-staff').click(function () {
        const data = {
            userId: $('#staff-userId').val(),
            role: parseInt($('#staff-role').val()),
            startWorkDate: $('#staff-date').val()
        };

        $.ajax({
            url: '/BranchAdmin/Staff/add',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function () {
                $('#staffModal').modal('hide');
                loadStaff();
                // اینجا بهتر است از SweetAlert یا Toast استفاده کنید
                alert('کارمند با موفقیت اضافه شد');
            },
            error: function (err) {
                alert(err.responseText || 'خطا در انجام عملیات');
            }
        });
    });
});

// تابع انتخاب کاربر از لیست جستجو
function selectUser(id, name) {
    $('#staff-userId').val(id);
    $('#selected-user-name').text(name);

    $('#user-search').val('').prop('disabled', true);
    $('#search-results').hide();
    $('#selected-user').removeClass('d-none');
    $('#btn-save-staff').prop('disabled', false);
}

// لود کردن لیست پرسنل
function loadStaff() {
    const tbody = $('#tblStaff tbody');
    tbody.html('<tr><td colspan="6" class="text-center py-4"><i class="fa fa-spinner fa-spin text-primary"></i> در حال بارگذاری...</td></tr>');

    $.get('/BranchAdmin/Staff/list', function (data) {
        tbody.empty();
        if (data.length === 0) {
            tbody.html('<tr><td colspan="6" class="text-center py-4 text-muted">هنوز کارمندی ثبت نشده است.</td></tr>');
            return;
        }

        data.forEach(item => {
            let avatar = item.avatar ? `/images/avatars/${item.avatar}` : '/images/default-avatar.png';
            let badgeClass = item.role === 2 ? 'bg-warning text-dark' : 'bg-info text-dark';
            let statusBtn = item.isActive
                ? `<button class="btn btn-sm btn-outline-success" onclick="toggleStaff(${item.id})" title="فعال"><i class="fa fa-check"></i></button>`
                : `<button class="btn btn-sm btn-outline-secondary" onclick="toggleStaff(${item.id})" title="غیرفعال"><i class="fa fa-ban"></i></button>`;

            tbody.append(`
                <tr>
                    <td class="ps-4">
                        <div class="d-flex align-items-center">
                            <img src="${avatar}" class="rounded-circle border me-2" width="40" height="40" object-fit: cover;">
                            <span class="fw-bold text-dark">${item.fullName}</span>
                        </div>
                    </td>
                    <td>${item.mobile}</td>
                    <td><span class="badge ${badgeClass} fw-normal px-2 py-1">${item.roleTitle}</span></td>
                    <td class="small text-muted">${item.startDate}</td>
                    <td class="text-center">${item.isActive ? '<span class="badge bg-success bg-opacity-10 text-success rounded-pill">فعال</span>' : '<span class="badge bg-danger bg-opacity-10 text-danger rounded-pill">غیرفعال</span>'}</td>
                    <td class="text-end pe-4">
                        <div class="d-flex justify-content-end gap-1">
                            ${statusBtn}
                            <button class="btn btn-sm btn-outline-danger" onclick="removeStaff(${item.id})" title="حذف دسترسی">
                                <i class="fa fa-trash"></i>
                            </button>
                        </div>
                    </td>
                </tr>
            `);
        });
    });
}

function toggleStaff(id) {
    $.ajax({
        url: '/BranchAdmin/Staff/toggle',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(id),
        success: loadStaff
    });
}

function removeStaff(id) {
    if (confirm('آیا مطمئن هستید؟ دسترسی این کاربر قطع خواهد شد.')) {
        $.ajax({
            url: '/BranchAdmin/Staff/remove',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(id),
            success: loadStaff
        });
    }
}
