$(document).on('click', '.qty-plus, .qty-minus', function () {

    const container = $(this).closest('.input-counter');
    const productId = container.data('id');
    const qtyEl = container.find('.qty-value');

    let oldQty = parseInt(qtyEl.text());
    let newQty = oldQty;

    if ($(this).hasClass('qty-plus')) newQty++;
    else newQty--;

    if (newQty < 1) return;

    // 🔥 Optimistic UI
    qtyEl.text(newQty);

    $.ajax({
        url: '/cart/update-qty',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            productId,
            quantity: newQty
        })
    })
        .done(res => {
            $(`#item-total-${productId}`).text(res.itemTotal.toLocaleString() + ' تومان');
            $('#sub-total').text(res.subTotal.toLocaleString() + ' تومان');
            $('#tax-amount').text(res.taxAmount.toLocaleString() + ' تومان');
            $('#grand-total').text(res.grandTotal.toLocaleString() + ' تومان');

            updateCartBadge(res.count);
            localStorage.setItem('cartCount', res.count);
        })
        .fail(xhr => {
            // ⛔ rollback UI
            qtyEl.text(oldQty);

            Toast.fire({
                icon: 'error',
                title: xhr.responseJSON?.message || 'خطا در بروزرسانی'
            });
        });
});

$(document).on('click', '.remove-item', function () {

    const productId = $(this).data('id');
    const row = $(this).closest('tr');

    Swal.fire({
        title: 'حذف شود؟',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'بله',
        cancelButtonText: 'خیر'
    }).then(result => {

        if (!result.isConfirmed) return;

        $.ajax({
            url: '/cart/remove',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ productId })
        })
            .done(res => {
                row.fadeOut(200, () => row.remove());

                updateCartBadge(res.count);
                localStorage.setItem('cartCount', res.count);

                Toast.fire({ icon: 'success', title: 'محصول حذف شد' });

                if (res.count === 0) {
                    location.reload();
                }
            })
            .fail(() => {
                Toast.fire({ icon: 'error', title: 'خطا در حذف محصول' });
            });
    });
});
