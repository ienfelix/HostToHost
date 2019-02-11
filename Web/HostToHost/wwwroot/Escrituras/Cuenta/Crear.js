
var cantidadControles = constante._8;

$(window.document).ready(function () {
    fcCargaInicial();
});

function fcCargaInicial() {
    try {
        fcMostrarProgreso();

        $("#btnCrear").on("click", function ()
        {
            fcCrearUsuario();
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
                case "txtClave1":
                case "txtClave2":
                isValid = felix.fcValidarClave(event);
                break;
                case "txtApePaterno":
                case "txtApeMaterno":
                case "txtNombres":
                isValid = felix.fcValidarLetras(event);
                break;
                case "txtCorreo":
                isValid = felix.fcValidarLetrasNumeros(event);
                break;
                case "txtCelular":
                isValid = felix.fcValidarTelefono(event);
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

function fcLimpiarCampos()
{
    try {
        $("#txtUsuario").val("");
        $("#txtClave1").val("");
        $("#txtClave2").val("");
        $("#txtApePaterno").val("");
        $("#txtApeMaterno").val("");
        $("#txtNombres").val("");
        $("#txtCorreo").val("");
        $("#txtCelular").val("");
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcCrearUsuario() {
    try {
        var usuario = $("#txtUsuario").val().trim();
        var clave1 = $("#txtClave1").val().trim();
        var clave2 = $("#txtClave2").val().trim();
        var apePaterno = $("#txtApePaterno").val().trim();
        var apeMaterno = $("#txtApeMaterno").val().trim();
        var nombres = $("#txtNombres").val().trim();
        var correo = $("#txtCorreo").val().trim();
        var celular = $("#txtCelular").val().trim();
        var mensaje = "";

        if (usuario == "") {
            mensaje += constante.MENSAJE_INGRESE_USUARIO;
            $("#txtUsuario").css("border-color", "red");
            $("#lblUsuario").css("display", "inline");
        }
        if (clave1 == "") {
            mensaje += constante.MENSAJE_INGRESE_CLAVE;
            $("#txtClave1").css("border-color", "red");
            $("#lblClave1").css("display", "inline");
        } else {
            if (clave2 == "") {
                mensaje += constante.MENSAJE_CONFIRME_CLAVE;
                $("#txtClave2").css("border-color", "red");
                $("#lblClave2").css("display", "inline");
            } else {
                if (clave1 !== clave2) {
                    mensaje += constante.MENSAJE_CLAVES_NO_COINCIDEN;
                    $("#txtClave1").css("border-color", "red");
                    $("#lblClave1").css("display", "inline");
                    $("#txtClave2").css("border-color", "red");
                    $("#lblClave2").css("display", "inline");
                }
            }
        }
        if (apePaterno == "") {
            mensaje += constante.MENSAJE_INGRESE_APE_PATERNO;
            $("#txtApePaterno").css("border-color", "red");
            $("#lblApePaterno").css("display", "inline");
        }
        if (apeMaterno == "") {
            mensaje += constante.MENSAJE_INGRESE_APE_MATERNO;
            $("#txtApeMaterno").css("border-color", "red");
            $("#lblApeMaterno").css("display", "inline");
        }
        if (nombres == "") {
            mensaje += constante.MENSAJE_INGRESE_NOMBRES;
            $("#txtNombres").css("border-color", "red");
            $("#lblNombres").css("display", "inline");
        }
        if (correo == "") {
            mensaje += constante.MENSAJE_INGRESE_CORREO;
            $("#txtCorreo").css("border-color", "red");
            $("#lblCorreo").css("display", "inline");
        } else {
            var isValid = felix.fcValidarCorreo(correo);
            if (!isValid) {
                mensaje += constante.MENSAJE_INGRESE_CORREO_VALIDO;
                $("#txtCorreo").css("border-color", "red");
                $("#lblCorreo").css("display", "inline");
            }
        }
        if (celular == "") {
            mensaje += constante.MENSAJE_INGRESE_CELULAR;
            $("#txtCelular").css("border-color", "red");
            $("#lblCelular").css("display", "inline");
        }
        
        if (mensaje != "") {
            felix.fcAlert(constante.TITULO_MENSAJE, mensaje);
            return false;
        }

        var parametros = {
            Usuario: usuario.toLowerCase(),
            Clave: clave1,
            ApePaterno: apePaterno,
            ApeMaterno: apeMaterno,
            Nombres: nombres,
            Correo: correo,
            Celular: celular
        };

        felix.fcHttpClient(constante.ASYNC_TRUE, constante.HTTP_POST, constante.CUENTA_CREAR_USUARIO_ASYNC, parametros, function (res) {
            if (res !== "")
            {
                if (res.codigo === constante.CODIGO_OK)
                {
                    felix.fcAlertRedirect(constante.TITULO_MENSAJE, res.mensaje, function () {
                        fcLimpiarCampos();
                        felix.fcMostrarProgreso(cantidadControles);
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