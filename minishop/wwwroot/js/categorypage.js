function main() {
    
    $(".search-btn").click(SearchButtonHandler);
    $(".load-more-btn").click(LoadMoreButtonHandler);
    $(".sort-btn").click(SortButtonHandler);
    $(".products-list").on('click', ".add-to-cart-btn", AddToCartHandler);
    $('.input-filter').on('input', FilterInputHandler);

    LoadProductsStart();
}
function FilterInputHandler() {
    if (!!$(this).val() && Math.abs($(this).val()) >= 0) {
        
        $(this).val(Math.abs($(this).val()));
        if ($(this).val().length > 8) {
            $(this).val($(this).val().slice(0, 8))
        }
    } else {
        $(this).val(null);
    }
}

function SearchButtonHandler() {
    var priceFrom = $('.price-from').val();
    var priceTo = $('.price-to').val();
    var sortType = $('.sort-type').hasClass('ascending');
    var category = 'all';
    var lastProdId = 0;

    let searchParams = new URLSearchParams(window.location.search);
    if (searchParams.has('category')) {
        category = searchParams.get('category');
    }

    ClearProductList();

    if (priceFrom == '' && priceTo == '' && sortType) {
        LoadWithoutBounds(false, category, lastProdId);
    } else if (priceFrom == '' && priceTo == '' && !sortType) {
        LoadWithoutBounds(true, category, lastProdId);
    } else if (priceTo == '' ) {
        LoadWithBounds(!sortType, category, lastProdId, Number(priceFrom), 99999999);
    }
    else if (sortType) {
        LoadWithBounds(false, category, lastProdId, Number(priceFrom), Number(priceTo));
    } else if (!sortType) {
        LoadWithBounds(true, category, lastProdId, Number(priceFrom), Number(priceTo));
    }

}

function ClearProductList() {
    var template = $('.product-card-template').clone();
    $('.products-list .product-card').remove();
    $('.products-list').append(template);
    console.log(template);
}

function SortButtonHandler() {
    if ($(this).hasClass('desc-btn')) {
        $('.sort-type').addClass('ascending');

        $(this).addClass('sort-selected');
        $('.asc-btn').removeClass('sort-selected');
    } else {
        $('.sort-type').removeClass('ascending');
        
        $(this).addClass('sort-selected');
        $('.desc-btn').removeClass('sort-selected');
    }
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
    } else if (priceTo == '') {
        LoadWithBounds(!sortType, category, lastProdId, Number(priceFrom), 99999999);
    }
    else if (sortType) {
        LoadWithBounds(false, category, lastProdId, Number(priceFrom), Number(priceTo));
    } else if (!sortType) {
        LoadWithBounds(true, category, lastProdId, Number(priceFrom), Number(priceTo));
    }
}
function LoadWithBounds(descPrice, category, lastProdId, priceFrom, priceTo) {
    $.ajax({
        url: "Product/LoadFromTo",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({
            lastProductId: lastProdId,
            category: category,
            DescendingPrice: descPrice,
            priceFrom: priceFrom,
            priceTo: priceTo
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
    var addToCardBtn = $(this);

    $.post('Cart/AddToCart', { idProduct: prId, count: 1 }).done(function (data, statusText) {
        var countInCart = data.count;
        console.log("c", countInCart);

        addToCardBtn.closest('.card-body').children('.btn-incard').removeClass('d-none');
        addToCardBtn.addClass('d-none');
        $(".count-cart-layout").text(countInCart);
    });

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
    product.find('.card-title').attr('href', '/Product/'+prData.id);
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


main();