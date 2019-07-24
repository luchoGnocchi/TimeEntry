//Le envia al usuario una alerta con el texto ingresado
//PARAMS - message: mensaje de la alerta, titleText: titulo de la alerta - por defecto es Web Service, buttonText: texto del boton de cerrar el dialogo - por defecto es Aceptar, bootstrapType: es el estilo bootstrap que va a tener el dialog - los valores pueden ser WARNING, DANGER, PRIMARY, SUCCESS, DEFAULT - por defecto es DANGER, CSSClass: clase adicional que se le puede agregar al dialog, sirve sobretodo para agregar estilos adicionales
function BootstrapDialogShow(message, titleText, buttonText) {
    var titleText = (titleText == undefined) ? 'Web Service' : titleText;
    var buttonText = (buttonText == undefined) ? 'Aceptar' : buttonText;

    BootstrapDialog.show({
        type: BootstrapDialog.TYPE_DANGER,
        title: '::: ' + titleText + ' :::',
        message: message,
        buttons: [{
            label: buttonText,
            cssClass: 'btn-primary',
            action: function (dialogItself) {
                dialogItself.close();
            }
        }],
    });
}
function ErrorDialogShow(message, titleText, buttonText, detail) {
    var titleText = (titleText == undefined) ? 'Ofrecimiento' : titleText;
    var buttonText = (buttonText == undefined) ? 'Aceptar' : buttonText;

    var htmlMessage = "<div>"
        + "<div>" + message + "</div><br>"
        + "<div><a href='#' onclick='togglerVerDetalle(this);'>Ver Detalle</a></div>"
        + "<div id='dialogErrorDetail' style='display:none; overflow-x:scroll; '><p>" + detail + "</p></div>"
        + "</div>";


    BootstrapDialog.show({
        type: BootstrapDialog.TYPE_DANGER,
        title: '::: ' + titleText + ' :::',
        message: htmlMessage,
        buttons: [{
            label: buttonText,
            cssClass: 'btn-primary',
            action: function (dialogItself) {
                dialogItself.close();
            }
        }],
    });
}
function togglerVerDetalle(self) {
    var detail = $(self).parents('div').first().next();
    detail.toggle();
}

function colorToHex(color) {
    if (color.substr(0, 1) === '#') {
        return color;
    }
    var digits = /(.*?)rgb\((\d+), (\d+), (\d+)\)/.exec(color);

    var red = parseInt(digits[2]);
    var green = parseInt(digits[3]);
    var blue = parseInt(digits[4]);

    var rgb = blue | (green << 8) | (red << 16);
    return digits[1] + '#' + rgb.toString(16);
}

function addCommonListener(event) {
    event.name = event.name || "click";
    $(document.body).off(event.name, event.selector);
    $(document.body).on(event.name, event.selector, event.callBack);
}

function datatableConfig(container, plugin, data) {
    $(container)[plugin]({
        data: data
    });
    $(container)[plugin]("redraw");
}