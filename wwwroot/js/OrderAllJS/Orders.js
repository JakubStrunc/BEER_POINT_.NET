// toggle the icon when the collapse section is shown or hidden
function toggleIcon(id) {
    const icon = document.getElementById("icon-" + id); 
    const collapse = document.getElementById("collapse-" + id); 

    // wait for bootstrap's collapse animation to complete
    setTimeout(() => {
        const isShown = collapse.classList.contains("show");

        if (isShown) {
            icon.classList.remove("bi-caret-down-fill"); 
            icon.classList.add("bi-caret-up-fill");
        } else {
            icon.classList.remove("bi-caret-up-fill"); 
            icon.classList.add("bi-caret-down-fill");
        }
    }, 100); 
}

// handle the update of order status 
$(document).on('change', '.update-status', function () {
    const orderId = $(this).data('order-id'); 
    const newStatus = $(this).val(); 

    // send AJAX request 
    $.ajax({
        url: "/OrdersManaging/UpdateOrderStatus", // URL to update the order status
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({
            id: orderId, 
            stav: newStatus 
        }),

        success: function (response) {
            if (response.success) {
                // success message
                showToast("Stav objednávky aktualizován.", "#28a745"); 
            } else {
                showToast("Chyba: " + response, "#dc3545"); 
            }
        },
        error: function (xhr) {
            // error handling
            showToast("Chyba při aktualizaci stavu.", "#dc3545"); 
        }
    });
});