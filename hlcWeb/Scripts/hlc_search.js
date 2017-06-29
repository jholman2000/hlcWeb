// Table row click launches hyperlink from first column
$(document).ready(function () {
    $("td > a").click(function(e) {
        e.preventDefault();
    });
    $(".js-click-row").click(function () {
        var href = $(this).find("a")[0].href;
        location.href = href;
    });
});