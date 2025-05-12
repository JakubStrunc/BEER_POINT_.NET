
function showToast(text, color) {
    Toastify({
        text: text,
        duration: 3000,
        gravity: "right",
        position: "right",
        backgroundColor: color,
        stopOnFocus: true
    }).showToast();
}


document.addEventListener("DOMContentLoaded", function () {
    if (sessionStorage.getItem("showToast") === "true") {
        Toastify({
            text: sessionStorage.getItem("toastText"),
            duration: 3000,
            gravity: "right",
            position: "right",
            backgroundColor: sessionStorage.getItem("toastColor"),
            stopOnFocus: true
        }).showToast();

        sessionStorage.removeItem("showToast");
        sessionStorage.removeItem("toastText");
        sessionStorage.removeItem("toastColor");
    }
});