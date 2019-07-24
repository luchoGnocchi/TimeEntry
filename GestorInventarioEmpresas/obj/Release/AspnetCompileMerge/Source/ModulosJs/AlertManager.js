var AlertManager = (function () {    
    // Toastr options

    function showMessage(message, title) {
        return {
            success: function () {
                $.Notification.notify('success', 'top right', title, message)

            },
            info: function () {
                $.Notification.notify('info', 'top right', title, message)
            },
            warning: function () {
                $.Notification.notify('warning', 'top right', title, message)
            },
            error: function () {
                $.Notification.notify('error', 'top right', title, message)
            }
        };
    }

    return {
        CreateAlert: function (alertText, alertTitle) {           
            return showMessage(alertText, alertTitle)
        },
        CreateAlertFromObject: function (object) {
            var message = "";
            var title = "";
            if (typeof object == "string") {
                message = object;
            } else {
                message = object.Message || false;
                title = object.Title || false;
            }
            
            if (message || title) {
                return showMessage(message, title);
            }            
        },
        CreateConfirmAlert: function (alertText, alertTitle, callback) {        
            swal({
                title: alertTitle || "Confirmación",
                text: alertText || "Your will not be able to recover this imaginary file!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Aceptar",
                cancelButtonText: "Cancelar",
                closeOnConfirm: true,
                closeOnCancel: true
            },
            function (isConfirm) {                  
                if (typeof callback == "function") {
                    callback.call(this, isConfirm);
                }
            });
        }
    }
})();