$(document).on('click', '.delete-product-btn', function () {
    const button = $(this);
    const productId = button.data('product-id');

    if (!confirm("Opravdu chcete smazat tento produkt?")) return;

    $.ajax({
        url: "/ProductsManaging/DeleteProduct",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ id: productId }),
        success: function () {
            showToast("Produkt byl úspěšně smazán.", "#28a745");

            // Odstranit řádek ze stránky
            button.closest("tr").remove();
        },
        error: function () {
            showToast("Chyba při mazání produktu.", "#dc3545");
        }
    });
});


$(document).on('click', '#pridatProdukt', function () {
    const name = $('#productName').val().trim();
    const description = $('#productDescription').val().trim();
    const price = parseFloat($('#productPrice').val());
    const stock = parseInt($('#productStock').val());
    const imageFile = $('#fileToUpload')[0].files[0]; // získání souboru

    if (description.length > 60) {
        showToast("Popis nesmí být delší než 60 znaků.", "#fd7e14");
        return;
    }

    const formData = new FormData();
    formData.append("Nazev", name);
    formData.append("Popis", description);
    formData.append("Cena", price);
    formData.append("Mnozstvi", stock);
    formData.append("FileToUpload", imageFile);

    $.ajax({
        url: "/ProductsManaging/AddProduct",
        type: "POST",
        processData: false,
        contentType: false,
        data: formData,
        success: function (response) {
            if (response.success) {
                showToast("Produkt úspěšně přidán!", "#28a745");
                $('#addproductModal').modal('hide');
                setTimeout(() => location.reload(), 1000);
            } else {
                showToast("Formulář obsahuje chyby.", "#fd7e14");
                console.log(response.errors);
            }
        },
        error: function () {
            showToast("Chyba při přidání produktu.", "#dc3545");
        }
    });
});



$(document).on('click', '.edit-product-btn', function () {
    const productId = $(this).data('product-id');

    //console.log("clicked");
    $.ajax({
        url: `/ProductsManaging/GetProduct/${productId}`,
        type: 'GET',
        success: function (product) {
            $('#editproductName').val(product.nazev);
            $('#editproductDescription').val(product.popis);
            $('#editproductPrice').val(product.cena);
            $('#editproductStock').val(product.mnozstvi);

            // Nastavíme tlačítku ID produktu
            $('#saveProductChanges').data('product-id', product.id);

            // Zobrazíme obrázek a checkbox
            $('#existingImageWrapper').html(`
                <img src="/img/produkty/${product.photo}" alt="Foto produktu" class="img-thumbnail mb-2" style="max-height: 200px;">
                <div class="form-check">
                    <input class="form-check-input" type="checkbox" value="" id="useExistingImage" checked>
                    <label class="form-check-label" for="useExistingImage">
                        Použít uložený obrázek
                    </label>
                </div>
            `);

            // Skryjeme input file
            $('#editfileToUpload').hide();
        },
        error: function () {
            showToast("Chyba při načítání produktu", "#dc3545");
        }
    });
});

$(document).on('click', '#saveProductChanges', function () {
    const productId = $(this).data('product-id');
    const useExistingImage = $('#useExistingImage').is(':checked');

    const formData = new FormData();
    formData.append('Id', productId);
    formData.append('Nazev', $('#editproductName').val().trim());
    formData.append('Popis', $('#editproductDescription').val().trim());
    formData.append('Cena', parseFloat($('#editproductPrice').val()));
    formData.append('Mnozstvi', parseInt($('#editproductStock').val()));
    formData.append('UseExistingImage', useExistingImage ? 'true' : 'false');

    if (!useExistingImage) {
        const fileInput = $('#editfileToUpload')[0];
        if (fileInput.files.length > 0) {
            formData.append('FileToUpload', fileInput.files[0]);
        }
    }

    $.ajax({
        url: '/ProductsManaging/EditProduct',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response.success) {
                showToast("Změny byly uloženy.", "#28a745");
                $('#editProductModal').modal('hide');
                setTimeout(() => location.reload(), 1000);
            } else if (response.errors) {
                showToast("Formulář obsahuje chyby.", "#fd7e14");
                console.warn("Chyby:", response.errors);
                // Volitelně zobrazit chyby u konkrétních polí
            }
        },
        error: function () {
            showToast("Chyba při ukládání produktu.", "#dc3545");
        }
    });
});


// Přepínání checkboxu
$(document).on('change', '#useExistingImage', function () {
    if (this.checked) {
        $('#editfileToUpload').hide();
    } else {
        $('#editfileToUpload').show();
    }
});
