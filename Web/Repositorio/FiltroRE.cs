using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Comun;
using Microsoft.Extensions.Configuration;
using Modelo;

namespace Repositorio
{
    public class FiltroRE
    {
        private String _conexionHostToHost = String.Empty;
        private SqlConnection _con = null;
        private SqlCommand _cmd = null;
        private SqlDataReader _reader = null;
        private Bitacora _bitacora = null;

        public FiltroRE(IConfiguration configuration)
        {
            _bitacora = _bitacora ?? new Bitacora(configuration);
            _conexionHostToHost = configuration.GetConnectionString(Constante.CONEXION_HOST_TO_HOST_PRODUCCION) ?? String.Empty;
        }
        
        public async Task<ObjetoFiltroMO> ListarFiltrosOrdenesBancariasLiberadasAsync(CancellationToken cancelToken, String idUsuario)
        {
            ObjetoFiltroMO objetoFiltroMO = new ObjetoFiltroMO();
            List<FiltroMO> listaFiltros = null;
            List<OpcionMO> listaOpciones = null;
            OpcionMO opcionMO = null;
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_LISTAR_FILTROS_ORDENES_BANCARIAS_LIBERADAS, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_USUARIO, System.Data.SqlDbType.NChar, Constante._6);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idUsuario;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.Default, cancelToken);

                        if (_reader != null && !_reader.HasRows)
                        {
                            objetoFiltroMO.Codigo = Constante.CODIGO_OMISION;
                            objetoFiltroMO.Mensaje = Constante.MENSAJE_SIN_RESULTADOS;
                        }
                        else if (_reader != null && _reader.HasRows)
                        {
                            listaFiltros = new List<FiltroMO>();
                            listaOpciones = new List<OpcionMO>();
                            FiltroMO filtroSociedadMO = new FiltroMO();
                            FiltroMO filtroBancoMO = new FiltroMO();
                            FiltroMO filtroTipoOrdenMO = new FiltroMO();

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                opcionMO = new OpcionMO();
                                opcionMO.IdSociedad = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                opcionMO.Sociedad = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                listaOpciones.Add(opcionMO);
                            }

                            filtroSociedadMO.Categoria = Constante.FILTRO_CATEGORIA_SOCIEDAD;
                            filtroSociedadMO.ListaOpciones = listaOpciones;
                            listaFiltros.Add(filtroSociedadMO);
                            listaOpciones = new List<OpcionMO>();
                            await _reader.NextResultAsync(cancelToken);

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                opcionMO = new OpcionMO();
                                opcionMO.IdBanco = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                opcionMO.Banco = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                listaOpciones.Add(opcionMO);
                            }

                            filtroBancoMO.Categoria = Constante.FILTRO_CATEGORIA_BANCO;
                            filtroBancoMO.ListaOpciones = listaOpciones;
                            listaFiltros.Add(filtroBancoMO);
                            listaOpciones = new List<OpcionMO>();
                            await _reader.NextResultAsync(cancelToken);

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                opcionMO = new OpcionMO();
                                opcionMO.IdTipoOrden = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                opcionMO.TipoOrden = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                listaOpciones.Add(opcionMO);
                            }

                            filtroTipoOrdenMO.Categoria = Constante.FILTRO_CATEGORIA_TIPO_ORDEN;
                            filtroTipoOrdenMO.ListaOpciones = listaOpciones;
                            listaFiltros.Add(filtroTipoOrdenMO);
                            objetoFiltroMO.Codigo = Constante.CODIGO_OK;
                        }

                        _reader.Close();
                        _con.Close();
                        objetoFiltroMO.Codigo = _reader != null ? objetoFiltroMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoFiltroMO.Mensaje = _reader != null ? objetoFiltroMO.Mensaje : Constante.MENSAJE_RECARGAR_PAGINA;
                        objetoFiltroMO.ListaFiltros = listaFiltros;
                        String mensaje = _reader != null ? Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_LIBERADAS_ASYNC_OK : Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_LIBERADAS_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_FILTRO_RE, Constante.METODO_LISTAR_FILTROS_ORDENES_BANCARIAS_LIBERADAS_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_FILTRO_RE, Constante.METODO_LISTAR_FILTROS_ORDENES_BANCARIAS_LIBERADAS_ASYNC, Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_LIBERADAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoFiltroMO;
        }

        public async Task<ObjetoFiltroMO> ListarFiltrosOrdenesBancariasDesechasAsync(CancellationToken cancelToken, String idUsuario)
        {
            ObjetoFiltroMO objetoFiltroMO = new ObjetoFiltroMO();
            List<FiltroMO> listaFiltros = null;
            List<OpcionMO> listaOpciones = null;
            OpcionMO opcionMO = null;
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_LISTAR_FILTROS_ORDENES_BANCARIAS_DESECHAS, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_USUARIO, System.Data.SqlDbType.NChar, Constante._6);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idUsuario;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.Default, cancelToken);

                        if (_reader != null && !_reader.HasRows)
                        {
                            objetoFiltroMO.Codigo = Constante.CODIGO_OMISION;
                            objetoFiltroMO.Mensaje = Constante.MENSAJE_SIN_RESULTADOS;
                        }
                        else if (_reader != null && _reader.HasRows)
                        {
                            listaFiltros = new List<FiltroMO>();
                            listaOpciones = new List<OpcionMO>();
                            FiltroMO filtroSociedadMO = new FiltroMO();
                            FiltroMO filtroBancoMO = new FiltroMO();
                            FiltroMO filtroTipoOrdenMO = new FiltroMO();

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                opcionMO = new OpcionMO();
                                opcionMO.IdSociedad = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                opcionMO.Sociedad = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                listaOpciones.Add(opcionMO);
                            }

                            filtroSociedadMO.Categoria = Constante.FILTRO_CATEGORIA_SOCIEDAD;
                            filtroSociedadMO.ListaOpciones = listaOpciones;
                            listaFiltros.Add(filtroSociedadMO);
                            listaOpciones = new List<OpcionMO>();
                            await _reader.NextResultAsync(cancelToken);

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                opcionMO = new OpcionMO();
                                opcionMO.IdBanco = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                opcionMO.Banco = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                listaOpciones.Add(opcionMO);
                            }

                            filtroBancoMO.Categoria = Constante.FILTRO_CATEGORIA_BANCO;
                            filtroBancoMO.ListaOpciones = listaOpciones;
                            listaFiltros.Add(filtroBancoMO);
                            listaOpciones = new List<OpcionMO>();
                            await _reader.NextResultAsync(cancelToken);

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                opcionMO = new OpcionMO();
                                opcionMO.IdTipoOrden = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                opcionMO.TipoOrden = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                listaOpciones.Add(opcionMO);
                            }

                            filtroTipoOrdenMO.Categoria = Constante.FILTRO_CATEGORIA_TIPO_ORDEN;
                            filtroTipoOrdenMO.ListaOpciones = listaOpciones;
                            listaFiltros.Add(filtroTipoOrdenMO);
                            objetoFiltroMO.Codigo = Constante.CODIGO_OK;
                        }

                        _reader.Close();
                        _con.Close();
                        objetoFiltroMO.Codigo = _reader != null ? objetoFiltroMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoFiltroMO.Mensaje = _reader != null ? objetoFiltroMO.Mensaje : Constante.MENSAJE_RECARGAR_PAGINA;
                        objetoFiltroMO.ListaFiltros = listaFiltros;
                        String mensaje = _reader != null ? Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_DESECHAS_ASYNC_OK : Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_DESECHAS_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_FILTRO_RE, Constante.METODO_LISTAR_FILTROS_ORDENES_BANCARIAS_DESECHAS_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_FILTRO_RE, Constante.METODO_LISTAR_FILTROS_ORDENES_BANCARIAS_DESECHAS_ASYNC, Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_DESECHAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoFiltroMO;
        }

        public async Task<ObjetoFiltroMO> ListarFiltrosOrdenesBancariasPorAprobarAsync(CancellationToken cancelToken, String idUsuario)
        {
            ObjetoFiltroMO objetoFiltroMO = new ObjetoFiltroMO();
            List<FiltroMO> listaFiltros = null;
            List<OpcionMO> listaOpciones = null;
            OpcionMO opcionMO = null;
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_LISTAR_FILTROS_ORDENES_BANCARIAS_POR_APROBAR, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_USUARIO, System.Data.SqlDbType.NChar, Constante._6);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idUsuario;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.Default, cancelToken);

                        if (_reader != null && !_reader.HasRows)
                        {
                            objetoFiltroMO.Codigo = Constante.CODIGO_OMISION;
                            objetoFiltroMO.Mensaje = Constante.MENSAJE_SIN_RESULTADOS;
                        }
                        else if (_reader != null && _reader.HasRows)
                        {
                            listaFiltros = new List<FiltroMO>();
                            listaOpciones = new List<OpcionMO>();
                            FiltroMO filtroSociedadMO = new FiltroMO();
                            FiltroMO filtroBancoMO = new FiltroMO();
                            FiltroMO filtroTipoOrdenMO = new FiltroMO();
                            FiltroMO filtroEstadoOrdenMO = new FiltroMO();

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                opcionMO = new OpcionMO();
                                opcionMO.IdSociedad = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                opcionMO.Sociedad = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                listaOpciones.Add(opcionMO);
                            }

                            filtroSociedadMO.Categoria = Constante.FILTRO_CATEGORIA_SOCIEDAD;
                            filtroSociedadMO.ListaOpciones = listaOpciones;
                            listaFiltros.Add(filtroSociedadMO);
                            listaOpciones = new List<OpcionMO>();
                            await _reader.NextResultAsync(cancelToken);

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                opcionMO = new OpcionMO();
                                opcionMO.IdBanco = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                opcionMO.Banco = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                listaOpciones.Add(opcionMO);
                            }

                            filtroBancoMO.Categoria = Constante.FILTRO_CATEGORIA_BANCO;
                            filtroBancoMO.ListaOpciones = listaOpciones;
                            listaFiltros.Add(filtroBancoMO);
                            listaOpciones = new List<OpcionMO>();
                            await _reader.NextResultAsync(cancelToken);

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                opcionMO = new OpcionMO();
                                opcionMO.IdTipoOrden = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                opcionMO.TipoOrden = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                listaOpciones.Add(opcionMO);
                            }

                            filtroTipoOrdenMO.Categoria = Constante.FILTRO_CATEGORIA_TIPO_ORDEN;
                            filtroTipoOrdenMO.ListaOpciones = listaOpciones;
                            listaFiltros.Add(filtroTipoOrdenMO);
                            listaOpciones = new List<OpcionMO>();
                            await _reader.NextResultAsync(cancelToken);

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                opcionMO = new OpcionMO();
                                opcionMO.IdEstadoOrden = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                opcionMO.EstadoOrden = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                listaOpciones.Add(opcionMO);
                            }

                            filtroEstadoOrdenMO.Categoria = Constante.FILTRO_CATEGORIA_ESTADO_ORDEN;
                            filtroEstadoOrdenMO.ListaOpciones = listaOpciones;
                            listaFiltros.Add(filtroEstadoOrdenMO);
                            objetoFiltroMO.Codigo = Constante.CODIGO_OK;
                        }

                        _reader.Close();
                        _con.Close();
                        objetoFiltroMO.Codigo = _reader != null ? objetoFiltroMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoFiltroMO.Mensaje = _reader != null ? objetoFiltroMO.Mensaje : Constante.MENSAJE_RECARGAR_PAGINA;
                        objetoFiltroMO.ListaFiltros = listaFiltros;
                        String mensaje = _reader != null ? Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_POR_APROBAR_ASYNC_OK : Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_POR_APROBAR_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_FILTRO_RE, Constante.METODO_LISTAR_FILTROS_ORDENES_BANCARIAS_POR_APROBAR_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_FILTRO_RE, Constante.METODO_LISTAR_FILTROS_ORDENES_BANCARIAS_POR_APROBAR_ASYNC, Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_POR_APROBAR_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoFiltroMO;
        }

        public async Task<ObjetoFiltroMO> ListarFiltrosOrdenesBancariasAprobadasAsync(CancellationToken cancelToken, String idUsuario)
        {
            ObjetoFiltroMO objetoFiltroMO = new ObjetoFiltroMO();
            List<FiltroMO> listaFiltros = null;
            List<OpcionMO> listaOpciones = null;
            OpcionMO opcionMO = null;
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_LISTAR_FILTROS_ORDENES_BANCARIAS_APROBADAS, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_USUARIO, System.Data.SqlDbType.NChar, Constante._6);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idUsuario;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.Default, cancelToken);

                        if (_reader != null && !_reader.HasRows)
                        {
                            objetoFiltroMO.Codigo = Constante.CODIGO_OMISION;
                            objetoFiltroMO.Mensaje = Constante.MENSAJE_SIN_RESULTADOS;
                        }
                        else if (_reader != null && _reader.HasRows)
                        {
                            listaFiltros = new List<FiltroMO>();
                            listaOpciones = new List<OpcionMO>();
                            FiltroMO filtroSociedadMO = new FiltroMO();
                            FiltroMO filtroBancoMO = new FiltroMO();
                            FiltroMO filtroTipoOrdenMO = new FiltroMO();

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                opcionMO = new OpcionMO();
                                opcionMO.IdSociedad = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                opcionMO.Sociedad = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                listaOpciones.Add(opcionMO);
                            }

                            filtroSociedadMO.Categoria = Constante.FILTRO_CATEGORIA_SOCIEDAD;
                            filtroSociedadMO.ListaOpciones = listaOpciones;
                            listaFiltros.Add(filtroSociedadMO);
                            listaOpciones = new List<OpcionMO>();
                            await _reader.NextResultAsync(cancelToken);

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                opcionMO = new OpcionMO();
                                opcionMO.IdBanco = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                opcionMO.Banco = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                listaOpciones.Add(opcionMO);
                            }

                            filtroBancoMO.Categoria = Constante.FILTRO_CATEGORIA_BANCO;
                            filtroBancoMO.ListaOpciones = listaOpciones;
                            listaFiltros.Add(filtroBancoMO);
                            listaOpciones = new List<OpcionMO>();
                            await _reader.NextResultAsync(cancelToken);

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                opcionMO = new OpcionMO();
                                opcionMO.IdTipoOrden = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                opcionMO.TipoOrden = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                listaOpciones.Add(opcionMO);
                            }

                            filtroTipoOrdenMO.Categoria = Constante.FILTRO_CATEGORIA_TIPO_ORDEN;
                            filtroTipoOrdenMO.ListaOpciones = listaOpciones;
                            listaFiltros.Add(filtroTipoOrdenMO);
                            objetoFiltroMO.Codigo = Constante.CODIGO_OK;
                        }

                        _reader.Close();
                        _con.Close();
                        objetoFiltroMO.Codigo = _reader != null ? objetoFiltroMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoFiltroMO.Mensaje = _reader != null ? objetoFiltroMO.Mensaje : Constante.MENSAJE_RECARGAR_PAGINA;
                        objetoFiltroMO.ListaFiltros = listaFiltros;
                        String mensaje = _reader != null ? Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_APROBADAS_ASYNC_OK : Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_APROBADAS_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_FILTRO_RE, Constante.METODO_LISTAR_FILTROS_ORDENES_BANCARIAS_APROBADAS_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_FILTRO_RE, Constante.METODO_LISTAR_FILTROS_ORDENES_BANCARIAS_APROBADAS_ASYNC, Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_APROBADAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoFiltroMO;
        }

        public async Task<ObjetoFiltroMO> ListarFiltrosOrdenesBancariasAnuladasAsync(CancellationToken cancelToken, String idUsuario)
        {
            ObjetoFiltroMO objetoFiltroMO = new ObjetoFiltroMO();
            List<FiltroMO> listaFiltros = null;
            List<OpcionMO> listaOpciones = null;
            OpcionMO opcionMO = null;
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_LISTAR_FILTROS_ORDENES_BANCARIAS_ANULADAS, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_USUARIO, System.Data.SqlDbType.NChar, Constante._6);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idUsuario;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.Default, cancelToken);

                        if (_reader != null && !_reader.HasRows)
                        {
                            objetoFiltroMO.Codigo = Constante.CODIGO_OMISION;
                            objetoFiltroMO.Mensaje = Constante.MENSAJE_SIN_RESULTADOS;
                        }
                        else if (_reader != null && _reader.HasRows)
                        {
                            listaFiltros = new List<FiltroMO>();
                            listaOpciones = new List<OpcionMO>();
                            FiltroMO filtroSociedadMO = new FiltroMO();
                            FiltroMO filtroBancoMO = new FiltroMO();
                            FiltroMO filtroTipoOrdenMO = new FiltroMO();

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                opcionMO = new OpcionMO();
                                opcionMO.IdSociedad = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                opcionMO.Sociedad = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                listaOpciones.Add(opcionMO);
                            }

                            filtroSociedadMO.Categoria = Constante.FILTRO_CATEGORIA_SOCIEDAD;
                            filtroSociedadMO.ListaOpciones = listaOpciones;
                            listaFiltros.Add(filtroSociedadMO);
                            listaOpciones = new List<OpcionMO>();
                            await _reader.NextResultAsync(cancelToken);

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                opcionMO = new OpcionMO();
                                opcionMO.IdBanco = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                opcionMO.Banco = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                listaOpciones.Add(opcionMO);
                            }

                            filtroBancoMO.Categoria = Constante.FILTRO_CATEGORIA_BANCO;
                            filtroBancoMO.ListaOpciones = listaOpciones;
                            listaFiltros.Add(filtroBancoMO);
                            listaOpciones = new List<OpcionMO>();
                            await _reader.NextResultAsync(cancelToken);

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                opcionMO = new OpcionMO();
                                opcionMO.IdTipoOrden = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                opcionMO.TipoOrden = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                listaOpciones.Add(opcionMO);
                            }

                            filtroTipoOrdenMO.Categoria = Constante.FILTRO_CATEGORIA_TIPO_ORDEN;
                            filtroTipoOrdenMO.ListaOpciones = listaOpciones;
                            listaFiltros.Add(filtroTipoOrdenMO);
                            objetoFiltroMO.Codigo = Constante.CODIGO_OK;
                        }

                        _reader.Close();
                        _con.Close();
                        objetoFiltroMO.Codigo = _reader != null ? objetoFiltroMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoFiltroMO.Mensaje = _reader != null ? objetoFiltroMO.Mensaje : Constante.MENSAJE_RECARGAR_PAGINA;
                        objetoFiltroMO.ListaFiltros = listaFiltros;
                        String mensaje = _reader != null ? Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_ANULADAS_ASYNC_OK : Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_ANULADAS_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_FILTRO_RE, Constante.METODO_LISTAR_FILTROS_ORDENES_BANCARIAS_ANULADAS_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_FILTRO_RE, Constante.METODO_LISTAR_FILTROS_ORDENES_BANCARIAS_ANULADAS_ASYNC, Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_ANULADAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoFiltroMO;
        }

        public async Task<ObjetoFiltroMO> ListarFiltrosOrdenesBancariasAprobadasTesoreriaAsync(CancellationToken cancelToken, String idUsuario)
        {
            ObjetoFiltroMO objetoFiltroMO = new ObjetoFiltroMO();
            List<FiltroMO> listaFiltros = null;
            List<OpcionMO> listaOpciones = null;
            OpcionMO opcionMO = null;
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_LISTAR_FILTROS_ORDENES_BANCARIAS_APROBADAS_TESORERIA, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_USUARIO, System.Data.SqlDbType.NChar, Constante._6);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idUsuario;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.Default, cancelToken);

                        if (_reader != null && !_reader.HasRows)
                        {
                            objetoFiltroMO.Codigo = Constante.CODIGO_OMISION;
                            objetoFiltroMO.Mensaje = Constante.MENSAJE_SIN_RESULTADOS;
                        }
                        else if (_reader != null && _reader.HasRows)
                        {
                            listaFiltros = new List<FiltroMO>();
                            listaOpciones = new List<OpcionMO>();
                            FiltroMO filtroSociedadMO = new FiltroMO();
                            FiltroMO filtroBancoMO = new FiltroMO();
                            FiltroMO filtroTipoOrdenMO = new FiltroMO();

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                opcionMO = new OpcionMO();
                                opcionMO.IdSociedad = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                opcionMO.Sociedad = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                listaOpciones.Add(opcionMO);
                            }

                            filtroSociedadMO.Categoria = Constante.FILTRO_CATEGORIA_SOCIEDAD;
                            filtroSociedadMO.ListaOpciones = listaOpciones;
                            listaFiltros.Add(filtroSociedadMO);
                            listaOpciones = new List<OpcionMO>();
                            await _reader.NextResultAsync(cancelToken);

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                opcionMO = new OpcionMO();
                                opcionMO.IdBanco = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                opcionMO.Banco = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                listaOpciones.Add(opcionMO);
                            }

                            filtroBancoMO.Categoria = Constante.FILTRO_CATEGORIA_BANCO;
                            filtroBancoMO.ListaOpciones = listaOpciones;
                            listaFiltros.Add(filtroBancoMO);
                            listaOpciones = new List<OpcionMO>();
                            await _reader.NextResultAsync(cancelToken);

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                opcionMO = new OpcionMO();
                                opcionMO.IdTipoOrden = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                opcionMO.TipoOrden = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                listaOpciones.Add(opcionMO);
                            }

                            filtroTipoOrdenMO.Categoria = Constante.FILTRO_CATEGORIA_TIPO_ORDEN;
                            filtroTipoOrdenMO.ListaOpciones = listaOpciones;
                            listaFiltros.Add(filtroTipoOrdenMO);
                            objetoFiltroMO.Codigo = Constante.CODIGO_OK;
                        }

                        _reader.Close();
                        _con.Close();
                        objetoFiltroMO.Codigo = _reader != null ? objetoFiltroMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoFiltroMO.Mensaje = _reader != null ? objetoFiltroMO.Mensaje : Constante.MENSAJE_RECARGAR_PAGINA;
                        objetoFiltroMO.ListaFiltros = listaFiltros;
                        String mensaje = _reader != null ? Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_APROBADAS_ASYNC_OK : Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_APROBADAS_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_FILTRO_RE, Constante.METODO_LISTAR_FILTROS_ORDENES_BANCARIAS_APROBADAS_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_FILTRO_RE, Constante.METODO_LISTAR_FILTROS_ORDENES_BANCARIAS_APROBADAS_ASYNC, Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_APROBADAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoFiltroMO;
        }

        public async Task<ObjetoFiltroMO> ListarFiltrosOrdenesBancariasDiariasAsync(CancellationToken cancelToken, String idUsuario)
        {
            ObjetoFiltroMO objetoFiltroMO = new ObjetoFiltroMO();
            List<FiltroMO> listaFiltros = null;
            List<OpcionMO> listaOpciones = null;
            OpcionMO opcionMO = null;
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_LISTAR_FILTROS_ORDENES_BANCARIAS_DIARIAS, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_USUARIO, System.Data.SqlDbType.NChar, Constante._6);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idUsuario;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.Default, cancelToken);

                        if (_reader != null && !_reader.HasRows)
                        {
                            objetoFiltroMO.Codigo = Constante.CODIGO_OMISION;
                            objetoFiltroMO.Mensaje = Constante.MENSAJE_SIN_RESULTADOS;
                        }
                        else if (_reader != null && _reader.HasRows)
                        {
                            listaFiltros = new List<FiltroMO>();
                            listaOpciones = new List<OpcionMO>();
                            FiltroMO filtroSociedadMO = new FiltroMO();
                            FiltroMO filtroBancoMO = new FiltroMO();
                            FiltroMO filtroTipoOrdenMO = new FiltroMO();
                            FiltroMO filtroEstadoOrdenMO = new FiltroMO();

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                opcionMO = new OpcionMO();
                                opcionMO.IdSociedad = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                opcionMO.Sociedad = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                listaOpciones.Add(opcionMO);
                            }

                            filtroSociedadMO.Categoria = Constante.FILTRO_CATEGORIA_SOCIEDAD;
                            filtroSociedadMO.ListaOpciones = listaOpciones;
                            listaFiltros.Add(filtroSociedadMO);
                            listaOpciones = new List<OpcionMO>();
                            await _reader.NextResultAsync(cancelToken);

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                opcionMO = new OpcionMO();
                                opcionMO.IdBanco = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                opcionMO.Banco = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                listaOpciones.Add(opcionMO);
                            }

                            filtroBancoMO.Categoria = Constante.FILTRO_CATEGORIA_BANCO;
                            filtroBancoMO.ListaOpciones = listaOpciones;
                            listaFiltros.Add(filtroBancoMO);
                            listaOpciones = new List<OpcionMO>();
                            await _reader.NextResultAsync(cancelToken);

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                opcionMO = new OpcionMO();
                                opcionMO.IdTipoOrden = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                opcionMO.TipoOrden = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                listaOpciones.Add(opcionMO);
                            }

                            filtroTipoOrdenMO.Categoria = Constante.FILTRO_CATEGORIA_TIPO_ORDEN;
                            filtroTipoOrdenMO.ListaOpciones = listaOpciones;
                            listaFiltros.Add(filtroTipoOrdenMO);
                            listaOpciones = new List<OpcionMO>();
                            await _reader.NextResultAsync(cancelToken);

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                opcionMO = new OpcionMO();
                                opcionMO.IdEstadoOrden = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                opcionMO.EstadoOrden = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                listaOpciones.Add(opcionMO);
                            }

                            filtroEstadoOrdenMO.Categoria = Constante.FILTRO_CATEGORIA_ESTADO_ORDEN;
                            filtroEstadoOrdenMO.ListaOpciones = listaOpciones;
                            listaFiltros.Add(filtroEstadoOrdenMO);
                            objetoFiltroMO.Codigo = Constante.CODIGO_OK;
                        }

                        _reader.Close();
                        _con.Close();
                        objetoFiltroMO.Codigo = _reader != null ? objetoFiltroMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoFiltroMO.Mensaje = _reader != null ? objetoFiltroMO.Mensaje : Constante.MENSAJE_RECARGAR_PAGINA;
                        objetoFiltroMO.ListaFiltros = listaFiltros;
                        String mensaje = _reader != null ? Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_DIARIAS_ASYNC_OK : Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_DIARIAS_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_FILTRO_RE, Constante.METODO_LISTAR_FILTROS_ORDENES_BANCARIAS_DIARIAS_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_FILTRO_RE, Constante.METODO_LISTAR_FILTROS_ORDENES_BANCARIAS_DIARIAS_ASYNC, Constante.MENSAJE_LISTAR_FILTROS_ORDENES_BANCARIAS_DIARIAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoFiltroMO;
        }
    }
}