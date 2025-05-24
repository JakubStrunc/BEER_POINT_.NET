// get the previous addresses
$(document).on('click', '#openSendModal', function () {
    const inputs = $('.address-input');
    const newAddressCheckbox = $('#newAddressCheckbox'); 
    const hiddenInput = $('#existingAddressId'); 
    const container = $('#recent-addresses'); 

    container.empty(); 
    inputs.prop('disabled', false).removeClass('bg-light'); 
    newAddressCheckbox.prop('checked', true);

    // fetch recent addresses via AJAX
    $.ajax({
        url: "/Cart/GetRecentAddresses", // fetch  address
        type: "GET",
        success: function (addresses) {
            if (!Array.isArray(addresses) || addresses.length === 0) return; // exit if no addresses found

            // create a checkbox for each address
            addresses.forEach(a => {
                const checkbox = $(`  
                    <label class="list-group-item">
                        <input class="form-check-input me-1 old-address-checkbox" type="checkbox" value="${a.id}">
                        ${a.firstName} ${a.lastName}, ${a.street} ${a.houseNumber}/${a.orientationNumber}, ${a.postalCode} ${a.city}
                    </label>
                `);
                container.append(checkbox);
            });

            // handle address selection from the list
            container.on('change', '.old-address-checkbox', function () {
                $('.old-address-checkbox').not(this).prop('checked', false); 
                if (this.checked) {
                    hiddenInput.val($(this).val()); 
                    newAddressCheckbox.prop('checked', false);
                    inputs.prop('disabled', true).addClass('bg-light');
                } else {
                    hiddenInput.val('');
                }
            });
        }
    });
});

// toggle between creating a new address or selecting an old one
$(document).on('change', '#newAddressCheckbox', function () {
    const inputs = $('.address-input'); 
    const isChecked = this.checked; 

    // disable the old address checkbox
    $('.old-address-checkbox').prop('checked', false);
    $('#existingAddressId').val(''); 

    if (isChecked) {
        inputs.prop('disabled', false).removeClass('bg-light'); 
    } else {
        inputs.prop('disabled', true).addClass('bg-light'); 
    }
});

// handle order submission
$(document).on('click', '#odeslat', function (event) {
    event.preventDefault(); 

    $('.text-danger').remove(); 

    const existingAddressId = $('#existingAddressId').val(); 
    const isUsingSaved = existingAddressId !== ""; 

    // submit order data via AJAX
    $.ajax({
        url: '/Cart/SubmitOrder', // URL to submit the order
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            existingAddressId: isUsingSaved ? parseInt(existingAddressId) : null,
            firstName: $('#firstName').val(),
            lastName: $('#lastName').val(), 
            street: $('#ulice').val(),
            houseNumber: parseInt($('#cislopopisne').val()), 
            orientationNumber: parseInt($('#orijentacnicislo').val()),
            postalCode: parseInt($('#psc').val()),
            city: $('#mesto').val()
        }),
        success: function (response) {
            if (response.success) {
                // if order is successfully submitted, show success alert
                sessionStorage.setItem("showToast", "true");
                sessionStorage.setItem("toastText", "Objednávka byla úspěšně odeslána!");
                sessionStorage.setItem("toastColor", "#28a745");
                window.location.href = '/'; 
            } else if (response.errors) {
                // show validation errors 
                for (const field in response.errors) {
                    showToast(response.errors[field], "#fd7e14");
                }
            } else if (response.error) {
                showToast("Chyba: " + response.error, "#dc3545");
            }
        },
        error: function (xhr) {
            // AJAX fails
            showToast("Chyba při odesílání: " + xhr.responseText, "#dc3545"); 
        }
    });
});
