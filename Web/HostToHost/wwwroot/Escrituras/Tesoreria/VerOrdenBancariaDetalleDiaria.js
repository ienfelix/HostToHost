
$(window.document).ready(function () {
    var string64OrdenBancaria;
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
            var keyTipoOrden = queryString[constante._1].split(constante.AMPERSON)[constante._0].split(constante.EQUAL)[constante._0];
            var idTipoOrden = queryString[constante._1].split(constante.AMPERSON)[constante._0].split(constante.EQUAL)[constante._1];
            var keyOrdenBancaria = queryString[constante._1].split(constante.AMPERSON)[constante._1].split(constante.EQUAL)[constante._0];
            string64OrdenBancaria = queryString[constante._1].split(constante.AMPERSON)[constante._1].split(constante.EQUAL)[constante._1];
            var keyOrdenBancariaDetalle = queryString[constante._1].split(constante.AMPERSON)[constante._2].split(constante.EQUAL)[constante._0];
            var string64OrdenBancariaDetalle = queryString[constante._1].split(constante.AMPERSON)[constante._2].split(constante.EQUAL)[constante._1];
            
            if (keyTipoOrden == "" || idTipoOrden == "" || keyOrdenBancaria == "" || keyOrdenBancariaDetalle == "" || string64OrdenBancariaDetalle == "")
            {
                esCorrecto = false;
            }
            else
            {
                var encodedString = atob(string64OrdenBancariaDetalle);
                var decodedString = decodeURIComponent(encodedString);
                var ordenBancariaDetalle = JSON.parse(decodedString);
                fcObtenerVistaParcialOrdenBancariaDetalleDiaria(idTipoOrden, ordenBancariaDetalle);
            }
        }

        if (esCorrecto === false)
        {
            felix.fcAlertRedirect(constante.TITULO_MENSAJE, constante.MENSAJE_PARAMETROS_NO_PRESENTES, function () {
                window.location.href = constante.HREF_TESORERIA_LISTAR_ORDENES_BANCARIAS_DIARIAS;
            });
        }
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcObtenerVistaParcialOrdenBancariaDetalleDiaria(idTipoOrden, ordenBancariaDetalle) {
    try {
        if (typeof idTipoOrden === "undefined" || idTipoOrden == "" || typeof ordenBancariaDetalle === "undefined" || ordenBancariaDetalle.length === 0)
        {
            felix.fcAlert(constante.TITULO_MENSAJE, constante.MENSAJE_PARAMETROS_NO_PRESENTES);
            return false;
        }

        var parametros = felix.format("{0}{1}{2}{3}", constante.QUESTION, constante.ID_TIPO_ORDEN, constante.EQUAL, idTipoOrden);

        felix.fcHttpClient(constante.ASYNC_TRUE, constante.HTTP_GET, constante.TESORERIA_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_DIARIA_ASYNC, parametros, function (res) {
            if (res !== "")
            {
                if (res.codigo === constante.CODIGO_OK)
                {
                    $("#dvOrdenBancariaDetalle").replaceWith(res.vistaParcialOrdenBancariaDetalleDiaria);
                    
                    switch (idTipoOrden)
                    {
                        case constante.ID_TIPO_ORDEN_TRANSFERENCIA_BCR:
                        fcMostrarOrdenBancariaDetalleTransferenciaBcr(ordenBancariaDetalle);
                        break;
                        case constante.ID_TIPO_ORDEN_PROVEEDOR:
                        case constante.ID_TIPO_ORDEN_CAMARA_COMERCIO:
                        fcMostrarOrdenBancariaDetalleProveedor(ordenBancariaDetalle);
                        break;
                    }

                    var url = felix.format(constante.TESORERIA_VER_ORDEN_BANCARIA_DIARIA, string64OrdenBancaria);
                    $("#btnRegresar").attr("href", url);
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

function fcMostrarOrdenBancariaDetalleTransferenciaBcr(ordenBancariaDetalle) {
    try {
        if (typeof ordenBancariaDetalle === "undefined" || ordenBancariaDetalle.length === 0)
        {
            felix.fcAlert(constante.TITULO_MENSAJE, constante.MENSAJE_PARAMETROS_NO_PRESENTES);
            return false;
        }

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
        var montoCargo = ordenBancariaDetalle.montoCargo === 0 ? "" : ordenBancariaDetalle.montoCargo.toLocaleString(constante.LOCALE_EN, {minimumFractionDigits: constante._2});
        $("#txtMontoCargo").val(montoCargo);
        $("#txtCuentaAbono").val(ordenBancariaDetalle.cuentaAbono);
        $("#txtCuentaGasto").val(ordenBancariaDetalle.cuentaGasto);
        $("#txtModuloRaiz").val(ordenBancariaDetalle.moduloRaiz);
        $("#txtDigitoControl").val(ordenBancariaDetalle.digitoControl);
        $("#txtBeneficiario").val(ordenBancariaDetalle.beneficiario);
        var tipoDocumento = felix.format("{0} - {1}", ordenBancariaDetalle.tipoDocumento, ordenBancariaDetalle.tipoDocumentoCorto);
        $("#txtTipoDocumento").val(tipoDocumento);
        $("#txtNroDocumento").val(ordenBancariaDetalle.nroDocumento);
        $("#txtNombreBanco").val(ordenBancariaDetalle.nombreBanco);
        $("#txtRucBanco").val(ordenBancariaDetalle.rucBanco);
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}

function fcMostrarOrdenBancariaDetalleProveedor(ordenBancariaDetalle) {
    try {
        if (typeof ordenBancariaDetalle === "undefined" || ordenBancariaDetalle.length === 0)
        {
            felix.fcAlert(constante.TITULO_MENSAJE, constante.MENSAJE_PARAMETROS_NO_PRESENTES);
            return false;
        }

        $("#txtIdRespuesta").val(ordenBancariaDetalle.idRespuesta);
        $("#txtRespuesta").val(ordenBancariaDetalle.respuesta);
        $("#txtNroOrden").val(ordenBancariaDetalle.nroOrden);
        $("#txtNroConvenio").val(ordenBancariaDetalle.nroConvenio);
        $("#txtFormaPago").val(ordenBancariaDetalle.formaPago);
        $("#txtSubTipoPago").val(ordenBancariaDetalle.subTipoPago);
        $("#txtReferencia1").val(ordenBancariaDetalle.referencia1);
        $("#txtReferencia2").val(ordenBancariaDetalle.referencia2);
        $("#txtCuentaCargo").val(ordenBancariaDetalle.cuentaCargo);
        var monedaAbono = felix.format("{0} - {1}", ordenBancariaDetalle.monedaAbono, ordenBancariaDetalle.monedaAbonoCorto);
        $("#txtMonedaAbono").val(monedaAbono);
        $("#txtCuentaAbono").val(ordenBancariaDetalle.cuentaAbono);
        var montoAbono = ordenBancariaDetalle.montoAbono === 0 ? "" : ordenBancariaDetalle.montoAbono.toLocaleString(constante.LOCALE_EN, {minimumFractionDigits: constante._2});
        $("#txtMontoAbono").val(montoAbono);
        $("#txtModuloRaiz").val(ordenBancariaDetalle.moduloRaiz);
        $("#txtDigitoControl").val(ordenBancariaDetalle.digitoControl);
        $("#txtBeneficiario").val(ordenBancariaDetalle.beneficiario);
        $("#txtNroDocumento").val(ordenBancariaDetalle.nroDocumento);
        $("#txtNroFactura").val(ordenBancariaDetalle.nroFactura);
        $("#txtFechaFactura").val(ordenBancariaDetalle.fechaFactura);
        $("#txtFechaFinFactura").val(ordenBancariaDetalle.fechaFinFactura);
        $("#txtSignoFactura").val(ordenBancariaDetalle.signoFactura);
    } catch (e) {
        felix.fcAlert(constante.TITULO_ERROR, e.message);
    }
}