function main() {
    
    $(".search-btn").click(SearchButtonHandler);
    $(".load-more-btn").click(LoadMoreButtonHandler);
    $(".products-list").on('click', ".add-to-cart-btn", AddToCartHandler);
    LoadProductsStart();
}

function LoadMoreButtonHandler() {
    var priceFrom = $('.price-from').val();
    var priceTo = $('.price-to').val();
    var sortType = $('.sort-type').hasClass('ascending');
    var category = 'all';
    var lastProdId = $('.products-list').children().last().attr('id');

    let searchParams = new URLSearchParams(window.location.search);
    if (searchParams.has('category')) {
        category = searchParams.get('category');
    }

    if (priceFrom == '' && priceTo == '' && sortType) {
        LoadWithoutBounds(false, category, lastProdId);
    } else if (priceFrom == '' && priceTo == '' && !sortType) {
        LoadWithoutBounds(true, category, lastProdId);
    }
}
function LoadWithoutBounds(descPrice, category, lastProdId) {

    $.ajax({
        url: "Product/LoadByPrice",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({
            lastProductId: lastProdId,
            category: category,
            DescendingPrice: descPrice
        })
    }).done(function (data) {
        for (var i = 0; i < data.products.length; i++) {
            PrintProductCard(data.products[i])
        }

        if (data.lastPage == false) {
            $('.load-more-btn').removeClass('d-none');
        } else {
            $('.load-more-btn').addClass('d-none');
        }
    }).fail(function (xhr) {
        alert(xhr.responseText);
    });


    //$.post('Product/LoadByPrice',
    //    { lastProductId: lastProdId, DescendingPrice: descPrice, Category: category })
    //    .done(function (data, statusText) {
    //        for (var i = 0; i < data.products.length; i++) {
    //            PrintProductCard(data.products[i])
    //        }

    //        if (data.lastPage == false) {
    //            $('.load-more-btn').removeClass('d-none');
    //        } else {
    //            $('.load-more-btn').addClass('d-none');
    //        }
    //});
}

function AddToCartHandler() {
    let prId = $(this).closest('.product-card').attr('id');

    $.post('/Product/AddToCart', { idProduct: prId, count: 1 }).done(function (data, statusText) {
        $(this).closest('.card-body').children('.btn-incard').removeClass('d-none');
        $(this).addClass('d-none');
    });

    
    console.log($(this).text());
}

function LoadProductsStart() {
    let searchParams = new URLSearchParams(window.location.search);

    var category = 'all';
    if (searchParams.has('category')) {
        category = searchParams.get('category');
    }
    

    $.ajax({
        url: "Product/LoadByPrice",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({
            lastProductId: 0,
            category: category
        })
    }).done(function (data) {
        for (var i = 0; i < data.products.length; i++) {
            PrintProductCard(data.products[i])
        }

        if (data.lastPage == false) {
            $('.load-more-btn').removeClass('d-none');
        } else {
            $('.load-more-btn').addClass('d-none');
        }
    }).fail(function (xhr) {
        alert(xhr.responseText);
    });
}

function PrintProductCard(prData) {
    var product = $(".product-card-template").clone();
    product.attr('id', prData.id);
    var imgPath = product.find('.card-img').attr('src') + prData.id + ".png";
    product.find('.card-img').attr('src', imgPath);
    product.find('.card-title').text(prData.title);
    product.find('.prod-price').text(prData.price + " ₽");

    if (prData.inCart == true) {
        product.find('.add-to-cart-btn').addClass('d-none');
        product.find('.btn-incard').removeClass('d-none');
    }

    product.removeClass('product-card-template');
    product.removeClass('d-none');

    $('.products-list').append(product);
    //₽
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