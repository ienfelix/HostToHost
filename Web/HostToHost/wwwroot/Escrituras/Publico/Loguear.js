
if (!jQuery) { throw new Error("Loguear.js requires jQuery."); }

$(window.document).ready(function () {
    fcCargaInicial();
});

function fcCargaInicial() {
    try {
        $("input").on({
            keypress: function (event) {
                var controlId = $(this)[0].id;
                var isValid = false;

                switch (controlId) {
                    case "txtUsuario":
                    isValid = felix.fcValidarUsuario(event);
                    break;
                    case "txtClave":
                    isValid = felix.fcValidarClave(event);
                    break;
                    case "txtToken":
                    isValid = felix.fcValidarLetrasNumeros(event);
                    break;
                }

                if (isValid) {
                    felix.fcQuitarNotificacion(controlId);
                }
                if (event.keyCode === constante.KEY_INTRO || event.which === constante.KEY_INTRO)
                {
                    fcIniciarSesion();
                }
            }
        });

        $("#btnLoguear").on("click", function () {
            fcIniciarSesion();
        });

        $("#btnSolicitar").on("click", function () {
            fcSolicitarCodigoToken();
        });
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcLimpiarCampos()
{
    try {
        $("#txtUsuario").val("");
        $("#txtUsuario").css("border-color", "");
        $("#lblUsuario").css("display", "none");
        $("#txtClave").val("");
        $("#txtClave").css("border-color", "");
        $("#lblClave").css("display", "none");
        $("#txtToken").val("");
        $("#txtToken").css("border-color", "");
        $("#lblToken").css("display", "none");
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcIniciarSesion() {
    try {
        var mensaje = "";
        var usuario = $("#txtUsuario").val().trim();
        var clave = $("#txtClave").val().trim();
        var token = $("#txtToken").val().trim();

        if (usuario == "") {
            mensaje += constante.MENSAJE_INGRESE_USUARIO;
            $("#txtUsuario").css("border-color", "red");
            $("#lblUsuario").css("display", "inline");
        }
        if (clave == "") {
            mensaje += constante.MENSAJE_INGRESE_CLAVE;
            $("#txtClave").css("border-color", "red");
            $("#lblClave").css("display", "inline");
        }
        if (token == "") {
            mensaje += constante.MENSAJE_INGRESE_TOKEN;
            $("#txtToken").css("border-color", "red");
            $("#lblToken").css("display", "inline");
        }

        if (mensaje != "") {
            felix.fcAlert(constante.TITULO_MENSAJE, mensaje);
            return false;
        }

        var parametros = new Array();
        parametros[0] = usuario;
        parametros[1] = clave;
        parametros[2] = token;

        felix.fcHttpClient(constante.ASYNC_TRUE, constante.HTTP_POST, constante.PUBLICO_INICIAR_SESION_ASYNC, parametros, function (res) {
            if (res !== "")
            {
                if (res.codigo === constante.CODIGO_OK)
                {
                    window.location.href = res.url;
                }
                else
                {
                    felix.fcAlert(constante.TITULO_MENSAJE, res.mensaje);
                    fcLimpiarCampos();
                }
            }
        });
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcSolicitarCodigoToken() {
    try {
        var mensaje = "";
        var usuario = $("#txtUsuario").val().trim();
        
        if (usuario == "")
        {
            mensaje += constante.MENSAJE_INGRESE_USUARIO;
            $("#txtUsuario").css("border-color", "red");
            $("#lblUsuario").css("display", "inline");
        }

        if (mensaje !== "")
        {
            felix.fcAlert(constante.TITULO_MENSAJE, mensaje);
            return false;
        }

        felix.fcHttpClient(constante.ASYNC_TRUE, constante.HTTP_POST, constante.PUBLICO_SOLICITAR_CODIGO_TOKEN_ASYNC, usuario, function (res) {
            if (res !== "")
            {
                if (res.codigo === constante.CODIGO_OK)
                {
                    felix.fcAlert(constante.TITULO_MENSAJE, res.mensaje);
                }
                else
                {
                    felix.fcAlert(constante.TITULO_MENSAJE, res.mensaje);
                    fcLimpiarCampos();
                }
            }
        });
    } catch (e) {
        felix.fcAlert(constante.TIUTLO_ERROR, e.message);
    }
}