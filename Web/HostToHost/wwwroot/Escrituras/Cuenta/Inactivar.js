
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

function fcMostrarUsuario(usuario) {
    try {
        if (typeof usuario === "undefined" || usuario.length === 0)
        {
            felix.fcAlert(constante.TITULO_MENSAJE, constante.MENSAJE_PARAMETROS_NO_PRESENTES);
            return false;
        }

        $("#txtIdUsuario").val(usuario.idUsuario);
        $("#txtUsuario").val(usuario.usuario);
        $("#txtApePaterno").val(usuario.apePaterno);
        $("#txtApeMaterno").val(usuario.apeMaterno);
        $("#txtNombres").val(usuario.nombres);
        $("#txtCorreo").val(usuario.correo);
        $("#txtCelular").val(usuario.celular);
        $("#txtEstado").val(usuario.estado);
        $("#txtUsuarioCreacion").val(usuario.usuarioCreacion);
        var fechaCreacion = felix.format("{0} {1}", usuario.fechaCreacion, usuario.horaCreacion);
        $("#txtFechaCreacion").val(fechaCreacion);
        $("#btnInactivar").on("click", function () {
            fcInactivarUsuario();
        });
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcInactivarUsuario() {
    try {
        var idUsuario = $("#txtIdUsuario").val().trim();
        var mensaje = "";

        if (idUsuario == "")
        {
            mensaje += felix.format("{0}<br>.", constante.MENSAJE_PARAMETROS_NO_PRESENTES);
        }
        
        if (mensaje != "") {
            felix.fcAlert(constante.TITULO_MENSAJE, mensaje);
            return false;
        }

        felix.fcHttpClient(constante.ASYNC_TRUE, constante.HTTP_PUT, constante.CUENTA_INACTIVAR_USUARIO_ASYNC, idUsuario, function (res) {
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