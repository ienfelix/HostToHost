using System;
using System.Threading;
using System.Threading.Tasks;
using Comun;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Modelo;
using Repositorio;

namespace Negocio
{
    public class AprobacionNE
    {
        private Bitacora _bitacora = null;
        private Util _util = null;
        private OrdenBancariaRE _ordenBancariaRE = null;
        private ServicioNE _servicioNE = null;

        public AprobacionNE(IConfiguration configuration)
        {
            _bitacora = _bitacora ?? new Bitacora(configuration);
            _util = _util ?? new Util(configuration);
            _ordenBancariaRE = _ordenBancariaRE ?? new OrdenBancariaRE(configuration);
            _servicioNE = _servicioNE ?? new ServicioNE(configuration);
        }

        public async Task<Object> ListarOrdenesBancariasPorAprobarAsync(CancellationToken cancelToken, String idUsuario, Int32 pagina, Int32 filas, String idSociedad, String idBanco, String idTipoOrden, String idEstadoOrden)
        {
            Object objeto = null;
            Boolean esCorrecto = false;
            try
            {
                if (idUsuario == String.Empty)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_PARAMETROS_NO_PRESENTES
                    };
                }
                else
                {
                    idSociedad = idSociedad ?? String.Empty;
                    idBanco = idBanco ?? String.Empty;
                    idTipoOrden = idTipoOrden ?? String.Empty;
                    idEstadoOrden = idEstadoOrden ?? String.Empty;
                    ObjetoOrdenBancariaMO objetoOrdenBancariaMO = await _ordenBancariaRE.ListarOrdenesBancariasPorAprobarAsync(cancelToken, idUsuario, pagina, filas, idSociedad, idBanco, idTipoOrden, idEstadoOrden);
                    objeto = new
                    {
                        codigo = objetoOrdenBancariaMO.Codigo,
                        mensaje = objetoOrdenBancariaMO.Mensaje,
                        listaOrdenesBancarias = objetoOrdenBancariaMO.ListaOrdenesBancarias,
                        totalRegistros = objetoOrdenBancariaMO.TotalRegistros
                    };
                    esCorrecto = objetoOrdenBancariaMO.Codigo != Constante.CODIGO_NO_OK ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_POR_APROBAR_ASYNC_OK : Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_POR_APROBAR_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_APROBACION_NE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_POR_APROBAR_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_APROBACION_NE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_POR_APROBAR_ASYNC, Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_POR_APROBAR_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> ListarOrdenesBancariasDetallePorAprobarAsync(CancellationToken cancelToken, String idSociedad, String idSap, String anio, String momentoOrden, String idTipoOrden)
        {
            Object objeto = null;
            Boolean esCorrecto = false;
            try
            {
                if (idSociedad == String.Empty || idSap == String.Empty || anio == String.Empty || momentoOrden == String.Empty || idTipoOrden == String.Empty)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_PARAMETROS_NO_PRESENTES
                    };
                }
                else
                {
                    ObjetoOrdenBancariaDetalleMO objetoOrdenBancariaDetalleMO = await _ordenBancariaRE.ListarOrdenesBancariasDetallePorAprobarAsync(cancelToken, idSociedad, idSap, anio, momentoOrden, idTipoOrden);
                    objeto = new
                    {
                        codigo = objetoOrdenBancariaDetalleMO.Codigo,
                        mensaje = objetoOrdenBancariaDetalleMO.Mensaje,
                        listaOrdenesBancariasDetalle = objetoOrdenBancariaDetalleMO.ListaOrdenesBancariasDetalle
                    };
                    esCorrecto = objetoOrdenBancariaDetalleMO.Codigo != Constante.CODIGO_NO_OK ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_POR_APROBAR_ASYNC_OK : Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_POR_APROBAR_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_APROBACION_NE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DETALLE_POR_APROBAR_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_APROBACION_NE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DETALLE_POR_APROBAR_ASYNC, Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_POR_APROBAR_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> BuscarOrdenesBancariasPorAprobarAsync(CancellationToken cancelToken, String idUsuario, String idSap, String fechaInicio, String fechaFin)
        {
            Object objeto = null;
            Boolean esCorrecto = false;
            try
            {
                if (idUsuario == String.Empty)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_PARAMETROS_NO_PRESENTES
                    };
                }
                else
                {
                    idSap = idSap ?? String.Empty;
                    fechaInicio = fechaInicio ?? String.Empty;
                    fechaFin = fechaFin ?? String.Empty;
                    ObjetoOrdenBancariaMO objetoOrdenBancariaMO = await _ordenBancariaRE.BuscarOrdenesBancariasPorAprobarAsync(cancelToken, idUsuario, idSap, fechaInicio, fechaFin);
                    objeto = new
                    {
                        codigo = objetoOrdenBancariaMO.Codigo,
                        mensaje = objetoOrdenBancariaMO.Mensaje,
                        listaOrdenesBancarias = objetoOrdenBancariaMO.ListaOrdenesBancarias
                    };
                    esCorrecto = objetoOrdenBancariaMO.Codigo != Constante.CODIGO_NO_OK ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_POR_APROBAR_ASYNC_OK : Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_POR_APROBAR_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_APROBACION_NE, Constante.METODO_BUSCAR_ORDENES_BANCARIAS_POR_APROBAR_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_APROBACION_NE, Constante.METODO_BUSCAR_ORDENES_BANCARIAS_POR_APROBAR_ASYNC, Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_POR_APROBAR_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> ObtenerVistaParcialOrdenBancariaPorAprobarAsync(CancellationToken cancelToken, Controller controller, String idTipoOrden)
        {
            Object objeto = null;
            Boolean esCorrecto = false;
            try
            {
                if (idTipoOrden == null || idTipoOrden == String.Empty)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_PARAMETROS_NO_PRESENTES
                    };
                }
                else
                {
                    String nombreVistaParcialOrdenBancariaPorAprobar = String.Empty;
                    switch (idTipoOrden)
                    {
                        case Constante.ID_TIPO_ORDEN_TRANSFERENCIA:
                            nombreVistaParcialOrdenBancariaPorAprobar = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_TRANSFERENCIA_POR_APROBAR;
                            break;
                        case Constante.ID_TIPO_ORDEN_TRANSFERENCIA_BCR:
                            nombreVistaParcialOrdenBancariaPorAprobar = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_TRANSFERENCIA_BCR_POR_APROBAR;
                            break;
                        case Constante.ID_TIPO_ORDEN_PROVEEDOR:
                        case Constante.ID_TIPO_ORDEN_CAMARA_COMERCIO:
                            nombreVistaParcialOrdenBancariaPorAprobar = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_PROVEEDOR_POR_APROBAR;
                            break;
                    }

                    String vistaParcialOrdenBancariaPorAprobar = await RenderViewOrPartialView.RenderViewAsync(controller, nombreVistaParcialOrdenBancariaPorAprobar, String.Empty, Constante.ES_VISTA_PARCIAL);
                    String vistaParcialAccionesOrdenBancaria = await RenderViewOrPartialView.RenderViewAsync(controller, Constante.VISTA_PARCIAL_ACCIONES_ORDEN_BANCARIA, String.Empty, Constante.ES_VISTA_PARCIAL);
                    objeto = new
                    {
                        codigo = vistaParcialOrdenBancariaPorAprobar != null ? Constante.CODIGO_OK : Constante.CODIGO_NO_OK,
                        mensaje = vistaParcialOrdenBancariaPorAprobar != null ? Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_POR_APROBAR_ASYNC_OK : Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_POR_APROBAR_ASYNC_NO_OK,
                        vistaParcialOrdenBancariaPorAprobar = vistaParcialOrdenBancariaPorAprobar,
                        vistaParcialAccionesOrdenBancaria = vistaParcialAccionesOrdenBancaria
                    };
                    esCorrecto = vistaParcialOrdenBancariaPorAprobar != null ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_POR_APROBAR_ASYNC_OK : Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_POR_APROBAR_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_APROBACION_NE, Constante.METODO_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_POR_APROBAR_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_APROBACION_NE, Constante.METODO_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_POR_APROBAR_ASYNC, Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_POR_APROBAR_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> ObtenerVistaParcialOrdenBancariaDetallePorAprobarAsync(CancellationToken cancelToken, Controller controller, String idTipoOrden)
        {
            Object objeto = null;
            Boolean esCorrecto = false;
            try
            {
                if (idTipoOrden == null || idTipoOrden == String.Empty)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_PARAMETROS_NO_PRESENTES
                    };
                }
                else
                {
                    String nombreVistaParcialOrdenBancariaDetallePorAprobar = String.Empty;

                    switch (idTipoOrden)
                    {
                        case Constante.ID_TIPO_ORDEN_TRANSFERENCIA:
                            nombreVistaParcialOrdenBancariaDetallePorAprobar = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_TRANSFERENCIA_DETALLE_POR_APROBAR;
                            break;
                        case Constante.ID_TIPO_ORDEN_TRANSFERENCIA_BCR:
                            nombreVistaParcialOrdenBancariaDetallePorAprobar = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_TRANSFERENCIA_BCR_DETALLE_POR_APROBAR;;
                            break;
                        case Constante.ID_TIPO_ORDEN_PROVEEDOR:
                        case Constante.ID_TIPO_ORDEN_CAMARA_COMERCIO:
                            nombreVistaParcialOrdenBancariaDetallePorAprobar = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_PROVEEDOR_DETALLE_POR_APROBAR;;
                            break;
                    }

                    String vistaParcialOrdenBancariaDetallePorAprobar = await RenderViewOrPartialView.RenderViewAsync(controller, nombreVistaParcialOrdenBancariaDetallePorAprobar, String.Empty, Constante.ES_VISTA_PARCIAL);
                    
                    objeto = new
                    {
                        codigo = vistaParcialOrdenBancariaDetallePorAprobar != null ? Constante.CODIGO_OK : Constante.CODIGO_NO_OK,
                        mensaje = vistaParcialOrdenBancariaDetallePorAprobar != null ? Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_POR_APROBAR_ASYNC_OK : Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_POR_APROBAR_ASYNC_NO_OK,
                        vistaParcialOrdenBancariaDetallePorAprobar = vistaParcialOrdenBancariaDetallePorAprobar
                    };
                    esCorrecto = vistaParcialOrdenBancariaDetallePorAprobar != null ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_POR_APROBAR_ASYNC_OK : Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_POR_APROBAR_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_APROBACION_NE, Constante.METODO_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_POR_APROBAR_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_APROBACION_NE, Constante.METODO_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_POR_APROBAR_ASYNC, Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_POR_APROBAR_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> ListarOrdenesBancariasAprobadasAsync(CancellationToken cancelToken, String idUsuario, Int32 pagina, Int32 filas, String idSociedad, String idBanco, String idTipoOrden)
        {
            Object objeto = null;
            Boolean esCorrecto = false;
            try
            {
                if (idUsuario == String.Empty)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_PARAMETROS_NO_PRESENTES
                    };
                }
                else
                {
                    idSociedad = idSociedad ?? String.Empty;
                    idBanco = idBanco ?? String.Empty;
                    idTipoOrden = idTipoOrden ?? String.Empty;
                    ObjetoOrdenBancariaMO objetoOrdenBancariaMO = await _ordenBancariaRE.ListarOrdenesBancariasAprobadasAsync(cancelToken, idUsuario, pagina, filas, idSociedad, idBanco, idTipoOrden);
                    objeto = new
                    {
                        codigo = objetoOrdenBancariaMO.Codigo,
                        mensaje = objetoOrdenBancariaMO.Mensaje,
                        listaOrdenesBancarias = objetoOrdenBancariaMO.ListaOrdenesBancarias,
                        totalRegistros = objetoOrdenBancariaMO.TotalRegistros
                    };
                    esCorrecto = objetoOrdenBancariaMO.Codigo != Constante.CODIGO_NO_OK ? true : false;
                }
                
                String mensaje = esCorrecto ? Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_APROBADAS_ASYNC_OK : Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_APROBADAS_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_APROBACION_NE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_APROBADAS_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_APROBACION_NE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_APROBADAS_ASYNC, Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_APROBADAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> ListarOrdenesBancariasDetalleAprobadasAsync(CancellationToken cancelToken, String idSociedad, String idSap, String anio, String momentoOrden, String idTipoOrden)
        {
            Object objeto = null;
            Boolean esCorrecto = false;
            try
            {
                if (idSociedad == String.Empty || idSap == String.Empty || anio == String.Empty || momentoOrden == String.Empty || idTipoOrden == String.Empty)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_PARAMETROS_NO_PRESENTES
                    };
                }
                else
                {
                    ObjetoOrdenBancariaDetalleMO objetoOrdenBancariaDetalleMO = await _ordenBancariaRE.ListarOrdenesBancariasDetalleAprobadasAsync(cancelToken, idSociedad, idSap, anio, momentoOrden, idTipoOrden);
                    objeto = new
                    {
                        codigo = objetoOrdenBancariaDetalleMO.Codigo,
                        mensaje = objetoOrdenBancariaDetalleMO.Mensaje,
                        listaOrdenesBancariasDetalle = objetoOrdenBancariaDetalleMO.ListaOrdenesBancariasDetalle
                    };
                    esCorrecto = objetoOrdenBancariaDetalleMO.Codigo != Constante.CODIGO_NO_OK ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_APROBADAS_ASYNC_OK : Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_APROBADAS_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_APROBACION_NE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DETALLE_APROBADAS_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_APROBACION_NE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DETALLE_APROBADAS_ASYNC, Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_APROBADAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> BuscarOrdenesBancariasAprobadasAsync(CancellationToken cancelToken, String idUsuario, String idSap, String fechaInicio, String fechaFin)
        {
            Object objeto = null;
            Boolean esCorrecto = false;
            try
            {
                if (idUsuario == String.Empty)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_PARAMETROS_NO_PRESENTES
                    };
                }
                else
                {
                    idSap = idSap ?? String.Empty;
                    fechaInicio = fechaInicio ?? String.Empty;
                    fechaFin = fechaFin ?? String.Empty;
                    ObjetoOrdenBancariaMO objetoOrdenBancariaMO = await _ordenBancariaRE.BuscarOrdenesBancariasAprobadasAsync(cancelToken, idUsuario, idSap, fechaInicio, fechaFin);
                    objeto = new
                    {
                        codigo = objetoOrdenBancariaMO.Codigo,
                        mensaje = objetoOrdenBancariaMO.Mensaje,
                        listaOrdenesBancarias = objetoOrdenBancariaMO.ListaOrdenesBancarias
                    };
                    esCorrecto = objetoOrdenBancariaMO.Codigo != Constante.CODIGO_NO_OK ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_APROBADAS_ASYNC_OK : Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_APROBADAS_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_APROBACION_NE, Constante.METODO_BUSCAR_ORDENES_BANCARIAS_APROBADAS_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_APROBACION_NE, Constante.METODO_BUSCAR_ORDENES_BANCARIAS_APROBADAS_ASYNC, Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_APROBADAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> ObtenerVistaParcialOrdenBancariaAprobadaAsync(CancellationToken cancelToken, Controller controller, String idTipoOrden)
        {
            Object objeto = null;
            Boolean esCorrecto = false;
            try
            {
                if (idTipoOrden == null || idTipoOrden == String.Empty)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_PARAMETROS_NO_PRESENTES
                    };
                }
                else
                {
                    String nombreVistaParcialOrdenBancariaAprobada = String.Empty;
                    switch (idTipoOrden)
                    {
                        case Constante.ID_TIPO_ORDEN_TRANSFERENCIA:
                            nombreVistaParcialOrdenBancariaAprobada = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_TRANSFERENCIA_APROBADA;
                            break;
                        case Constante.ID_TIPO_ORDEN_TRANSFERENCIA_BCR:
                            nombreVistaParcialOrdenBancariaAprobada = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_TRANSFERENCIA_BCR_APROBADA;
                            break;
                        case Constante.ID_TIPO_ORDEN_PROVEEDOR:
                        case Constante.ID_TIPO_ORDEN_CAMARA_COMERCIO:
                            nombreVistaParcialOrdenBancariaAprobada = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_PROVEEDOR_APROBADA;
                            break;
                    }

                    String vistaParcialOrdenBancariaAprobada = await RenderViewOrPartialView.RenderViewAsync(controller, nombreVistaParcialOrdenBancariaAprobada, String.Empty, Constante.ES_VISTA_PARCIAL);
                    String vistaParcialAccionesOrdenBancaria = await RenderViewOrPartialView.RenderViewAsync(controller, Constante.VISTA_PARCIAL_ACCIONES_ORDEN_BANCARIA, String.Empty, Constante.ES_VISTA_PARCIAL);
                    objeto = new
                    {
                        codigo = vistaParcialOrdenBancariaAprobada != null ? Constante.CODIGO_OK : Constante.CODIGO_NO_OK,
                        mensaje = vistaParcialOrdenBancariaAprobada != null ? Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_APROBADA_ASYNC_OK : Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_APROBADA_ASYNC_NO_OK,
                        vistaParcialOrdenBancariaAprobada = vistaParcialOrdenBancariaAprobada,
                        vistaParcialAccionesOrdenBancaria = vistaParcialAccionesOrdenBancaria
                    };
                    esCorrecto = vistaParcialOrdenBancariaAprobada != null ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_APROBADA_ASYNC_OK : Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_APROBADA_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_APROBACION_NE, Constante.METODO_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_APROBADA_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_APROBACION_NE, Constante.METODO_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_APROBADA_ASYNC, Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_APROBADA_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> ObtenerVistaParcialOrdenBancariaDetalleAprobadaAsync(CancellationToken cancelToken, Controller controller, String idTipoOrden)
        {
            Object objeto = null;
            Boolean esCorrecto = false;
            try
            {
                if (idTipoOrden == null || idTipoOrden == String.Empty)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_PARAMETROS_NO_PRESENTES
                    };
                }
                else
                {
                    String nombreVistaParcialOrdenBancariaDetalleAprobada = String.Empty;

                    switch (idTipoOrden)
                    {
                        case Constante.ID_TIPO_ORDEN_TRANSFERENCIA:
                            nombreVistaParcialOrdenBancariaDetalleAprobada = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_TRANSFERENCIA_DETALLE_APROBADA;
                            break;
                        case Constante.ID_TIPO_ORDEN_TRANSFERENCIA_BCR:
                            nombreVistaParcialOrdenBancariaDetalleAprobada = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_TRANSFERENCIA_BCR_DETALLE_APROBADA;
                            break;
                        case Constante.ID_TIPO_ORDEN_PROVEEDOR:
                        case Constante.ID_TIPO_ORDEN_CAMARA_COMERCIO:
                            nombreVistaParcialOrdenBancariaDetalleAprobada = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_PROVEEDOR_DETALLE_APROBADA;
                            break;
                    }

                    String vistaParcialOrdenBancariaDetalleAprobada = await RenderViewOrPartialView.RenderViewAsync(controller, nombreVistaParcialOrdenBancariaDetalleAprobada, String.Empty, Constante.ES_VISTA_PARCIAL);
                    
                    objeto = new
                    {
                        codigo = vistaParcialOrdenBancariaDetalleAprobada != null ? Constante.CODIGO_OK : Constante.CODIGO_NO_OK,
                        mensaje = vistaParcialOrdenBancariaDetalleAprobada != null ? Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_APROBADA_ASYNC_OK : Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_APROBADA_ASYNC_NO_OK,
                        vistaParcialOrdenBancariaDetalleAprobada = vistaParcialOrdenBancariaDetalleAprobada
                    };
                    esCorrecto = vistaParcialOrdenBancariaDetalleAprobada != null ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_APROBADA_ASYNC_OK : Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_APROBADA_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_APROBACION_NE, Constante.METODO_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_APROBADA_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_APROBACION_NE, Constante.METODO_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_APROBADA_ASYNC, Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_APROBADA_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> ConsultarEstadoOrdenBancariaAsync(CancellationToken cancelToken, String idSociedad, String idSap, String anio, String momentoOrden, String idTipoOrden)
        {
            Object objeto = null;
            Boolean esCorrecto = false;
            try
            {
                if (idSociedad == String.Empty || idSap == String.Empty || anio == String.Empty || momentoOrden == String.Empty || idTipoOrden == String.Empty)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_PARAMETROS_NO_PRESENTES
                    };
                }
                else
                {
                    ObjetoOrdenBancariaMO objetoOrdenBancariaMO = await _ordenBancariaRE.ConsultarEstadoOrdenBancariaAsync(cancelToken, idSociedad, idSap, anio, momentoOrden, idTipoOrden);
                    objeto = new
                    {
                        codigo = objetoOrdenBancariaMO.Codigo,
                        mensaje = objetoOrdenBancariaMO.Mensaje,
                        idEstadoOrden = objetoOrdenBancariaMO.IdEstadoOrden,
                        esHorarioBanco = objetoOrdenBancariaMO.EsHorarioBanco
                    };
                    esCorrecto = objetoOrdenBancariaMO.Codigo != Constante.CODIGO_NO_OK ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_CONSULTAR_ESTADO_ORDEN_BANCARIA_ASYNC_OK : Constante.MENSAJE_CONSULTAR_ESTADO_ORDEN_BANCARIA_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_APROBACION_NE, Constante.METODO_CONSULTAR_ESTADO_ORDEN_BANCARIA_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_APROBACION_NE, Constante.METODO_CONSULTAR_ESTADO_ORDEN_BANCARIA_ASYNC, Constante.MENSAJE_CONSULTAR_ESTADO_ORDEN_BANCARIA_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> AprobarOrdenBancariaLiberadaAsync(CancellationToken cancelToken, String[] parametros, String idUsuario)
        {
            Object objeto = null;
            Boolean esCorrecto = false;
            try
            {
                String idSociedad = parametros[0] == null ? String.Empty : parametros[0].ToString().Trim();
                String idSap = parametros[1] == null ? String.Empty : parametros[1].ToString().Trim();
                String anio = parametros[2] == null ? String.Empty : parametros[2].ToString().Trim();
                String momentoOrden = parametros[3] == null ? String.Empty : parametros[3].ToString().Trim();
                String idTipoOrden = parametros[4] == null ? String.Empty : parametros[4].ToString().Trim();
                String comentarios = parametros[5] == null ? String.Empty : parametros[5].ToString().Trim();
                String nombreArchivo = parametros[6] == null ? String.Empty : parametros[6].ToString().Trim();
                String rutaArchivo = parametros[7] == null ? String.Empty : parametros[7].ToString().Trim();
                String idEstadoOrden = parametros[8] == null ? String.Empty : parametros[8].ToString().Trim();

                if (idSociedad == String.Empty || idSap == String.Empty || anio == String.Empty || momentoOrden == String.Empty || idEstadoOrden == String.Empty || idUsuario == String.Empty)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_PARAMETROS_NO_PRESENTES
                    };
                }
                else
                {
                    ObjetoOrdenBancariaMO objetoOrdenBancariaMO = await _ordenBancariaRE.ActualizarEstadoOrdenBancariaAsync(cancelToken, idSociedad, idSap, anio, momentoOrden, idTipoOrden, idUsuario, idEstadoOrden, comentarios);
                    
                    if (objetoOrdenBancariaMO.Codigo == Constante.CODIGO_OK && objetoOrdenBancariaMO.IdEstadoOrden == Constante.ID_ESTADO_ORDEN_APROBADO)
                    {
                        Boolean esEncriptadoEnviado = await _util.EncriptarEnviarArchivoAsync(cancelToken, nombreArchivo, rutaArchivo);

                        if (!esEncriptadoEnviado)
                        {
                            objetoOrdenBancariaMO.Codigo = Constante.CODIGO_NO_OK;
                            await _ordenBancariaRE.ActualizarEstadoOrdenBancariaAsync(new CancellationToken(false), idSociedad, idSap, anio, momentoOrden, idTipoOrden, idUsuario, Constante.ID_ESTADO_ORDEN_PRE_APROBADO, Constante.MENSAJE_REVERSION_ANULACION);
                        }
                        else
                        {
                            objetoOrdenBancariaMO = await _ordenBancariaRE.ActualizarEstadoOrdenBancariaAsync(new CancellationToken(false), idSociedad, idSap, anio, momentoOrden, idTipoOrden, idUsuario, Constante.ID_ESTADO_ORDEN_ENTREGADO, Constante.MENSAJE_ENVIAR_ARCHIVO_HACIA_BANCO_ASYNC_OK);
                        }

                        objeto = new
                        {
                            codigo = esEncriptadoEnviado ? Constante.CODIGO_OK : Constante.CODIGO_NO_OK,
                            mensaje = esEncriptadoEnviado ? String.Format("{0}<br>{1}<br>{2}", objetoOrdenBancariaMO.Mensaje, Constante.MENSAJE_ENCRIPTAR_ARCHIVO_ASYNC_OK, Constante.MENSAJE_ENVIAR_ARCHIVO_HACIA_BANCO_ASYNC_OK) : String.Format("{0}<br>{1}", Constante.MENSAJE_ENCRIPTAR_ARCHIVO_ASYNC_NO_OK, Constante.MENSAJE_ENVIAR_ARCHIVO_HACIA_BANCO_ASYNC_NO_OK)
                        };
                    }
                    else
                    {
                        objeto = new
                        {
                            codigo = objetoOrdenBancariaMO.Codigo,
                            mensaje = objetoOrdenBancariaMO.Mensaje
                        };
                    }

                    esCorrecto = objetoOrdenBancariaMO.Codigo == Constante.CODIGO_OK ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_APROBAR_ORDEN_BANCARIA_LIBERADA_ASYNC_OK : Constante.MENSAJE_APROBAR_ORDEN_BANCARIA_LIBERADA_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_APROBACION_NE, Constante.METODO_APROBAR_ORDEN_BANCARIA_LIBERADA_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_APROBACION_NE, Constante.METODO_APROBAR_ORDEN_BANCARIA_LIBERADA_ASYNC, Constante.MENSAJE_APROBAR_ORDEN_BANCARIA_LIBERADA_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> AnularOrdenBancariaLiberadaAsync(CancellationToken cancelToken, String[] parametros, String idUsuario, String usuario)
        {
            Object objeto = null;
            Boolean esCorrecto = false;
            try
            {
                String idSociedad = parametros[0] == null ? String.Empty : parametros[0].ToString().Trim();
                String idSap = parametros[1] == null ? String.Empty : parametros[1].ToString().Trim();
                String anio = parametros[2] == null ? String.Empty : parametros[2].ToString().Trim();
                String momentoOrden = parametros[3] == null ? String.Empty : parametros[3].ToString().Trim();
                String idTipoOrden = parametros[4] == null ? String.Empty : parametros[4].ToString().Trim();
                String comentarios = parametros[5] == null ? String.Empty : parametros[5].ToString().Trim();
                String idEstadoOrden = parametros[6] == null ? String.Empty : parametros[6].ToString().Trim();
                String tipoOrden = parametros[7] == null ? String.Empty : parametros[7].ToString().Trim();

                if (idSociedad == String.Empty || idSap == String.Empty || anio == String.Empty || momentoOrden == String.Empty || idUsuario == String.Empty)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_PARAMETROS_NO_PRESENTES
                    };
                }
                else
                {
                    Boolean esConforme = false;
                    String idEstadoOrdenAnulado = Constante.ID_ESTADO_ORDEN_ANULADO;
                    ObjetoOrdenBancariaMO objetoOrdenBancariaMO = await _ordenBancariaRE.ActualizarEstadoOrdenBancariaAsync(cancelToken, idSociedad, idSap, anio, momentoOrden, idTipoOrden, idUsuario, idEstadoOrdenAnulado, comentarios);
                    
                    if (objetoOrdenBancariaMO.Codigo == Constante.CODIGO_OK)
                    {
                        RespuestaDetalleMO respuestaDetalleMO = await _servicioNE.EnviarEstadoProcesoHaciaSapAsync(cancelToken, idSociedad, anio, momentoOrden, idEstadoOrdenAnulado, idSap, usuario, tipoOrden);

                        if (respuestaDetalleMO != null)
                        {
                            objeto = new
                            {
                                codigo = objetoOrdenBancariaMO.Codigo,
                                mensaje = String.Format("{0}<br>{1}", objetoOrdenBancariaMO.Mensaje, respuestaDetalleMO.Respuesta)
                            };
                            esConforme = true;
                        }
                    }

                    if (!esConforme)
                    {
                        objetoOrdenBancariaMO.Codigo = Constante.CODIGO_NO_OK;
                        await _ordenBancariaRE.ActualizarEstadoOrdenBancariaAsync(cancelToken, idSociedad, idSap, anio, momentoOrden, idTipoOrden, idUsuario, idEstadoOrden, Constante.MENSAJE_REVERSION_ANULACION);
                        objeto = new
                        {
                            codigo = Constante.CODIGO_NO_OK,
                            mensaje = Constante.MENSAJE_ACTUALIZAR_ESTADO_ORDEN_BANCARIA_ASYNC_NO_OK
                        };
                    }

                    esCorrecto = objetoOrdenBancariaMO.Codigo == Constante.CODIGO_OK ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_ANULAR_ORDEN_BANCARIA_LIBERADA_ASYNC_OK : Constante.MENSAJE_ANULAR_ORDEN_BANCARIA_LIBERADA_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_APROBACION_NE, Constante.METODO_ANULAR_ORDEN_BANCARIA_LIBERADA_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_APROBACION_NE, Constante.METODO_ANULAR_ORDEN_BANCARIA_LIBERADA_ASYNC, Constante.MENSAJE_ANULAR_ORDEN_BANCARIA_LIBERADA_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }
        
        public async Task<Object> ConsultarEstadoMasivoOrdenesBancariasAsync(CancellationToken cancelToken, String cadena)
        {
            Object objeto = null;
            Boolean esCorrecto = false;
            try
            {
                if (cadena == String.Empty)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_PARAMETROS_NO_PRESENTES
                    };
                }
                else
                {
                    ObjetoOrdenBancariaMO objetoOrdenBancariaMO = await _ordenBancariaRE.ConsultarEstadoMasivoOrdenesBancariasAsync(cancelToken, cadena);
                    objeto = new
                    {
                        codigo = objetoOrdenBancariaMO.Codigo,
                        mensaje = objetoOrdenBancariaMO.Mensaje,
                        esEstadoLiberado = objetoOrdenBancariaMO.EsEstadoLiberado,
                        esEstadoPreAprobado = objetoOrdenBancariaMO.EsEstadoPreAprobado,
                        esHorarioBanco = objetoOrdenBancariaMO.EsHorarioBanco
                    };
                    esCorrecto = objetoOrdenBancariaMO.Codigo != Constante.CODIGO_NO_OK ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_CONSULTAR_ESTADO_MASIVO_ORDENES_BANCARIAS_ASYNC_OK : Constante.MENSAJE_CONSULTAR_ESTADO_MASIVO_ORDENES_BANCARIAS_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_APROBACION_NE, Constante.METODO_CONSULTAR_ESTADO_MASIVO_ORDENES_BANCARIAS_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_APROBACION_NE, Constante.METODO_CONSULTAR_ESTADO_MASIVO_ORDENES_BANCARIAS_ASYNC, Constante.MENSAJE_CONSULTAR_ESTADO_MASIVO_ORDENES_BANCARIAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> AprobarMasivoOrdenesBancariasAsync(CancellationToken cancelToken, String[] parametros, String idUsuario)
        {
            Object objeto = null;
            Boolean esCorrecto = false;
            Int32 contador1 = 0, contador2 = 0;
            try
            {
                String cadena = parametros[0] == null ? String.Empty : parametros[0].ToString().Trim();
                String idEstadoOrden = parametros[1] == null ? String.Empty : parametros[1].ToString().Trim();

                if (cadena == String.Empty || idEstadoOrden == String.Empty || idUsuario == String.Empty)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_PARAMETROS_NO_PRESENTES
                    };
                }
                else
                {
                    String[] arregloOrdenesBancarias = cadena.Split(Constante.DELIMITADOR_AMPERSON);
                    String mensajeError = String.Empty, mensajeAprobacion = String.Empty;

                    foreach (var item in arregloOrdenesBancarias)
                    {
                        String idSociedad = item.Split(Constante.DELIMITADOR_BARRA)[Constante._0].ToString().Trim();
                        String idSap = item.Split(Constante.DELIMITADOR_BARRA)[Constante._1].ToString().Trim();
                        String anio = item.Split(Constante.DELIMITADOR_BARRA)[Constante._2].ToString().Trim();
                        String momentoOrden = item.Split(Constante.DELIMITADOR_BARRA)[Constante._3].ToString().Trim();
                        String idTipoOrden = item.Split(Constante.DELIMITADOR_BARRA)[Constante._4].ToString().Trim();
                        String nombreArchivo = item.Split(Constante.DELIMITADOR_BARRA)[Constante._5].ToString().Trim();
                        String rutaArchivo = item.Split(Constante.DELIMITADOR_BARRA)[Constante._6].ToString().Trim();
                        ObjetoOrdenBancariaMO objetoOrdenBancariaMO = await _ordenBancariaRE.ActualizarEstadoMasivoOrdenesBancariasAsync(cancelToken, idSociedad, idSap, anio, momentoOrden, idTipoOrden, idUsuario, idEstadoOrden, String.Empty);
                        
                        if (objetoOrdenBancariaMO.Codigo == Constante.CODIGO_OK && objetoOrdenBancariaMO.IdEstadoOrden == Constante.ID_ESTADO_ORDEN_APROBADO)
                        {
                            Boolean esEncriptadoEnviado = await _util.EncriptarEnviarArchivoAsync(cancelToken, nombreArchivo, rutaArchivo);
                            
                            if (!esEncriptadoEnviado)
                            {
                                await _ordenBancariaRE.ActualizarEstadoMasivoOrdenesBancariasAsync(new CancellationToken(false), idSociedad, idSap, anio, momentoOrden, idTipoOrden, idUsuario, Constante.ID_ESTADO_ORDEN_PRE_APROBADO, Constante.MENSAJE_REVERSION_ANULACION);
                                mensajeError += String.Format("{0} {1} | {2}<br>", Constante.MENSAJE_ENCRIPTAR_ARCHIVO_ASYNC_NO_OK, Constante.MENSAJE_ENVIAR_ARCHIVO_HACIA_BANCO_ASYNC_NO_OK, nombreArchivo);
                            }
                            else
                            {
                                await _ordenBancariaRE.ActualizarEstadoMasivoOrdenesBancariasAsync(new CancellationToken(false), idSociedad, idSap, anio, momentoOrden, idTipoOrden, idUsuario, Constante.ID_ESTADO_ORDEN_ENTREGADO, Constante.MENSAJE_ENVIAR_ARCHIVO_HACIA_BANCO_ASYNC_OK);
                                contador2++;
                            }
                        }

                        contador1++;
                    }
                    
                    if (idEstadoOrden == Constante.ID_ESTADO_ORDEN_LIBERADO)
                    {
                        esCorrecto = contador1 == arregloOrdenesBancarias.Length ? true : false;
                        mensajeAprobacion = Constante.MENSAJE_ACTUALIZAR_ESTADO_MASIVO_ORDENES_BANCARIAS_ASYNC_OK;
                    }
                    else if (idEstadoOrden == Constante.ID_ESTADO_ORDEN_PRE_APROBADO)
                    {
                        esCorrecto = contador2 == arregloOrdenesBancarias.Length ? true : false;
                        mensajeAprobacion = String.Format("{0}<br>{1}", Constante.MENSAJE_ENCRIPTAR_ARCHIVOS_MASIVO_ASYNC_OK, Constante.MENSAJE_ENVIAR_ARCHIVOS_HACIA_BANCO_MASIVO_ASYNC_OK);
                    }
                    
                    objeto = new
                    {
                        codigo = esCorrecto ? Constante.CODIGO_OK : Constante.CODIGO_NO_OK,
                        mensaje = esCorrecto ? mensajeAprobacion : mensajeError
                    };
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_APROBAR_MASIVO_ORDENES_BANCARIAS_ASYNC_OK : Constante.MENSAJE_APROBAR_MASIVO_ORDENES_BANCARIAS_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_APROBACION_NE, Constante.METODO_APROBAR_MASIVO_ORDENES_BANCARIAS_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_APROBACION_NE, Constante.METODO_APROBAR_MASIVO_ORDENES_BANCARIAS_ASYNC, Constante.MENSAJE_APROBAR_MASIVO_ORDENES_BANCARIAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> ListarFlujoAprobacionAsync(CancellationToken cancelToken, String idSociedad, String idSap, String anio, String momentoOrden, String idTipoOrden)
        {
            Object objeto = null;
            Boolean esCorrecto = false;
            try
            {
                if (idSociedad == String.Empty || idSap == String.Empty || anio == String.Empty || momentoOrden == String.Empty)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_PARAMETROS_NO_PRESENTES
                    };
                }
                else
                {
                    ObjetoFlujoAprobacionMO objetoFlujoAprobacionMO = await _ordenBancariaRE.ListarFlujoAprobacionAsync(cancelToken, idSociedad, idSap, anio, momentoOrden, idTipoOrden);
                    objeto = new
                    {
                        codigo = objetoFlujoAprobacionMO.Codigo,
                        mensaje = objetoFlujoAprobacionMO.Mensaje,
                        listaFlujoAprobacion = objetoFlujoAprobacionMO.ListaFlujoAprobacion
                    };
                    esCorrecto = objetoFlujoAprobacionMO.Codigo != Constante.CODIGO_NO_OK ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_LISTAR_FLUJO_APROBACION_ASYNC_OK : Constante.MENSAJE_LISTAR_FLUJO_APROBACION_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_APROBACION_NE, Constante.METODO_LISTAR_FLUJO_APROBACION_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_APROBACION_NE, Constante.METODO_LISTAR_FLUJO_APROBACION_ASYNC, Constante.MENSAJE_LISTAR_FLUJO_APROBACION_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> ListarOrdenesBancariasAnuladasAsync(CancellationToken cancelToken, String idUsuario, Int32 pagina, Int32 filas, String idSociedad, String idBanco, String idTipoOrden)
        {
            Object objeto = null;
            Boolean esCorrecto = false;
            try
            {
                if (idUsuario == String.Empty)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_PARAMETROS_NO_PRESENTES
                    };
                }
                else
                {
                    idSociedad = idSociedad ?? String.Empty;
                    idBanco = idBanco ?? String.Empty;
                    idTipoOrden = idTipoOrden ?? String.Empty;
                    ObjetoOrdenBancariaMO objetoOrdenBancariaMO = await _ordenBancariaRE.ListarOrdenesBancariasAnuladasAsync(cancelToken, idUsuario, pagina, filas, idSociedad, idBanco, idTipoOrden);
                    objeto = new
                    {
                        codigo = objetoOrdenBancariaMO.Codigo,
                        mensaje = objetoOrdenBancariaMO.Mensaje,
                        listaOrdenesBancarias = objetoOrdenBancariaMO.ListaOrdenesBancarias,
                        totalRegistros = objetoOrdenBancariaMO.TotalRegistros
                    };
                    esCorrecto = objetoOrdenBancariaMO.Codigo != Constante.CODIGO_NO_OK ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_ANULADAS_ASYNC_OK : Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_ANULADAS_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_ANULADAS_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_ANULADAS_ASYNC, Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_ANULADAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> ListarOrdenesBancariasDetalleAnuladasAsync(CancellationToken cancelToken, String idSociedad, String idSap, String anio, String momentoOrden, String idTipoOrden)
        {
            Object objeto = null;
            Boolean esCorrecto = false;
            try
            {
                if (idSociedad == String.Empty || idSap == String.Empty || anio == String.Empty || momentoOrden == String.Empty || idTipoOrden == String.Empty)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_PARAMETROS_NO_PRESENTES
                    };
                }
                else
                {
                    ObjetoOrdenBancariaDetalleMO objetoOrdenBancariaDetalleMO = await _ordenBancariaRE.ListarOrdenesBancariasDetalleAnuladasAsync(cancelToken, idSociedad, idSap, anio, momentoOrden, idTipoOrden);
                    objeto = new
                    {
                        codigo = objetoOrdenBancariaDetalleMO.Codigo,
                        mensaje = objetoOrdenBancariaDetalleMO.Mensaje,
                        listaOrdenesBancariasDetalle = objetoOrdenBancariaDetalleMO.ListaOrdenesBancariasDetalle
                    };
                    esCorrecto = objetoOrdenBancariaDetalleMO.Codigo != Constante.CODIGO_NO_OK ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_ANULADAS_ASYNC_OK : Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_ANULADAS_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DETALLE_ANULADAS_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DETALLE_ANULADAS_ASYNC, Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_ANULADAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> BuscarOrdenesBancariasAnuladasAsync(CancellationToken cancelToken, String idUsuario, String idSap, String fechaInicio, String fechaFin)
        {
            Object objeto = null;
            Boolean esCorrecto = false;
            try
            {
                if (idUsuario == String.Empty)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_PARAMETROS_NO_PRESENTES
                    };
                }
                else
                {
                    idSap = idSap ?? String.Empty;
                    fechaInicio = fechaInicio ?? String.Empty;
                    fechaFin = fechaFin ?? String.Empty;
                    ObjetoOrdenBancariaMO objetoOrdenBancariaMO = await _ordenBancariaRE.BuscarOrdenesBancariasAnuladasAsync(cancelToken, idUsuario, idSap, fechaInicio, fechaFin);
                    objeto = new
                    {
                        codigo = objetoOrdenBancariaMO.Codigo,
                        mensaje = objetoOrdenBancariaMO.Mensaje,
                        listaOrdenesBancarias = objetoOrdenBancariaMO.ListaOrdenesBancarias
                    };
                    esCorrecto = objetoOrdenBancariaMO.Codigo != Constante.CODIGO_NO_OK ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_ANULADAS_ASYNC_OK : Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_ANULADAS_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_BUSCAR_ORDENES_BANCARIAS_ANULADAS_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_BUSCAR_ORDENES_BANCARIAS_ANULADAS_ASYNC, Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_ANULADAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }
        
        public async Task<Object> ObtenerVistaParcialOrdenBancariaAnuladaAsync(CancellationToken cancelToken, Controller controller, String idTipoOrden)
        {
            Object objeto = null;
            Boolean esCorrecto = false;
            try
            {
                if (idTipoOrden == null || idTipoOrden == String.Empty)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_PARAMETROS_NO_PRESENTES
                    };
                }
                else
                {
                    String nombreVistaParcialOrdenBancariaAnulada = String.Empty;

                    switch (idTipoOrden)
                    {
                        case Constante.ID_TIPO_ORDEN_TRANSFERENCIA:
                            nombreVistaParcialOrdenBancariaAnulada = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_TRANSFERENCIA_ANULADA;
                            break;
                        case Constante.ID_TIPO_ORDEN_TRANSFERENCIA_BCR:
                            nombreVistaParcialOrdenBancariaAnulada = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_TRANSFERENCIA_BCR_ANULADA;
                            break;
                        case Constante.ID_TIPO_ORDEN_PROVEEDOR:
                        case Constante.ID_TIPO_ORDEN_CAMARA_COMERCIO:
                            nombreVistaParcialOrdenBancariaAnulada = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_PROVEEDOR_ANULADA;
                            break;
                    }

                    String vistaParcialOrdenBancariaAnulada = await RenderViewOrPartialView.RenderViewAsync(controller, nombreVistaParcialOrdenBancariaAnulada, String.Empty, Constante.ES_VISTA_PARCIAL);
                    String vistaParcialAccionesOrdenBancaria = await RenderViewOrPartialView.RenderViewAsync(controller, Constante.VISTA_PARCIAL_ACCIONES_ORDEN_BANCARIA, String.Empty, Constante.ES_VISTA_PARCIAL);
                    objeto = new
                    {
                        codigo = vistaParcialOrdenBancariaAnulada != null ? Constante.CODIGO_OK : Constante.CODIGO_NO_OK,
                        mensaje = vistaParcialOrdenBancariaAnulada != null ? Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_ANULADA_ASYNC_OK : Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_ANULADA_ASYNC_NO_OK,
                        vistaParcialOrdenBancariaAnulada = vistaParcialOrdenBancariaAnulada,
                        vistaParcialAccionesOrdenBancaria = vistaParcialAccionesOrdenBancaria
                    };
                    esCorrecto = vistaParcialOrdenBancariaAnulada != null ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_ANULADA_ASYNC_OK : Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_ANULADA_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_ANULADA_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_ANULADA_ASYNC, Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_ANULADA_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> ObtenerVistaParcialOrdenBancariaDetalleAnuladaAsync(CancellationToken cancelToken, Controller controller, String idTipoOrden)
        {
            Object objeto = null;
            Boolean esCorrecto = false;
            try
            {
                if (idTipoOrden == null || idTipoOrden == String.Empty)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_PARAMETROS_NO_PRESENTES
                    };
                }
                else
                {
                    String nombreVistaParcialOrdenBancariaDetalleAnulada = String.Empty;

                    switch (idTipoOrden)
                    {
                        case Constante.ID_TIPO_ORDEN_TRANSFERENCIA:
                            nombreVistaParcialOrdenBancariaDetalleAnulada = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_TRANSFERENCIA_DETALLE_ANULADA;
                            break;
                        case Constante.ID_TIPO_ORDEN_TRANSFERENCIA_BCR:
                            nombreVistaParcialOrdenBancariaDetalleAnulada = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_TRANSFERENCIA_BCR_DETALLE_ANULADA;
                            break;
                        case Constante.ID_TIPO_ORDEN_PROVEEDOR:
                        case Constante.ID_TIPO_ORDEN_CAMARA_COMERCIO:
                            nombreVistaParcialOrdenBancariaDetalleAnulada = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_PROVEEDOR_DETALLE_ANULADA;
                            break;
                    }

                    String vistaParcialOrdenBancariaDetalleAnulada = await RenderViewOrPartialView.RenderViewAsync(controller, nombreVistaParcialOrdenBancariaDetalleAnulada, String.Empty, Constante.ES_VISTA_PARCIAL);
                    
                    objeto = new
                    {
                        codigo = vistaParcialOrdenBancariaDetalleAnulada != null ? Constante.CODIGO_OK : Constante.CODIGO_NO_OK,
                        mensaje = vistaParcialOrdenBancariaDetalleAnulada != null ? Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_ANULADA_ASYNC_OK : Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_ANULADA_ASYNC_NO_OK,
                        vistaParcialOrdenBancariaDetalleAnulada = vistaParcialOrdenBancariaDetalleAnulada
                    };
                    esCorrecto = vistaParcialOrdenBancariaDetalleAnulada != null ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_ANULADA_ASYNC_OK : Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_ANULADA_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_ANULADA_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_ANULADA_ASYNC, Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_ANULADA_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }
    }
}