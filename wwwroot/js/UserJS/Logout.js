$(document).on('click', '#logoutButton', function (event) {
    event.preventDefault();

    $.ajax({
        url: '/Home/Logout',
        type: 'POST',
        contentType: 'application/json',
        success: function (response) {
            if (response.success) {
                // Store toast info for showing after reload
                sessionStorage.setItem("showToast", "true");
                sessionStorage.setItem("toastText", "Odhlášení proběhlo úspěšně!");
                sessionStorage.setItem("toastColor", "#28a745");

                // Redirect to homepage after logout
                window.location.href = '/';
            } else {
                showToast("Error during logout: " + response.message, "#dc3545");
            }
        },
        error: function (xhr) {
            showToast("Error during logout: " + xhr.responseText, "#dc3545");
        }
    });
});
