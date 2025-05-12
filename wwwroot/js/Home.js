

$(document).on('click', '.add-to-cart-btn', function (event) {
    event.preventDefault();

    console.log("clicked");
    const produktId = $(this).data('product-id');

    $.ajax({
        url: "/Home/AddProduct",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ id: produktId }),
        success: function (response) {
            if (response.success) {
                showToast("Produkt byl úspěšně přidán do košíku!", "#28a745");
            } else {
                showToast("Produkt již byl přidán do košíku.", "#fd7e14");
            }
        },
        error: function (xhr) {
            showToast("Chyba při přidávání do košíku.", "#dc3545");
            console.error(xhr.responseText);
        }
    });
});
