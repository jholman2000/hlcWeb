$(document).ready(function () {
    $(".js-click-row").click(function () {
        var href = $(this).find("a")[0].href;
        location.href = href;
    });
});