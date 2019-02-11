
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
            var string64OrdenBancaria = queryString[constante._1].split(constante.EQUAL)[constante._1];
            
            if (key == "" || string64OrdenBancaria == "")
            {
                esCorrecto = false;
            }
            else
            {
                var encodedOrdenBancaria = atob(string64OrdenBancaria);
                var decodedOrdenBancaria = decodeURIComponent(encodedOrdenBancaria);
                var ordenBancaria = JSON.parse(decodedOrdenBancaria);
                fcObtenerVistaParcialOrdenBancariaLiberada(ordenBancaria);
            }
        }

        if (esCorrecto === false)
        {
            felix.fcAlertRedirect(constante.TITULO_MENSAJE, constante.MENSAJE_PARAMETROS_NO_PRESENTES, function () {
                window.location.href = constante.HREF_TESORERIA_LISTAR_ORDENES_BANCARIAS_LIBERADAS;
            });
        }
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcObtenerVistaParcialOrdenBancariaLiberada(ordenBancaria) {
    try {
        if (typeof ordenBancaria === "undefined" || ordenBancaria.length === 0)
        {
            felix.fcAlert(constante.TITULO_MENSAJE, constante.MENSAJE_PARAMETROS_NO_PRESENTES);
            return false;
        }

        var parametros = felix.format("{0}{1}{2}{3}", constante.QUESTION, constante.ID_TIPO_ORDEN, constante.EQUAL, ordenBancaria.idTipoOrden);

        felix.fcHttpClient(constante.ASYNC_TRUE, constante.HTTP_GET, constante.TESORERIA_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_LIBERADA_ASYNC, parametros, function (res) {
            if (res !== "")
            {
                if (res.codigo === constante.CODIGO_OK)
                {
                    $("#dvOrdenBancaria").replaceWith(res.vistaParcialOrdenBancariaLiberada);
                    $("#dvAccionesOrdenBancaria").replaceWith(res.vistaParcialAccionesOrdenBancaria);
                    fcMostrarOrdenBancariaLiberada(ordenBancaria);
                    fcListarFlujoAprobacion(ordenBancaria);
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

function fcMostrarOrdenBancariaLiberada(ordenBancaria) {
    try {
        if (typeof ordenBancaria === "undefined" || ordenBancaria.length === 0)
        {
            felix.fcAlert(constante.TITULO_MENSAJE, constante.MENSAJE_PARAMETROS_NO_PRESENTES);
            return false;
        }

        $("#nav-obd-tab").hide();
        $("#dvComentarios").show();
        $("#btnAprobar").hide();
        $("#btnDeshacer").show();
        $("#btnAnular").hide();
        $("#btnRegresar").attr("href", constante.HREF_TESORERIA_LISTAR_ORDENES_BANCARIAS_LIBERADAS);
        var sociedad = felix.format("{0} - {1}", ordenBancaria.sociedad, ordenBancaria.sociedadCorto);
        $("#txtSociedad").val(sociedad);
        $("#txtIdSap").val(ordenBancaria.idSap);
        $("#txtAnio").val(ordenBancaria.anio);
        $("#txtFechaOrden").val(ordenBancaria.fechaOrden);
        var banco = felix.format("{0} - {1}", ordenBancaria.banco, ordenBancaria.bancoCorto);
        $("#txtBanco").val(banco);
        var tipoOrden = felix.format("{0} - {1}", ordenBancaria.tipoOrden, ordenBancaria.tipoOrdenCorto);
        $("#txtTipoOrden").val(tipoOrden);
        $("#txtEstadoOrden").val(ordenBancaria.estadoOrden);
        var monedaLocal = ordenBancaria.monedaLocal === "" ? "" : felix.format("{0} - {1}", ordenBancaria.monedaLocal, ordenBancaria.monedaCortoLocal);
        $("#txtMonedaLocal").val(monedaLocal);
        var importeLocal = ordenBancaria.importeLocal === 0 ? "" : ordenBancaria.importeLocal.toLocaleString();
        $("#txtImporteLocal").val(importeLocal);
        var monedaForanea = ordenBancaria.monedaForanea === "" ? "" : felix.format("{0} - {1}", ordenBancaria.monedaForanea, ordenBancaria.monedaCortoForanea);
        $("#txtMonedaForanea").val(monedaForanea);
        var importeForanea = ordenBancaria.importeForanea === 0 ? "" : ordenBancaria.importeForanea.toLocaleString();
        $("#txtImporteForanea").val(importeForanea);
        $("#txtUsuarioCreacion").val(ordenBancaria.usuarioCreacion);
        var fechaCreacion = felix.format("{0} {1}", ordenBancaria.fechaCreacion, ordenBancaria.horaCreacion);
        $("#txtFechaCreacion").val(fechaCreacion);
        $("#btnDeshacer").on("click", function () {
            fcDeshacerOrdenBancariaLiberada(ordenBancaria.idSociedad, ordenBancaria.idSap, ordenBancaria.anio, ordenBancaria.momentoOrden, ordenBancaria.idTipoOrden, ordenBancaria.idEstadoOrden, ordenBancaria.tipoOrdenCorto);
        });
        fcListarOrdenesBancariasDetalleLiberadas(ordenBancaria);
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcListarOrdenesBancariasDetalleLiberadas(ordenBancaria)
{
    try {
        if (typeof ordenBancaria === "undefined" || ordenBancaria.length === 0)
        {
            felix.fcAlert(constante.TITULO_MENSAJE, constante.MENSAJE_PARAMETROS_NO_PRESENTES);
            return false;
        }

        var parametros = felix.format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}", constante.QUESTION, constante.ID_SOCIEDAD, constante.EQUAL, ordenBancaria.idSociedad, constante.AMPERSON, constante.ID_SAP, constante.EQUAL, ordenBancaria.idSap, constante.AMPERSON, constante.ANIO, constante.EQUAL, ordenBancaria.anio, constante.AMPERSON, constante.MOMENTO_ORDEN, constante.EQUAL, ordenBancaria.momentoOrden, constante.AMPERSON, constante.ID_TIPO_ORDEN, constante.EQUAL, ordenBancaria.idTipoOrden);

        felix.fcHttpClient(constante.ASYNC_TRUE, constante.HTTP_GET, constante.TESORERIA_LISTAR_ORDENES_BANCARIAS_DETALLE_LIBERADAS_ASYNC, parametros, function (res) {
            if (res !== "")
            {
                if (res.codigo === constante.CODIGO_OK)
                {
                    switch (ordenBancaria.idTipoOrden)
                    {
                        case constante.ID_TIPO_ORDEN_TRANSFERENCIA:
                        fcMostrarOrdenBancariaTransferencia(ordenBancaria, res.listaOrdenesBancariasDetalle[constante._0]);
                        break;
                        case constante.ID_TIPO_ORDEN_TRANSFERENCIA_BCR:
                        fcMostrarOrdenBancariaTransferenciaBcr(ordenBancaria, res.listaOrdenesBancariasDetalle);
                        break;
                        case constante.ID_TIPO_ORDEN_PROVEEDOR:
                        case constante.ID_TIPO_ORDEN_CAMARA_COMERCIO:
                        fcMostrarOrdenBancariaProveedor(ordenBancaria, res.listaOrdenesBancariasDetalle);
                        break;
                    }
                }
                else if (res.codigo === constante.CODIGO_OMISION)
                {
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
                }
            }
        });
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcMostrarOrdenBancariaTransferencia(ordenBancaria, ordenBancariaDetalle) {
    try {
        if (typeof ordenBancaria === "undefined" || ordenBancaria.length === 0 || typeof ordenBancariaDetalle == "undefined" || ordenBancariaDetalle.length === 0)
        {
            felix.fcAlert(constante.TITULO_MENSAJE, constante.MENSAJE_PARAMETROS_NO_PRESENTES);
            return false;
        }

        var parametros = felix.format("{0}{1}{2}{3}", constante.QUESTION, constante.ID_TIPO_ORDEN, constante.EQUAL, ordenBancaria.idTipoOrden);

        felix.fcHttpClient(constante.ASYNC_TRUE, constante.HTTP_GET, constante.TESORERIA_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_LIBERADA_ASYNC, parametros, function (res) {
            if (res !== "")
            {
                if (res.codigo === constante.CODIGO_OK)
                {
                    $("#dvOrdenBancariaDetalle").replaceWith(res.vistaParcialOrdenBancariaDetalleLiberada);
                    $("#txtIdRespuesta").val(ordenBancariaDetalle.idRespuesta);
                    $("#txtRespuesta").val(ordenBancariaDetalle.respuesta);
                    $("#txtNroOrden").val(ordenBancariaDetalle.nroOrden);
                    $("#txtNroConvenio").val(ordenBancariaDetalle.nroConvenio);
                    $("#txtTipoTransferencia").val(ordenBancariaDetalle.tipoTransferencia);
                    $("#txtReferencia1").val(ordenBancariaDetalle.referencia1);
                    $("#txtReferencia2").val(ordenBancariaDetalle.referencia2);
                    var monedaCargo = felix.format("{0} - {1}", ordenBancariaDetalle.monedaCargo, ordenBancariaDetalle.monedaCargoCorto);
                    $("#txtMonedaCargo").val(monedaCargo);
                    $("#txtCuentaCargo").val(ordenBancariaDetalle.cuentaCargo);
                    var montoCargo = ordenBancariaDetalle.montoCargo === 0 ? "" : ordenBancariaDetalle.montoCargo.toLocaleString();
                    $("#txtMontoCargo").val(montoCargo);
                    var monedaAbono = felix.format("{0} - {1}", ordenBancariaDetalle.monedaAbono, ordenBancariaDetalle.monedaAbonoCorto);
                    $("#txtMonedaAbono").val(monedaAbono);
                    $("#txtCuentaAbono").val(ordenBancariaDetalle.cuentaAbono);
                    var montoAbono = ordenBancariaDetalle.montoAbono === 0 ? "" : ordenBancariaDetalle.montoAbono.toLocaleString();
                    $("#txtMontoAbono").val(montoAbono);
                    var tipoCambio  = ordenBancariaDetalle.tipoCambio || "";
                    $("#txtTipoCambio").val(tipoCambio);
                    $("#txtModuloRaiz").val(ordenBancariaDetalle.moduloRaiz);
                    $("#txtDigitoControl").val(ordenBancariaDetalle.digitoControl);
                    $("#txtIndicador").val(ordenBancariaDetalle.indicador);
                    $("#txtNroOperacion").val(ordenBancariaDetalle.nroOperacion);
                    $("#txtBeneficiario").val(ordenBancariaDetalle.beneficiario);
                    var tipoDocumento = felix.format("{0} - {1}", ordenBancariaDetalle.tipoDocumento, ordenBancariaDetalle.tipoDocumentoCorto);
                    $("#txtTipoDocumento").val(tipoDocumento);
                    $("#txtNroDocumento").val(ordenBancariaDetalle.nroDocumento);
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

function fcMostrarOrdenBancariaTransferenciaBcr(ordenBancaria, listaOrdenesBancariasDetalle) {
    try {
        if (typeof ordenBancaria === "undefined" || ordenBancaria.length === 0 || typeof listaOrdenesBancariasDetalle == "undefined" || listaOrdenesBancariasDetalle.length === 0)
        {
            felix.fcAlert(constante.TITULO_MENSAJE, constante.MENSAJE_PARAMETROS_NO_PRESENTES);
            return false;
        }

        $("#nav-obd-tab").show();
        $("#dvOrdenBancariaDesglose").empty();
        var tabla = "<table class=\"table table-sm table-striped table-hover\">";
        tabla += "<thead class=\"thead-dark\" align=\"center\"><tr>";
        tabla += "<th>Referencia 1</th>";
        tabla += "<th>Moneda Cargo</th>";
        tabla += "<th>Cuenta Cargo</th>";
        tabla += "<th>Monto Cargo</th>";
        tabla += "<th>Beneficiario</th>";
        tabla += "<th>Nombre Banco</th>";
        tabla += "<th></th>";
        tabla += "</tr></thead>";

        for (var i = 0; i < listaOrdenesBancariasDetalle.length; i++)
        {
            tabla += "<tr>";
            tabla += "<td>" + listaOrdenesBancariasDetalle[i].referencia1 + "</td>";
            tabla += "<td>" + listaOrdenesBancariasDetalle[i].monedaCargo + "</td>";
            tabla += "<td>" + listaOrdenesBancariasDetalle[i].cuentaCargo + "</td>";
            var montoCargo = listaOrdenesBancariasDetalle[i].montoCargo === 0 ? "" : listaOrdenesBancariasDetalle[i].montoCargo.toLocaleString();
            tabla += "<td>" + montoCargo + "</td>";
            tabla += "<td>" + listaOrdenesBancariasDetalle[i].beneficiario + "</td>";
            tabla += "<td>" + listaOrdenesBancariasDetalle[i].nombreBanco + "</td>";
            var stringOrdenBancaria = JSON.stringify(ordenBancaria);
            var encodedOrdenBancaria = encodeURIComponent(stringOrdenBancaria);
            var string64OrdenBancaria = btoa(encodedOrdenBancaria);
            var stringOrdenBancariaDetalle = JSON.stringify(listaOrdenesBancariasDetalle[i]);
            var encodedOrdenBancariaDetalle = encodeURIComponent(stringOrdenBancariaDetalle);
            var string64OrdenBancariaDetalle = btoa(encodedOrdenBancariaDetalle);
            tabla += "<td><div class=\"d-flex\"><i class=\"material-icons cursor\" onclick=\"fcDeshacerOrdenBancariaDetalleLiberada('" + ordenBancaria.idTipoOrden + "', '" + string64OrdenBancaria + "', '" + string64OrdenBancariaDetalle + "');\" title=\"Ver Pago\" >search</i>";
            tabla += "</div></td>";
            tabla += "</tr>";
        }

        tabla += "</table>";
        $("#dvOrdenBancariaDesglose").append(tabla);
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcMostrarOrdenBancariaProveedor(ordenBancaria, listaOrdenesBancariasDetalle) {
    try {
        if (typeof ordenBancaria === "undefined" || ordenBancaria.length === 0 || typeof listaOrdenesBancariasDetalle == "undefined" || listaOrdenesBancariasDetalle.length === 0)
        {
            felix.fcAlert(constante.TITULO_MENSAJE, constante.MENSAJE_PARAMETROS_NO_PRESENTES);
            return false;
        }

        $("#nav-obd-tab").show();
        $("#dvOrdenBancariaDesglose").empty();
        var tabla = "<table class=\"table table-sm table-striped table-hover\">";
        tabla += "<thead class=\"thead-dark\" align=\"center\"><tr>";
        tabla += "<th>Sub Tipo Pago</th>";
        tabla += "<th>Referencia 1</th>";
        tabla += "<th>Moneda Abono</th>";
        tabla += "<th>Cuenta Abono</th>";
        tabla += "<th>Importe Neto</th>";
        tabla += "<th>Beneficiario</th>";
        tabla += "<th>Nro Factura</th>";
        tabla += "<th>Fecha Factura</th>";
        tabla += "<th>Fecha Fin Factura</th>";
        tabla += "<th></th>";
        tabla += "</tr></thead>";

        for (var i = 0; i < listaOrdenesBancariasDetalle.length; i++)
        {
            tabla += "<tr>";
            tabla += "<td>" + listaOrdenesBancariasDetalle[i].subTipoPago + "</td>";
            tabla += "<td>" + listaOrdenesBancariasDetalle[i].referencia1 + "</td>";
            tabla += "<td>" + listaOrdenesBancariasDetalle[i].monedaAbono + "</td>";
            tabla += "<td>" + listaOrdenesBancariasDetalle[i].cuentaAbono + "</td>";
            var montoAbono = listaOrdenesBancariasDetalle[i].montoAbono === 0 ? "" : listaOrdenesBancariasDetalle[i].montoAbono.toLocaleString();
            tabla += "<td>" + montoAbono + "</td>";
            tabla += "<td>" + listaOrdenesBancariasDetalle[i].beneficiario + "</td>";
            tabla += "<td>" + listaOrdenesBancariasDetalle[i].nroFactura + "</td>";
            tabla += "<td>" + listaOrdenesBancariasDetalle[i].fechaFactura + "</td>";
            tabla += "<td>" + listaOrdenesBancariasDetalle[i].fechaFinFactura + "</td>";
            var stringOrdenBancaria = JSON.stringify(ordenBancaria);
            var encodedOrdenBancaria = encodeURIComponent(stringOrdenBancaria);
            var string64OrdenBancaria = btoa(encodedOrdenBancaria);
            var stringOrdenBancariaDetalle = JSON.stringify(listaOrdenesBancariasDetalle[i]);
            var encodedOrdenBancariaDetalle = encodeURIComponent(stringOrdenBancariaDetalle);
            var string64OrdenBancariaDetalle = btoa(encodedOrdenBancariaDetalle);
            tabla += "<td><div class=\"d-flex\"><i class=\"material-icons cursor\" onclick=\"fcDeshacerOrdenBancariaDetalleLiberada('" + ordenBancaria.idTipoOrden + "', '" + string64OrdenBancaria + "', '" + string64OrdenBancariaDetalle + "');\" title=\"Ver Pago\" >search</i>";
            tabla += "</div></td>";
            tabla += "</tr>";
        }

        tabla += "</table>";
        $("#dvOrdenBancariaDesglose").append(tabla);
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcDeshacerOrdenBancariaDetalleLiberada(idTipoOrden, string64OrdenBancaria, string64OrdenBancariaDetalle) {
    try {
        if (typeof idTipoOrden === "undefined" || idTipoOrden == "" || typeof string64OrdenBancaria === "undefined" || string64OrdenBancaria == "" || typeof string64OrdenBancariaDetalle === "undefined" || string64OrdenBancariaDetalle == "")
        {
            felix.fcAlert(constante.TITULO_MENSAJE, constante.MENSAJE_PARAMETROS_NO_PRESENTES);
            return false;
        }

        var url = felix.format(constante.TESORERIA_DESHACER_ORDEN_BANCARIA_DETALLE_LIBERADA, idTipoOrden, string64OrdenBancaria, string64OrdenBancariaDetalle);
        window.location.href = url;
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcDeshacerOrdenBancariaLiberada(idSociedad, idSap, anio, momentoOrden, idTipoOrden, idEstadoOrden, tipoOrden) {
    try {
        if (typeof idSociedad === "undefined" || idSociedad == "" || typeof idSap === "undefined" || idSap == "" || typeof anio === "undefined" || anio == "" || typeof momentoOrden === "undefined" || momentoOrden == "" || typeof idEstadoOrden === "undefined" || idEstadoOrden == "")
        {
            felix.fcAlert(constante.TITULO_MENSAJE, constante.MENSAJE_PARAMETROS_NO_PRESENTES);
            return false;
        }

        var comentarios = $("#txtComentarios").val().trim();
        var parametros = new Array();
        parametros[0] = idSociedad;
        parametros[1] = idSap;
        parametros[2] = anio;
        parametros[3] = momentoOrden;
        parametros[4] = idTipoOrden;
        parametros[5] = comentarios;
        parametros[6] = idEstadoOrden;
        parametros[7] = tipoOrden;

        felix.fcHttpClient(constante.ASYNC_TRUE, constante.HTTP_PUT, constante.TESORERIA_DESHACER_ORDEN_BANCARIA_LIBERADA_ASYNC, parametros, function (res) {
            if (res !== "")
            {
                if (res.codigo === constante.CODIGO_OK)
                {
                    felix.fcAlertRedirect(constante.TITULO_MENSAJE, res.mensaje, function () {
                        var href = $("#btnRegresar").attr("href");
                        window.location.href = href;
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

function fcListarFlujoAprobacion(ordenBancaria) {
    try {
        if (typeof ordenBancaria === "undefined" || ordenBancaria.length === 0)
        {
            felix.fcAlert(constante.TITULO_MENSAJE, constante.MENSAJE_PARAMETROS_NO_PRESENTES);
            return false;
        }

        var parametros = felix.format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}", constante.QUESTION, constante.ID_SOCIEDAD, constante.EQUAL, ordenBancaria.idSociedad, constante.AMPERSON, constante.ID_SAP, constante.EQUAL, ordenBancaria.idSap, constante.AMPERSON, constante.ANIO, constante.EQUAL, ordenBancaria.anio, constante.AMPERSON, constante.MOMENTO_ORDEN, constante.EQUAL, ordenBancaria.momentoOrden, constante.AMPERSON, constante.ID_TIPO_ORDEN, constante.EQUAL, ordenBancaria.idTipoOrden);

        felix.fcHttpClient(constante.ASYNC_TRUE, constante.HTTP_GET, constante.TESORERIA_LISTAR_FLUJO_APROBACION_ASYNC, parametros, function (res) {
            if (res !== "")
            {
                if (res.codigo === constante.CODIGO_OK)
                {
                    $("#dvFlujoAprobacion").empty();
                    var tabla = "<table class=\"table table-sm table-striped table-hover\">";
                    tabla += "<thead class=\"thead-dark\" align=\"center\"><tr>";
                    tabla += "<th>#</th>";
                    tabla += "<th>Estado Flujo</th>";
                    tabla += "<th>Comentarios</th>";
                    tabla += "<th>Usuario Ejecución</th>";
                    tabla += "<th>Fecha Ejecución</th>";
                    tabla += "</tr></thead>";

                    for (var i = 0; i < res.listaFlujoAprobacion.length; i++)
                    {
                        tabla += "<tr>";
                        tabla += "<td>" + res.listaFlujoAprobacion[i].idFlujoAprobacion + "</td>";
                        var estadoFlujo = felix.format("{0} - {1}", res.listaFlujoAprobacion[i].estadoFlujo, res.listaFlujoAprobacion[i].estadoFlujoCorto);
                        tabla += "<td>" + estadoFlujo + "</td>";
                        tabla += "<td>" + res.listaFlujoAprobacion[i].comentarios + "</td>";
                        tabla += "<td>" + res.listaFlujoAprobacion[i].usuarioCreacion + "</td>";
                        var fechaCreacion = felix.format("{0} {1}", res.listaFlujoAprobacion[i].fechaCreacion, res.listaFlujoAprobacion[i].horaCreacion);
                        tabla += "<td>" + fechaCreacion + "</td>";
                        tabla += "</tr>";
                    }

                    tabla += "</table>";
                    $("#dvFlujoAprobacion").append(tabla);
                }
                else if (res.codigo === constante.CODIGO_OMISION)
                {
                    $("#dvFlujoAprobacion").empty();
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
                    $("#dvFlujoAprobacion").empty();
                }
            }
        });
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}