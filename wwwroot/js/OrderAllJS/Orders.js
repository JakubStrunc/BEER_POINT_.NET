

function toggleIcon(id) {
	const icon = document.getElementById("icon-" + id);
	const collapse = document.getElementById("collapse-" + id);

	// Počkej na Bootstrap animaci collapse
	setTimeout(() => {
		const isShown = collapse.classList.contains("show");

		if (isShown) {
			icon.classList.remove("bi-caret-down-fill");
			icon.classList.add("bi-caret-up-fill");
		} else {
			icon.classList.remove("bi-caret-up-fill");
			icon.classList.add("bi-caret-down-fill");
		}
	}, 100); // zpoždění kvůli animaci Bootstrap collapse
}

$(document).on('change', '.update-status', function () {
    const orderId = $(this).data('order-id');
    const newStatus = $(this).val();

    $.ajax({
        url: "/OrdersManaging/UpdateOrderStatus",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({
            id: orderId,
            stav: newStatus
        }),
        
        success: function (response) {
            if (response.success) {
                showToast("Stav objednávky aktualizován.", "#28a745");
            } else {
                showToast("Chyba: " + response, "#dc3545");
            }
        },
        error: function (xhr) {
            sshowToast("Chyba při aktualizaci stavu.", "#dc3545");
        }
    });
});
