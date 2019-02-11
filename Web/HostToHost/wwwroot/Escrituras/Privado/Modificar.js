
var cantidadControles = constante._3;

$(window.document).ready(function () {
    window.setTimeout(fcCargaInicial, constante.SWAL_TIMER_1);
    fcMostrarProgreso();
});

function fcCargaInicial() {
    try {
        var usuario = $("#dropdownUserMenu").text().trim();
        usuario = usuario !== "" ? usuario.split(constante.COMMA)[constante._1] : usuario.trim();
        $("#txtUsuario").val(usuario.trim());
        $("#btnModificar").on("click", function () {
            fcModificarClave();
        });
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcMostrarProgreso() {
    try {
        $("input").on("keypress", function (event)
        {
            var controlId = $(this)[0].id;
            var isValid = false;

            switch (controlId) {
                case "txtClave1":
                case "txtClave2":
                isValid = felix.fcValidarClave(event);
                break;
            }

            if (isValid) {
                felix.fcQuitarNotificacion(controlId);
            }
        });

        $(".progreso").on("change", function () {
            var tagName = $(this)[0].tagName;

            if (typeof tagName !== "undefined" && tagName === "SELECT")
            {
                felix.fcQuitarNotificacion($(this)[0].id);
            }

            felix.fcMostrarProgreso(cantidadControles);
        });
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcModificarClave() {
    try {
        var usuario = $("#txtUsuario").val().trim();
        var claveActual = $("#txtClave1").val().trim();
        var claveNueva1 = $("#txtClave2").val().trim();
        var claveNueva2 = $("#txtClave3").val().trim();
        var mensaje = "";

        if (usuario == "") {
            mensaje += constante.MENSAJE_PARAMETROS_NO_PRESENTES;
        }
        if (claveActual == "") {
            mensaje += constante.MENSAJE_INGRESE_CLAVE;
            $("#txtClave1").css("border-color", "red");
            $("#lblClave1").css("display", "inline");
        }
        if (claveNueva1 == "") {
            mensaje += constante.MENSAJE_INGRESE_CLAVE;
            $("#txtClave2").css("border-color", "red");
            $("#lblClave2").css("display", "inline");
        }
        else
        {
            if (claveNueva2 == "")
            {
                mensaje += constante.MENSAJE_INGRESE_CLAVE;
                $("#txtClave3").css("border-color", "red");
                $("#lblClave3").css("display", "inline");
            }
            else
            {
                if (claveNueva1 !== claveNueva2)
                {
                    mensaje += constante.MENSAJE_CLAVES_NO_COINCIDEN;
                    $("#txtClave2").css("border-color", "red");
                    $("#lblClave2").css("display", "inline");
                    $("#txtClave3").css("border-color", "red");
                    $("#lblClave3").css("display", "inline");
                }
            }
        }
        
        if (mensaje != "") {
            felix.fcAlert(constante.TITULO_MENSAJE, mensaje);
            return false;
        }

        var parametros = new Array();
        parametros[0] = claveActual;
        parametros[1] = claveNueva1;

        felix.fcHttpClient(constante.ASYNC_TRUE, constante.HTTP_POST, constante.PRIVADO_MODIFICAR_CLAVE_ASYNC, parametros, function (res) {
            if (res !== "")
            {
                if (res.codigo === constante.CODIGO_OK)
                {
                    felix.fcAlertRedirect(constante.TITULO_MENSAJE, res.mensaje, function () {
                        fcCerrarSesion();
                    });
                }
                else if (res.codigo === constante.CODIGO_NO_AUTENTICADO) {
                    felix.fcAlertRedirect(constante.TITULO_MENSAJE, res.mensaje, function () {
                        window.location.href = res.url;
                    });
                }
                else
                {
                    felix.fcAlert(constante.TITULO_MENSAJE, res.mensaje);
                }
            }
        });
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcCerrarSesion() {
    try {
        var parametros;
        
        felix.fcHttpClient(constante.ASYNC_TRUE, constante.HTTP_GET, constante.PRIVADO_CERRAR_SESION_ASYNC, parametros, function (res) {
            if (res !== "")
            {
                if (res.codigo === constante.CODIGO_OK)
                {
                    var nombres = $("#dropdownUserMenu").text().split(constante.COMMA)[1].trim() || "";
                    nombres = felix.format(constante.MENSAJE_CERRAR_SESION, nombres);
                    felix.fcAlertTimer(constante.TITULO_MENSAJE, nombres, constante.SWAL_TIMER_1, function () {
                        window.location.href = res.url;
                    });
                }
                else if (res.codigo === constante.CODIGO_NO_AUTENTICADO)
                {
                    felix.fcAlertRedirect(constante.TITULO_MENSAJE, res.mensaje, function () {
                        window.location.href = res.url;
                    });
                }
                else
                {
                    felix.fcAlert(constante.TITULO_MENSAJE, res.mensaje);
                }
            }
        });
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}