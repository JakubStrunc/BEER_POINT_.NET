// handle the logout button click event
$(document).on('click', '#logoutButton', function (event) {
    event.preventDefault();

    // Send AJAX request to logout
    $.ajax({
        url: '/Home/Logout',
        type: 'POST',
        contentType: 'application/json',
        success: function (response) {
            if (response.success) {
                // if logout is successful, store toast info for displaying after reload
                sessionStorage.setItem("showToast", "true");
                sessionStorage.setItem("toastText", "Odhlášení proběhlo úspěšně!");
                sessionStorage.setItem("toastColor", "#28a745");

                window.location.href = '/';
            } else {
                // show error
                showToast("Error during logout: " + response.message, "#dc3545");
            }
        },
        error: function (xhr) {
            // show error if AJAX request fails
            showToast("Error during logout: " + xhr.responseText, "#dc3545");
        }
    });
});
