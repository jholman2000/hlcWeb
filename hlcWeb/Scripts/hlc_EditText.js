﻿function hlcEditText(controller, id, field, doctorId, userId) {
    /*
     * Pop-up window for editing text in HLC using Bootbox.js
     */
    var title = $("#" + field + " > .panel-heading").text().trim();
    var initValue = $("#" + field + " > .panel-body").data("hlc-text");
    console.log("title: " + title);
    console.log("initValue: " + initValue);

    bootbox.prompt({
        title: title,
        className: "hlc-textarea",
        size: "large",
        inputType: 'textarea',
        value: initValue,
        callback: function (result) {
            //console.log("Bootbox returned: " + result);
            if (result !== null) {
                $.ajax({
                        method: "POST",
                        url: "/api/" + controller + "/savetext",
                        data: { Id: id, FieldName: field, FieldText: result, DoctorId: doctorId, UserId: userId }
                    })
                    .done(function (msg) {
                        console.log("api returned: " + msg);
                        if (id === "0") {
                            console.log("Reloading Doctor view");
                            window.location.reload();
                        } else {
                            $("#" + field + " > .panel-body").html(result.replace(/(?:\r\n|\r|\n)/g, '<br />'));
                            $("#" + field + " > .panel-body").data("hlc-text", result);
                        }
                    })
                    .fail(function (jqXhr) {
                        alert(jqXhr.responseText);
                    });

            }
        }
    });
}