$(document).on('click', '#prihlasit', function (event) {
    event.preventDefault();

    $('.text-danger').remove();

    $.ajax({
        url: '/Home/Login',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            Email: $('#signinEmailInput').val(),
            Password: $('#signinPassInput').val()
        }),
        success: function (response) {
            if (response.success) {
                // Uložíme info pro toast po reloadu
                sessionStorage.setItem("showToast", "true");
                sessionStorage.setItem("toastText", "Přihlášení bylo úspěšné!");
                sessionStorage.setItem("toastColor", "#28a745");
                window.location.href = '/';
            } else {
                showToast("Chyba: " + response, "#dc3545");
            }
        },
        error: function (xhr) {
            showToast("Chyba při přihlášení: " + xhr.responseText, "#dc3545");
        }
    });
});


