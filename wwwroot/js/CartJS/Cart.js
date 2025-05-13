function updateQuantityInDatabase(productId, newQuantity) {

   // console.log(` Odesílám množství pro produkt ID ${productId}: ${newQuantity}`);

    $.ajax({
        url: "/Cart/UpdateQuantity",
        method: "POST",
        contentType: "application/json",
        data: JSON.stringify({
            productId: productId,
            newQuantity: newQuantity
        }),
        success: function () {
            updateProductTotal(productId);
        },
        error: function (xhr) {
            showToast("Chyba při aktualizaci množství", "#dc3545");
        }
    });
}

function updateProductTotal(productId) {
    const input = $(`#quantity${productId}`);
    const count = parseInt(input.val());
    const price = parseFloat(input.data('price'));
    const total = count * price;
    if (isNaN(total)) total = 0;

    $(`#productTotal${productId}`).html(`<h5 class="mb-0 text-success">${total} Kč</h5>`);
    updateOrderTotal();
}

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

$(document).on('click', '.btn-plus', function () {
    const productId = $(this).attr('id').replace('btnPlus', '');
    const input = $(`#quantity${productId}`);
    const newValue = parseInt(input.val()) + 1;
    input.val(newValue);
    updateQuantityInDatabase(productId, newValue);
});

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

$(document).on('input', 'input.product-quantity', function () {
    const input = $(this);
    const productId = input.data('product-id');
    const newQuantity = parseInt(input.val());


    if (isNaN(newQuantity) || newQuantity < 1) {

        updateQuantityInDatabase(productId, 1);
        return;
    }

    updateQuantityInDatabase(productId, newQuantity);
});

$(document).on('click', '.remove-product-btn', function () {
    const productId = $(this).data('product-id');

    $.ajax({
        url: "/Cart/RemoveProduct",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ id: productId }),
        success: function () {
            $(`#product${productId}`).remove();
            updateOrderTotal();
            showToast("Produkt byl odebrán!", "#28a745");

            // Pokud už žádný produkt nezůstal, zobrazíme prázdný košík
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
            showToast("Chyba při odebírání produktu", "#dc3545");
        }
    });
});
