// Function to show a toast message
function showToast(text, color) {
    Toastify({
        text: text,
        duration: 3000,
        gravity: "right",
        position: "right",
        backgroundColor: color,
        stopOnFocus: true
    }).showToast(); // display the toast
}

// listen for the DOM to be fully loaded
document.addEventListener("DOMContentLoaded", function () {
    // check if a toast message is stored in session storage
    if (sessionStorage.getItem("showToast") === "true") {
        // show the stored toast message
        Toastify({
            text: sessionStorage.getItem("toastText"),
            duration: 3000,
            gravity: "right",
            position: "right",
            backgroundColor: sessionStorage.getItem("toastColor"), 
            stopOnFocus: true
        }).showToast();

        // remove the toast data from session storage 
        sessionStorage.removeItem("showToast");
        sessionStorage.removeItem("toastText");
        sessionStorage.removeItem("toastColor");
    }
});
