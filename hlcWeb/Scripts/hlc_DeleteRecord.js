// hlc_DeleteRecord.js
// Call the remove operation on a given controller
//
function DeleteRecord(controller, id, confirmDesc, msg) {
    bootbox.confirm({
        title: "Delete " + confirmDesc,
        message: msg,
        buttons: {
            confirm: {
                label: '<i class="fa fa-trash-o fa-lg"></i> Delete',
                className: 'btn-danger'
            },
            cancel: {
                label: 'Cancel',
                className: 'btn-default'
            }
        },
        callback: function (result) {
            if (result === true) {
                $.ajax({
                        method: "POST",
                        type: "json",
                        url: "/api/" + controller + "/remove",
                        data: { Id: id, FieldName: null, FieldText: null, DoctorId: null, UserId: null }
                    })
                    .done(function (msg) {
                        if (msg === true) {
                            window.location.href = "/Home/Search?msg=" + confirmDesc + " was successfully deleted.";
                        }
                        else {
                            window.location.href = "/Home/Search?msg=An error occurred! " + confirmDesc + " was not deleted.";
                        }
                    })
                    .fail(function (jqXhr) {
                        alert(jqXhr.responseText);
                    });

            }
        }
    });
}
