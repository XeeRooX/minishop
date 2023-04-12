function main() {
    
    $(".cart-items").on('click', ".del-btn-cart", delIem);
    $(".plus").click(plusItem);
    $(".minus").click(minusItem);
    $(".add-cart").click(addCart);
    
}

$(document).ready(getCountCart());
function addCart() {
    var Count = $(".count-cart").attr("value");
    var id = $(this).attr("id");
    $.post("/Cart/AddToCart", { IdProduct: id, count: Count })
        .done(function (data, statusText) {
            console.log("ok " + data);
            hideButton();
            getCountCart();
        }).fail(function (xhr, textStatus, errorThrown) { alert("error: " + xhr.responseText); })
        ;
}

function getCountCart() {
    console.log("start ");
    $.post("/Cart/GetCount")
        .done(function (data, statusText) {
            console.log("ok " + data);
            updateCoutCart(data);
        }).fail(function (xhr, textStatus, errorThrown) { console.log("user not login"); })
        ;
}

function updateCoutCart(dat) {

    $(".count-cart-layout").text(dat.count);
}

function hideButton() {
    $(".plus").attr("class","d-none");
    $(".minus").attr("class", "d-none");
    $('.add-cart').text("В корзине");
    $('.add-cart').attr('disabled', true);
}

function plusItem() {
    var count = $(".count-cart").attr("value");
    var price = $(".price-container").attr("value") / count;
    
    count++;
    $(".count-cart").attr("value", count);
    $(".count-cart").text("шт: " + count);
    price = price * count;
    $(".count-cart").attr("id", count);
    $(".price-container").attr("value",price);
    $(".price-container").text(price + " ₽");
}

function minusItem() {
    var count = $(".count-cart").attr("value");
    if (count == 1)
        return;
    var price = $(".price-container").attr("value") / count;

    count--;
    $(".count-cart").attr("value", count);
    $(".count-cart").text("шт: " + count);
    price = price * count;
    $(".count-cart").attr("id", count);
    $(".price-container").attr("value", price);
    $(".price-container").text(price + " ₽");
}

function deleteItem(id) {
	console.log("sdfsdfsd");
	$(".cart-items").children("#" + id).remove();
}

function delIem() {
	var id = $(this).attr("id");
    $.ajax({
        url: "/Cart/DeleteInCart",
        type: "DELETE",
        data: "id="+id,
    }).done(function (data) {
        deleteItem(id);
        console.log("Item deleted", data);
        updateCoutCart(data);
    }).fail(function (xhr, textStatus) {
        alert(xhr.responseText);
    });
}
main();
