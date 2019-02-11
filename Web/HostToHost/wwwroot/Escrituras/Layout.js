
if (!jQuery) { throw new Error("Layout.js requires jQuery.js"); }
if (!felix) { throw new Error("Layout.js requires Felix.js"); }
if (!constante) { throw new Error("Layout.js requires Constante.js"); }

$(window.document).ready(function () {
    $("#menu-toggle").click(function(e) {
        e.preventDefault();
        $("#wrapper").toggleClass("toggled");
    });
    
    fcCargaPrimaria();
});

function fcCargaPrimaria() {
    try {
        fcConsultarNombreUsuario();
        $("#lblCerrarSesion").on("click", function () {
            fcCerrarSesion();
        });
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcConsultarNombreUsuario() {
    try {
        var parametros;

        felix.fcHttpClient(constante.ASYNC_TRUE, constante.HTTP_GET, constante.PRIVADO_CONSULTAR_NOMBRE_USUARIO_ASYNC, parametros, function (res) {
            if (res !== "")
            {
                if (res.codigo === constante.CODIGO_OK)
                {
                    $("#dropdownUserMenu").append(res.nombres);
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