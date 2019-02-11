
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
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}