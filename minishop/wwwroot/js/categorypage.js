function main() {
    
    $(".search-btn").click(SearchButtonHandler);
}
function LoadProductsStart() {
    $.ajax({
        url: "Product/LoadByPrice",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({
            lastProductId: 0,
            category: "cat"
        })
    }).done(function (data) {
       
    }).fail(function (xhr) {
        alert(xhr.responseText);
    });
}

function SearchButtonHandler() {
    var priceFrom = $(".price-from").val();
    var priceTo = $(".price-to").val();

    priceFrom = Number(priceFrom);
    priceTo = Number(priceTo);

    if (priceFrom > priceTo) {
        alert("Значение 'от' должно быть меньше, чем значение 'до'");
    }

    console.log(priceFrom, priceTo);
}

main();