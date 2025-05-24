// handle the register button click event
$(document).on('click', '#registerButton', function () {
    const username = $('#signupFullnameInput').val().trim();
    const email = $('#signupEmailInput').val().trim();
    const password = $('#signupPasswordInput').val();
    const confirmPassword = $('#signupConfirmPasswordInput').val();

    const errors = [];

    
    if (!username) errors.push("Uživatelské jméno je povinné.");
    if (!email) errors.push("Email je povinný.");
    if (!password) errors.push("Heslo je povinné.");
    if (password !== confirmPassword) errors.push("Hesla se neshodují.");

    
    if (errors.length > 0) {
        showToast(errors.join("<br>"), "#dc3545");
        return;
    }

    // Send AJAX request to register the user
    $.ajax({
        url: '/Home/Register', // URL to the registration endpoint
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            username: username,
            email: email,
            password: password
        }),
        success: function (response) {
            if (response.success) {
                // if registration is successful, show success toast and redirect
                sessionStorage.setItem("showToast", "true");
                sessionStorage.setItem("toastText", "Registrace byla úspěšná!");
                sessionStorage.setItem("toastColor", "#28a745");
                window.location.href = '/'; // Redirect to home page
            } else {
                // show error toast
                showToast(response.message || "Registrace selhala.", "#dc3545");
            }
        },
        error: function () {
            // show error 
            showToast("Chyba při registraci.", "#dc3545");
        }
    });
});