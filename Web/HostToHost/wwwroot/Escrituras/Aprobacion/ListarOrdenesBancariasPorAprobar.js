
$(window.document).ready(function () {
    fcCargaInicial();
});

function fcCargaInicial() {
    try {
        fcListarOrdenesBancariasPorAprobar(constante.PAGINA_CLICADA, constante.PRIMERA_PAGINA, constante.CANTIDAD_FILAS, constante.VACIO, constante.VACIO, constante.VACIO, constante.VACIO);
        fcListarFiltrosOrdenesBancariasPorAprobar();

        $("#btnBuscar").on("click", function () {
            fcBuscarOrdenesBancariasPorAprobar();
        });
        $("#btnLimpiar").on("click", function () {
            fcLimpiarBusqueda();
        });
        $("#btnExportar").on("click", function () {
            fcExportarHaciaExcel();
        });

        $("#btnAprobar").show();
        $("#btnAprobar").on("click", function () {
            fcValidarSeleccionMasivoOrdenesBancarias();
        });

        $("input").on("keypress", function (event) {
            var inputId = $(this)[constante._0].id;
            var isValid = false;

            switch (inputId)
            {
                case "txtIdSap":
                isValid = felix.fcValidarLetrasNumeros(event);
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

function fcListarOrdenesBancariasPorAprobar(accion, pagina, filas, idSociedad, idBanco, idTipoOrden, idEstadoOrden) {
    try {
        var parametros = felix.format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}{20}{21}{22}{23}", constante.QUESTION, constante.PAGINA, constante.EQUAL, pagina, constante.AMPERSON, constante.FILAS, constante.EQUAL, filas, constante.AMPERSON, constante.ID_SOCIEDAD, constante.EQUAL, idSociedad, constante.AMPERSON, constante.ID_BANCO, constante.EQUAL, idBanco, constante.AMPERSON, constante.ID_TIPO_ORDEN, constante.EQUAL, idTipoOrden, constante.AMPERSON, constante.ID_ESTADO_ORDEN, constante.EQUAL, idEstadoOrden);

        felix.fcHttpClient(constante.ASYNC_TRUE, constante.HTTP_GET, constante.APROBACION_LISTAR_ORDENES_BANCARIAS_POR_APROBAR_ASYNC, parametros, function (res) {
            if (res !== "")
            {
                if (res.codigo === constante.CODIGO_OK)
                {
                    $("#dvOrdenBancaria").empty();
                    var tabla = fcConstruirBandeja(res.listaOrdenesBancarias);
                    $("#dvOrdenBancaria").append(tabla);
                    fcConstruirPaginado(accion, pagina, res.totalRegistros);
                    $("#chkAprobar").on("change", function () {
                        fcSeleccionarOrdenesBancariasPorAprobar();
                    });
                }
                else if (res.codigo === constante.CODIGO_OMISION)
                {
                    $("#dvOrdenBancaria, #nvPaginacion").empty();
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
                    $("#dvOrdenBancaria, #nvPaginacion").empty();
                    $("#lblRegistros").html("");
                }
            }
        });
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcConstruirBandeja(listaOrdenesBancarias) {
    try {
        if (typeof listaOrdenesBancarias == "undefined" || listaOrdenesBancarias.length === 0)
        {
            felix.fcAlert(constante.TITULO_MENSAJE, constante.MENSAJE_PARAMETROS_NO_PRESENTES);
            return false;
        }

        var tabla = "<table id=\"tbOrdenBancaria\" class=\"table table-sm table-striped table-hover\">";
        tabla += "<thead class=\"thead-dark\" align=\"center\"><tr>";
        tabla += "<th><div class=\"form-check\"><input type=\"checkbox\" id=\"chkAprobar\" class=\"form-check-input position-static cursor\" /></div></th>";
        tabla += "<th></th>";
        tabla += "<th>Sociedad</th>";
        tabla += "<th>Fecha Orden</th>";
        tabla += "<th>Banco</th>";
        tabla += "<th>Tipo Orden</th>";
        tabla += "<th>Moneda ML</th>";
        tabla += "<th>Importe</th>";
        tabla += "<th>Moneda MD</th>";
        tabla += "<th>Importe</th>";
        tabla += "<th>Liberador</th>";
        tabla += "<th>Pre Aprobador</th>";
        tabla += "<th>Id Sap</th>";
        tabla += "<th>Estado</th>";
        tabla += "<th>Fecha Creación</th>";
        tabla += "<th></th>";
        tabla += "</tr></thead>";

        for (var i = 0; i < listaOrdenesBancarias.length; i++)
        {
            tabla += "<tr>";
            var stringOrdenBancaria = JSON.stringify(listaOrdenesBancarias[i]);
            var encodedOrdenBancaria = encodeURIComponent(stringOrdenBancaria);
            var string64OrdenBancaria = btoa(encodedOrdenBancaria);
            var checkValue = felix.format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}", listaOrdenesBancarias[i].idSociedad, constante.PIPE, listaOrdenesBancarias[i].idSap, constante.PIPE, listaOrdenesBancarias[i].anio, constante.PIPE, listaOrdenesBancarias[i].momentoOrden, constante.PIPE, listaOrdenesBancarias[i].idTipoOrden, constante.PIPE, listaOrdenesBancarias[i].nombreArchivo, constante.PIPE, listaOrdenesBancarias[i].rutaArchivo);
            tabla += "<td><div class=\"form-check\"><input type=\"checkbox\" class=\"form-check-input position-static cursor\" value=\"" + checkValue + "\" /></div></td>";
            tabla += "<td><div class=\"d-flex\"><i class=\"material-icons cursor\" onclick=\"fcAprobarOrdenBancariaLiberada('" + string64OrdenBancaria + "');\" title=\"Aprobar Orden Bancaria Liberada\" >check_circle</i></div></td>";
            tabla += "<td>" + listaOrdenesBancarias[i].sociedadCorto + "</td>";
            tabla += "<td>" + listaOrdenesBancarias[i].fechaOrden + "</td>";
            tabla += "<td>" + listaOrdenesBancarias[i].bancoCorto + "</td>";
            tabla += "<td align=\"center\">" + listaOrdenesBancarias[i].tipoOrdenCorto + "</td>";
            tabla += "<td align=\"center\">" + listaOrdenesBancarias[i].monedaLocal + "</td>";
            var importeLocal = listaOrdenesBancarias[i].importeLocal === 0 ? "" : listaOrdenesBancarias[i].importeLocal.toLocaleString(constante.LOCALE_EN, {minimumFractionDigits: constante.INDICE_2});
            tabla += "<td align=\"right\">" + importeLocal + "</td>";
            tabla += "<td align=\"center\">" + listaOrdenesBancarias[i].monedaForanea + "</td>";
            var importeForanea = listaOrdenesBancarias[i].importeForanea === 0 ? "" : listaOrdenesBancarias[i].importeForanea.toLocaleString(constante.LOCALE_EN, {minimumFractionDigits: constante.INDICE_2});
            tabla += "<td align=\"right\">" + importeForanea + "</td>";
            tabla += "<td>" + listaOrdenesBancarias[i].liberador + "</td>";
            tabla += "<td>" + listaOrdenesBancarias[i].preAprobador + "</td>";
            tabla += "<td>" + listaOrdenesBancarias[i].idSap + "</td>";
            tabla += "<td>" + listaOrdenesBancarias[i].estadoOrden + "</td>";
            var fechaCreacion = felix.format("{0} {1}", listaOrdenesBancarias[i].fechaCreacion, listaOrdenesBancarias[i].horaCreacion);
            tabla += "<td>" + fechaCreacion  + "</td>";
            tabla += "<td><div class=\"d-flex\"><i class=\"material-icons cursor\" onclick=\"fcVerOrdenBancariaPorAprobar('" + string64OrdenBancaria + "');\" title=\"Ver Orden Bancaria Liberada\" >search</i>";
            tabla += "<i class=\"material-icons cursor\" onclick=\"fcAnularOrdenBancariaLiberada('" + string64OrdenBancaria + "');\" title=\"Anular Orden Bancaria Liberada\" >delete</i></div></td>";
            tabla += "</tr>";
        }

        tabla += "</table>";
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
    return tabla;
}

function fcBuscarOrdenesBancariasPorAprobar() {
    try {
        var idSap = $("#txtIdSap").val();
        var fechaInicio = $("#txtFechaInicio").val();
        var fechaFin = $("#txtFechaFin").val();
        var mensaje = "";

        if (idSap == "" && fechaInicio == "" && fechaFin == "")
        {
            mensaje += constante.MENSAJE_INGRESE_CAMPO_BUSQUEDA;
            $("#txtIdSap").css("border-color", "red");
            $("#lblIdSap").css("display", "inline");
            $("#txtFechaInicio").css("border-color", "red");
            $("#lblFechaInicio").css("display", "inline");
            $("#txtFechaFin").css("border-color", "red");
            $("#lblFechaFin").css("display", "inline");
        }
        else
        {
            if (fechaInicio != "" || fechaFin != "")
            {
                if (fechaInicio == "")
                {
                    mensaje += constante.MENSAJE_INGRESE_FECHA_INICIO;
                    $("#txtFechaInicio").css("border-color", "red");
                    $("#lblFechaInicio").css("display", "inline");
                }
                if (fechaFin == "")
                {
                    mensaje += constante.MENSAJE_INGRESE_FECHA_FIN;
                    $("#txtFechaFin").css("border-color", "red");
                    $("#lblFechaFin").css("display", "inline");
                }
            }
            else if (idSap == "")
            {
                mensaje += constante.MENSAJE_INGRESE_ID_SAP;
                $("#txtIdSap").css("border-color", "red");
                $("#lblIdSap").css("display", "inline");
            }
        }

        if (mensaje !== "")
        {
            felix.fcAlert(constante.TITULO_MENSAJE, mensaje);
            return false;
        }

        var parametros = felix.format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}", constante.QUESTION, constante.ID_SAP, constante.EQUAL, idSap, constante.AMPERSON, constante.FECHA_INICIO, constante.EQUAL, fechaInicio.trim(), constante.AMPERSON, constante.FECHA_FIN, constante.EQUAL, fechaFin.trim());
        
        felix.fcHttpClient(constante.ASYNC_TRUE, constante.HTTP_GET, constante.APROBACION_BUSCAR_ORDENES_BANCARIAS_POR_APROBAR_ASYNC, parametros, function (res) {
            if (res !== "")
            {
                if (res.codigo === constante.CODIGO_OK)
                {
                    $("#dvOrdenBancaria, #nvPaginacion").empty();
                    $("#lblRegistros").html("");
                    var tabla = fcConstruirBandeja(res.listaOrdenesBancarias);
                    $("#dvOrdenBancaria").append(tabla);
                    $("#chkAprobar").on("change", function () {
                        fcSeleccionarOrdenesBancariasPorAprobar();
                    });
                }
                else if (res.codigo === constante.CODIGO_OMISION)
                {
                    $("#dvOrdenBancaria, #nvPaginacion").empty();
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
                    $("#dvOrdenBancaria, #nvPaginacion").empty();
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
        var residuoDecimal = totalPaginas % constante._1;
        totalPaginas = residuoDecimal !== constante._0 ? parseInt(totalPaginas.toString(), constante._10) + constante._1 : totalPaginas;
        var contador = constante._1;
        var paginaAnterior = pagina === constante.PRIMERA_PAGINA ? pagina : pagina - constante._1;
        var paginaSiguiente = pagina === totalPaginas ? pagina : pagina + constante._1;
        
        if (totalPaginas > constante.CANTIDAD_PAGINAS && pagina > constante.CANTIDAD_PAGINAS)
        {
            var redondeo = pagina % 2 === constante._0 ? pagina - constante._1 : pagina;
            redondeo = parseInt(redondeo / constante._10, constante._10) * constante._10;
            totalPaginas = redondeo + constante.CANTIDAD_PAGINAS;
            contador = redondeo + constante._1;
        }
        else if (totalPaginas > constante.CANTIDAD_PAGINAS && pagina <= constante.CANTIDAD_PAGINAS)
        {
            totalPaginas = constante.CANTIDAD_PAGINAS;
        }

        $("#nvPaginacion").empty();
        var paginado = "";
        paginado += "<ul class=\"pagination justify-content-end\">";
        paginado += "<li class\"page-item\"><a class=\"page-link\" href=\"javascript:void(0);\"><span aria-hidden=\"true\">&laquo;</span></a></li>";
        paginado += "<li class\"page-item\"><a class=\"page-link\" href=\"javascript:void(0);\" onclick=\"fcListarOrdenesBancariasAprobadasPaginado('" + constante.PAGINA_ANTERIOR + "', " + paginaAnterior + ", " + constante.CANTIDAD_FILAS + ", '', '', '', '');\"><span aria-hidden=\"true\">&larr;</span></a></li>";
        
        for (var i = contador; i <= totalPaginas; i++)
        {
            paginado += "<li class=\"page-item correlativo\"><a class=\"page-link\" href=\"javascript:void(0);\" onclick=\"fcListarOrdenesBancariasAprobadasPaginado('" + constante.PAGINA_CLICADA + "', " + i + ", " + constante.CANTIDAD_FILAS + ", '', '', '', '');\">" + i + "</a></li>";
        }

        paginado += "<li class\"page-item\"><a class=\"page-link\" href=\"javascript:void(0);\" onclick=\"fcListarOrdenesBancariasAprobadasPaginado('" + constante.PAGINA_SIGUIENTE + "', " + paginaSiguiente + ", " + constante.CANTIDAD_FILAS + ", '', '', '', '');\"><span aria-hidden=\"true\">&rarr;</span></a></li>";
        paginado += "<li class\"page-item\"><a class=\"page-link\" href=\"javascript:void(0);\"><span aria-hidden=\"true\">&raquo;</span></a></li>";
        paginado += "</ul>";
        $("#nvPaginacion").append(paginado);
        var children = $("#nvPaginacion > ul").find("li.correlativo");

        $(children).each(function (index, element) {
            $(element).removeClass("active");
            var paginaActual = parseInt($(element).text(), constante._10);

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

        var desde = (pagina * constante.CANTIDAD_FILAS) - constante.CANTIDAD_FILAS + constante._1;
        var hasta = pagina * constante.CANTIDAD_FILAS > totalRegistros ? totalRegistros : pagina * constante.CANTIDAD_FILAS;
        var registros = felix.format(constante.MENSAJE_REGISTROS, desde, hasta, totalRegistros);
        $("#lblRegistros").html(registros);
        felix.fcDesplazarArriba();
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcListarOrdenesBancariasAprobadasPaginado(accion, pagina, filas, idSociedad, idBanco, idTipoOrden, idEstadoOrden) {
    try {
        var esPaginaActual = false;

        if (accion !== constante.PAGINA_FILTRADA)
        {
            var children = $("#nvPaginacion > ul").find("li.correlativo");

            $(children).each(function (index, element) {
                var esActivo = $(element).hasClass("active");
                var paginaActual = parseInt($(element).text(), constante._10);

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

        fcListarOrdenesBancariasPorAprobar(accion, pagina, filas, idSociedad, idBanco, idTipoOrden, idEstadoOrden);
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcListarFiltrosOrdenesBancariasPorAprobar() {
    try {
        var parametros;

        felix.fcHttpClient(constante.ASYNC_TRUE, constante.HTTP_GET, constante.APROBACION_LISTAR_FILTROS_ORDENES_BANCARIAS_POR_APROBAR_ASYNC, parametros, function (res) {
            if (res !== "")
            {
                if (res.codigo === constante.CODIGO_OK)
                {
                    $("#ulFiltrado").empty();
                    var tabla = fcConstruirFiltrado(res.listaFiltros);
                    $("#ulFiltrado").append(tabla);
                    $("#btnFiltrar").on("click", function () {
                        fcLimpiarBusqueda();
                        fcListarFiltrosOrdenesBancariasPorAprobar();
                    });
                }
                else if (res.codigo === constante.CODIGO_OMISION)
                {
                    $("#ulFiltrado").empty();
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
                    $("#ulFiltrado").empty();
                }
            }
        });
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcConstruirFiltrado(listaFiltros) {
    try {
        if (typeof listaFiltros == "undefined" || listaFiltros.length === 0)
        {
            felix.fcAlert(constante.TITULO_MENSAJE, constante.MENSAJE_PARAMETROS_NO_PRESENTES);
            return false;
        }

        var filtrado = "";
        filtrado += "<li class=\"sidebar-brand\"><a href=\"javascript:void(0);\">[Filtros]</a></li>";
        filtrado += "<li class=\"sidebar-brand-label\"><a href=\"javascript:void(0);\"><i class=\"material-icons\">list</i> Seleccione Criterio</a></li>";
        
        for (var i = 0; i < listaFiltros.length; i++)
        {
            var categoria = listaFiltros[i];
            var nombreCategoria = categoria.categoria.trim().replace(/ /gi, "");
            var parent = "<li class=\"sidebar-categoria\">";
            parent += "<a href=\"#" + nombreCategoria + "\" data-toggle=\"collapse\" aria-expanded=\"false\" class=\"dropdown-toggle\">" + categoria.categoria + "</a>";
            parent += "<ul class=\"collapse sidebar-opcion\" id=\"" + nombreCategoria +  "\">";

            for (var j = 0; j < categoria.listaOpciones.length; j++)
            {
                var opcion = categoria.listaOpciones[j];
                var idSociedad = "", idBanco = "", idTipoOrden = "", idEstadoOrden = "", id = "", descripcion = "";
                var child = "<li>";
                child += "<div class=\"form-check\">";

                if (categoria.categoria === constante.FILTRO_CATEGORIA_SOCIEDAD)
                {
                    idSociedad = opcion.idSociedad;
                    id = felix.format("{0}{1}", constante.CHECK_SOCIEDAD, opcion.idSociedad);
                    descripcion = opcion.sociedad;
                }
                else if (categoria.categoria === constante.FILTRO_CATEGORIA_BANCO)
                {
                    idBanco = opcion.idBanco;
                    id = felix.format("{0}{1}", constante.CHECK_BANCO, opcion.idBanco);
                    descripcion = opcion.banco;
                }
                else if (categoria.categoria === constante.FILTRO_CATEGORIA_TIPO_ORDEN)
                {
                    idTipoOrden = opcion.idTipoOrden;
                    id = felix.format("{0}{1}", constante.CHECK_TIPO_ORDEN, opcion.idTipoOrden);
                    descripcion = opcion.tipoOrden;
                }
                else if (categoria.categoria === constante.FILTRO_CATEGORIA_ESTADO_ORDEN)
                {
                    idEstadoOrden = opcion.idEstadoOrden;
                    id = felix.format("{0}{1}", constante.CHECK_ESTADO_ORDEN, opcion.idEstadoOrden);
                    descripcion = opcion.estadoOrden;
                }

                child += "<input type=\"checkbox\" id=\"" + id + "\" class=\"form-check-input\" readonly />";
                child += "<label class=\"form-check-label\" for=\"" + id + "\" onclick=\"fcListarOrdenesBancariasPorAprobarFiltrado('" + categoria.categoria + "', '" + constante.PAGINA_FILTRADA + "', " + constante.PRIMERA_PAGINA + ", " + constante.CANTIDAD_FILAS + ", '" + idSociedad + "', '" + idBanco + "', '" + idTipoOrden + "', '" + idEstadoOrden + "');\">" + descripcion + "</label>";
                child += "</div>";
                child += "</li>";
                parent += child;
            }

            parent += "</ul>";
            parent += "</li>";
            filtrado += parent;
        }

        filtrado += "<li class=\"mb-2\"><div class=\"buttons\"><button type=\"button\" id=\"btnFiltrar\" class=\"btn btn-outline-dark\">Limpiar Filtros</button></div></li>";
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
    return filtrado;
}

function fcListarOrdenesBancariasPorAprobarFiltrado(categoria, accion, pagina, filas, idSociedad, idBanco, idTipoOrden, idEstadoOrden) {
    try {
        var parents = $("#ulFiltrado").find(".sidebar-categoria");

        $(parents).each(function (index1, element1) {
            var actualCategoria = $(element1).find("a").text();
            var children = $(element1).find("ul li .form-check-input");

            $(children).each(function (index2, element2) {
                var checkId = $(element2).attr("id");
                var isChecked = $(element2).is(":checked");

                switch (actualCategoria)
                {
                    case constante.FILTRO_CATEGORIA_SOCIEDAD:
                    var actualIdSociedad = checkId.replace(constante.CHECK_SOCIEDAD, "");
                    
                    if (actualCategoria === categoria)
                    {
                        if (actualIdSociedad === idSociedad)
                        {
                            idSociedad = isChecked ? "" : idSociedad;
                        }
                        else
                        {
                            $(element2).prop("checked", false);
                        }
                    }
                    else
                    {
                        if (isChecked)
                        {
                            idSociedad = actualIdSociedad;
                        }
                    }
                    break;
                    case constante.FILTRO_CATEGORIA_BANCO:
                    var actualIdBanco = checkId.replace(constante.CHECK_BANCO, "");

                    if (actualCategoria === categoria)
                    {
                        if (actualIdBanco === idBanco)
                        {
                            idBanco = isChecked ? "" : idBanco;
                        }
                        else
                        {
                            $(element2).prop("checked", false);
                        }
                    }
                    else
                    {
                        if (isChecked)
                        {
                            idBanco = actualIdBanco;
                        }
                    }
                    break;
                    case constante.FILTRO_CATEGORIA_TIPO_ORDEN:
                    var actualIdTipoOrden = checkId.replace(constante.CHECK_TIPO_ORDEN, "");

                    if (actualCategoria === categoria)
                    {
                        if (actualIdTipoOrden === idTipoOrden)
                        {
                            idTipoOrden = isChecked ? "" : idTipoOrden;
                        }
                        else
                        {
                            $(element2).prop("checked", false);
                        }
                    }
                    else
                    {
                        if (isChecked)
                        {
                            idTipoOrden = actualIdTipoOrden;
                        }
                    }
                    break;
                    case constante.FILTRO_CATEGORIA_ESTADO_ORDEN:
                    var actualIdEstadoOrden = checkId.replace(constante.CHECK_ESTADO_ORDEN, "");

                    if (actualCategoria === categoria)
                    {
                        if (actualIdEstadoOrden === idEstadoOrden)
                        {
                            idEstadoOrden = isChecked ? "" : idEstadoOrden;
                        }
                        else
                        {
                            $(element2).prop("checked", false);
                        }
                    }
                    else
                    {
                        if (isChecked)
                        {
                            idEstadoOrden = actualIdEstadoOrden;
                        }
                    }
                    break;
                }
            });
        });

        fcListarOrdenesBancariasPorAprobar(accion, pagina, filas, idSociedad, idBanco, idTipoOrden, idEstadoOrden);
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcLimpiarCampos() {
    try {
        $("#txtIdSap").val("");
        $("#txtIdSap").css("border-color", "");
        $("#lblIdSap").css("display", "none");
        $("#txtFechaInicio").val("");
        $("#txtFechaInicio").css("border-color", "");
        $("#lblFechaInicio").css("display", "none");
        $("#txtFechaFin").val("");
        $("#txtFechaFin").css("border-color", "");
        $("#lblFechaFin").css("display", "none");
        $("#dvOrdenBancaria").empty();
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcLimpiarBusqueda() {
    try {
        fcLimpiarCampos();
        fcListarOrdenesBancariasPorAprobar(constante.PAGINA_CLICADA, constante.PRIMERA_PAGINA, constante.CANTIDAD_FILAS, constante.VACIO, constante.VACIO, constante.VACIO, constante.VACIO);
        fcListarFiltrosOrdenesBancariasPorAprobar();
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcVerOrdenBancariaPorAprobar(string64OrdenBancaria) {
    try {
        if (typeof string64OrdenBancaria === "undefined" || string64OrdenBancaria == "")
        {
            felix.fcAlert(constante.TITULO_MENSAJE, constante.MENSAJE_PARAMETROS_NO_PRESENTES);
            return false;
        }

        var url = felix.format(constante.APROBACION_VER_ORDEN_BANCARIA_POR_APROBAR, string64OrdenBancaria);
        window.location.href = url;
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcAprobarOrdenBancariaLiberada(string64OrdenBancaria) {
    try {
        if (typeof string64OrdenBancaria === "undefined" || string64OrdenBancaria == "")
        {
            felix.fcAlert(constante.TITULO_MENSAJE, constante.MENSAJE_PARAMETROS_NO_PRESENTES);
            return false;
        }

        var url = felix.format(constante.APROBACION_APROBAR_ORDEN_BANCARIA_LIBERADA, string64OrdenBancaria);
        window.location.href = url;
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcAnularOrdenBancariaLiberada(string64OrdenBancaria) {
    try {
        if (typeof string64OrdenBancaria === "undefined" || string64OrdenBancaria == "")
        {
            felix.fcAlert(constante.TITULO_MENSAJE, constante.MENSAJE_PARAMETROS_NO_PRESENTES);
            return false;
        }

        var url = felix.format(constante.APROBACION_ANULAR_ORDEN_BANCARIA_LIBERADA, string64OrdenBancaria);
        window.location.href = url;
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcSeleccionarOrdenesBancariasPorAprobar() {
    try {
        var isChecked = $("#chkAprobar").is(":checked");
        var parents = $("#tbOrdenBancaria").find("tbody").children();

        $(parents).each(function (index, element) {
            var checkBox = $(element).find(".form-check-input");
            $(checkBox).prop("checked", isChecked);
        });
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcValidarSeleccionMasivoOrdenesBancarias() {
    try {
        var cadena = "";
        var esEstadoLiberado = false, esEstadoPreAprobado = false;
        var parents = $("#tbOrdenBancaria").find("tbody").children();

        $(parents).each(function (index, element) {
            var tdCheckBox = $(element).children()[constante._0];
            var tdEstadoOrden = $(element).children()[constante._13];
            var checkBox = $(tdCheckBox).find(".form-check-input");
            var isChecked = $(checkBox).is(":checked");
            var estadoOrden = $(tdEstadoOrden).text();
            var value = $(checkBox).val();
            
            if (isChecked)
            {
                cadena += felix.format("{0}{1}", value, constante.AMPERSON);

                if (estadoOrden === constante.ESTADO_ORDEN_LIBERADO)
                {
                    esEstadoLiberado = true;
                }
                else if (estadoOrden === constante.ESTADO_ORDEN_PRE_APROBADO)
                {
                    esEstadoPreAprobado = true;
                }
            }
        });

        if (cadena === "")
        {
            felix.fcAlert(constante.TITULO_MENSAJE, constante.MENSAJE_SELECCIONE_REGISTROS);
            return false;
        }

        cadena = cadena.substr(constante._0, cadena.length - constante._1);

        if (esEstadoLiberado && esEstadoPreAprobado)
        {
            felix.fcAlert(constante.TITULO_MENSAJE, constante.MENSAJE_SELECCIONE_MISMO_ESTADO);
            return false;
        }

        fcConsultarEstadoMasivoOrdenesBancarias(cadena);
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcConsultarEstadoMasivoOrdenesBancarias(cadena) {
    try {
        if (typeof cadena === "undefined" || cadena == "")
        {
            felix.fcAlert(constante.TITULO_MENSAJE, constante.MENSAJE_PARAMETROS_NO_PRESENTES);
            return false;
        }

        var parametros = felix.format("{0}{1}{2}{3}", constante.QUESTION, constante.CADENA, constante.EQUAL, cadena);

        felix.fcHttpClient(constante.ASYNC_TRUE, constante.HTTP_GET, constante.APROBACION_CONSULTAR_ESTADO_MASIVO_ORDENES_BANCARIAS_ASYNC, parametros, function (res) {
            if (res !== "")
            {
                if (res.codigo === constante.CODIGO_OK)
                {
                    if (res.esEstadoLiberado && res.esEstadoPreAprobado)
                    {
                        felix.fcAlert(constante.TITULO_MENSAJE, constante.MENSAJE_CAMBIO_ESTADO_ORDEN_BANCARIA);
                    }
                    else
                    {
                        if (res.esEstadoLiberado)
                        {
                            fcAprobarMasivoOrdenesBancarias(cadena, constante.ID_ESTADO_ORDEN_LIBERADO);
                        }
                        else if (res.esEstadoPreAprobado)
                        {
                            if (res.esHorarioBanco === constante.ES_HORARIO_BANCO_SI)
                            {
                                felix.fcAlertConfirm(constante.TITULO_MENSAJE, constante.MENSAJE_APROBAR_MASIVO_ORDENES_BANCARIAS, function () {
                                    fcAprobarMasivoOrdenesBancarias(cadena, constante.ID_ESTADO_ORDEN_PRE_APROBADO);
                                });
                            }
                            else if (res.esHorarioBanco === constante.ES_HORARIO_BANCO_NO)
                            {
                                felix.fcAlert(constante.TITULO_MENSAJE, constante.MENSAJE_HORARIO_ATENCION_BANCO);
                            }
                        }
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

function fcAprobarMasivoOrdenesBancarias(cadena, idEstadoOrden) {
    try {
        if (typeof cadena === "undefined" || cadena == "" || typeof idEstadoOrden === "undefined" || idEstadoOrden == "")
        {
            felix.fcAlert(constante.TITULO_MENSAJE, constante.MENSAJE_PARAMETROS_NO_PRESENTES);
            return false;
        }

        var parametros = new Array();
        parametros[0] = cadena;
        parametros[1] = idEstadoOrden;

        felix.fcHttpClient(constante.ASYNC_TRUE, constante.HTTP_POST, constante.APROBACION_APROBAR_MASIVO_ORDENES_BANCARIAS_ASYNC, parametros, function (res) {
            if (res !== "")
            {
                if (res.codigo === constante.CODIGO_OK)
                {
                    felix.fcAlertRedirect(constante.TITULO_MENSAJE, res.mensaje, function () {
                        window.location.href = constante.HREF_APROBACION_LISTAR_ORDENES_BANCARIAS_POR_APROBAR;
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

function fcExportarHaciaExcel() {
    try {
        felix.fcExportarHaciaExcel(constante.TABLA_ORDEN_BANCARIA, constante.ORDENES_BANCARIAS_POR_APROBAR, constante.ORDENES_BANCARIAS_POR_APROBAR);
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}