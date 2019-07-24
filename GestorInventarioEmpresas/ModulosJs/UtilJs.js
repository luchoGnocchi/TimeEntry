var UtilJs = UtilJs ||
    {
        //Variable en que deberá almacenarse el redirect para su envío en cada ajax
        redirect: '',
   
        language: {

            noResults: function() {

                return "No hay resultado";        
            },
            searching: function() {

                return "Buscando..";
            }
        }, 
   
        //Muestra es un modal de bootstrap utilizando el BootstrapDialog
        showBootstrapModal: function (settings) {
            var options = {
                type: settings.type || BootstrapDialog.TYPE_DEFAULT,
                size: settings.size || BootstrapDialog.SIZE_NORMAL,
                message: "<div style=\"margin:10px;\">" + settings.message + "</div>",
                title: settings.title
            };
            if (settings.buttons != undefined)
                if (settings.buttons.length === 0) {
                    options.buttons = [
                        {
                            label: "Aceptar",
                            cssClass: "btn-primary",
                            action: function (dialogItself) {
                                dialogItself.close();
                            }
                        }
                    ];
                } else {
                    options.buttons = settings.buttons;
                }
            if (settings.onShown != undefined)
                options.onshown = settings.onShown;
            if (settings.onHidden != undefined)
                options.onhidden = settings.onHidden;
            BootstrapDialog.show(options);
        },

        // Bloquea la pantalla y muestra un mensaje
        // Solo se puede desbloquear con la función $.unblockUI
        blockScreen: function (mensaje) {
            mensaje = mensaje || "Procesando...";
            $.blockUI({
                message: mensaje,
                baseZ: 50000,
                css: {
                    border: 'none',
                    padding: '15px',
                    backgroundColor: '#000',
                    color: '#ffffff',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: .5,
                    
                }
            });
        },

        //Invoca un plugin y llama al método que lo redibuja
        pluginConfig: function (containerId, plugin, data, method) {
            containerId = containerId.charAt(0) === '#' ? containerId : '#' + containerId;
            $(containerId)[plugin]({
                data: data
            });
            method = method || 'redraw';
            $(containerId)[plugin](method);
        },

        //Permite conocer si un elemento está sin definir
        isUndefined: function (obj) {
            return obj == undefined;
        },

        //Para conocer si un elemento html no tiene contenido
        isEmptyHtml: function (el) {
            return !$.trim(el.html());
        },

        //Para conocer si un obj está vacío
        isEmptyObj: function (obj) {
            return $.isEmptyObject(obj);
        },

        //Devuelve si una cadena está vacía o sólo contiene espacios
        isEmptyStr: function (str) {
            if (typeof str === 'string')
                return str === '';
            return false;
        },

        //Devuelve si un objeto es null
        isNullObj: function (obj) {
            return obj === null;
        },

        //Permite conocer si una cadena está sin definir, vacía o null
        isNullOrEmptyStr: function (str) {
            if (typeof str === 'string')
                return UtilJs.isNullObj(str) || str === 'null' || UtilJs.isEmptyStr(str);
            return UtilJs.isUndefined(str);
        },

        //Repite una cadena de texto un determinado número de veces
        repeatString: function (str, times) {
            return (new Array(times + 1)).join(str);
        },

        //Agrega un evento (click por defecto) a un contenedor (body) por defecto. Evita duplicar el evento
        addCommonListener: function (listener) {
            listener.event = listener.event || 'click';
            var container = listener.container ? $(listener.container) : $(document.body);
            if (listener.container) {
                container.off(listener.event);
                container.on(listener.event, listener.callback);
            } else {
                container.off(listener.event, listener.selector);
                container.on(listener.event, listener.selector, listener.callback);
            }

        },

        //Elimina un evento (click por defecto) a un contenedor (body) por defecto. Evita duplicar el evento
        removeCommonListener: function (listener) {
            listener.event = listener.event || 'click';
            var container = listener.container ? $(listener.container) : $(document.body);
            container.off(listener.event, listener.selector);
        },

        //Convierte el valor a entero, si el valor no es un número devuelve false.
        toInt: function (value) {
            if (isNaN(value)) {
                return false;
            }
            var x = parseFloat(value);
            var flag = (x | 0) === x;
            return !flag ? false : x;
        },

        //Determina si un valor es entero
        isInt: function (value) {
            return !isNaN(value);// && data === parseInt(value, 10);
        },

        //Obtener la parte numérica de una cadena de texto
        getIntFromStr: function (str, base) {
            var strNum = str.replace(/[^\d.]/g, '');
            var result = 0;
            if (!UtilJs.isEmptyStr(strNum) && !isNaN(strNum))
                result = parseInt(strNum, base || 10);
            return result;
        },

        //Obtener un objeto con el alto y ancho de la ventana.
        viewport: function () {
            var e = window
                , a = 'inner';
            if (!('innerWidth' in window)) {
                a = 'client';
                e = document.documentElement || document.body;
            }
            return { width: e[a + 'Width'], height: e[a + 'Height'] }
        },
 
        execAjaxTrace: function (request) {
            var blockScreen = request.blockScreen == undefined ? true : request.blockScreen;
            request.message = request.message || "Procesando...";
            request.type = request.type || httpMethod.Post;

            var stringify = request.stringify ? request.stringify : request.type === httpMethod.Post ? true : false;
            var data = request.data || {};
            

            var options = {
                url: request.url,
                type: request.type,
                data: stringify ? JSON.stringify(data) : data,
            };
            if (request.contentType) options.contentType = request.contentType;
            if (request.dataType) options.dataType = request.dataType;
            if (request.cache) options.cache = request.cache;
            if (request.timeout) options.timeout = request.timeout;

            
            if (blockScreen)
                UtilJs.blockScreen(request.message);
            MDSJsUtil.ajax(options,
                function (response) {
                    //success
                    request.success.call(this, response.Data);
                },
                function (codeError, messageError, detailError) {
                    //error
                    if (codeError !== 0 && messageError !== defaultStr.Timeout) {
                        if (undefined != request.errorBE) {
                            request.errorBE.call(this, codeError, messageError, detailError);
                        } else {
                            request.errorText = request.errorText || "Hubo un problema al actualizar la operación.";
                            ErrorDialogShow(request.errorText, "Aviso", "ACEPTAR", messageError);
                        }
                    } else {
                        if (request.timeoutError)
                            request.timeoutError.call(this, codeError, messageError, detailError);
                    }
                },
                //vaciar el trace
                clearTrace
            );
        },

        generarExcel: function (obj, nameController) {
            console.log(obj);
            if ((obj.list).length == 0) {
                //  swal("ALERTA!", "DEBE EXISTIR ALGUN REGISTRO PARA PODER REALIZAR ESTA ACCIÓN", "error");
                swal({
                    icon: "error",
                    title: "ALERTA!",
                    text: "DEBE EXISTIR ALGUN REGISTRO PARA PODER REALIZAR ESTA ACCIÓN",
                    buttons: {
                        confirm: {
                            text: "ACEPTAR",
                            value: true,
                            visible: true,
                            className: "",
                            closeModal: true,
                            focus: false

                        }
                    },

                });
                return;
            }
            UtilJs.execAjaxTrace({
                url: SitePath + "FileManager/" + nameController,
                data: obj,
                success: function (data) {

                    window.location.href = SitePath + "/FileManager/DescargarArchivo?nombreFichero=" + obj.nombre;
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    ErrorDialogShow("Hubo un problema al consultar los datos.", "Aviso", "Aceptar", errorThrown, "");
                }
            })
        },

        exportarExcel: function (obj, nameController) {
            if ((obj.list).length == 0) {
                //  swal("ALERTA!", "DEBE EXISTIR ALGUN REGISTRO PARA PODER REALIZAR ESTA ACCIÓN", "error");
                swal({
                    icon: "error",
                    title: "ALERTA!",
                    text: "DEBE EXISTIR ALGUN REGISTRO PARA PODER REALIZAR ESTA ACCIÓN",
                    buttons: {
                        confirm: {
                            text: "ACEPTAR",
                            value: true,
                            visible: true,
                            className: "",
                            closeModal: true,
                            focus: false

                        }
                    },

                });
                return;
            }
            UtilJs.execAjaxTrace({
                url: SitePath + "FileManager/" + nameController,
                data: {
                    filtros: obj.filtros,
                    search: obj.search,
                    nombreArchivo: obj.nombre
                },
                success: function (data) {

                    window.location.href = SitePath + "/FileManager/DescargarArchivoNombrado?nombreFicheroInterno=" + data.NombreArchivoInterno + "&nombreFichero=" + data.NombreArchivo;
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    ErrorDialogShow("Hubo un problema al consultar los datos.", "Aviso", "Aceptar", errorThrown, "");
                }
            })
        },
    }