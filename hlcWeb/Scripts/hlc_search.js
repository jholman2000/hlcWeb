﻿// Table row click launches hyperlink from first column
$(document).ready(function () {

    $("#searchTable").tablesorter();

    $("td > a").click(function(e) {
        e.preventDefault();
    });

    $(".js-click-row").click(function () {
        debugger;
        var href = $(this).find("a")[0].href;
        location.href = href;
    });
});