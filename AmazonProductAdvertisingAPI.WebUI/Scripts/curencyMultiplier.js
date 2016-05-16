function getMultiplier(key) {
    var multiplier
    $.ajax({
        type: 'POST',
        async: false,
        url: '/Product/CurrencyConverter',
        data: key,
        success: function (result) {
            multiplier = parseFloat(result.replace(",", "."));

        }
    });
    return multiplier;
};