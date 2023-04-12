function main() {
    $(".products-list").on('click', ".add-to-cart-btn", AddToCartHandler);
}
main();

function AddToCartHandler() {
    let prId = $(this).closest('.product-card').attr('id');
    var addToCardBtn = $(this);

    $.post('Cart/AddToCart', { idProduct: prId, count: 1 }).done(function (data, statusText) {
        var countInCart = data.count;
        console.log("c", countInCart);

        addToCardBtn.closest('.card-body').children('.btn-incard').removeClass('d-none');
        addToCardBtn.addClass('d-none');
        $(".count-cart-layout").text(countInCart);
    });

}