
var cantidadControles = constante._1;

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
                window.location.href = constante.HREF_ROL_LISTAR_USUARIOS;
            });
        }
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcMostrarProgreso() {
    try {
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
        $("#txtUsuario").val(usuario.usuario);
        $("#btnAsignar").on("click", function () {
            fcAsignarRol();
        });
        fcListarRoles();
        fcListarRolesUsuario(usuario.usuario);
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcListarRoles() {
    try {
        var parametros;
        
        felix.fcHttpClient(constante.ASYNC_TRUE, constante.HTTP_GET, constante.ROL_LISTAR_ROLES_ASYNC, parametros, function (res) {
            if (res !== "")
            {
                if (res.codigo === constante.CODIGO_OK)
                {
                    $("#slRol").empty();
                    $("#slRol").append($("<option></option>").val("0").html("--SELECCIONE--"));
                    for (var i = 0; i < res.listaRoles.length; i++) {
                        $("#slRol").append($("<option></option>").val(res.listaRoles[i].id).html(res.listaRoles[i].name));
                    }
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

function fcListarRolesUsuario(usuario) {
    try {
        if (typeof usuario === "undefined")
        {
            felix.fcAlert(constante.TITULO_MENSAJE, "Seleccione un usuario.");
            return false;
        }

        var parametros = felix.format("{0}{1}{2}{3}", constante.QUESTION, constante.USUARIO, constante.EQUAL, usuario);

        felix.fcHttpClient(constante.ASYNC_TRUE, constante.HTTP_GET, constante.ROL_LISTAR_ROLES_USUARIO_ASYNC, parametros, function (res) {
            if (res !== "")
            {
                if (res.codigo === constante.CODIGO_OK)
                {
                    $("#dvRol").empty();
                    var table = "<table id=\"tbRol\" class=\"table table-sm table-hover\">";
                    for (var i = 0; i < res.listaRoles.length; i++) {
                        table += "<tr><td>" + res.listaRoles[i] + "</td></tr>";
                    }
                    table += "</table>";
                    $("#dvRol").append(table);
                }
                else if (res.codigo === constante.CODIGO_OMISION)
                {
                    felix.fcAlertFixed(res.mensaje, constante.SWAL_POSITION_TOP_END, constante.SWAL_TIMER_2);
                    $("#dvRol").empty();
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

function fcValidarRol(rol) {
    var esRolAsignado = false;
    try {
        var children = $("#tbRol").find("tbody tr");
        $(children).each(function (index, element) {
            var tdRol = $(element).children()[constante._0];
            var rolAsignado = $(tdRol).text();
            if (rolAsignado === rol)
            {
                esRolAsignado = true;
                return false;
            }
        });
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
    return esRolAsignado;
}

function fcAsignarRol() {
    try {
        var idUsuario = $("#txtIdUsuario").val().trim();
        var usuario = $("#txtUsuario").val().trim();
        var rol = $("#slRol :selected").text();
        var mensaje = "";

        if (idUsuario == "" || usuario == "")
        {
            mensaje += constante.MENSAJE_PARAMETROS_NO_PRESENTES;
        }
        if (rol == "" || rol === "--SELECCIONE--")
        {
            mensaje += constante.MENSAJE_SELECCIONE_ROL;
        }
        else
        {
            var esRolAsignado = fcValidarRol(rol);
            
            if (esRolAsignado)
            {
                mensaje += constante.MENSAJE_ROL_SI_ASIGNADO;
            }
        }
        
        if (mensaje != "") {
            felix.fcAlert(constante.TITULO_MENSAJE, mensaje);
            return false;
        }

        var parametros = new Array();
        parametros[0] = usuario;
        parametros[1] = rol;

        felix.fcHttpClient(constante.ASYNC_TRUE, constante.HTTP_PUT, constante.ROL_ASIGNAR_ROL_ASYNC, parametros, function (res) {
            if (res !== "")
            {
                if (res.codigo === constante.CODIGO_OK)
                {
                    felix.fcAlertRedirect(constante.TITULO_MENSAJE, res.mensaje, function () {
                        fcListarRolesUsuario(usuario);
                        $("#slRol").val("0");
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