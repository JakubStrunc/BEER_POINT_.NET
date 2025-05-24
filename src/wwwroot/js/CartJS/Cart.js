// function to update product quantity
function updateQuantityInDatabase(productId, newQuantity) {
    $.ajax({
        url: "/Cart/UpdateQuantity", // URL to update product quantity in the cart
        method: "POST",
        contentType: "application/json",
        data: JSON.stringify({
            productId: productId,
            newQuantity: newQuantity
        }),
        success: function () {
            //  update the product total price
            updateProductTotal(productId);
        },
        error: function (xhr) {
            // show error message 
            showToast("Chyba při aktualizaci množství", "#dc3545");
        }
    });
}

// function to update the product's total price based on quantity
function updateProductTotal(productId) {
    const input = $(`#quantity${productId}`);
    const count = parseInt(input.val());
    const price = parseFloat(input.data('price'));
    let total = count * price;
    if (isNaN(total)) total = 0; 

    // update displayed total price for product
    $(`#productTotal${productId}`).html(`<h5 class="mb-0 text-success">${total} Kč</h5>`);
    updateOrderTotal(); 
}

// function to update total price for entire order
function updateOrderTotal() {
    let total = 0;
    $('.product-quantity').each(function () {
        const count = parseInt($(this).val());
        const price = parseFloat($(this).data('price'));
        total += count * price;
        if (isNaN(total)) total = 0; 
    });

    
    $('#orderTotal').text(`${total} Kč`);
}

// event handler for increasing the quantity of a product
$(document).on('click', '.btn-plus', function () {
    const productId = $(this).attr('id').replace('btnPlus', ''); 
    const input = $(`#quantity${productId}`);
    const newValue = parseInt(input.val()) + 1;
    input.val(newValue); 
    updateQuantityInDatabase(productId, newValue); 
});

// event handler for decreasing the quantity of a product
$(document).on('click', '.btn-minus', function () {
    const productId = $(this).attr('id').replace('btnMinus', '');
    const input = $(`#quantity${productId}`);
    const current = parseInt(input.val());
    if (current > 1) { 
        const newValue = current - 1;
        input.val(newValue);
        updateQuantityInDatabase(productId, newValue); 
    }
});

// event handler for manual input of product quantity
$(document).on('input', 'input.product-quantity', function () {
    const input = $(this);
    const productId = input.data('product-id');
    const newQuantity = parseInt(input.val());

    if (isNaN(newQuantity) || newQuantity < 1) {
        updateQuantityInDatabase(productId, 1); // if invalid input, reset to 1
        return;
    }

    updateQuantityInDatabase(productId, newQuantity); // Update quantity in database
});

// event handler for removing a product from the cart
$(document).on('click', '.remove-product-btn', function () {
    const productId = $(this).data('product-id'); // get the product ID from the button

    $.ajax({
        url: "/Cart/RemoveProduct", // URL to remove product from cart
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ id: productId }), 
        success: function () {
            // remove the product element
            $(`#product${productId}`).remove();
            updateOrderTotal(); 
            showToast("Produkt byl odebrán!", "#28a745");

            // if the cart is empty, display change contain
            if ($('.product-quantity').length === 0) {
                $('.card.shadow-sm').html(`
                    <div class="d-flex justify-content-center align-items-center flex-column" style="height: 100%;">
                        <h3>HMM...</h3>
                        <p>Vypadá to, že košík je prázdný.</p>
                    </div>
                `);
            }
        },
        error: function (xhr) {
            // show error message 
            showToast("Chyba při odebírání produktu", "#dc3545");
        }
    });
});
