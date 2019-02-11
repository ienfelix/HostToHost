using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Comun;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Modelo;
using Repositorio;

namespace Negocio
{
    public class TesoreriaNE
    {
        private Bitacora _bitacora = null;
        private OrdenBancariaRE _ordenBancariaRE = null;
        private ServicioNE _servicioNE = null;

        public TesoreriaNE(IConfiguration configuration)
        {
            _bitacora = _bitacora ?? new Bitacora(configuration);
            _ordenBancariaRE = _ordenBancariaRE ?? new OrdenBancariaRE(configuration);
            _servicioNE = _servicioNE ?? new ServicioNE(configuration);
        }

        public async Task<Object> ListarOrdenesBancariasLiberadasAsync(CancellationToken cancelToken, String idUsuario, Int32 pagina, Int32 filas, String idSociedad, String idBanco, String idTipoOrden)
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
                    ObjetoOrdenBancariaMO objetoOrdenBancariaMO = await _ordenBancariaRE.ListarOrdenesBancariasLiberadasAsync(cancelToken, idUsuario, pagina, filas, idSociedad, idBanco, idTipoOrden);
                    objeto = new
                    {
                        codigo = objetoOrdenBancariaMO.Codigo,
                        mensaje = objetoOrdenBancariaMO.Mensaje,
                        listaOrdenesBancarias = objetoOrdenBancariaMO.ListaOrdenesBancarias,
                        totalRegistros = objetoOrdenBancariaMO.TotalRegistros
                    };
                    esCorrecto = objetoOrdenBancariaMO.Codigo != Constante.CODIGO_NO_OK ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_LIBERADAS_ASYNC_OK : Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_LIBERADAS_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_LIBERADAS_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_LIBERADAS_ASYNC, Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_LIBERADAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> ListarOrdenesBancariasDetalleLiberadasAsync(CancellationToken cancelToken, String idSociedad, String idSap, String anio, String momentoOrden, String idTipoOrden)
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
                    ObjetoOrdenBancariaDetalleMO objetoOrdenBancariaDetalleMO = await _ordenBancariaRE.ListarOrdenesBancariasDetalleLiberadasAsync(cancelToken, idSociedad, idSap, anio, momentoOrden, idTipoOrden);
                    objeto = new
                    {
                        codigo = objetoOrdenBancariaDetalleMO.Codigo,
                        mensaje = objetoOrdenBancariaDetalleMO.Mensaje,
                        listaOrdenesBancariasDetalle = objetoOrdenBancariaDetalleMO.ListaOrdenesBancariasDetalle
                    };
                    esCorrecto = objetoOrdenBancariaDetalleMO.Codigo != Constante.CODIGO_NO_OK ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_LIBERADAS_ASYNC_OK : Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_LIBERADAS_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DETALLE_LIBERADAS_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DETALLE_LIBERADAS_ASYNC, Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_LIBERADAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> BuscarOrdenesBancariasLiberadasAsync(CancellationToken cancelToken, String idUsuario, String idSap, String fechaInicio, String fechaFin)
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
                    ObjetoOrdenBancariaMO objetoOrdenBancariaMO = await _ordenBancariaRE.BuscarOrdenesBancariasLiberadasAsync(cancelToken, idUsuario, idSap, fechaInicio, fechaFin);
                    objeto = new
                    {
                        codigo = objetoOrdenBancariaMO.Codigo,
                        mensaje = objetoOrdenBancariaMO.Mensaje,
                        listaOrdenesBancarias = objetoOrdenBancariaMO.ListaOrdenesBancarias
                    };
                    esCorrecto = objetoOrdenBancariaMO.Codigo != Constante.CODIGO_NO_OK ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_LIBERADAS_ASYNC_OK : Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_LIBERADAS_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_BUSCAR_ORDENES_BANCARIAS_LIBERADAS_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_BUSCAR_ORDENES_BANCARIAS_LIBERADAS_ASYNC, Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_LIBERADAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }
        
        public async Task<Object> ObtenerVistaParcialOrdenBancariaLiberadaAsync(CancellationToken cancelToken, Controller controller, String idTipoOrden)
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
                    String nombreVistaParcialOrdenBancariaLiberada = String.Empty;

                    switch (idTipoOrden)
                    {
                        case Constante.ID_TIPO_ORDEN_TRANSFERENCIA:
                            nombreVistaParcialOrdenBancariaLiberada = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_TRANSFERENCIA_LIBERADA;
                            break;
                        case Constante.ID_TIPO_ORDEN_TRANSFERENCIA_BCR:
                            nombreVistaParcialOrdenBancariaLiberada = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_TRANSFERENCIA_BCR_LIBERADA;
                            break;
                        case Constante.ID_TIPO_ORDEN_PROVEEDOR:
                        case Constante.ID_TIPO_ORDEN_CAMARA_COMERCIO:
                            nombreVistaParcialOrdenBancariaLiberada = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_PROVEEDOR_LIBERADA;
                            break;
                    }

                    String vistaParcialOrdenBancariaLiberada = await RenderViewOrPartialView.RenderViewAsync(controller, nombreVistaParcialOrdenBancariaLiberada, String.Empty, Constante.ES_VISTA_PARCIAL);
                    String vistaParcialAccionesOrdenBancaria = await RenderViewOrPartialView.RenderViewAsync(controller, Constante.VISTA_PARCIAL_ACCIONES_ORDEN_BANCARIA, String.Empty, Constante.ES_VISTA_PARCIAL);
                    objeto = new
                    {
                        codigo = vistaParcialOrdenBancariaLiberada != null ? Constante.CODIGO_OK : Constante.CODIGO_NO_OK,
                        mensaje = vistaParcialOrdenBancariaLiberada != null ? Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_LIBERADA_ASYNC_OK : Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_LIBERADA_ASYNC_NO_OK,
                        vistaParcialOrdenBancariaLiberada = vistaParcialOrdenBancariaLiberada,
                        vistaParcialAccionesOrdenBancaria = vistaParcialAccionesOrdenBancaria
                    };
                    esCorrecto = vistaParcialOrdenBancariaLiberada != null ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_LIBERADA_ASYNC_OK : Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_LIBERADA_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_LIBERADA_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_LIBERADA_ASYNC, Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_LIBERADA_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> ObtenerVistaParcialOrdenBancariaDetalleLiberadaAsync(CancellationToken cancelToken, Controller controller, String idTipoOrden)
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
                    String nombreVistaParcialOrdenBancariaDetalleLiberada = String.Empty;

                    switch (idTipoOrden)
                    {
                        case Constante.ID_TIPO_ORDEN_TRANSFERENCIA:
                            nombreVistaParcialOrdenBancariaDetalleLiberada = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_TRANSFERENCIA_DETALLE_LIBERADA;
                            break;
                        case Constante.ID_TIPO_ORDEN_TRANSFERENCIA_BCR:
                            nombreVistaParcialOrdenBancariaDetalleLiberada = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_TRANSFERENCIA_BCR_DETALLE_LIBERADA;
                            break;
                        case Constante.ID_TIPO_ORDEN_PROVEEDOR:
                        case Constante.ID_TIPO_ORDEN_CAMARA_COMERCIO:
                            nombreVistaParcialOrdenBancariaDetalleLiberada = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_PROVEEDOR_DETALLE_LIBERADA;
                            break;
                    }

                    String vistaParcialOrdenBancariaDetalleLiberada = await RenderViewOrPartialView.RenderViewAsync(controller, nombreVistaParcialOrdenBancariaDetalleLiberada, String.Empty, Constante.ES_VISTA_PARCIAL);
                    
                    objeto = new
                    {
                        codigo = vistaParcialOrdenBancariaDetalleLiberada != null ? Constante.CODIGO_OK : Constante.CODIGO_NO_OK,
                        mensaje = vistaParcialOrdenBancariaDetalleLiberada != null ? Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_LIBERADA_ASYNC_OK : Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_LIBERADA_ASYNC_NO_OK,
                        vistaParcialOrdenBancariaDetalleLiberada = vistaParcialOrdenBancariaDetalleLiberada
                    };
                    esCorrecto = vistaParcialOrdenBancariaDetalleLiberada != null ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_LIBERADA_ASYNC_OK : Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_LIBERADA_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_LIBERADA_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_LIBERADA_ASYNC, Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_LIBERADA_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> DeshacerOrdenBancariaLiberadaAsync(CancellationToken cancelToken, String[] parametros, String idUsuario, String usuario)
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

                if (idSociedad == String.Empty || idSap == String.Empty || anio == String.Empty || momentoOrden == String.Empty || idTipoOrden == String.Empty || idUsuario == String.Empty)
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
                    String idEstadoOrdenDeshecho = Constante.ID_ESTADO_ORDEN_DESHECHO;
                    ObjetoOrdenBancariaMO objetoOrdenBancariaMO = await _ordenBancariaRE.ActualizarEstadoOrdenBancariaAsync(cancelToken, idSociedad, idSap, anio, momentoOrden, idTipoOrden, idUsuario, idEstadoOrdenDeshecho, comentarios);
                    
                    if (objetoOrdenBancariaMO.Codigo == Constante.CODIGO_OK)
                    {
                        String idEstadoOrdenAnulado = Constante.ID_ESTADO_ORDEN_ANULADO;
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

                String mensaje = esCorrecto == true ? Constante.MENSAJE_DESHACER_ORDEN_BANCARIA_LIBERADA_ASYNC_OK : Constante.MENSAJE_DESHACER_ORDEN_BANCARIA_LIBERADA_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_DESHACER_ORDEN_BANCARIA_LIBERADA_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_DESHACER_ORDEN_BANCARIA_LIBERADA_ASYNC, Constante.MENSAJE_DESHACER_ORDEN_BANCARIA_LIBERADA_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }
    
        public async Task<Object> ListarOrdenesBancariasDeshechasAsync(CancellationToken cancelToken, String idUsuario, Int32 pagina, Int32 filas, String idSociedad, String idBanco, String idTipoOrden)
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
                    ObjetoOrdenBancariaMO objetoOrdenBancariaMO = await _ordenBancariaRE.ListarOrdenesBancariasDeshechasAsync(cancelToken, idUsuario, pagina, filas, idSociedad, idBanco, idTipoOrden);
                    objeto = new
                    {
                        codigo = objetoOrdenBancariaMO.Codigo,
                        mensaje = objetoOrdenBancariaMO.Mensaje,
                        listaOrdenesBancarias = objetoOrdenBancariaMO.ListaOrdenesBancarias,
                        totalRegistros = objetoOrdenBancariaMO.TotalRegistros
                    };
                    esCorrecto = objetoOrdenBancariaMO.Codigo != Constante.CODIGO_NO_OK ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DESHECHAS_ASYNC_OK : Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DESHECHAS_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DESHECHAS_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DESHECHAS_ASYNC, Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DESHECHAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> ListarOrdenesBancariasDetalleDeshechasAsync(CancellationToken cancelToken, String idSociedad, String idSap, String anio, String momentoOrden, String idTipoOrden)
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
                    ObjetoOrdenBancariaDetalleMO objetoOrdenBancariaDetalleMO = await _ordenBancariaRE.ListarOrdenesBancariasDetalleDeshechasAsync(cancelToken, idSociedad, idSap, anio, momentoOrden, idTipoOrden);
                    objeto = new
                    {
                        codigo = objetoOrdenBancariaDetalleMO.Codigo,
                        mensaje = objetoOrdenBancariaDetalleMO.Mensaje,
                        listaOrdenesBancariasDetalle = objetoOrdenBancariaDetalleMO.ListaOrdenesBancariasDetalle
                    };
                    esCorrecto = objetoOrdenBancariaDetalleMO.Codigo != Constante.CODIGO_NO_OK ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_DESHECHAS_ASYNC_OK : Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_DESHECHAS_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DETALLE_DESHECHAS_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DETALLE_DESHECHAS_ASYNC, Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_DESHECHAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> BuscarOrdenesBancariasDeshechasAsync(CancellationToken cancelToken, String idUsuario, String idSap, String fechaInicio, String fechaFin)
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
                    ObjetoOrdenBancariaMO objetoOrdenBancariaMO = await _ordenBancariaRE.BuscarOrdenesBancariasDeshechasAsync(cancelToken, idUsuario, idSap, fechaInicio, fechaFin);
                    objeto = new
                    {
                        codigo = objetoOrdenBancariaMO.Codigo,
                        mensaje = objetoOrdenBancariaMO.Mensaje,
                        listaOrdenesBancarias = objetoOrdenBancariaMO.ListaOrdenesBancarias
                    };
                    esCorrecto = objetoOrdenBancariaMO.Codigo != Constante.CODIGO_NO_OK ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_DESHECHAS_ASYNC_OK : Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_DESHECHAS_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_BUSCAR_ORDENES_BANCARIAS_DESHECHAS_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_BUSCAR_ORDENES_BANCARIAS_DESHECHAS_ASYNC, Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_DESHECHAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }
        
        public async Task<Object> ObtenerVistaParcialOrdenBancariaDeshechaAsync(CancellationToken cancelToken, Controller controller, String idTipoOrden)
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
                    String nombreVistaParcialOrdenBancariaDeshecha = String.Empty;

                    switch (idTipoOrden)
                    {
                        case Constante.ID_TIPO_ORDEN_TRANSFERENCIA:
                            nombreVistaParcialOrdenBancariaDeshecha = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_TRANSFERENCIA_DESHECHA;
                            break;
                        case Constante.ID_TIPO_ORDEN_TRANSFERENCIA_BCR:
                            nombreVistaParcialOrdenBancariaDeshecha = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_TRANSFERENCIA_BCR_DESHECHA;
                            break;
                        case Constante.ID_TIPO_ORDEN_PROVEEDOR:
                        case Constante.ID_TIPO_ORDEN_CAMARA_COMERCIO:
                            nombreVistaParcialOrdenBancariaDeshecha = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_PROVEEDOR_DESHECHA;
                            break;
                    }

                    String vistaParcialOrdenBancariaDeshecha = await RenderViewOrPartialView.RenderViewAsync(controller, nombreVistaParcialOrdenBancariaDeshecha, String.Empty, Constante.ES_VISTA_PARCIAL);
                    String vistaParcialAccionesOrdenBancaria = await RenderViewOrPartialView.RenderViewAsync(controller, Constante.VISTA_PARCIAL_ACCIONES_ORDEN_BANCARIA, String.Empty, Constante.ES_VISTA_PARCIAL);
                    objeto = new
                    {
                        codigo = vistaParcialOrdenBancariaDeshecha != null ? Constante.CODIGO_OK : Constante.CODIGO_NO_OK,
                        mensaje = vistaParcialOrdenBancariaDeshecha != null ? Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DESHECHA_ASYNC_OK : Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DESHECHA_ASYNC_NO_OK,
                        vistaParcialOrdenBancariaDeshecha = vistaParcialOrdenBancariaDeshecha,
                        vistaParcialAccionesOrdenBancaria = vistaParcialAccionesOrdenBancaria
                    };
                    esCorrecto = vistaParcialOrdenBancariaDeshecha != null ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DESHECHA_ASYNC_OK : Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DESHECHA_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DESHECHA_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DESHECHA_ASYNC, Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DESHECHA_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> ObtenerVistaParcialOrdenBancariaDetalleDeshechaAsync(CancellationToken cancelToken, Controller controller, String idTipoOrden)
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
                    String nombreVistaParcialOrdenBancariaDetalleDeshecha = String.Empty;

                    switch (idTipoOrden)
                    {
                        case Constante.ID_TIPO_ORDEN_TRANSFERENCIA:
                            nombreVistaParcialOrdenBancariaDetalleDeshecha = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_TRANSFERENCIA_DETALLE_DESHECHA;
                            break;
                        case Constante.ID_TIPO_ORDEN_TRANSFERENCIA_BCR:
                            nombreVistaParcialOrdenBancariaDetalleDeshecha = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_TRANSFERENCIA_BCR_DETALLE_DESHECHA;
                            break;
                        case Constante.ID_TIPO_ORDEN_PROVEEDOR:
                        case Constante.ID_TIPO_ORDEN_CAMARA_COMERCIO:
                            nombreVistaParcialOrdenBancariaDetalleDeshecha = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_PROVEEDOR_DETALLE_DESHECHA;
                            break;
                    }

                    String vistaParcialOrdenBancariaDetalleDeshecha = await RenderViewOrPartialView.RenderViewAsync(controller, nombreVistaParcialOrdenBancariaDetalleDeshecha, String.Empty, Constante.ES_VISTA_PARCIAL);
                    
                    objeto = new
                    {
                        codigo = vistaParcialOrdenBancariaDetalleDeshecha != null ? Constante.CODIGO_OK : Constante.CODIGO_NO_OK,
                        mensaje = vistaParcialOrdenBancariaDetalleDeshecha != null ? Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_DESHECHA_ASYNC_OK : Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_DESHECHA_ASYNC_NO_OK,
                        vistaParcialOrdenBancariaDetalleDeshecha = vistaParcialOrdenBancariaDetalleDeshecha
                    };
                    esCorrecto = vistaParcialOrdenBancariaDetalleDeshecha != null ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_DESHECHA_ASYNC_OK : Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_DESHECHA_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_DESHECHA_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_DESHECHA_ASYNC, Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_DESHECHA_ASYNC_NO_OK, e.Message);
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
                    ObjetoOrdenBancariaMO objetoOrdenBancariaMO = await _ordenBancariaRE.ListarOrdenesBancariasAprobadasTesoreriaAsync(cancelToken, idUsuario, pagina, filas, idSociedad, idBanco, idTipoOrden);
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
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_APROBADAS_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_APROBADAS_ASYNC, Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_APROBADAS_ASYNC_NO_OK, e.Message);
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
                    ObjetoOrdenBancariaDetalleMO objetoOrdenBancariaDetalleMO = await _ordenBancariaRE.ListarOrdenesBancariasDetalleAprobadasTesoreriaAsync(cancelToken, idSociedad, idSap, anio, momentoOrden, idTipoOrden);
                    objeto = new
                    {
                        codigo = objetoOrdenBancariaDetalleMO.Codigo,
                        mensaje = objetoOrdenBancariaDetalleMO.Mensaje,
                        listaOrdenesBancariasDetalle = objetoOrdenBancariaDetalleMO.ListaOrdenesBancariasDetalle
                    };
                    esCorrecto = objetoOrdenBancariaDetalleMO.Codigo != Constante.CODIGO_NO_OK ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_APROBADAS_ASYNC_OK : Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_APROBADAS_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DETALLE_APROBADAS_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DETALLE_APROBADAS_ASYNC, Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_APROBADAS_ASYNC_NO_OK, e.Message);
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
                    ObjetoOrdenBancariaMO objetoOrdenBancariaMO = await _ordenBancariaRE.BuscarOrdenesBancariasAprobadasTesoreriaAsync(cancelToken, idUsuario, idSap, fechaInicio, fechaFin);
                    objeto = new
                    {
                        codigo = objetoOrdenBancariaMO.Codigo,
                        mensaje = objetoOrdenBancariaMO.Mensaje,
                        listaOrdenesBancarias = objetoOrdenBancariaMO.ListaOrdenesBancarias
                    };
                    esCorrecto = objetoOrdenBancariaMO.Codigo != Constante.CODIGO_NO_OK ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_APROBADAS_ASYNC_OK : Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_APROBADAS_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_BUSCAR_ORDENES_BANCARIAS_APROBADAS_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_BUSCAR_ORDENES_BANCARIAS_APROBADAS_ASYNC, Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_APROBADAS_ASYNC_NO_OK, e.Message);
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
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_APROBADA_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_APROBADA_ASYNC, Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_APROBADA_ASYNC_NO_OK, e.Message);
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
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_APROBADA_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_APROBADA_ASYNC, Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_APROBADA_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> ListarOrdenesBancariasDiariasAsync(CancellationToken cancelToken, String idUsuario, Int32 pagina, Int32 filas, String idSociedad, String idBanco, String idTipoOrden, String idEstadoOrden)
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
                    ObjetoOrdenBancariaMO objetoOrdenBancariaMO = await _ordenBancariaRE.ListarOrdenesBancariasDiariasAsync(cancelToken, idUsuario, pagina, filas, idSociedad, idBanco, idTipoOrden, idEstadoOrden);
                    objeto = new
                    {
                        codigo = objetoOrdenBancariaMO.Codigo,
                        mensaje = objetoOrdenBancariaMO.Mensaje,
                        listaOrdenesBancarias = objetoOrdenBancariaMO.ListaOrdenesBancarias,
                        totalRegistros = objetoOrdenBancariaMO.TotalRegistros
                    };
                    esCorrecto = objetoOrdenBancariaMO.Codigo != Constante.CODIGO_NO_OK ? true : false;
                }
                
                String mensaje = esCorrecto ? Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DIARIAS_ASYNC_OK : Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DIARIAS_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DIARIAS_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DIARIAS_ASYNC, Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DIARIAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> ListarOrdenesBancariasDetalleDiariasAsync(CancellationToken cancelToken, String idSociedad, String idSap, String anio, String momentoOrden, String idTipoOrden)
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
                    ObjetoOrdenBancariaDetalleMO objetoOrdenBancariaDetalleMO = await _ordenBancariaRE.ListarOrdenesBancariasDetalleDiariasAsync(cancelToken, idSociedad, idSap, anio, momentoOrden, idTipoOrden);
                    objeto = new
                    {
                        codigo = objetoOrdenBancariaDetalleMO.Codigo,
                        mensaje = objetoOrdenBancariaDetalleMO.Mensaje,
                        listaOrdenesBancariasDetalle = objetoOrdenBancariaDetalleMO.ListaOrdenesBancariasDetalle
                    };
                    esCorrecto = objetoOrdenBancariaDetalleMO.Codigo != Constante.CODIGO_NO_OK ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_DIARIAS_ASYNC_OK : Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_DIARIAS_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DETALLE_DIARIAS_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DETALLE_DIARIAS_ASYNC, Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_DIARIAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> BuscarOrdenesBancariasDiariasAsync(CancellationToken cancelToken, String idUsuario, String idSap)
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
                    ObjetoOrdenBancariaMO objetoOrdenBancariaMO = await _ordenBancariaRE.BuscarOrdenesBancariasDiariasAsync(cancelToken, idUsuario, idSap);
                    objeto = new
                    {
                        codigo = objetoOrdenBancariaMO.Codigo,
                        mensaje = objetoOrdenBancariaMO.Mensaje,
                        listaOrdenesBancarias = objetoOrdenBancariaMO.ListaOrdenesBancarias
                    };
                    esCorrecto = objetoOrdenBancariaMO.Codigo != Constante.CODIGO_NO_OK ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_DIARIAS_ASYNC_OK : Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_DIARIAS_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_BUSCAR_ORDENES_BANCARIAS_DIARIAS_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_BUSCAR_ORDENES_BANCARIAS_DIARIAS_ASYNC, Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_DIARIAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> ObtenerVistaParcialOrdenBancariaDiariaAsync(CancellationToken cancelToken, Controller controller, String idTipoOrden)
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
                    String nombreVistaParcialOrdenBancariaDiaria = String.Empty;
                    switch (idTipoOrden)
                    {
                        case Constante.ID_TIPO_ORDEN_TRANSFERENCIA:
                            nombreVistaParcialOrdenBancariaDiaria = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_TRANSFERENCIA_APROBADA;
                            break;
                        case Constante.ID_TIPO_ORDEN_TRANSFERENCIA_BCR:
                            nombreVistaParcialOrdenBancariaDiaria = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_TRANSFERENCIA_BCR_APROBADA;
                            break;
                        case Constante.ID_TIPO_ORDEN_PROVEEDOR:
                        case Constante.ID_TIPO_ORDEN_CAMARA_COMERCIO:
                            nombreVistaParcialOrdenBancariaDiaria = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_PROVEEDOR_APROBADA;
                            break;
                    }

                    String vistaParcialOrdenBancariaDiaria = await RenderViewOrPartialView.RenderViewAsync(controller, nombreVistaParcialOrdenBancariaDiaria, String.Empty, Constante.ES_VISTA_PARCIAL);
                    String vistaParcialAccionesOrdenBancaria = await RenderViewOrPartialView.RenderViewAsync(controller, Constante.VISTA_PARCIAL_ACCIONES_ORDEN_BANCARIA, String.Empty, Constante.ES_VISTA_PARCIAL);
                    objeto = new
                    {
                        codigo = vistaParcialOrdenBancariaDiaria != null ? Constante.CODIGO_OK : Constante.CODIGO_NO_OK,
                        mensaje = vistaParcialOrdenBancariaDiaria != null ? Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DIARIA_ASYNC_OK : Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DIARIA_ASYNC_NO_OK,
                        vistaParcialOrdenBancariaDiaria = vistaParcialOrdenBancariaDiaria,
                        vistaParcialAccionesOrdenBancaria = vistaParcialAccionesOrdenBancaria
                    };
                    esCorrecto = vistaParcialOrdenBancariaDiaria != null ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DIARIA_ASYNC_OK : Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DIARIA_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DIARIA_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DIARIA_ASYNC, Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DIARIA_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> ObtenerVistaParcialOrdenBancariaDetalleDiariaAsync(CancellationToken cancelToken, Controller controller, String idTipoOrden)
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
                    String nombreVistaParcialOrdenBancariaDetalleDiaria = String.Empty;

                    switch (idTipoOrden)
                    {
                        case Constante.ID_TIPO_ORDEN_TRANSFERENCIA:
                            nombreVistaParcialOrdenBancariaDetalleDiaria = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_TRANSFERENCIA_DETALLE_APROBADA;
                            break;
                        case Constante.ID_TIPO_ORDEN_TRANSFERENCIA_BCR:
                            nombreVistaParcialOrdenBancariaDetalleDiaria = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_TRANSFERENCIA_BCR_DETALLE_APROBADA;
                            break;
                        case Constante.ID_TIPO_ORDEN_PROVEEDOR:
                        case Constante.ID_TIPO_ORDEN_CAMARA_COMERCIO:
                            nombreVistaParcialOrdenBancariaDetalleDiaria = Constante.VISTA_PARCIAL_ORDEN_BANCARIA_PROVEEDOR_DETALLE_APROBADA;
                            break;
                    }

                    String vistaParcialOrdenBancariaDetalleDiaria = await RenderViewOrPartialView.RenderViewAsync(controller, nombreVistaParcialOrdenBancariaDetalleDiaria, String.Empty, Constante.ES_VISTA_PARCIAL);
                    
                    objeto = new
                    {
                        codigo = vistaParcialOrdenBancariaDetalleDiaria != null ? Constante.CODIGO_OK : Constante.CODIGO_NO_OK,
                        mensaje = vistaParcialOrdenBancariaDetalleDiaria != null ? Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_DIARIA_ASYNC_OK : Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_DIARIA_ASYNC_NO_OK,
                        vistaParcialOrdenBancariaDetalleDiaria = vistaParcialOrdenBancariaDetalleDiaria
                    };
                    esCorrecto = vistaParcialOrdenBancariaDetalleDiaria != null ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_DIARIA_ASYNC_OK : Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_DIARIA_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_DIARIA_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_TESORERIA_NE, Constante.METODO_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_DIARIA_ASYNC, Constante.MENSAJE_OBTENER_VISTA_PARCIAL_ORDEN_BANCARIA_DETALLE_DIARIA_ASYNC_NO_OK, e.Message);
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
    }
}