using System;
using System.Threading;
using System.Threading.Tasks;
using Comun;
using Microsoft.Extensions.Configuration;
using Modelo;
using Repositorio;

namespace Negocio
{
    public class FiltroNE
    {
        private Bitacora _bitacora = null;
        private FiltroRE _filtroRE = null;

        public FiltroNE(IConfiguration configuration)
        {
            _bitacora = _bitacora ?? new Bitacora(configuration);
            _filtroRE = _filtroRE ?? new FiltroRE(configuration);
        }

        public async Task<Object> ListarFiltrosOrdenesBancariasPorAprobarAsync(CancellationToken cancelToken, String idUsuario)
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
                    ObjetoFiltroMO objetoFiltroMO = await _filtroRE.ListarFiltrosOrdenesBancariasPorAprobarAsync(cancelToken, idUsuario);
                    objeto = new
                    {
                        codigo = objetoFiltroMO.Codigo,
                        mensaje = objetoFiltroMO.Mensaje,
                        listaFiltros = objetoFiltroMO.ListaFiltros
                    };
                    esCorrecto = objetoFiltroMO.Codigo != Constante.CODIGO_NO_OK ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_POR_APROBAR_ASYNC_OK : Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_POR_APROBAR_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_FILTRO_NE, Constante.METODO_LISTAR_FILTROS_ORDENES_BANCARIAS_POR_APROBAR_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_FILTRO_NE, Constante.METODO_LISTAR_FILTROS_ORDENES_BANCARIAS_POR_APROBAR_ASYNC, Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_POR_APROBAR_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> ListarFiltrosOrdenesBancariasAprobadasAsync(CancellationToken cancelToken, String idUsuario)
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
                    ObjetoFiltroMO objetoFiltroMO = await _filtroRE.ListarFiltrosOrdenesBancariasAprobadasAsync(cancelToken, idUsuario);
                    objeto = new
                    {
                        codigo = objetoFiltroMO.Codigo,
                        mensaje = objetoFiltroMO.Mensaje,
                        listaFiltros = objetoFiltroMO.ListaFiltros
                    };
                    esCorrecto = objetoFiltroMO.Codigo != Constante.CODIGO_NO_OK ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_APROBADAS_ASYNC_OK : Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_APROBADAS_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_FILTRO_NE, Constante.METODO_LISTAR_FILTROS_ORDENES_BANCARIAS_APROBADAS_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_FILTRO_NE, Constante.METODO_LISTAR_FILTROS_ORDENES_BANCARIAS_APROBADAS_ASYNC, Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_APROBADAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> ListarFiltrosOrdenesBancariasAnuladasAsync(CancellationToken cancelToken, String idUsuario)
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
                    ObjetoFiltroMO objetoFiltroMO = await _filtroRE.ListarFiltrosOrdenesBancariasAnuladasAsync(cancelToken, idUsuario);
                    objeto = new
                    {
                        codigo = objetoFiltroMO.Codigo,
                        mensaje = objetoFiltroMO.Mensaje,
                        listaFiltros = objetoFiltroMO.ListaFiltros
                    };
                    esCorrecto = objetoFiltroMO.Codigo != Constante.CODIGO_NO_OK ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_ANULADAS_ASYNC_OK : Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_ANULADAS_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_FILTRO_NE, Constante.METODO_LISTAR_FILTROS_ORDENES_BANCARIAS_ANULADAS_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_FILTRO_NE, Constante.METODO_LISTAR_FILTROS_ORDENES_BANCARIAS_ANULADAS_ASYNC, Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_ANULADAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> ListarFiltrosOrdenesBancariasLiberadasAsync(CancellationToken cancelToken, String idUsuario)
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
                    ObjetoFiltroMO objetoFiltroMO = await _filtroRE.ListarFiltrosOrdenesBancariasLiberadasAsync(cancelToken, idUsuario);
                    objeto = new
                    {
                        codigo = objetoFiltroMO.Codigo,
                        mensaje = objetoFiltroMO.Mensaje,
                        listaFiltros = objetoFiltroMO.ListaFiltros
                    };
                    esCorrecto = objetoFiltroMO.Codigo != Constante.CODIGO_NO_OK ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_LIBERADAS_ASYNC_OK : Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_LIBERADAS_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_FILTRO_NE, Constante.METODO_LISTAR_FILTROS_ORDENES_BANCARIAS_LIBERADAS_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_FILTRO_NE, Constante.METODO_LISTAR_FILTROS_ORDENES_BANCARIAS_LIBERADAS_ASYNC, Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_LIBERADAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> ListarFiltrosOrdenesBancariasDesechasAsync(CancellationToken cancelToken, String idUsuario)
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
                    ObjetoFiltroMO objetoFiltroMO = await _filtroRE.ListarFiltrosOrdenesBancariasDesechasAsync(cancelToken, idUsuario);
                    objeto = new
                    {
                        codigo = objetoFiltroMO.Codigo,
                        mensaje = objetoFiltroMO.Mensaje,
                        listaFiltros = objetoFiltroMO.ListaFiltros
                    };
                    esCorrecto = objetoFiltroMO.Codigo != Constante.CODIGO_NO_OK ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_DESECHAS_ASYNC_OK : Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_DESECHAS_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_FILTRO_NE, Constante.METODO_LISTAR_FILTROS_ORDENES_BANCARIAS_DESECHAS_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_FILTRO_NE, Constante.METODO_LISTAR_FILTROS_ORDENES_BANCARIAS_DESECHAS_ASYNC, Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_DESECHAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> ListarFiltrosOrdenesBancariasAprobadasTesoreriaAsync(CancellationToken cancelToken, String idUsuario)
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
                    ObjetoFiltroMO objetoFiltroMO = await _filtroRE.ListarFiltrosOrdenesBancariasAprobadasTesoreriaAsync(cancelToken, idUsuario);
                    objeto = new
                    {
                        codigo = objetoFiltroMO.Codigo,
                        mensaje = objetoFiltroMO.Mensaje,
                        listaFiltros = objetoFiltroMO.ListaFiltros
                    };
                    esCorrecto = objetoFiltroMO.Codigo != Constante.CODIGO_NO_OK ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_APROBADAS_ASYNC_OK : Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_APROBADAS_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_FILTRO_NE, Constante.METODO_LISTAR_FILTROS_ORDENES_BANCARIAS_APROBADAS_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_FILTRO_NE, Constante.METODO_LISTAR_FILTROS_ORDENES_BANCARIAS_APROBADAS_ASYNC, Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_APROBADAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> ListarFiltrosOrdenesBancariasDiariasAsync(CancellationToken cancelToken, String idUsuario)
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
                    ObjetoFiltroMO objetoFiltroMO = await _filtroRE.ListarFiltrosOrdenesBancariasDiariasAsync(cancelToken, idUsuario);
                    objeto = new
                    {
                        codigo = objetoFiltroMO.Codigo,
                        mensaje = objetoFiltroMO.Mensaje,
                        listaFiltros = objetoFiltroMO.ListaFiltros
                    };
                    esCorrecto = objetoFiltroMO.Codigo != Constante.CODIGO_NO_OK ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_DIARIAS_ASYNC_OK : Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_DIARIAS_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_FILTRO_NE, Constante.METODO_LISTAR_FILTROS_ORDENES_BANCARIAS_DIARIAS_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_FILTRO_NE, Constante.METODO_LISTAR_FILTROS_ORDENES_BANCARIAS_DIARIAS_ASYNC, Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_DIARIAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }
    }
}