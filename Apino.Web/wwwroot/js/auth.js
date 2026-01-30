console.log("auth.js loaded");

/* =========================
   SweetAlert Toast Config
========================= */
const Toast = Swal.mixin({
    toast: true,
    position: 'top-end',
    showConfirmButton: false,
    timer: 3000,
    timerProgressBar: true
});

/* =========================
   Bootstrap Modal Instance
========================= */
let loginModal;
let otpTimerInterval;
let otpSecondsLeft = 120;

/* =========================
   Refresh Token Handler
========================= */
let isRefreshingToken = false;
let failedQueue = [];

function processQueue(error, token = null) {
    failedQueue.forEach(promise => {
        if (error) promise.reject(error);
        else promise.resolve(token);
    });
    failedQueue = [];
}

function refreshTokenAndRetry() {
    return new Promise((resolve, reject) => {
        const refreshToken = localStorage.getItem('refreshToken');
        if (!refreshToken) {
            logout();
            return reject();
        }

        if (isRefreshingToken) {
            failedQueue.push({ resolve, reject });
            return;
        }

        isRefreshingToken = true;

        $.ajax({
            url: '/auth/refresh',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ refreshToken })
        })
            .done(res => {
                localStorage.setItem('accessToken', res.accessToken);
                localStorage.setItem('refreshToken', res.refreshToken);

                isRefreshingToken = false;
                processQueue(null, res.accessToken);
                resolve(res.accessToken);
            })
            .fail(err => {
                isRefreshingToken = false;
                processQueue(err, null);
                logout();
                reject(err);
            });
    });
}

/* =========================
   Global AJAX Setup
========================= */
$.ajaxSetup({
    beforeSend: function (xhr) {
        const token = localStorage.getItem('accessToken');
        if (token) {
            xhr.setRequestHeader('Authorization', 'Bearer ' + token);
        }
    }
});

$(document).ajaxError(function (event, jqXHR, ajaxSettings) {
    if (jqXHR.status === 401 && !ajaxSettings._retry) {
        ajaxSettings._retry = true;

        refreshTokenAndRetry().then(newToken => {
            ajaxSettings.headers = ajaxSettings.headers || {};
            ajaxSettings.headers['Authorization'] = 'Bearer ' + newToken;
            $.ajax(ajaxSettings);
        });
    }
});

/* =========================
   DOM Ready
========================= */
document.addEventListener('DOMContentLoaded', () => {
    const modalEl = document.getElementById('loginModal');
    if (modalEl) {
        loginModal = new bootstrap.Modal(modalEl, { backdrop: 'static', keyboard: false });
    }

    const username = localStorage.getItem('username');
    const role = localStorage.getItem('role');
    renderUserMenu(username, role);

    $(document).on('click', '#login-btn', openLoginModal);
    $(document).on('click', '#send-otp-btn', () => sendOtp($('#mobile').val()));
    $(document).on('click', '#verify-otp-btn', () => verifyOtp($('#mobile').val(), getOtpCode()));
    $(document).on('click', '#resend-otp-btn', () => sendOtp($('#mobile').val()));

    $(document).on('input', '.otp-input', function () {
        if (this.value.length === 1) $(this).next('.otp-input').focus();
    });

    $(document).on('keydown', '.otp-input', function (e) {
        if (e.key === 'Backspace' && !this.value) $(this).prev('.otp-input').focus();
    });

    renderOtpInputs();
});

/* =========================
   Modal Helpers
========================= */
function openLoginModal() {
    loginModal?.show();
}

function closeLoginModal() {
    loginModal?.hide();
    clearOtpTimer();
    resetOtpInputs();
    $('#mobile').prop('disabled', false);
    $('#send-otp-btn').show();
    $('#otpSection').addClass('d-none');
}

/* =========================
   OTP Helpers
========================= */
function openOtpStep() {
    $('#otpSection').removeClass('d-none');
    $('#send-otp-btn').hide();
    $('#mobile').prop('disabled', true);
    startOtpTimer();
}

function startOtpTimer() {
    otpSecondsLeft = 120;
    $('#otp-timer, #resend-otp-btn').remove();
    $('#otpSection').append('<div id="otp-timer" class="mt-2 text-muted">02:00</div>');

    clearInterval(otpTimerInterval);
    otpTimerInterval = setInterval(() => {
        otpSecondsLeft--;
        const m = String(Math.floor(otpSecondsLeft / 60)).padStart(2, '0');
        const s = String(otpSecondsLeft % 60).padStart(2, '0');
        $('#otp-timer').text(`${m}:${s}`);

        if (otpSecondsLeft <= 0) {
            clearInterval(otpTimerInterval);
            $('#otp-timer').text('کد منقضی شد');
            if (!$('#resend-otp-btn').length) {
                $('#otpSection').append('<button class="btn btn-link mt-2" id="resend-otp-btn">ارسال مجدد</button>');
            }
        }
    }, 1000);
}

function clearOtpTimer() {
    clearInterval(otpTimerInterval);
}

function renderOtpInputs() {
    $('#otpInputs').html(
        Array.from({ length: 5 })
            .map(() => `<input type="text" maxlength="1" class="otp-input form-control text-center mx-1" style="width:50px">`)
            .join('')
    );
}

function resetOtpInputs() {
    $('.otp-input').val('');
}

function getOtpCode() {
    return $('.otp-input').map((_, el) => $(el).val()).get().join('');
}

/* =========================
   Auth Requests
========================= */
function sendOtp(mobile) {
    if (!mobile || mobile.length < 10) {
        return Toast.fire({ icon: 'warning', title: 'شماره معتبر نیست' });
    }

    Swal.fire({ title: 'ارسال کد...', didOpen: () => Swal.showLoading() });

    $.ajax({
        url: '/auth/send-otp',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ mobile })
    })
        .done(() => {
            Swal.close();
            Toast.fire({ icon: 'success', title: 'کد ارسال شد' });
            renderOtpInputs();
            openOtpStep();
        })
        .fail(e => {
            Swal.close();
            Toast.fire({ icon: 'error', title: e.responseJSON?.message || 'ارسال ناموفق' });
        });
}

function verifyOtp(mobile, code) {
    if (code.length !== 5) {
        return Toast.fire({ icon: 'warning', title: 'کد ۵ رقمی وارد کنید' });
    }

    Swal.fire({ title: 'در حال ورود...', didOpen: () => Swal.showLoading() });

    $.ajax({
        url: '/auth/verify-otp',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ mobile, code })
    }).done(res => {
        Swal.close();

        localStorage.setItem('accessToken', res.accessToken);
        localStorage.setItem('refreshToken', res.refreshToken);
        localStorage.setItem('username', res.username || mobile);
        localStorage.setItem('role', res.role || 'User');

        renderUserMenu(res.username || mobile, res.role || 'User');
        mergeGuestCart();
    });
}

/* =========================
   UI Helpers
========================= */
function renderUserMenu(username, role) {
    const el = $('.user-menu');
    if (!username) {
        return el.html(`<li class="nav-item"><a class="nav-link" id="login-btn">ورود / عضویت</a></li>`);
    }

    el.html(`
        <li class="nav-item dropdown">
            <a class="nav-link dropdown-toggle" data-bs-toggle="dropdown">${username}</a>
            <ul class="dropdown-menu">
                ${role === 'SysAdmin' ? '<li><a href="/sysAdmin">پنل مدیریت</a></li>' : ''}
                ${role === 'BranchAdmin' ? '<li><a href="/branchAdmin">مدیریت شعبه</a></li>' : ''}
                <li><a href="/profile">پروفایل</a></li>
                <li><a href="javascript:void(0)" onclick="logout()">خروج</a></li>
            </ul>
        </li>
    `);
}

/* =========================
   Merge Guest Cart (FINAL)
========================= */
function mergeGuestCart() {
    $.post('/cart/merge').always(() => {

        const pendingProductId = sessionStorage.getItem('pendingProductId');
        const pendingPayAtPlace = sessionStorage.getItem('pendingPayAtPlace') === 'true';

        closeLoginModal();

        if (pendingProductId) {
            sessionStorage.removeItem('pendingProductId');
            sessionStorage.removeItem('pendingPayAtPlace');
            addToCart(pendingProductId, pendingPayAtPlace);
        } else {
            setTimeout(() => {
                Toast.fire({ icon: 'success', title: 'ورود موفق 🎉' });
            }, 300);
        }

        $(document).trigger('cart:afterLogin');
    });
}

function logout() {
    $.post({
        url: '/auth/logout',
        contentType: 'application/json',
        data: JSON.stringify({ refreshToken: localStorage.getItem('refreshToken') })
    }).always(() => {
        localStorage.clear();
        renderUserMenu(null, null);
        location.reload();
    });
}
