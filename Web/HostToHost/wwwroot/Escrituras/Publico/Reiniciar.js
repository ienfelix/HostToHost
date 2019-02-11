
var cantidadControles = constante._2;

$(window.document).ready(function () {
    fcCargaInicial();
    fcMostrarProgreso();
});

function fcCargaInicial() {
    var esCorrecto = true;
    try {
        var queryString = window.location.search.split(constante.QUESTION);
        if (queryString == "" || queryString.length < constante._2)
        {
            esCorrecto = false;
        }
        else
        {
            var key = queryString[constante._1].split(constante.AMPERSON)[constante._0].split(constante.EQUAL)[constante._0];
            var usuario = queryString[constante._1].split(constante.AMPERSON)[constante._0].split(constante.EQUAL)[constante._1];
            var keyToken = queryString[constante._1].split(constante.AMPERSON)[constante._1].split(constante.EQUAL)[constante._0];
            var token = queryString[constante._1].split(constante.AMPERSON)[constante._1].split(constante.EQUAL)[constante._1];
            
            if (key == "" || usuario == "" || keyToken == "" || token == "")
            {
                esCorrecto = false;
            }
            else
            {
                fcMostrarUsuario(usuario, token);
            }
        }

        if (esCorrecto === false)
        {
            felix.fcAlertRedirect(constante.TITULO_MENSAJE, constante.MENSAJE_PARAMETROS_NO_PRESENTES, function () {
                window.location.href = constante.HREF_PUBLICO_LOGUEAR;
            });
        }
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

function fcMostrarUsuario(usuario, token) {
    try {
        if (typeof usuario === "undefined" || usuario.length == "")
        {
            felix.fcAlert(constante.TITULO_MENSAJE, constante.MENSAJE_PARAMETROS_NO_PRESENTES);
            return false;
        }

        $("#txtUsuario").val(usuario);
        $("#btnReiniciar").val(token);
        $("#btnReiniciar").on("click", function () {
            fcReiniciarClave();
        });
        felix.fcMostrarProgreso(cantidadControles);
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcReiniciarClave() {
    try {
        var usuario = $("#txtUsuario").val().trim();
        var clave1 = $("#txtClave1").val().trim();
        var clave2 = $("#txtClave2").val().trim();
        var token = $("#btnReiniciar").val();
        var mensaje = "";

        if (clave1 == "") {
            mensaje += constante.MENSAJE_INGRESE_CLAVE;
            $("#txtClave1").css("border-color", "red");
            $("#lblClave1").css("display", "inline");
        }
        else
        {
            if (clave2 == "")
            {
                mensaje += constante.MENSAJE_INGRESE_CLAVE;
                $("#txtClave2").css("border-color", "red");
                $("#lblClave2").css("display", "inline");
            }
            else
            {
                if (clave1 !== clave2)
                {
                    mensaje += constante.MENSAJE_CLAVES_NO_COINCIDEN;
                    $("#txtClave1").css("border-color", "red");
                    $("#lblClave1").css("display", "inline");
                    $("#txtClave2").css("border-color", "red");
                    $("#lblClave2").css("display", "inline");
                }
            }
        }
        
        if (mensaje != "") {
            felix.fcAlert(constante.TITULO_MENSAJE, mensaje);
            return false;
        }

        var parametros = new Array();
        parametros[0] = usuario;
        parametros[1] = clave1;
        parametros[2] = token;

        felix.fcHttpClient(constante.ASYNC_TRUE, constante.HTTP_POST, constante.PUBLICO_REINICIAR_CLAVE_ASYNC, parametros, function (res) {
            if (res !== "")
            {
                if (res.codigo === constante.CODIGO_OK)
                {
                    felix.fcAlertRedirect(constante.TITULO_MENSAJE, res.mensaje, function () {
                        window.location.href = res.url;
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