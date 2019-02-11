
var cantidadControles = constante._2;

$(window.document).ready(function () {
    fcCargaInicial();
    fcMostrarProgreso();
});

function fcCargaInicial() {
    try {
        $("#btnRecuperar").on("click", function () {
            fcRecuperarClave();
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
                case "txtUsuario":
                isValid = felix.fcValidarUsuario(event);
                break;
                case "txtCorreo":
                isValid = felix.fcValidarLetrasNumeros(event);
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

function fcRecuperarClave() {
    try {
        var usuario = $("#txtUsuario").val().trim();
        var correo = $("#txtCorreo").val().trim();
        var mensaje = "";

        if (usuario == "" && correo == "")
        {
            mensaje += felix.format("{0} ó {1}", constante.MENSAJE_INGRESE_USUARIO, constante.MENSAJE_INGRESE_CORREO);
            $("#txtUsuario").css("border-color", "red");
            $("#lblUsuario").css("display", "inline");
            $("#txtCorreo").css("border-color", "red");
            $("#lblCorreo").css("display", "inline");
        }
        else
        {
            if (correo != "")
            {
                var isValid = felix.fcValidarCorreo(correo);
                if (!isValid) {
                    mensaje += constante.MENSAJE_INGRESE_CORREO_VALIDO;
                    $("#txtCorreo").css("border-color", "red");
                    $("#lblCorreo").css("display", "inline");
                }
            }
        }

        if (mensaje !== "")
        {
            felix.fcAlert(constante.TITULO_MENSAJE, mensaje);
            return false;
        }

        var parametros = felix.format("{0}{1}{2}{3}{4}{5}{6}{7}", constante.QUESTION, constante.USUARIO, constante.EQUAL, usuario, constante.AMPERSON, constante.CORREO, constante.EQUAL, correo);

        felix.fcHttpClient(constante.ASYNC_TRUE, constante.HTTP_GET, constante.PUBLICO_RECUPERAR_CLAVE_ASYNC, parametros, function (res) {
            if (res !== "")
            {
                if (res.codigo === constante.CODIGO_OK)
                {
                    felix.fcAlertRedirect(constante.TITULO_MENSAJE, res.mensaje, function () {
                        window.location.href = constante.HREF_PUBLICO_LOGUEAR;
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