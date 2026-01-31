$(document).ready(function () {

    loadNotifications();

    // اعمال فیلتر
    $('#apply-filter').on('click', function () {
        loadNotifications();
    });

    // سرچ با Enter
    $('#filter-search').on('keypress', function (e) {
        if (e.which === 13) {
            loadNotifications();
        }
    });

    // صفحه‌بندی
    $(document).on('click', '.notification-page', function () {
        const page = $(this).data('page');
        loadNotifications(page);
    });

    // Mark All Read
    $('#mark-all-read').on('click', function () {

        Swal.fire({
            title: 'همه اعلان‌ها خوانده شوند؟',
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: 'بله',
            cancelButtonText: 'خیر'
        }).then(res => {

            if (!res.isConfirmed) return;

            $.post('/Notification/MarkAllRead')
                .done(() => {
                    Toast.fire({
                        icon: 'success',
                        title: 'همه اعلان‌ها خوانده شدند'
                    });
                    loadNotifications();
                    updateNotificationBadge(0);
                });
        });
    });

});

/* ========================== */

function loadNotifications(page = 1) {

    const isRead = $('#filter-isread').val();
    const search = $('#filter-search').val();

    $('#notification-table-wrapper').html(
        '<div class="text-center py-5">در حال بارگذاری...</div>'
    );

    $.get('/Notification/List', {
        isRead: isRead,
        search: search,
        page: page
    })
        .done(html => {
            $('#notification-table-wrapper').html(html);
        });
}
