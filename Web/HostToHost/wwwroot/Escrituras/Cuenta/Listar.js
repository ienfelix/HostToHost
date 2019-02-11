
$(window.document).ready(function () {
    fcCargaInicial();
});

function fcCargaInicial() {
    try {
        fcListarUsuarios(constante.PAGINA_CLICADA, constante.PRIMERA_PAGINA, constante.CANTIDAD_FILAS);
        
        $("#btnBuscar").on("click", function () {
            fcBuscarUsuarios();
        });
        $("#btnLimpiar").on("click", function () {
            fcLimpiarBusqueda();
        });

        $("input").on("keypress", function (event) {
            var inputId = $(this)[constante.INDICE_0].id;
            var isValid = false;

            switch (inputId)
            {
                case "txtUsuario":
                case "txtApePaterno":
                case "txtNombres":
                isValid = felix.fcValidarLetras(event);
                break;
            }

            if (isValid)
            {
                felix.fcQuitarNotificacion(inputId);
            }
        });
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcListarUsuarios(accion, pagina, filas) {
    try {
        var parametros = felix.format("{0}{1}{2}{3}{4}{5}{6}{7}", constante.QUESTION, constante.PAGINA, constante.EQUAL, pagina, constante.AMPERSON, constante.FILAS, constante.EQUAL, filas);

        felix.fcHttpClient(constante.ASYNC_TRUE, constante.HTTP_GET, constante.CUENTA_LISTAR_USUARIOS_ASYNC, parametros, function (res) {
            if (res !== "")
            {
                if (res.codigo === constante.CODIGO_OK)
                {
                    $("#dvUsuario").empty();
                    var tabla = fcConstruirBandeja(res.listaUsuarios);                    
                    $("#dvUsuario").append(tabla);
                    fcConstruirPaginado(accion, pagina, res.totalRegistros);
                }
                else if (res.codigo === constante.CODIGO_OMISION)
                {
                    $("#dvUsuario, #nvPaginacion").empty();
                    $("#lblRegistros").html("");
                    felix.fcAlertFixed(res.mensaje, constante.SWAL_POSITION_TOP_END, constante.SWAL_TIMER_2);
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
                    $("#dvUsuario, #nvPaginacion").empty();
                    $("#lblRegistros").html("");
                }
            }
        });
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcConstruirBandeja(listaUsuarios) {
    try {
        if (typeof listaUsuarios == "undefined" || listaUsuarios.length === 0)
        {
            felix.fcAlert(constante.TITULO_MENSAJE, constante.MENSAJE_PARAMETROS_NO_PRESENTES);
            return false;
        }

        var tabla = "<table class=\"table table-sm table-striped table-hover\">";
        tabla += "<thead class=\"thead-dark\" align=\"center\"><tr>";
        tabla += "<th>Usuario</th>";
        tabla += "<th>Ape. Paterno</th>";
        tabla += "<th>Ape. Materno</th>";
        tabla += "<th>Nombres</th>";
        tabla += "<th>Correo</th>";
        tabla += "<th>Celular</th>";
        tabla += "<th>Estado</th>";
        tabla += "<th>Usuario Creación</th>";
        tabla += "<th>Fecha Creación</th>";
        tabla += "<th></th>";
        tabla += "</tr></thead>";

        for (var i = 0; i < listaUsuarios.length; i++)
        {
            tabla += "<tr>";
            tabla += "<td>" + listaUsuarios[i].usuario + "</td>";
            tabla += "<td>" + listaUsuarios[i].apePaterno + "</td>";
            tabla += "<td>" + listaUsuarios[i].apeMaterno + "</td>";
            tabla += "<td>" + listaUsuarios[i].nombres + "</td>";
            tabla += "<td>" + listaUsuarios[i].correo + "</td>";
            tabla += "<td>" + listaUsuarios[i].celular + "</td>";
            tabla += "<td>" + listaUsuarios[i].estado + "</td>";
            tabla += "<td>" + listaUsuarios[i].usuarioCreacion + "</td>";
            var fechaCreacion = felix.format("{0} {1}", listaUsuarios[i].fechaCreacion, listaUsuarios[i].horaCreacion);
            tabla += "<td>" + fechaCreacion  + "</td>";
            var stringUsuario = JSON.stringify(listaUsuarios[i]);
            var encodedUsuario = encodeURIComponent(stringUsuario);
            var string64Usuario = btoa(encodedUsuario);
            tabla += "<td><div class=\"d-flex\">";
            tabla += "<i class=\"material-icons cursor\" onclick=\"fcVerUsuario('" + string64Usuario + "');\" title=\"Ver Usuario\" >search</i>";
            tabla += "<i class=\"material-icons cursor\" onclick=\"fcEditarUsuario('" + string64Usuario + "');\" title=\"Editar Usuario\" >edit</i>";
            tabla += "<i class=\"material-icons cursor\" onclick=\"fcInactivarUsuario('" + string64Usuario + "');\" title=\"Inactivar Usuario\" >delete</i>";
            tabla += "</div></td></tr>";
        }

        tabla += "</table>";
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
    return tabla;
}

function fcBuscarUsuarios() {
    try {
        var usuario = $("#txtUsuario").val().trim();
        var apePaterno = $("#txtApePaterno").val().trim();
        var nombres = $("#txtNombres").val().trim();
        var mensaje = "";

        if (usuario == "" && apePaterno == "" && nombres == "")
        {
            mensaje += constante.MENSAJE_INGRESE_CAMPO_BUSQUEDA;
            $("#txtUsuario").css("border-color", "red");
            $("#lblUsuario").css("display", "inline");
            $("#txtApePaterno").css("border-color", "red");
            $("#lblApePaterno").css("display", "inline");
            $("#txtNombres").css("border-color", "red");
            $("#lblNombres").css("display", "inline");
        }
        
        if (mensaje !== "")
        {
            felix.fcAlert(constante.TITULO_MENSAJE, mensaje);
            return false;
        }

        var parametros = felix.format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}", constante.QUESTION, constante.USUARIO, constante.EQUAL, usuario, constante.AMPERSON, constante.APE_PATERNO, constante.EQUAL, apePaterno, constante.AMPERSON, constante.NOMBRES, constante.EQUAL, nombres);
        
        felix.fcHttpClient(constante.ASYNC_TRUE, constante.HTTP_GET, constante.CUENTA_BUSCAR_USUARIOS_ASYNC, parametros, function (res) {
            if (res !== "")
            {
                if (res.codigo === constante.CODIGO_OK)
                {
                    $("#dvUsuario, #nvPaginacion").empty();
                    $("#lblRegistros").html("");
                    var tabla = fcConstruirBandeja(res.listaUsuarios);
                    $("#dvUsuario").append(tabla);
                }
                else if (res.codigo === constante.CODIGO_OMISION)
                {
                    $("#dvUsuario, #nvPaginacion").empty();
                    $("#lblRegistros").html("");
                    felix.fcAlertFixed(res.mensaje, constante.SWAL_POSITION_TOP_END, constante.SWAL_TIMER_2);
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
                    $("#dvUsuario, #nvPaginacion").empty();
                    $("#txtRegistros").html("");
                }
            }
        });
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcConstruirPaginado(accion, pagina, totalRegistros) {
    try {
        var totalPaginas = totalRegistros / constante.CANTIDAD_FILAS;
        var residuoDecimal = totalPaginas % constante.INDICE_1;
        totalPaginas = residuoDecimal !== constante.INDICE_0 ? parseInt(totalPaginas.toString(), constante.INDICE_10) + constante.INDICE_1 : totalPaginas;
        var contador = constante.INDICE_1;
        var paginaAnterior = pagina === constante.PRIMERA_PAGINA ? pagina : pagina - constante.INDICE_1;
        var paginaSiguiente = pagina === totalPaginas ? pagina : pagina + constante.INDICE_1;
        
        if (totalPaginas > constante.CANTIDAD_PAGINAS && pagina > constante.CANTIDAD_PAGINAS)
        {
            var redondeo = pagina % 2 === constante.INDICE_0 ? pagina - constante.INDICE_1 : pagina;
            redondeo = parseInt(redondeo / constante.INDICE_10, constante.INDICE_10) * constante.INDICE_10;
            totalPaginas = redondeo + constante.CANTIDAD_PAGINAS;
            contador = redondeo + constante.INDICE_1;
        }
        else if (totalPaginas > constante.CANTIDAD_PAGINAS && pagina <= constante.CANTIDAD_PAGINAS)
        {
            totalPaginas = constante.CANTIDAD_PAGINAS;
        }

        $("#nvPaginacion").empty();
        var paginado = "";
        paginado += "<ul class=\"pagination justify-content-end\">";
        paginado += "<li class\"page-item\"><a class=\"page-link\" href=\"javascript:void(0);\"><span aria-hidden=\"true\">&laquo;</span></a></li>";
        paginado += "<li class\"page-item\"><a class=\"page-link\" href=\"javascript:void(0);\" onclick=\"fcListarUsuariosPaginado('" + constante.PAGINA_ANTERIOR + "', " + paginaAnterior + ", " + constante.CANTIDAD_FILAS + ");\"><span aria-hidden=\"true\">&larr;</span></a></li>";
        
        for (var i = contador; i <= totalPaginas; i++)
        {
            paginado += "<li class=\"page-item correlativo\"><a class=\"page-link\" href=\"javascript:void(0);\" onclick=\"fcListarUsuariosPaginado('" + constante.PAGINA_CLICADA + "', " + i + ", " + constante.CANTIDAD_FILAS + ");\">" + i + "</a></li>";
        }

        paginado += "<li class\"page-item\"><a class=\"page-link\" href=\"javascript:void(0);\" onclick=\"fcListarUsuariosPaginado('" + constante.PAGINA_SIGUIENTE + "', " + paginaSiguiente + ", " + constante.CANTIDAD_FILAS + ");\"><span aria-hidden=\"true\">&rarr;</span></a></li>";
        paginado += "<li class\"page-item\"><a class=\"page-link\" href=\"javascript:void(0);\"><span aria-hidden=\"true\">&raquo;</span></a></li>";
        paginado += "</ul>";
        $("#nvPaginacion").append(paginado);
        var children = $("#nvPaginacion > ul").find("li.correlativo");

        $(children).each(function (index, element) {
            $(element).removeClass("active");
            var paginaActual = parseInt($(element).text(), constante.INDICE_10);

            if (paginaActual === pagina)
            {
                $(element).addClass("active");
                return false;
            }
        });

        switch (accion)
        {
            case constante.PAGINA_CLICADA:
            break;
            case constante.PAGINA_ANTERIOR:
            case constante.PAGINA_SIGUIENTE:
            break;
        }

        var desde = (pagina * constante.CANTIDAD_FILAS) - constante.CANTIDAD_FILAS + constante.INDICE_1;
        var hasta = pagina * constante.CANTIDAD_FILAS > totalRegistros ? totalRegistros : pagina * constante.CANTIDAD_FILAS;
        var registros = felix.format(constante.MENSAJE_REGISTROS, desde, hasta, totalRegistros);
        $("#lblRegistros").html(registros);
        felix.fcDesplazarArriba();
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcListarUsuariosPaginado(accion, pagina, filas) {
    try {
        var esPaginaActual = false;

        if (accion !== constante.PAGINA_FILTRADA)
        {
            var children = $("#nvPaginacion > ul").find("li.correlativo");

            $(children).each(function (index, element) {
                var esActivo = $(element).hasClass("active");
                var paginaActual = parseInt($(element).text(), constante.INDICE_10);

                if (paginaActual === pagina && esActivo)
                {
                    esPaginaActual = true;
                    return false;
                }
            });
        }

        if (esPaginaActual)
        {
            return false;
        }

        fcListarUsuarios(accion, pagina, filas);
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcLimpiarCampos() {
    try {
        $("#txtUsuario").val("");
        $("#txtUsuario").css("border-color", "");
        $("#lblUsuario").css("display", "none");
        $("#txtApePaterno").val("");
        $("#txtApePaterno").css("border-color", "");
        $("#lblApePaterno").css("display", "none");
        $("#txtNombres").val("");
        $("#txtNombres").css("border-color", "");
        $("#lblNombres").css("display", "none");
        $("#dvUsuario").empty();
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcLimpiarBusqueda() {
    try {
        fcLimpiarCampos();
        fcListarUsuarios(constante.PAGINA_CLICADA, constante.PRIMERA_PAGINA, constante.CANTIDAD_FILAS);
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcVerUsuario(string64Usuario) {
    try {
        if (typeof string64Usuario === "undefined" || string64Usuario == "")
        {
            felix.fcAlert(constante.TITULO_MENSAJE, constante.MENSAJE_PARAMETROS_NO_PRESENTES);
            return false;
        }

        var url = felix.format(constante.CUENTA_VER_USUARIO, string64Usuario);
        window.location.href = url;
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcEditarUsuario(string64Usuario) {
    try {
        if (typeof string64Usuario === "undefined" || string64Usuario == "")
        {
            felix.fcAlert(constante.TITULO_MENSAJE, constante.MENSAJE_PARAMETROS_NO_PRESENTES);
            return false;
        }

        var url = felix.format(constante.CUENTA_EDITAR_USUARIO, string64Usuario);
        window.location.href = url;
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcInactivarUsuario(string64Usuario) {
    try {
        if (typeof string64Usuario === "undefined" || string64Usuario == "")
        {
            felix.fcAlert(constante.TITULO_MENSAJE, constante.MENSAJE_PARAMETROS_NO_PRESENTES);
            return false;
        }

        var url = felix.format(constante.CUENTA_INACTIVAR_USUARIO, string64Usuario);
        window.location.href = url;
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}