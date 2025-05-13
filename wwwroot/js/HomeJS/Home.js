// handle clicking on add to cart button
$(document).on('click', '.add-to-cart-btn', function (event) {
    event.preventDefault(); 

    const produktId = $(this).data('product-id'); 

    // send AJAX request 
    $.ajax({
        url: "/Home/AddProduct", // URL for adding product to cart
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ id: produktId }), 
        success: function (response) {
            // show success or warning message based on response
            if (response.success) {
                showToast("Produkt byl úspěšně přidán do košíku!", "#28a745"); 
            } else {
                showToast("Produkt již byl přidán do košíku.", "#fd7e14"); 
            }
        },
        error: function (xhr) {
            // error message
            showToast("Chyba při přidávání do košíku.", "#dc3545"); 
            console.error(xhr.responseText);
        }
    });
});
