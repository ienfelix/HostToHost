
if (!jQuery) { throw new Error("Felix.js requires jQuery."); }

var felix = (function () {
    "use strict";

    if (!String.prototype.format) {
        String.prototype.format = function() {
            var args = Array.prototype.slice.call(arguments, 1);
            return this.replace(/{(\d+)}/g, function(match, number) {
                return typeof args[number] != 'undefined' ? args[number] : match;
            });
        };
    }

    if (!String.format) {
        String.format = function() {
            var s = arguments[0];
            for (var i = 0; i < arguments.length - 1; i++) {       
                var reg = new RegExp("\\{" + i + "\\}", "gm");             
                s = s.replace(reg, arguments[i + 1]);
            }
            return s;
        }
    }
    
    return {
        fcHttpClient: function (asincrono, tipo, url, parametros, fcCallback) {
            try {
                if (tipo === constante.HTTP_GET && typeof parametros !== "undefined")
                {
                    url += parametros
                }

                var backdrop;
                $.ajax({
                    async: asincrono,
                    type: tipo,
                    url: url,
                    data: JSON.stringify(parametros),
                    dataType: constante.DATA_TYPE_JSON,
                    contentType: constante.CONTENT_TYPE_JSON,
                    beforeSend: function (jqXHR) {
                        backdrop = $("<div style=\"width: 100%; height: 100%; top: 0px; left: 0px; position: fixed; z-index: 100000; background-color: #101014; opacity: 0.85; display: table;\"><div style=\"display: table-cell; vertical-align: middle; text-align: center;\"><img src=\"/Recursos/Animacion/ajaxLoader.gif\" /></div></div>").appendTo(window.document.body);
                    }
                }).always(function () {
                    backdrop.remove();
                }).done(function (data) {
                    fcCallback(data);
                }).fail(function (jqXHR, textStatus, errorThrown) {
                    backdrop.remove();
                    var message = "";
                    if (jqXHR.status === 401) {
                        location.reload(true);
                    } else if (jqXHR.status === 0 || jqXHR.status === 404 || jqXHR.status === 500) {
                        message = errorThrown;
                    } else if (textStatus === constante.TIME_OUT) {
                        message = String.format("{0}<br>{1}.", constante.TIME_OUT_MESSAGE, errorThrown.message);
                    } else if (textStatus === constante.PARSE_ERROR) {
                        message = String.format("{0}<br>{1}.", constante.PARSE_ERROR_MESSAGE, errorThrown.message);
                    } else if (textStatus === constante.ABORT) {
                        message = String.format("{0}<br>{1}.", constante.ABORT_MESSAGE, errorThrown.message);
                    } else {
                        message = String.format("{0}<br>{1}.", constante.ERROR_MESSAGE, errorThrown);
                    }
                    felix.fcAlert(constante.TITULO_ERROR, message);
                });
            } catch (e) {
                felix.fcAlert(constante.TITULO_ERROR, e.message);
            }
        },

        fcAlert: function (titulo, mensaje) {
            swal({
                title: titulo,
                html: mensaje,
                allowOutsideClick: false
            });
        },

        fcAlertFixed: function (mensaje, posicion, tiempo) {
            swal({
                position: posicion,
                html: mensaje,
                showConfirmButton: false,
                timer: tiempo
            });
        },
    
        fcAlertTimer: function (titulo, mensaje, tiempo, fcCallback) {
            swal({
                title: titulo,
                text: mensaje,
                showConfirmButton: false,
                timer: tiempo
            }).then(function () {
                setTimeout(function () {
                    fcCallback();
                }, tiempo);
            });
        },
    
        fcAlertConfirm: function (title, message, fcCallback) {
            swal({
                title: title,
                html: message,
                confirmButtonText: "Ok",
                showCancelButton: true
            }).then(function (result) {
                if (result.value)
                {
                    fcCallback();
                }
            });
        },
    
        fcAlertRedirect: function (titulo, mensaje, fcCallback) {
            swal({
                title: titulo,
                html: mensaje,
                confirmButtonText: "Ok",
                showCancelButton: false
            }).then(function () {
                fcCallback();
            });
        },

        fcValidarUsuario: function (event) {
            var isValid = false;
            try {
                var key = event.key;
                var pattern = /[a-zA-Z0-9]/;
                isValid = pattern.test(key);
                if (!isValid) {
                    event.preventDefault();
                }
            } catch (e) {
                felix.fcAlert("Error", e.message);
            }
            return isValid;
        },

        fcValidarClave: function (event) {
            var isValid = false;
            try {
                var key = event.key;
                var pattern = /[a-zA-Z0-9.]/;
                isValid = pattern.test(key);
                if (!isValid) {
                    event.preventDefault();
                }
            } catch (e) {
                felix.fcAlert("Error", e.message);
            }
            return isValid;
        },

        fcValidarLetras: function (event) {
            var isValid = false;
            try {
                var key = event.key;
                var pattern = /[a-zA-Z\sáéíóú]/;
                isValid = pattern.test(key);
                if (!isValid) {
                    event.preventDefault();
                }
            } catch (e) {
                felix.fcAlert("Error", e.message);
            }
            return isValid;
        },
        
        fcValidarNumeros: function (event) {
            var isValid = false;
            try {
                var key = event.key;
                var pattern = /[0-9]/;
                isValid = pattern.test(key);
                if (!isValid) {
                    event.preventDefault();
                }
            } catch (e) {
                felix.fcAlert("Error", e.message);
            }
            return isValid;
        },
        
        fcValidarLetrasNumeros: function (event) {
            var isValid = false;
            try {
                var key = event.key;
                var pattern = /[a-zA-Z0-9\s-.()_,áéíóúÁÉÍÓÚ@]/;
                isValid = pattern.test(key);
                if (!isValid) {
                    event.preventDefault();
                }
            } catch (e) {
                felix.fcAlert("Error", e.message);
            }
            return isValid;
        },
        
        fcValidarTelefono: function (event) {
            var isValid = false;
            try {
                var key = event.key;
                var pattern = /[0-9-+]/;
                isValid = pattern.test(key);
                if (!isValid) {
                    event.preventDefault();
                }
            } catch (e) {
                felix.fcAlert("Error", e.message);
            }
            return isValid;
        },
        
        fcValidarCorreo: function (correo) {
            var isValid = false;
            try {
                var pattern = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
                isValid = pattern.test(correo);
            } catch (e) {
                felix.fcAlert("Error", e.message);
            }
            return isValid;
        },

        fcQuitarNotificacion: function (controlId) {
            try {
                var labelId = "#lbl";
                var tagName = $("#" + controlId)[0].tagName;
                switch (tagName) {
                    case "SELECT":
                    labelId += controlId.substring(2, controlId.length);
                    break;
                    case "INPUT":
                    case "PASSWORD":
                    labelId += controlId.substring(3, controlId.length);
                    break;
                    case "TEXTAREA":
                    labelId += controlId.substring(3, controlId.length);
                    break;
                }
                $("#" + controlId).css("border-color", "");
                $(labelId).css("display", "none");
            } catch (e) {
                felix.fcAlert("Error", e.message);
            }
        },
        
        fcMostrarProgreso: function (cantidadControles) {
            try {
                var contador = 0;
                $(".progreso").each(function (index, element) {
                    var tagName = $(this)[0].tagName;
                    if (tagName === "SELECT") {
                        var value = $(":selected", this).val();
                        if (typeof value !== "undefined" && value !== "0") {
                            contador++;
                        }
                    } else if (tagName === "INPUT") {
                        var value = $(this).val().trim();
                        if (value !== "") {
                            contador++;
                        }
                    }
                });
                var porcentaje = (contador / cantidadControles) * 100;
                $("#dvProgressBar").css("width", porcentaje.toFixed(1) + "%");
                $("#lblProgressBar").html(porcentaje === 0 ? "0%" : porcentaje.toFixed() + "%");
            } catch (e) {
                felix.fcAlert("Error", e.message);
            }
        },

        format: function () {
            var s = arguments[0];
            for (var i = 0; i < arguments.length - 1; i++) {       
                var reg = new RegExp("\\{" + i + "\\}", "gm");             
                s = s.replace(reg, arguments[i + 1]);
            }
            return s;
        },

        fcDesplazarArriba: function () {
            $("html, body").animate({ scrollTop: 0 }, "slow");
        },

        fcExportarHaciaExcel: function (nombreTabla, nombreArchivo, nombreHoja) {
            try {
                var htmls = $("#" + nombreTabla).html();

                if (typeof htmls !== "undefined" && htmls !== "")
                {
                    var uri = "data:application/vnd.ms-excel;base64,";
                    var template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>'; 
                    
                    var base64 = function(s) {
                        return window.btoa(decodeURIComponent(encodeURIComponent(s)));
                    };

                    var format = function(s, c) {
                        return s.replace(/{(\w+)}/g, function(m, p) {
                            return c[p];
                        })
                    };

                    var ctx = {
                        worksheet : nombreHoja,
                        table : htmls
                    }

                    var fileName = felix.format("{0}.xls", nombreArchivo);
                    var ua = window.navigator.userAgent;

                    if (ua.match(/Edge/))
                    {
                        var content = format(template, ctx);
                        var blob = new Blob(["\ufeff", content], {type: "data:application/vnd.ms-excel;charset=UTF-8"});
                        window.navigator.msSaveBlob(blob, fileName);
                    }
                    else
                    {
                        var link = document.createElement("a");
                        link.download = fileName;
                        link.href = uri + base64(format(template, ctx));
                        document.body.appendChild(link);
                        link.click();
                        document.body.removeChild(link);
                    }
                }
            } catch (e) {
                felix.fcAlert(constante.TITULO_ERROR, e.message);
            }
        }
    };
})();