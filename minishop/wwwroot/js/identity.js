function main() {
    getUserInfo();

}

function getUserInfo() {

    $.post("/User/Info")
        .done(function (data, statusText) {
            console.log("ok " + data);
            updateUserInfo(data);
        }).fail(function (xhr, textStatus, errorThrown) { notIdentityUser(); })
        ;
}
function notIdentityUser() {
    $(".user-info").text("Войдите в систему");
    var userbut = $(".button-login");
    userbut.attr("href", "/login");
    userbut.text("Войдите");

}
function updateUserInfo(data) {
    $(".user-info").text(data.name+" "+data.surname);
}

main()