$(document).ready(function () {
    // Get the anti-forgery token value from the hidden field
    var antiForgeryToken = $('input[name="__RequestVerificationToken"]').val();
    $.ajaxSetup({
        headers: {
            'RequestVerificationToken': antiForgeryToken
        }
    });

    // init tooltips
    $('.showTootip').tooltip();


    //Plus & Minus for Quantity product
    var quantity = 1;

    $('.quantity-right-plus').click(function (e) {
        e.preventDefault();
        var quantity = parseInt($('#quantity').val());
        $('#quantity').val(quantity + 1);
    });

    $('.quantity-left-minus').click(function (e) {
        e.preventDefault();
        var quantity = parseInt($('#quantity').val());
        if (quantity > 1) {
            $('#quantity').val(quantity - 1);
        }
    });

});