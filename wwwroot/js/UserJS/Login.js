// Handle the login button click event
$(document).on('click', '#prihlasit', function (event) {
    event.preventDefault();

    $('.text-danger').remove();

    // Send AJAX request to login
    $.ajax({
        url: '/Home/Login', // URL for the login 
        type: 'POST', 
        contentType: 'application/json', 
        data: JSON.stringify({
            Email: $('#signinEmailInput').val(), 
            Password: $('#signinPassInput').val() 
        }),
        success: function (response) {
            if (response.success) {
                // if login is successful, store toast info for displaying after reload
                sessionStorage.setItem("showToast", "true");
                sessionStorage.setItem("toastText", "Přihlášení bylo úspěšné!");
                sessionStorage.setItem("toastColor", "#28a745");

                window.location.href = '/';
            } else {
                // show error
                showToast("Chyba: " + response, "#dc3545"); 
            }
        },
        error: function (xhr) {
            // show error if AJAX request fails
            showToast("Chyba při přihlášení: " + xhr.responseText, "#dc3545"); 
        }
    });
});


