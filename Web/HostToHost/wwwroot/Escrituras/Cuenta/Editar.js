
var cantidadControles = constante._5;

$(window.document).ready(function () {
    fcCargaInicial();
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
            var key = queryString[constante._1].split(constante.EQUAL)[constante._0];
            var string64Usuario = queryString[constante._1].split(constante.EQUAL)[constante._1];
            
            if (key == "" || string64Usuario == "")
            {
                esCorrecto = false;
            }
            else
            {
                var encodedUsuario = atob(string64Usuario);
                var decodedUsuario = decodeURIComponent(encodedUsuario);
                var usuario = JSON.parse(decodedUsuario);
                fcMostrarUsuario(usuario);
            }
        }

        if (esCorrecto === false)
        {
            felix.fcAlertRedirect(constante.TITULO_MENSAJE, constante.MENSAJE_PARAMETROS_NO_PRESENTES, function () {
                window.location.href = constante.HREF_CUENTA_LISTAR_USUARIOS;
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

function fcMostrarUsuario(usuario) {
    try {
        if (typeof usuario === "undefined" || usuario.length === 0)
        {
            felix.fcAlert(constante.TITULO_MENSAJE, constante.MENSAJE_PARAMETROS_NO_PRESENTES);
            return false;
        }

        $("#txtIdUsuario").val(usuario.idUsuario);
        $("#txtApePaterno").val(usuario.apePaterno);
        $("#txtApeMaterno").val(usuario.apeMaterno);
        $("#txtNombres").val(usuario.nombres);
        $("#txtCorreo").val(usuario.correo);
        $("#txtCelular").val(usuario.celular);
        $("#btnEditar").on("click", function () {
            fcEditarUsuario();
        });
        felix.fcMostrarProgreso(cantidadControles);
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcEditarUsuario() {
    try {
        var idUsuario = $("#txtIdUsuario").val().trim();
        var apePaterno = $("#txtApePaterno").val().trim();
        var apeMaterno = $("#txtApeMaterno").val().trim();
        var nombres = $("#txtNombres").val().trim();
        var correo = $("#txtCorreo").val().trim();
        var celular = $("#txtCelular").val().trim();
        var mensaje = "";

        if (idUsuario == "")
        {
            mensaje += constante.MENSAJE_PARAMETROS_NO_PRESENTES;
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
            mensaje += constante.MENSAJE_INGRESE_NOMBRES
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
            mensaje += constante.MENSAJE_INGRESE_CELULAR
            $("#txtCelular").css("border-color", "red");
            $("#lblCelular").css("display", "inline");
        }
        
        if (mensaje != "") {
            felix.fcAlert(constante.TITULO_MENSAJE, mensaje);
            return false;
        }

        var parametros = {
            IdUsuario: idUsuario,
            ApePaterno: apePaterno,
            ApeMaterno: apeMaterno,
            Nombres: nombres,
            Correo: correo,
            Celular: celular
        };

        felix.fcHttpClient(constante.ASYNC_TRUE, constante.HTTP_POST, constante.CUENTA_EDITAR_USUARIO_ASYNC, parametros, function (res) {
            if (res !== "")
            {
                if (res.codigo === constante.CODIGO_OK)
                {
                    felix.fcAlertRedirect(constante.TITULO_MENSAJE, res.mensaje, function () {
                        window.location.href = constante.HREF_CUENTA_LISTAR_USUARIOS;
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