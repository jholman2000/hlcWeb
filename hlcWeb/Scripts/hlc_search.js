$(document).ready(function () {
    $("td > a").click(function(e) {
        console.log("Anchor clicked");
        e.preventDefault();
    });
    $(".js-click-row").click(function () {
        var href = $(this).find("a")[0].href;
        location.href = href;
    });
});