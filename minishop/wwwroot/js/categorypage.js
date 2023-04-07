function main() {
    $(".search-btn").click(SearchButtonHandler);
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