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
    public class OrdenBancariaRE
    {
        private String _conexionHostToHost = String.Empty;
        private SqlConnection _con = null;
        private SqlCommand _cmd = null;
        private SqlDataReader _reader = null;
        private Bitacora _bitacora = null;

        public OrdenBancariaRE(IConfiguration configuration)
        {
            _bitacora = _bitacora ?? new Bitacora(configuration);
            _conexionHostToHost = configuration.GetConnectionString(Constante.CONEXION_HOST_TO_HOST_PRODUCCION) ?? String.Empty;
        }

        public async Task<ObjetoOrdenBancariaMO> ListarOrdenesBancariasLiberadasAsync(CancellationToken cancelToken, String idUsuario, Int32 pagina, Int32 filas, String idSociedad, String idBanco, String idTipoOrden)
        {
            ObjetoOrdenBancariaMO objetoOrdenBancariaMO = new ObjetoOrdenBancariaMO();
            List<OrdenBancariaMO> listaOrdenesBancarias = null;
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_LISTAR_ORDENES_BANCARIAS_LIBERADAS, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        
                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_USUARIO, System.Data.SqlDbType.NChar, Constante._6);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idUsuario;

                        SqlParameter par2 = _cmd.Parameters.Add(Constante.PAGINA, System.Data.SqlDbType.Int);
                        par2.Direction = System.Data.ParameterDirection.Input;
                        par2.Value = pagina;

                        SqlParameter par3 = _cmd.Parameters.Add(Constante.FILAS, System.Data.SqlDbType.Int);
                        par3.Direction = System.Data.ParameterDirection.Input;
                        par3.Value = filas;

                        SqlParameter par4 = _cmd.Parameters.Add(Constante.ID_SOCIEDAD, System.Data.SqlDbType.NChar, 4);
                        par4.Direction = System.Data.ParameterDirection.Input;
                        par4.Value = idSociedad;

                        SqlParameter par5 = _cmd.Parameters.Add(Constante.ID_BANCO, System.Data.SqlDbType.NChar, 5);
                        par5.Direction = System.Data.ParameterDirection.Input;
                        par5.Value = idBanco;

                        SqlParameter par6 = _cmd.Parameters.Add(Constante.ID_TIPO_ORDEN, System.Data.SqlDbType.NVarChar, 5);
                        par6.Direction = System.Data.ParameterDirection.Input;
                        par6.Value = idTipoOrden;

                        SqlParameter par7 = _cmd.Parameters.Add(Constante.TOTAL_REGISTROS, System.Data.SqlDbType.Int);
                        par7.Direction = System.Data.ParameterDirection.Output;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult, cancelToken);

                        if (_reader != null && !_reader.HasRows)
                        {
                            objetoOrdenBancariaMO.Codigo = Constante.CODIGO_OMISION;
                            objetoOrdenBancariaMO.Mensaje = Constante.MENSAJE_SIN_RESULTADOS;
                        }
                        else if (_reader != null && _reader.HasRows)
                        {
                            listaOrdenesBancarias = new List<OrdenBancariaMO>();

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                OrdenBancariaMO ordenBancariaMO = new OrdenBancariaMO();
                                ordenBancariaMO.IdOrdenBancaria = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                ordenBancariaMO.Banco = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                ordenBancariaMO.BancoCorto = await _reader.IsDBNullAsync(Constante._2, cancelToken) ? String.Empty : _reader.GetString(Constante._2);
                                ordenBancariaMO.IdTipoOrden = await _reader.IsDBNullAsync(Constante._3, cancelToken) ? String.Empty : _reader.GetString(Constante._3);
                                ordenBancariaMO.TipoOrden = await _reader.IsDBNullAsync(Constante._4, cancelToken) ? String.Empty : _reader.GetString(Constante._4);
                                ordenBancariaMO.TipoOrdenCorto = await _reader.IsDBNullAsync(Constante._5, cancelToken) ? String.Empty : _reader.GetString(Constante._5);
                                ordenBancariaMO.IdEstadoOrden = await _reader.IsDBNullAsync(Constante._6, cancelToken) ? String.Empty : _reader.GetString(Constante._6);
                                ordenBancariaMO.EstadoOrden = await _reader.IsDBNullAsync(Constante._7, cancelToken) ? String.Empty : _reader.GetString(Constante._7);
                                ordenBancariaMO.IdSociedad = await _reader.IsDBNullAsync(Constante._8, cancelToken) ? String.Empty : _reader.GetString(Constante._8);
                                ordenBancariaMO.Sociedad = await _reader.IsDBNullAsync(Constante._9, cancelToken) ? String.Empty : _reader.GetString(Constante._9);
                                ordenBancariaMO.SociedadCorto = await _reader.IsDBNullAsync(Constante._10, cancelToken) ? String.Empty : _reader.GetString(Constante._10);
                                ordenBancariaMO.IdSap = await _reader.IsDBNullAsync(Constante._11, cancelToken) ? String.Empty : _reader.GetString(Constante._11);
                                ordenBancariaMO.Anio = await _reader.IsDBNullAsync(Constante._12, cancelToken) ? String.Empty : _reader.GetString(Constante._12);
                                ordenBancariaMO.MomentoOrden = await _reader.IsDBNullAsync(Constante._13, cancelToken) ? String.Empty : _reader.GetString(Constante._13);
                                ordenBancariaMO.FechaOrden = await _reader.IsDBNullAsync(Constante._14, cancelToken) ? String.Empty : _reader.GetString(Constante._14);
                                ordenBancariaMO.NombreArchivo = await _reader.IsDBNullAsync(Constante._15, cancelToken) ? String.Empty : _reader.GetString(Constante._15);
                                ordenBancariaMO.RutaArchivo = await _reader.IsDBNullAsync(Constante._16, cancelToken) ? String.Empty : _reader.GetString(Constante._16);
                                ordenBancariaMO.MonedaLocal = await _reader.IsDBNullAsync(Constante._17, cancelToken) ? String.Empty : _reader.GetString(Constante._17);
                                ordenBancariaMO.MonedaCortoLocal = await _reader.IsDBNullAsync(Constante._18, cancelToken) ? String.Empty : _reader.GetString(Constante._18);
                                ordenBancariaMO.ImporteLocal = await _reader.IsDBNullAsync(Constante._19, cancelToken) ? 0 : _reader.GetDecimal(Constante._19);
                                ordenBancariaMO.MonedaForanea = await _reader.IsDBNullAsync(Constante._20, cancelToken) ? String.Empty : _reader.GetString(Constante._20);
                                ordenBancariaMO.MonedaCortoForanea = await _reader.IsDBNullAsync(Constante._21, cancelToken) ? String.Empty : _reader.GetString(Constante._21);
                                ordenBancariaMO.ImporteForanea = await _reader.IsDBNullAsync(Constante._22, cancelToken) ? 0 : _reader.GetDecimal(Constante._22);
                                ordenBancariaMO.Liberador = await _reader.IsDBNullAsync(Constante._23, cancelToken) ? String.Empty : _reader.GetString(Constante._23);
                                ordenBancariaMO.UsuarioCreacion = await _reader.IsDBNullAsync(Constante._24, cancelToken) ? String.Empty : _reader.GetString(Constante._24);
                                DateTime? fechaCreacion = await _reader.IsDBNullAsync(Constante._25, cancelToken) ? (DateTime?)null : _reader.GetDateTime(Constante._25);
                                ordenBancariaMO.FechaCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortDateString();
                                ordenBancariaMO.HoraCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortTimeString();
                                listaOrdenesBancarias.Add(ordenBancariaMO);
                            }

                            objetoOrdenBancariaMO.Codigo = Constante.CODIGO_OK;
                        }

                        _reader.Close();
                        _con.Close();
                        objetoOrdenBancariaMO.Codigo = _reader != null ? objetoOrdenBancariaMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoOrdenBancariaMO.Mensaje = _reader != null ? objetoOrdenBancariaMO.Mensaje: Constante.MENSAJE_RECARGAR_PAGINA;
                        objetoOrdenBancariaMO.ListaOrdenesBancarias = listaOrdenesBancarias;
                        objetoOrdenBancariaMO.TotalRegistros = par7.Value == null ? 0 : Convert.ToInt32(par7.Value);
                        String mensaje = _reader != null ? Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_LIBERADAS_ASYNC_OK : Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_LIBERADAS_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_LIBERADAS_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_LIBERADAS_ASYNC, Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_LIBERADAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoOrdenBancariaMO;
        }

        public async Task<ObjetoOrdenBancariaDetalleMO> ListarOrdenesBancariasDetalleLiberadasAsync(CancellationToken cancelToken, String idSociedad, String idSap, String anio, String momentoOrden, String idTipoOrden)
        {
            ObjetoOrdenBancariaDetalleMO objetoOrdenBancariaDetalleMO = new ObjetoOrdenBancariaDetalleMO();
            List<OrdenBancariaDetalleMO> listaOrdenesBancariasDetalle = null;
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_LISTAR_ORDENES_BANCARIAS_DETALLE_LIBERADAS, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        
                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_SOCIEDAD, System.Data.SqlDbType.NChar, Constante._4);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idSociedad;

                        SqlParameter par2 = _cmd.Parameters.Add(Constante.ID_SAP, System.Data.SqlDbType.NVarChar, Constante._10);
                        par2.Direction = System.Data.ParameterDirection.Input;
                        par2.Value = idSap;

                        SqlParameter par3 = _cmd.Parameters.Add(Constante.ANIO, System.Data.SqlDbType.NChar, Constante._4);
                        par3.Direction = System.Data.ParameterDirection.Input;
                        par3.Value = anio;

                        SqlParameter par4 = _cmd.Parameters.Add(Constante.MOMENTO_ORDEN, System.Data.SqlDbType.NChar, Constante._8);
                        par4.Direction = System.Data.ParameterDirection.Input;
                        par4.Value = momentoOrden;

                        SqlParameter par5 = _cmd.Parameters.Add(Constante.ID_TIPO_ORDEN, System.Data.SqlDbType.NVarChar, Constante._5);
                        par5.Direction = System.Data.ParameterDirection.Input;
                        par5.Value = idTipoOrden;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult, cancelToken);

                        if (_reader != null && !_reader.HasRows)
                        {
                            objetoOrdenBancariaDetalleMO.Codigo = Constante.CODIGO_OMISION;
                            objetoOrdenBancariaDetalleMO.Mensaje = Constante.MENSAJE_SIN_RESULTADOS;
                        }
                        else if (_reader != null && _reader.HasRows)
                        {
                            listaOrdenesBancariasDetalle = new List<OrdenBancariaDetalleMO>();

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                OrdenBancariaDetalleMO ordenBancariaDetalleMO = new OrdenBancariaDetalleMO();
                                ordenBancariaDetalleMO.IdOrdenBancariaDetalle = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                ordenBancariaDetalleMO.TipoTransferencia = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                ordenBancariaDetalleMO.FormaPago = await _reader.IsDBNullAsync(Constante._2, cancelToken) ? String.Empty : _reader.GetString(Constante._2);
                                ordenBancariaDetalleMO.SubTipoPago = await _reader.IsDBNullAsync(Constante._3, cancelToken) ? String.Empty : _reader.GetString(Constante._3);
                                ordenBancariaDetalleMO.Referencia1 = await _reader.IsDBNullAsync(Constante._4, cancelToken) ? String.Empty : _reader.GetString(Constante._4);
                                ordenBancariaDetalleMO.Referencia2 = await _reader.IsDBNullAsync(Constante._5, cancelToken) ? String.Empty : _reader.GetString(Constante._5);
                                ordenBancariaDetalleMO.MonedaCargo = await _reader.IsDBNullAsync(Constante._6, cancelToken) ? String.Empty : _reader.GetString(Constante._6);
                                ordenBancariaDetalleMO.MonedaCargoCorto = await _reader.IsDBNullAsync(Constante._7, cancelToken) ? String.Empty : _reader.GetString(Constante._7);
                                ordenBancariaDetalleMO.CuentaCargo = await _reader.IsDBNullAsync(Constante._8, cancelToken) ? String.Empty : _reader.GetString(Constante._8);
                                ordenBancariaDetalleMO.MontoCargo = await _reader.IsDBNullAsync(Constante._9, cancelToken) ? 0 : _reader.GetDecimal(Constante._9);
                                ordenBancariaDetalleMO.MonedaAbono = await _reader.IsDBNullAsync(Constante._10, cancelToken) ? String.Empty : _reader.GetString(Constante._10);
                                ordenBancariaDetalleMO.MonedaAbonoCorto = await _reader.IsDBNullAsync(Constante._11, cancelToken) ? String.Empty : _reader.GetString(Constante._11);
                                ordenBancariaDetalleMO.CuentaAbono = await _reader.IsDBNullAsync(Constante._12, cancelToken) ? String.Empty : _reader.GetString(Constante._12);
                                ordenBancariaDetalleMO.CuentaGasto = await _reader.IsDBNullAsync(Constante._13, cancelToken) ? String.Empty : _reader.GetString(Constante._13);
                                ordenBancariaDetalleMO.MontoAbono = await _reader.IsDBNullAsync(Constante._14, cancelToken) ? 0 : _reader.GetDecimal(Constante._14);
                                ordenBancariaDetalleMO.TipoCambio = await _reader.IsDBNullAsync(Constante._15, cancelToken) ? 0 : _reader.GetDecimal(Constante._15);
                                ordenBancariaDetalleMO.ModuloRaiz = await _reader.IsDBNullAsync(Constante._16, cancelToken) ? 0 : _reader.GetInt32(Constante._16);
                                ordenBancariaDetalleMO.DigitoControl = await _reader.IsDBNullAsync(Constante._17, cancelToken) ? 0 : _reader.GetInt32(Constante._17);
                                ordenBancariaDetalleMO.Indicador = await _reader.IsDBNullAsync(Constante._18, cancelToken) ? String.Empty : _reader.GetString(Constante._18);
                                ordenBancariaDetalleMO.NroOperacion = await _reader.IsDBNullAsync(Constante._19, cancelToken) ? String.Empty : _reader.GetString(Constante._19);
                                ordenBancariaDetalleMO.Beneficiario = await _reader.IsDBNullAsync(Constante._20, cancelToken) ? String.Empty : _reader.GetString(Constante._20);
                                ordenBancariaDetalleMO.TipoDocumento = await _reader.IsDBNullAsync(Constante._21, cancelToken) ? String.Empty : _reader.GetString(Constante._21);
                                ordenBancariaDetalleMO.TipoDocumentoCorto = await _reader.IsDBNullAsync(Constante._22, cancelToken) ? String.Empty : _reader.GetString(Constante._22);
                                ordenBancariaDetalleMO.NroDocumento = await _reader.IsDBNullAsync(Constante._23, cancelToken) ? String.Empty : _reader.GetString(Constante._23);
                                ordenBancariaDetalleMO.Correo = await _reader.IsDBNullAsync(Constante._24, cancelToken) ? String.Empty : _reader.GetString(Constante._24);
                                ordenBancariaDetalleMO.NombreBanco = await _reader.IsDBNullAsync(Constante._25, cancelToken) ? String.Empty : _reader.GetString(Constante._25);
                                ordenBancariaDetalleMO.RucBanco = await _reader.IsDBNullAsync(Constante._26, cancelToken) ? String.Empty : _reader.GetString(Constante._26);
                                ordenBancariaDetalleMO.NroFactura = await _reader.IsDBNullAsync(Constante._27, cancelToken) ? String.Empty : _reader.GetString(Constante._27);
                                ordenBancariaDetalleMO.FechaFactura = await _reader.IsDBNullAsync(Constante._28, cancelToken) ? String.Empty : _reader.GetString(Constante._28);
                                ordenBancariaDetalleMO.FechaFinFactura = await _reader.IsDBNullAsync(Constante._29, cancelToken) ? String.Empty : _reader.GetString(Constante._29);
                                ordenBancariaDetalleMO.SignoFactura = await _reader.IsDBNullAsync(Constante._30, cancelToken) ? String.Empty : _reader.GetString(Constante._30);
                                ordenBancariaDetalleMO.UsuarioCreacion = await _reader.IsDBNullAsync(Constante._31, cancelToken) ? String.Empty : _reader.GetString(Constante._31);
                                DateTime? fechaCreacion = await _reader.IsDBNullAsync(Constante._32, cancelToken) ? (DateTime?)null : _reader.GetDateTime(Constante._32);
                                ordenBancariaDetalleMO.FechaCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortDateString();
                                ordenBancariaDetalleMO.HoraCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortTimeString();
                                listaOrdenesBancariasDetalle.Add(ordenBancariaDetalleMO);
                            }

                            objetoOrdenBancariaDetalleMO.Codigo = Constante.CODIGO_OK;
                        }

                        _reader.Close();
                        _con.Close();
                        objetoOrdenBancariaDetalleMO.Codigo = _reader != null ? objetoOrdenBancariaDetalleMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoOrdenBancariaDetalleMO.Mensaje = _reader != null ? objetoOrdenBancariaDetalleMO.Mensaje: Constante.MENSAJE_RECARGAR_PAGINA;
                        objetoOrdenBancariaDetalleMO.ListaOrdenesBancariasDetalle = listaOrdenesBancariasDetalle;
                        String mensaje = _reader != null ? Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_LIBERADAS_ASYNC_OK : Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_LIBERADAS_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DETALLE_LIBERADAS_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DETALLE_LIBERADAS_ASYNC, Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_LIBERADAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoOrdenBancariaDetalleMO;
        }

        public async Task<ObjetoOrdenBancariaMO> BuscarOrdenesBancariasLiberadasAsync(CancellationToken cancelToken, String idUsuario, String idSap, String fechaInicio, String fechaFin)
        {
            ObjetoOrdenBancariaMO objetoOrdenBancariaMO = new ObjetoOrdenBancariaMO();
            List<OrdenBancariaMO> listaOrdenesBancarias = null;
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_BUSCAR_ORDENES_BANCARIAS_LIBERADAS, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        
                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_USUARIO, System.Data.SqlDbType.NChar, Constante._6);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idUsuario;

                        SqlParameter par2 = _cmd.Parameters.Add(Constante.ID_SAP, System.Data.SqlDbType.NVarChar, Constante._10);
                        par2.Direction = System.Data.ParameterDirection.Input;
                        par2.Value = idSap;

                        SqlParameter par3 = _cmd.Parameters.Add(Constante.FECHA_INICIO, System.Data.SqlDbType.NChar, Constante._10);
                        par3.Direction = System.Data.ParameterDirection.Input;
                        par3.Value = fechaInicio;

                        SqlParameter par4 = _cmd.Parameters.Add(Constante.FECHA_FIN, System.Data.SqlDbType.NChar, Constante._10);
                        par4.Direction = System.Data.ParameterDirection.Input;
                        par4.Value = fechaFin;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult, cancelToken);

                        if (_reader != null && !_reader.HasRows)
                        {
                            objetoOrdenBancariaMO.Codigo = Constante.CODIGO_OMISION;
                            objetoOrdenBancariaMO.Mensaje = Constante.MENSAJE_SIN_RESULTADOS;
                        }
                        else if (_reader != null && _reader.HasRows)
                        {
                            listaOrdenesBancarias = new List<OrdenBancariaMO>();

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                OrdenBancariaMO ordenBancariaMO = new OrdenBancariaMO();
                                ordenBancariaMO.IdOrdenBancaria = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                ordenBancariaMO.Banco = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                ordenBancariaMO.BancoCorto = await _reader.IsDBNullAsync(Constante._2, cancelToken) ? String.Empty : _reader.GetString(Constante._2);
                                ordenBancariaMO.IdTipoOrden = await _reader.IsDBNullAsync(Constante._3, cancelToken) ? String.Empty : _reader.GetString(Constante._3);
                                ordenBancariaMO.TipoOrden = await _reader.IsDBNullAsync(Constante._4, cancelToken) ? String.Empty : _reader.GetString(Constante._4);
                                ordenBancariaMO.TipoOrdenCorto = await _reader.IsDBNullAsync(Constante._5, cancelToken) ? String.Empty : _reader.GetString(Constante._5);
                                ordenBancariaMO.IdEstadoOrden = await _reader.IsDBNullAsync(Constante._6, cancelToken) ? String.Empty : _reader.GetString(Constante._6);
                                ordenBancariaMO.EstadoOrden = await _reader.IsDBNullAsync(Constante._7, cancelToken) ? String.Empty : _reader.GetString(Constante._7);
                                ordenBancariaMO.IdSociedad = await _reader.IsDBNullAsync(Constante._8, cancelToken) ? String.Empty : _reader.GetString(Constante._8);
                                ordenBancariaMO.Sociedad = await _reader.IsDBNullAsync(Constante._9, cancelToken) ? String.Empty : _reader.GetString(Constante._9);
                                ordenBancariaMO.SociedadCorto = await _reader.IsDBNullAsync(Constante._10, cancelToken) ? String.Empty : _reader.GetString(Constante._10);
                                ordenBancariaMO.IdSap = await _reader.IsDBNullAsync(Constante._11, cancelToken) ? String.Empty : _reader.GetString(Constante._11);
                                ordenBancariaMO.Anio = await _reader.IsDBNullAsync(Constante._12, cancelToken) ? String.Empty : _reader.GetString(Constante._12);
                                ordenBancariaMO.MomentoOrden = await _reader.IsDBNullAsync(Constante._13, cancelToken) ? String.Empty : _reader.GetString(Constante._13);
                                ordenBancariaMO.FechaOrden = await _reader.IsDBNullAsync(Constante._14, cancelToken) ? String.Empty : _reader.GetString(Constante._14);
                                ordenBancariaMO.NombreArchivo = await _reader.IsDBNullAsync(Constante._15, cancelToken) ? String.Empty : _reader.GetString(Constante._15);
                                ordenBancariaMO.RutaArchivo = await _reader.IsDBNullAsync(Constante._16, cancelToken) ? String.Empty : _reader.GetString(Constante._16);
                                ordenBancariaMO.MonedaLocal = await _reader.IsDBNullAsync(Constante._17, cancelToken) ? String.Empty : _reader.GetString(Constante._17);
                                ordenBancariaMO.MonedaCortoLocal = await _reader.IsDBNullAsync(Constante._18, cancelToken) ? String.Empty : _reader.GetString(Constante._18);
                                ordenBancariaMO.ImporteLocal = await _reader.IsDBNullAsync(Constante._19, cancelToken) ? 0 : _reader.GetDecimal(Constante._19);
                                ordenBancariaMO.MonedaForanea = await _reader.IsDBNullAsync(Constante._20, cancelToken) ? String.Empty : _reader.GetString(Constante._20);
                                ordenBancariaMO.MonedaCortoForanea = await _reader.IsDBNullAsync(Constante._21, cancelToken) ? String.Empty : _reader.GetString(Constante._21);
                                ordenBancariaMO.ImporteForanea = await _reader.IsDBNullAsync(Constante._22, cancelToken) ? 0 : _reader.GetDecimal(Constante._22);
                                ordenBancariaMO.Liberador = await _reader.IsDBNullAsync(Constante._23, cancelToken) ? String.Empty : _reader.GetString(Constante._23);
                                ordenBancariaMO.UsuarioCreacion = await _reader.IsDBNullAsync(Constante._24, cancelToken) ? String.Empty : _reader.GetString(Constante._24);
                                DateTime? fechaCreacion = await _reader.IsDBNullAsync(Constante._25, cancelToken) ? (DateTime?)null : _reader.GetDateTime(Constante._25);
                                ordenBancariaMO.FechaCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortDateString();
                                ordenBancariaMO.HoraCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortTimeString();
                                listaOrdenesBancarias.Add(ordenBancariaMO);
                            }

                            objetoOrdenBancariaMO.Codigo = Constante.CODIGO_OK;
                        }

                        _reader.Close();
                        _con.Close();
                        objetoOrdenBancariaMO.Codigo = _reader != null ? objetoOrdenBancariaMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoOrdenBancariaMO.Mensaje = _reader != null ? objetoOrdenBancariaMO.Mensaje: Constante.MENSAJE_RECARGAR_PAGINA;
                        objetoOrdenBancariaMO.ListaOrdenesBancarias = listaOrdenesBancarias;
                        String mensaje = _reader != null ? Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_LIBERADAS_ASYNC_OK : Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_LIBERADAS_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_BUSCAR_ORDENES_BANCARIAS_LIBERADAS_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_BUSCAR_ORDENES_BANCARIAS_LIBERADAS_ASYNC, Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_LIBERADAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoOrdenBancariaMO;
        }

        public async Task<ObjetoOrdenBancariaMO> ListarOrdenesBancariasDeshechasAsync(CancellationToken cancelToken, String idUsuario, Int32 pagina, Int32 filas, String idSociedad, String idBanco, String idTipoOrden)
        {
            ObjetoOrdenBancariaMO objetoOrdenBancariaMO = new ObjetoOrdenBancariaMO();
            List<OrdenBancariaMO> listaOrdenesBancarias = null;
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_LISTAR_ORDENES_BANCARIAS_DESHECHAS, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        
                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_USUARIO, System.Data.SqlDbType.NChar, Constante._6);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idUsuario;

                        SqlParameter par2 = _cmd.Parameters.Add(Constante.PAGINA, System.Data.SqlDbType.Int);
                        par2.Direction = System.Data.ParameterDirection.Input;
                        par2.Value = pagina;

                        SqlParameter par3 = _cmd.Parameters.Add(Constante.FILAS, System.Data.SqlDbType.Int);
                        par3.Direction = System.Data.ParameterDirection.Input;
                        par3.Value = filas;

                        SqlParameter par4 = _cmd.Parameters.Add(Constante.ID_SOCIEDAD, System.Data.SqlDbType.NChar, 4);
                        par4.Direction = System.Data.ParameterDirection.Input;
                        par4.Value = idSociedad;

                        SqlParameter par5 = _cmd.Parameters.Add(Constante.ID_BANCO, System.Data.SqlDbType.NChar, 5);
                        par5.Direction = System.Data.ParameterDirection.Input;
                        par5.Value = idBanco;

                        SqlParameter par6 = _cmd.Parameters.Add(Constante.ID_TIPO_ORDEN, System.Data.SqlDbType.NVarChar, 5);
                        par6.Direction = System.Data.ParameterDirection.Input;
                        par6.Value = idTipoOrden;

                        SqlParameter par7 = _cmd.Parameters.Add(Constante.TOTAL_REGISTROS, System.Data.SqlDbType.Int);
                        par7.Direction = System.Data.ParameterDirection.Output;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult, cancelToken);

                        if (_reader != null && !_reader.HasRows)
                        {
                            objetoOrdenBancariaMO.Codigo = Constante.CODIGO_OMISION;
                            objetoOrdenBancariaMO.Mensaje = Constante.MENSAJE_SIN_RESULTADOS;
                        }
                        else if (_reader != null && _reader.HasRows)
                        {
                            listaOrdenesBancarias = new List<OrdenBancariaMO>();

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                OrdenBancariaMO ordenBancariaMO = new OrdenBancariaMO();
                                ordenBancariaMO.IdOrdenBancaria = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                ordenBancariaMO.Banco = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                ordenBancariaMO.BancoCorto = await _reader.IsDBNullAsync(Constante._2, cancelToken) ? String.Empty : _reader.GetString(Constante._2);
                                ordenBancariaMO.IdTipoOrden = await _reader.IsDBNullAsync(Constante._3, cancelToken) ? String.Empty : _reader.GetString(Constante._3);
                                ordenBancariaMO.TipoOrden = await _reader.IsDBNullAsync(Constante._4, cancelToken) ? String.Empty : _reader.GetString(Constante._4);
                                ordenBancariaMO.TipoOrdenCorto = await _reader.IsDBNullAsync(Constante._5, cancelToken) ? String.Empty : _reader.GetString(Constante._5);
                                ordenBancariaMO.IdEstadoOrden = await _reader.IsDBNullAsync(Constante._6, cancelToken) ? String.Empty : _reader.GetString(Constante._6);
                                ordenBancariaMO.EstadoOrden = await _reader.IsDBNullAsync(Constante._7, cancelToken) ? String.Empty : _reader.GetString(Constante._7);
                                ordenBancariaMO.IdSociedad = await _reader.IsDBNullAsync(Constante._8, cancelToken) ? String.Empty : _reader.GetString(Constante._8);
                                ordenBancariaMO.Sociedad = await _reader.IsDBNullAsync(Constante._9, cancelToken) ? String.Empty : _reader.GetString(Constante._9);
                                ordenBancariaMO.SociedadCorto = await _reader.IsDBNullAsync(Constante._10, cancelToken) ? String.Empty : _reader.GetString(Constante._10);
                                ordenBancariaMO.IdSap = await _reader.IsDBNullAsync(Constante._11, cancelToken) ? String.Empty : _reader.GetString(Constante._11);
                                ordenBancariaMO.Anio = await _reader.IsDBNullAsync(Constante._12, cancelToken) ? String.Empty : _reader.GetString(Constante._12);
                                ordenBancariaMO.MomentoOrden = await _reader.IsDBNullAsync(Constante._13, cancelToken) ? String.Empty : _reader.GetString(Constante._13);
                                ordenBancariaMO.FechaOrden = await _reader.IsDBNullAsync(Constante._14, cancelToken) ? String.Empty : _reader.GetString(Constante._14);
                                ordenBancariaMO.NombreArchivo = await _reader.IsDBNullAsync(Constante._15, cancelToken) ? String.Empty : _reader.GetString(Constante._15);
                                ordenBancariaMO.RutaArchivo = await _reader.IsDBNullAsync(Constante._16, cancelToken) ? String.Empty : _reader.GetString(Constante._16);
                                ordenBancariaMO.MonedaLocal = await _reader.IsDBNullAsync(Constante._17, cancelToken) ? String.Empty : _reader.GetString(Constante._17);
                                ordenBancariaMO.MonedaCortoLocal = await _reader.IsDBNullAsync(Constante._18, cancelToken) ? String.Empty : _reader.GetString(Constante._18);
                                ordenBancariaMO.ImporteLocal = await _reader.IsDBNullAsync(Constante._19, cancelToken) ? 0 : _reader.GetDecimal(Constante._19);
                                ordenBancariaMO.MonedaForanea = await _reader.IsDBNullAsync(Constante._20, cancelToken) ? String.Empty : _reader.GetString(Constante._20);
                                ordenBancariaMO.MonedaCortoForanea = await _reader.IsDBNullAsync(Constante._21, cancelToken) ? String.Empty : _reader.GetString(Constante._21);
                                ordenBancariaMO.ImporteForanea = await _reader.IsDBNullAsync(Constante._22, cancelToken) ? 0 : _reader.GetDecimal(Constante._22);
                                ordenBancariaMO.Liberador = await _reader.IsDBNullAsync(Constante._23, cancelToken) ? String.Empty : _reader.GetString(Constante._23);
                                ordenBancariaMO.PreAprobador = await _reader.IsDBNullAsync(Constante._24, cancelToken) ? String.Empty : _reader.GetString(Constante._24);
                                ordenBancariaMO.Anulador = await _reader.IsDBNullAsync(Constante._25, cancelToken) ? String.Empty : _reader.GetString(Constante._25);
                                ordenBancariaMO.Comentarios = await _reader.IsDBNullAsync(Constante._26, cancelToken) ? String.Empty : _reader.GetString(Constante._26);
                                ordenBancariaMO.UsuarioCreacion = await _reader.IsDBNullAsync(Constante._27, cancelToken) ? String.Empty : _reader.GetString(Constante._27);
                                DateTime? fechaCreacion = await _reader.IsDBNullAsync(Constante._28, cancelToken) ? (DateTime?)null : _reader.GetDateTime(Constante._28);
                                ordenBancariaMO.FechaCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortDateString();
                                ordenBancariaMO.HoraCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortTimeString();
                                ordenBancariaMO.UsuarioEdicion = await _reader.IsDBNullAsync(Constante._29, cancelToken) ? String.Empty : _reader.GetString(Constante._29);
                                DateTime? fechaEdicion = await _reader.IsDBNullAsync(Constante._30, cancelToken) ? (DateTime?)null : _reader.GetDateTime(Constante._30);
                                ordenBancariaMO.FechaEdicion = fechaEdicion == null ? String.Empty : fechaEdicion.Value.ToShortDateString();
                                ordenBancariaMO.HoraEdicion = fechaEdicion == null ? String.Empty : fechaEdicion.Value.ToShortTimeString();
                                listaOrdenesBancarias.Add(ordenBancariaMO);
                            }

                            objetoOrdenBancariaMO.Codigo = Constante.CODIGO_OK;
                        }

                        _reader.Close();
                        _con.Close();
                        objetoOrdenBancariaMO.Codigo = _reader != null ? objetoOrdenBancariaMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoOrdenBancariaMO.Mensaje = _reader != null ? objetoOrdenBancariaMO.Mensaje: Constante.MENSAJE_RECARGAR_PAGINA;
                        objetoOrdenBancariaMO.ListaOrdenesBancarias = listaOrdenesBancarias;
                        objetoOrdenBancariaMO.TotalRegistros = par7.Value == null ? 0 : Convert.ToInt32(par7.Value);
                        String mensaje = _reader != null ? Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DESHECHAS_ASYNC_OK : Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DESHECHAS_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DESHECHAS_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DESHECHAS_ASYNC, Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DESHECHAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoOrdenBancariaMO;
        }

        public async Task<ObjetoOrdenBancariaDetalleMO> ListarOrdenesBancariasDetalleDeshechasAsync(CancellationToken cancelToken, String idSociedad, String idSap, String anio, String momentoOrden, String idTipoOrden)
        {
            ObjetoOrdenBancariaDetalleMO objetoOrdenBancariaDetalleMO = new ObjetoOrdenBancariaDetalleMO();
            List<OrdenBancariaDetalleMO> listaOrdenesBancariasDetalle = null;
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_LISTAR_ORDENES_BANCARIAS_DETALLE_DESHECHAS, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        
                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_SOCIEDAD, System.Data.SqlDbType.NChar, Constante._4);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idSociedad;

                        SqlParameter par2 = _cmd.Parameters.Add(Constante.ID_SAP, System.Data.SqlDbType.NVarChar, Constante._10);
                        par2.Direction = System.Data.ParameterDirection.Input;
                        par2.Value = idSap;

                        SqlParameter par3 = _cmd.Parameters.Add(Constante.ANIO, System.Data.SqlDbType.NChar, Constante._4);
                        par3.Direction = System.Data.ParameterDirection.Input;
                        par3.Value = anio;

                        SqlParameter par4 = _cmd.Parameters.Add(Constante.MOMENTO_ORDEN, System.Data.SqlDbType.NChar, Constante._8);
                        par4.Direction = System.Data.ParameterDirection.Input;
                        par4.Value = momentoOrden;

                        SqlParameter par5 = _cmd.Parameters.Add(Constante.ID_TIPO_ORDEN, System.Data.SqlDbType.NVarChar, Constante._5);
                        par5.Direction = System.Data.ParameterDirection.Input;
                        par5.Value = idTipoOrden;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult, cancelToken);

                        if (_reader != null && !_reader.HasRows)
                        {
                            objetoOrdenBancariaDetalleMO.Codigo = Constante.CODIGO_OMISION;
                            objetoOrdenBancariaDetalleMO.Mensaje = Constante.MENSAJE_SIN_RESULTADOS;
                        }
                        else if (_reader != null && _reader.HasRows)
                        {
                            listaOrdenesBancariasDetalle = new List<OrdenBancariaDetalleMO>();

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                OrdenBancariaDetalleMO ordenBancariaDetalleMO = new OrdenBancariaDetalleMO();
                                ordenBancariaDetalleMO.IdOrdenBancariaDetalle = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                ordenBancariaDetalleMO.TipoTransferencia = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                ordenBancariaDetalleMO.FormaPago = await _reader.IsDBNullAsync(Constante._2, cancelToken) ? String.Empty : _reader.GetString(Constante._2);
                                ordenBancariaDetalleMO.SubTipoPago = await _reader.IsDBNullAsync(Constante._3, cancelToken) ? String.Empty : _reader.GetString(Constante._3);
                                ordenBancariaDetalleMO.Referencia1 = await _reader.IsDBNullAsync(Constante._4, cancelToken) ? String.Empty : _reader.GetString(Constante._4);
                                ordenBancariaDetalleMO.Referencia2 = await _reader.IsDBNullAsync(Constante._5, cancelToken) ? String.Empty : _reader.GetString(Constante._5);
                                ordenBancariaDetalleMO.MonedaCargo = await _reader.IsDBNullAsync(Constante._6, cancelToken) ? String.Empty : _reader.GetString(Constante._6);
                                ordenBancariaDetalleMO.MonedaCargoCorto = await _reader.IsDBNullAsync(Constante._7, cancelToken) ? String.Empty : _reader.GetString(Constante._7);
                                ordenBancariaDetalleMO.CuentaCargo = await _reader.IsDBNullAsync(Constante._8, cancelToken) ? String.Empty : _reader.GetString(Constante._8);
                                ordenBancariaDetalleMO.MontoCargo = await _reader.IsDBNullAsync(Constante._9, cancelToken) ? 0 : _reader.GetDecimal(Constante._9);
                                ordenBancariaDetalleMO.MonedaAbono = await _reader.IsDBNullAsync(Constante._10, cancelToken) ? String.Empty : _reader.GetString(Constante._10);
                                ordenBancariaDetalleMO.MonedaAbonoCorto = await _reader.IsDBNullAsync(Constante._11, cancelToken) ? String.Empty : _reader.GetString(Constante._11);
                                ordenBancariaDetalleMO.CuentaAbono = await _reader.IsDBNullAsync(Constante._12, cancelToken) ? String.Empty : _reader.GetString(Constante._12);
                                ordenBancariaDetalleMO.CuentaGasto = await _reader.IsDBNullAsync(Constante._13, cancelToken) ? String.Empty : _reader.GetString(Constante._13);
                                ordenBancariaDetalleMO.MontoAbono = await _reader.IsDBNullAsync(Constante._14, cancelToken) ? 0 : _reader.GetDecimal(Constante._14);
                                ordenBancariaDetalleMO.TipoCambio = await _reader.IsDBNullAsync(Constante._15, cancelToken) ? 0 : _reader.GetDecimal(Constante._15);
                                ordenBancariaDetalleMO.ModuloRaiz = await _reader.IsDBNullAsync(Constante._16, cancelToken) ? 0 : _reader.GetInt32(Constante._16);
                                ordenBancariaDetalleMO.DigitoControl = await _reader.IsDBNullAsync(Constante._17, cancelToken) ? 0 : _reader.GetInt32(Constante._17);
                                ordenBancariaDetalleMO.Indicador = await _reader.IsDBNullAsync(Constante._18, cancelToken) ? String.Empty : _reader.GetString(Constante._18);
                                ordenBancariaDetalleMO.NroOperacion = await _reader.IsDBNullAsync(Constante._19, cancelToken) ? String.Empty : _reader.GetString(Constante._19);
                                ordenBancariaDetalleMO.Beneficiario = await _reader.IsDBNullAsync(Constante._20, cancelToken) ? String.Empty : _reader.GetString(Constante._20);
                                ordenBancariaDetalleMO.TipoDocumento = await _reader.IsDBNullAsync(Constante._21, cancelToken) ? String.Empty : _reader.GetString(Constante._21);
                                ordenBancariaDetalleMO.TipoDocumentoCorto = await _reader.IsDBNullAsync(Constante._22, cancelToken) ? String.Empty : _reader.GetString(Constante._22);
                                ordenBancariaDetalleMO.NroDocumento = await _reader.IsDBNullAsync(Constante._23, cancelToken) ? String.Empty : _reader.GetString(Constante._23);
                                ordenBancariaDetalleMO.Correo = await _reader.IsDBNullAsync(Constante._24, cancelToken) ? String.Empty : _reader.GetString(Constante._24);
                                ordenBancariaDetalleMO.NombreBanco = await _reader.IsDBNullAsync(Constante._25, cancelToken) ? String.Empty : _reader.GetString(Constante._25);
                                ordenBancariaDetalleMO.RucBanco = await _reader.IsDBNullAsync(Constante._26, cancelToken) ? String.Empty : _reader.GetString(Constante._26);
                                ordenBancariaDetalleMO.NroFactura = await _reader.IsDBNullAsync(Constante._27, cancelToken) ? String.Empty : _reader.GetString(Constante._27);
                                ordenBancariaDetalleMO.FechaFactura = await _reader.IsDBNullAsync(Constante._28, cancelToken) ? String.Empty : _reader.GetString(Constante._28);
                                ordenBancariaDetalleMO.FechaFinFactura = await _reader.IsDBNullAsync(Constante._29, cancelToken) ? String.Empty : _reader.GetString(Constante._29);
                                ordenBancariaDetalleMO.SignoFactura = await _reader.IsDBNullAsync(Constante._30, cancelToken) ? String.Empty : _reader.GetString(Constante._30);
                                ordenBancariaDetalleMO.UsuarioCreacion = await _reader.IsDBNullAsync(Constante._31, cancelToken) ? String.Empty : _reader.GetString(Constante._31);
                                DateTime? fechaCreacion = await _reader.IsDBNullAsync(Constante._32, cancelToken) ? (DateTime?)null : _reader.GetDateTime(Constante._32);
                                ordenBancariaDetalleMO.FechaCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortDateString();
                                ordenBancariaDetalleMO.HoraCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortTimeString();
                                listaOrdenesBancariasDetalle.Add(ordenBancariaDetalleMO);
                            }

                            objetoOrdenBancariaDetalleMO.Codigo = Constante.CODIGO_OK;
                        }

                        _reader.Close();
                        _con.Close();
                        objetoOrdenBancariaDetalleMO.Codigo = _reader != null ? objetoOrdenBancariaDetalleMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoOrdenBancariaDetalleMO.Mensaje = _reader != null ? objetoOrdenBancariaDetalleMO.Mensaje: Constante.MENSAJE_RECARGAR_PAGINA;
                        objetoOrdenBancariaDetalleMO.ListaOrdenesBancariasDetalle = listaOrdenesBancariasDetalle;
                        String mensaje = _reader != null ? Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_DESHECHAS_ASYNC_OK : Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_DESHECHAS_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DETALLE_DESHECHAS_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DETALLE_DESHECHAS_ASYNC, Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_DESHECHAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoOrdenBancariaDetalleMO;
        }

        public async Task<ObjetoOrdenBancariaMO> BuscarOrdenesBancariasDeshechasAsync(CancellationToken cancelToken, String idUsuario, String idSap, String fechaInicio, String fechaFin)
        {
            ObjetoOrdenBancariaMO objetoOrdenBancariaMO = new ObjetoOrdenBancariaMO();
            List<OrdenBancariaMO> listaOrdenesBancarias = null;
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_BUSCAR_ORDENES_BANCARIAS_DESHECHAS, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        
                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_USUARIO, System.Data.SqlDbType.NChar, Constante._6);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idUsuario;

                        SqlParameter par2 = _cmd.Parameters.Add(Constante.ID_SAP, System.Data.SqlDbType.NVarChar, Constante._10);
                        par2.Direction = System.Data.ParameterDirection.Input;
                        par2.Value = idSap;

                        SqlParameter par3 = _cmd.Parameters.Add(Constante.FECHA_INICIO, System.Data.SqlDbType.NChar, Constante._10);
                        par3.Direction = System.Data.ParameterDirection.Input;
                        par3.Value = fechaInicio;

                        SqlParameter par4 = _cmd.Parameters.Add(Constante.FECHA_FIN, System.Data.SqlDbType.NChar, Constante._10);
                        par4.Direction = System.Data.ParameterDirection.Input;
                        par4.Value = fechaFin;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult, cancelToken);

                        if (_reader != null && !_reader.HasRows)
                        {
                            objetoOrdenBancariaMO.Codigo = Constante.CODIGO_OMISION;
                            objetoOrdenBancariaMO.Mensaje = Constante.MENSAJE_SIN_RESULTADOS;
                        }
                        else if (_reader != null && _reader.HasRows)
                        {
                            listaOrdenesBancarias = new List<OrdenBancariaMO>();

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                OrdenBancariaMO ordenBancariaMO = new OrdenBancariaMO();
                                ordenBancariaMO.IdOrdenBancaria = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                ordenBancariaMO.Banco = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                ordenBancariaMO.BancoCorto = await _reader.IsDBNullAsync(Constante._2, cancelToken) ? String.Empty : _reader.GetString(Constante._2);
                                ordenBancariaMO.IdTipoOrden = await _reader.IsDBNullAsync(Constante._3, cancelToken) ? String.Empty : _reader.GetString(Constante._3);
                                ordenBancariaMO.TipoOrden = await _reader.IsDBNullAsync(Constante._4, cancelToken) ? String.Empty : _reader.GetString(Constante._4);
                                ordenBancariaMO.TipoOrdenCorto = await _reader.IsDBNullAsync(Constante._5, cancelToken) ? String.Empty : _reader.GetString(Constante._5);
                                ordenBancariaMO.IdEstadoOrden = await _reader.IsDBNullAsync(Constante._6, cancelToken) ? String.Empty : _reader.GetString(Constante._6);
                                ordenBancariaMO.EstadoOrden = await _reader.IsDBNullAsync(Constante._7, cancelToken) ? String.Empty : _reader.GetString(Constante._7);
                                ordenBancariaMO.IdSociedad = await _reader.IsDBNullAsync(Constante._8, cancelToken) ? String.Empty : _reader.GetString(Constante._8);
                                ordenBancariaMO.Sociedad = await _reader.IsDBNullAsync(Constante._9, cancelToken) ? String.Empty : _reader.GetString(Constante._9);
                                ordenBancariaMO.SociedadCorto = await _reader.IsDBNullAsync(Constante._10, cancelToken) ? String.Empty : _reader.GetString(Constante._10);
                                ordenBancariaMO.IdSap = await _reader.IsDBNullAsync(Constante._11, cancelToken) ? String.Empty : _reader.GetString(Constante._11);
                                ordenBancariaMO.Anio = await _reader.IsDBNullAsync(Constante._12, cancelToken) ? String.Empty : _reader.GetString(Constante._12);
                                ordenBancariaMO.MomentoOrden = await _reader.IsDBNullAsync(Constante._13, cancelToken) ? String.Empty : _reader.GetString(Constante._13);
                                ordenBancariaMO.FechaOrden = await _reader.IsDBNullAsync(Constante._14, cancelToken) ? String.Empty : _reader.GetString(Constante._14);
                                ordenBancariaMO.NombreArchivo = await _reader.IsDBNullAsync(Constante._15, cancelToken) ? String.Empty : _reader.GetString(Constante._15);
                                ordenBancariaMO.RutaArchivo = await _reader.IsDBNullAsync(Constante._16, cancelToken) ? String.Empty : _reader.GetString(Constante._16);
                                ordenBancariaMO.MonedaLocal = await _reader.IsDBNullAsync(Constante._17, cancelToken) ? String.Empty : _reader.GetString(Constante._17);
                                ordenBancariaMO.MonedaCortoLocal = await _reader.IsDBNullAsync(Constante._18, cancelToken) ? String.Empty : _reader.GetString(Constante._18);
                                ordenBancariaMO.ImporteLocal = await _reader.IsDBNullAsync(Constante._19, cancelToken) ? 0 : _reader.GetDecimal(Constante._19);
                                ordenBancariaMO.MonedaForanea = await _reader.IsDBNullAsync(Constante._20, cancelToken) ? String.Empty : _reader.GetString(Constante._20);
                                ordenBancariaMO.MonedaCortoForanea = await _reader.IsDBNullAsync(Constante._21, cancelToken) ? String.Empty : _reader.GetString(Constante._21);
                                ordenBancariaMO.ImporteForanea = await _reader.IsDBNullAsync(Constante._22, cancelToken) ? 0 : _reader.GetDecimal(Constante._22);
                                ordenBancariaMO.Liberador = await _reader.IsDBNullAsync(Constante._23, cancelToken) ? String.Empty : _reader.GetString(Constante._23);
                                ordenBancariaMO.PreAprobador = await _reader.IsDBNullAsync(Constante._24, cancelToken) ? String.Empty : _reader.GetString(Constante._24);
                                ordenBancariaMO.Anulador = await _reader.IsDBNullAsync(Constante._25, cancelToken) ? String.Empty : _reader.GetString(Constante._25);
                                ordenBancariaMO.Comentarios = await _reader.IsDBNullAsync(Constante._26, cancelToken) ? String.Empty : _reader.GetString(Constante._26);
                                ordenBancariaMO.UsuarioCreacion = await _reader.IsDBNullAsync(Constante._27, cancelToken) ? String.Empty : _reader.GetString(Constante._27);
                                DateTime? fechaCreacion = await _reader.IsDBNullAsync(Constante._28, cancelToken) ? (DateTime?)null : _reader.GetDateTime(Constante._28);
                                ordenBancariaMO.FechaCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortDateString();
                                ordenBancariaMO.HoraCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortTimeString();
                                ordenBancariaMO.UsuarioEdicion = await _reader.IsDBNullAsync(Constante._29, cancelToken) ? String.Empty : _reader.GetString(Constante._29);
                                DateTime? fechaEdicion = await _reader.IsDBNullAsync(Constante._30, cancelToken) ? (DateTime?)null : _reader.GetDateTime(Constante._30);
                                ordenBancariaMO.FechaEdicion = fechaEdicion == null ? String.Empty : fechaEdicion.Value.ToShortDateString();
                                ordenBancariaMO.HoraEdicion = fechaEdicion == null ? String.Empty : fechaEdicion.Value.ToShortTimeString();
                                listaOrdenesBancarias.Add(ordenBancariaMO);
                            }

                            objetoOrdenBancariaMO.Codigo = Constante.CODIGO_OK;
                        }

                        _reader.Close();
                        _con.Close();
                        objetoOrdenBancariaMO.Codigo = _reader != null ? objetoOrdenBancariaMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoOrdenBancariaMO.Mensaje = _reader != null ? objetoOrdenBancariaMO.Mensaje: Constante.MENSAJE_RECARGAR_PAGINA;
                        objetoOrdenBancariaMO.ListaOrdenesBancarias = listaOrdenesBancarias;
                        String mensaje = _reader != null ? Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_DESHECHAS_ASYNC_OK : Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_DESHECHAS_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_BUSCAR_ORDENES_BANCARIAS_DESHECHAS_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_BUSCAR_ORDENES_BANCARIAS_DESHECHAS_ASYNC, Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_DESHECHAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoOrdenBancariaMO;
        }

        public async Task<ObjetoOrdenBancariaMO> ListarOrdenesBancariasPorAprobarAsync(CancellationToken cancelToken, String idUsuario, Int32 pagina, Int32 filas, String idSociedad, String idBanco, String idTipoOrden, String idEstadoOrden)
        {
            ObjetoOrdenBancariaMO objetoOrdenBancariaMO = new ObjetoOrdenBancariaMO();
            List<OrdenBancariaMO> listaOrdenesBancarias = null;
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_LISTAR_ORDENES_BANCARIAS_POR_APROBAR, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        
                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_USUARIO, System.Data.SqlDbType.NChar, Constante._6);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idUsuario;
                        
                        SqlParameter par2 = _cmd.Parameters.Add(Constante.PAGINA, System.Data.SqlDbType.Int);
                        par2.Direction = System.Data.ParameterDirection.Input;
                        par2.Value = pagina;

                        SqlParameter par3 = _cmd.Parameters.Add(Constante.FILAS, System.Data.SqlDbType.Int);
                        par3.Direction = System.Data.ParameterDirection.Input;
                        par3.Value = filas;

                        SqlParameter par4 = _cmd.Parameters.Add(Constante.ID_SOCIEDAD, System.Data.SqlDbType.NChar, 4);
                        par4.Direction = System.Data.ParameterDirection.Input;
                        par4.Value = idSociedad;

                        SqlParameter par5 = _cmd.Parameters.Add(Constante.ID_BANCO, System.Data.SqlDbType.NChar, 5);
                        par5.Direction = System.Data.ParameterDirection.Input;
                        par5.Value = idBanco;

                        SqlParameter par6 = _cmd.Parameters.Add(Constante.ID_TIPO_ORDEN, System.Data.SqlDbType.NVarChar, 5);
                        par6.Direction = System.Data.ParameterDirection.Input;
                        par6.Value = idTipoOrden;

                        SqlParameter par7 = _cmd.Parameters.Add(Constante.ID_ESTADO_ORDEN, System.Data.SqlDbType.NChar, 2);
                        par7.Direction = System.Data.ParameterDirection.Input;
                        par7.Value = idEstadoOrden;

                        SqlParameter par8 = _cmd.Parameters.Add(Constante.TOTAL_REGISTROS, System.Data.SqlDbType.Int);
                        par8.Direction = System.Data.ParameterDirection.Output;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult, cancelToken);

                        if (_reader != null && !_reader.HasRows)
                        {
                            objetoOrdenBancariaMO.Codigo = Constante.CODIGO_OMISION;
                            objetoOrdenBancariaMO.Mensaje = Constante.MENSAJE_SIN_RESULTADOS;
                        }
                        else if (_reader != null && _reader.HasRows)
                        {
                            listaOrdenesBancarias = new List<OrdenBancariaMO>();

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                OrdenBancariaMO ordenBancariaMO = new OrdenBancariaMO();
                                ordenBancariaMO.IdOrdenBancaria = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                ordenBancariaMO.Banco = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                ordenBancariaMO.BancoCorto = await _reader.IsDBNullAsync(Constante._2, cancelToken) ? String.Empty : _reader.GetString(Constante._2);
                                ordenBancariaMO.IdTipoOrden = await _reader.IsDBNullAsync(Constante._3, cancelToken) ? String.Empty : _reader.GetString(Constante._3);
                                ordenBancariaMO.TipoOrden = await _reader.IsDBNullAsync(Constante._4, cancelToken) ? String.Empty : _reader.GetString(Constante._4);
                                ordenBancariaMO.TipoOrdenCorto = await _reader.IsDBNullAsync(Constante._5, cancelToken) ? String.Empty : _reader.GetString(Constante._5);
                                ordenBancariaMO.IdEstadoOrden = await _reader.IsDBNullAsync(Constante._6, cancelToken) ? String.Empty : _reader.GetString(Constante._6);
                                ordenBancariaMO.EstadoOrden = await _reader.IsDBNullAsync(Constante._7, cancelToken) ? String.Empty : _reader.GetString(Constante._7);
                                ordenBancariaMO.IdSociedad = await _reader.IsDBNullAsync(Constante._8, cancelToken) ? String.Empty : _reader.GetString(Constante._8);
                                ordenBancariaMO.Sociedad = await _reader.IsDBNullAsync(Constante._9, cancelToken) ? String.Empty : _reader.GetString(Constante._9);
                                ordenBancariaMO.SociedadCorto = await _reader.IsDBNullAsync(Constante._10, cancelToken) ? String.Empty : _reader.GetString(Constante._10);
                                ordenBancariaMO.IdSap = await _reader.IsDBNullAsync(Constante._11, cancelToken) ? String.Empty : _reader.GetString(Constante._11);
                                ordenBancariaMO.Anio = await _reader.IsDBNullAsync(Constante._12, cancelToken) ? String.Empty : _reader.GetString(Constante._12);
                                ordenBancariaMO.MomentoOrden = await _reader.IsDBNullAsync(Constante._13, cancelToken) ? String.Empty : _reader.GetString(Constante._13);
                                ordenBancariaMO.FechaOrden = await _reader.IsDBNullAsync(Constante._14, cancelToken) ? String.Empty : _reader.GetString(Constante._14);
                                ordenBancariaMO.NombreArchivo = await _reader.IsDBNullAsync(Constante._15, cancelToken) ? String.Empty : _reader.GetString(Constante._15);
                                ordenBancariaMO.RutaArchivo = await _reader.IsDBNullAsync(Constante._16, cancelToken) ? String.Empty : _reader.GetString(Constante._16);
                                ordenBancariaMO.MonedaLocal = await _reader.IsDBNullAsync(Constante._17, cancelToken) ? String.Empty : _reader.GetString(Constante._17);
                                ordenBancariaMO.MonedaCortoLocal = await _reader.IsDBNullAsync(Constante._18, cancelToken) ? String.Empty : _reader.GetString(Constante._18);
                                ordenBancariaMO.ImporteLocal = await _reader.IsDBNullAsync(Constante._19, cancelToken) ? 0 : _reader.GetDecimal(Constante._19);
                                ordenBancariaMO.MonedaForanea = await _reader.IsDBNullAsync(Constante._20, cancelToken) ? String.Empty : _reader.GetString(Constante._20);
                                ordenBancariaMO.MonedaCortoForanea = await _reader.IsDBNullAsync(Constante._21, cancelToken) ? String.Empty : _reader.GetString(Constante._21);
                                ordenBancariaMO.ImporteForanea = await _reader.IsDBNullAsync(Constante._22, cancelToken) ? 0 : _reader.GetDecimal(Constante._22);
                                ordenBancariaMO.Liberador = await _reader.IsDBNullAsync(Constante._23, cancelToken) ? String.Empty : _reader.GetString(Constante._23);
                                ordenBancariaMO.PreAprobador = await _reader.IsDBNullAsync(Constante._24, cancelToken) ? String.Empty : _reader.GetString(Constante._24);
                                ordenBancariaMO.UsuarioCreacion = await _reader.IsDBNullAsync(Constante._25, cancelToken) ? String.Empty : _reader.GetString(Constante._25);
                                DateTime? fechaCreacion = await _reader.IsDBNullAsync(Constante._26, cancelToken) ? (DateTime?)null : _reader.GetDateTime(Constante._26);
                                ordenBancariaMO.FechaCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortDateString();
                                ordenBancariaMO.HoraCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortTimeString();
                                listaOrdenesBancarias.Add(ordenBancariaMO);
                            }

                            objetoOrdenBancariaMO.Codigo = Constante.CODIGO_OK;
                        }

                        _reader.Close();
                        _con.Close();
                        objetoOrdenBancariaMO.Codigo = _reader != null ? objetoOrdenBancariaMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoOrdenBancariaMO.Mensaje = _reader != null ? objetoOrdenBancariaMO.Mensaje: Constante.MENSAJE_RECARGAR_PAGINA;
                        objetoOrdenBancariaMO.ListaOrdenesBancarias = listaOrdenesBancarias;
                        objetoOrdenBancariaMO.TotalRegistros = par8.Value == null ? 0 : Convert.ToInt32(par8.Value);
                        String mensaje = _reader != null ? Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_POR_APROBAR_ASYNC_OK : Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_POR_APROBAR_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_POR_APROBAR_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_POR_APROBAR_ASYNC, Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_POR_APROBAR_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoOrdenBancariaMO;
        }

        public async Task<ObjetoOrdenBancariaDetalleMO> ListarOrdenesBancariasDetallePorAprobarAsync(CancellationToken cancelToken, String idSociedad, String idSap, String anio, String momentoOrden, String idTipoOrden)
        {
            ObjetoOrdenBancariaDetalleMO objetoOrdenBancariaDetalleMO = new ObjetoOrdenBancariaDetalleMO();
            List<OrdenBancariaDetalleMO> listaOrdenesBancariasDetalle = null;
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_LISTAR_ORDENES_BANCARIAS_DETALLE_POR_APROBAR, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        
                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_SOCIEDAD, System.Data.SqlDbType.NChar, Constante._4);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idSociedad;

                        SqlParameter par2 = _cmd.Parameters.Add(Constante.ID_SAP, System.Data.SqlDbType.NVarChar, Constante._10);
                        par2.Direction = System.Data.ParameterDirection.Input;
                        par2.Value = idSap;

                        SqlParameter par3 = _cmd.Parameters.Add(Constante.ANIO, System.Data.SqlDbType.NChar, Constante._4);
                        par3.Direction = System.Data.ParameterDirection.Input;
                        par3.Value = anio;

                        SqlParameter par4 = _cmd.Parameters.Add(Constante.MOMENTO_ORDEN, System.Data.SqlDbType.NChar, Constante._8);
                        par4.Direction = System.Data.ParameterDirection.Input;
                        par4.Value = momentoOrden;

                        SqlParameter par5 = _cmd.Parameters.Add(Constante.ID_TIPO_ORDEN, System.Data.SqlDbType.NVarChar, Constante._5);
                        par5.Direction = System.Data.ParameterDirection.Input;
                        par5.Value = idTipoOrden;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult, cancelToken);

                        if (_reader != null && !_reader.HasRows)
                        {
                            objetoOrdenBancariaDetalleMO.Codigo = Constante.CODIGO_OMISION;
                            objetoOrdenBancariaDetalleMO.Mensaje = Constante.MENSAJE_SIN_RESULTADOS;
                        }
                        else if (_reader != null && _reader.HasRows)
                        {
                            listaOrdenesBancariasDetalle = new List<OrdenBancariaDetalleMO>();

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                OrdenBancariaDetalleMO ordenBancariaDetalleMO = new OrdenBancariaDetalleMO();
                                ordenBancariaDetalleMO.IdOrdenBancariaDetalle = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                ordenBancariaDetalleMO.TipoTransferencia = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                ordenBancariaDetalleMO.FormaPago = await _reader.IsDBNullAsync(Constante._2, cancelToken) ? String.Empty : _reader.GetString(Constante._2);
                                ordenBancariaDetalleMO.SubTipoPago = await _reader.IsDBNullAsync(Constante._3, cancelToken) ? String.Empty : _reader.GetString(Constante._3);
                                ordenBancariaDetalleMO.Referencia1 = await _reader.IsDBNullAsync(Constante._4, cancelToken) ? String.Empty : _reader.GetString(Constante._4);
                                ordenBancariaDetalleMO.Referencia2 = await _reader.IsDBNullAsync(Constante._5, cancelToken) ? String.Empty : _reader.GetString(Constante._5);
                                ordenBancariaDetalleMO.MonedaCargo = await _reader.IsDBNullAsync(Constante._6, cancelToken) ? String.Empty : _reader.GetString(Constante._6);
                                ordenBancariaDetalleMO.MonedaCargoCorto = await _reader.IsDBNullAsync(Constante._7, cancelToken) ? String.Empty : _reader.GetString(Constante._7);
                                ordenBancariaDetalleMO.CuentaCargo = await _reader.IsDBNullAsync(Constante._8, cancelToken) ? String.Empty : _reader.GetString(Constante._8);
                                ordenBancariaDetalleMO.MontoCargo = await _reader.IsDBNullAsync(Constante._9, cancelToken) ? 0 : _reader.GetDecimal(Constante._9);
                                ordenBancariaDetalleMO.MonedaAbono = await _reader.IsDBNullAsync(Constante._10, cancelToken) ? String.Empty : _reader.GetString(Constante._10);
                                ordenBancariaDetalleMO.MonedaAbonoCorto = await _reader.IsDBNullAsync(Constante._11, cancelToken) ? String.Empty : _reader.GetString(Constante._11);
                                ordenBancariaDetalleMO.CuentaAbono = await _reader.IsDBNullAsync(Constante._12, cancelToken) ? String.Empty : _reader.GetString(Constante._12);
                                ordenBancariaDetalleMO.CuentaGasto = await _reader.IsDBNullAsync(Constante._13, cancelToken) ? String.Empty : _reader.GetString(Constante._13);
                                ordenBancariaDetalleMO.MontoAbono = await _reader.IsDBNullAsync(Constante._14, cancelToken) ? 0 : _reader.GetDecimal(Constante._14);
                                ordenBancariaDetalleMO.TipoCambio = await _reader.IsDBNullAsync(Constante._15, cancelToken) ? 0 : _reader.GetDecimal(Constante._15);
                                ordenBancariaDetalleMO.ModuloRaiz = await _reader.IsDBNullAsync(Constante._16, cancelToken) ? 0 : _reader.GetInt32(Constante._16);
                                ordenBancariaDetalleMO.DigitoControl = await _reader.IsDBNullAsync(Constante._17, cancelToken) ? 0 : _reader.GetInt32(Constante._17);
                                ordenBancariaDetalleMO.Indicador = await _reader.IsDBNullAsync(Constante._18, cancelToken) ? String.Empty : _reader.GetString(Constante._18);
                                ordenBancariaDetalleMO.NroOperacion = await _reader.IsDBNullAsync(Constante._19, cancelToken) ? String.Empty : _reader.GetString(Constante._19);
                                ordenBancariaDetalleMO.Beneficiario = await _reader.IsDBNullAsync(Constante._20, cancelToken) ? String.Empty : _reader.GetString(Constante._20);
                                ordenBancariaDetalleMO.TipoDocumento = await _reader.IsDBNullAsync(Constante._21, cancelToken) ? String.Empty : _reader.GetString(Constante._21);
                                ordenBancariaDetalleMO.TipoDocumentoCorto = await _reader.IsDBNullAsync(Constante._22, cancelToken) ? String.Empty : _reader.GetString(Constante._22);
                                ordenBancariaDetalleMO.NroDocumento = await _reader.IsDBNullAsync(Constante._23, cancelToken) ? String.Empty : _reader.GetString(Constante._23);
                                ordenBancariaDetalleMO.Correo = await _reader.IsDBNullAsync(Constante._24, cancelToken) ? String.Empty : _reader.GetString(Constante._24);
                                ordenBancariaDetalleMO.NombreBanco = await _reader.IsDBNullAsync(Constante._25, cancelToken) ? String.Empty : _reader.GetString(Constante._25);
                                ordenBancariaDetalleMO.RucBanco = await _reader.IsDBNullAsync(Constante._26, cancelToken) ? String.Empty : _reader.GetString(Constante._26);
                                ordenBancariaDetalleMO.NroFactura = await _reader.IsDBNullAsync(Constante._27, cancelToken) ? String.Empty : _reader.GetString(Constante._27);
                                ordenBancariaDetalleMO.FechaFactura = await _reader.IsDBNullAsync(Constante._28, cancelToken) ? String.Empty : _reader.GetString(Constante._28);
                                ordenBancariaDetalleMO.FechaFinFactura = await _reader.IsDBNullAsync(Constante._29, cancelToken) ? String.Empty : _reader.GetString(Constante._29);
                                ordenBancariaDetalleMO.SignoFactura = await _reader.IsDBNullAsync(Constante._30, cancelToken) ? String.Empty : _reader.GetString(Constante._30);
                                ordenBancariaDetalleMO.UsuarioCreacion = await _reader.IsDBNullAsync(Constante._31, cancelToken) ? String.Empty : _reader.GetString(Constante._31);
                                DateTime? fechaCreacion = await _reader.IsDBNullAsync(Constante._32, cancelToken) ? (DateTime?)null : _reader.GetDateTime(Constante._32);
                                ordenBancariaDetalleMO.FechaCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortDateString();
                                ordenBancariaDetalleMO.HoraCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortTimeString();
                                listaOrdenesBancariasDetalle.Add(ordenBancariaDetalleMO);
                            }

                            objetoOrdenBancariaDetalleMO.Codigo = Constante.CODIGO_OK;
                        }

                        _reader.Close();
                        _con.Close();
                        objetoOrdenBancariaDetalleMO.Codigo = _reader != null ? objetoOrdenBancariaDetalleMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoOrdenBancariaDetalleMO.Mensaje = _reader != null ? objetoOrdenBancariaDetalleMO.Mensaje: Constante.MENSAJE_RECARGAR_PAGINA;
                        objetoOrdenBancariaDetalleMO.ListaOrdenesBancariasDetalle = listaOrdenesBancariasDetalle;
                        String mensaje = _reader != null ? Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_POR_APROBAR_ASYNC_OK : Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_POR_APROBAR_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DETALLE_POR_APROBAR_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DETALLE_POR_APROBAR_ASYNC, Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_POR_APROBAR_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoOrdenBancariaDetalleMO;
        }

        public async Task<ObjetoOrdenBancariaMO> BuscarOrdenesBancariasPorAprobarAsync(CancellationToken cancelToken, String idUsuario, String idSap, String fechaInicio, String fechaFin)
        {
            ObjetoOrdenBancariaMO objetoOrdenBancariaMO = new ObjetoOrdenBancariaMO();
            List<OrdenBancariaMO> listaOrdenesBancarias = null;
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_BUSCAR_ORDENES_BANCARIAS_POR_APROBAR, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        
                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_USUARIO, System.Data.SqlDbType.NChar, Constante._6);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idUsuario;

                        SqlParameter par2 = _cmd.Parameters.Add(Constante.ID_SAP, System.Data.SqlDbType.NVarChar, Constante._10);
                        par2.Direction = System.Data.ParameterDirection.Input;
                        par2.Value = idSap;

                        SqlParameter par3 = _cmd.Parameters.Add(Constante.FECHA_INICIO, System.Data.SqlDbType.NChar, Constante._10);
                        par3.Direction = System.Data.ParameterDirection.Input;
                        par3.Value = fechaInicio;

                        SqlParameter par4 = _cmd.Parameters.Add(Constante.FECHA_FIN, System.Data.SqlDbType.NChar, Constante._10);
                        par4.Direction = System.Data.ParameterDirection.Input;
                        par4.Value = fechaFin;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult, cancelToken);

                        if (_reader != null && !_reader.HasRows)
                        {
                            objetoOrdenBancariaMO.Codigo = Constante.CODIGO_OMISION;
                            objetoOrdenBancariaMO.Mensaje = Constante.MENSAJE_SIN_RESULTADOS;
                        }
                        else if (_reader != null && _reader.HasRows)
                        {
                            listaOrdenesBancarias = new List<OrdenBancariaMO>();

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                OrdenBancariaMO ordenBancariaMO = new OrdenBancariaMO();
                                ordenBancariaMO.IdOrdenBancaria = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                ordenBancariaMO.Banco = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                ordenBancariaMO.BancoCorto = await _reader.IsDBNullAsync(Constante._2, cancelToken) ? String.Empty : _reader.GetString(Constante._2);
                                ordenBancariaMO.IdTipoOrden = await _reader.IsDBNullAsync(Constante._3, cancelToken) ? String.Empty : _reader.GetString(Constante._3);
                                ordenBancariaMO.TipoOrden = await _reader.IsDBNullAsync(Constante._4, cancelToken) ? String.Empty : _reader.GetString(Constante._4);
                                ordenBancariaMO.TipoOrdenCorto = await _reader.IsDBNullAsync(Constante._5, cancelToken) ? String.Empty : _reader.GetString(Constante._5);
                                ordenBancariaMO.IdEstadoOrden = await _reader.IsDBNullAsync(Constante._6, cancelToken) ? String.Empty : _reader.GetString(Constante._6);
                                ordenBancariaMO.EstadoOrden = await _reader.IsDBNullAsync(Constante._7, cancelToken) ? String.Empty : _reader.GetString(Constante._7);
                                ordenBancariaMO.IdSociedad = await _reader.IsDBNullAsync(Constante._8, cancelToken) ? String.Empty : _reader.GetString(Constante._8);
                                ordenBancariaMO.Sociedad = await _reader.IsDBNullAsync(Constante._9, cancelToken) ? String.Empty : _reader.GetString(Constante._9);
                                ordenBancariaMO.SociedadCorto = await _reader.IsDBNullAsync(Constante._10, cancelToken) ? String.Empty : _reader.GetString(Constante._10);
                                ordenBancariaMO.IdSap = await _reader.IsDBNullAsync(Constante._11, cancelToken) ? String.Empty : _reader.GetString(Constante._11);
                                ordenBancariaMO.Anio = await _reader.IsDBNullAsync(Constante._12, cancelToken) ? String.Empty : _reader.GetString(Constante._12);
                                ordenBancariaMO.MomentoOrden = await _reader.IsDBNullAsync(Constante._13, cancelToken) ? String.Empty : _reader.GetString(Constante._13);
                                ordenBancariaMO.FechaOrden = await _reader.IsDBNullAsync(Constante._14, cancelToken) ? String.Empty : _reader.GetString(Constante._14);
                                ordenBancariaMO.NombreArchivo = await _reader.IsDBNullAsync(Constante._15, cancelToken) ? String.Empty : _reader.GetString(Constante._15);
                                ordenBancariaMO.RutaArchivo = await _reader.IsDBNullAsync(Constante._16, cancelToken) ? String.Empty : _reader.GetString(Constante._16);
                                ordenBancariaMO.MonedaLocal = await _reader.IsDBNullAsync(Constante._17, cancelToken) ? String.Empty : _reader.GetString(Constante._17);
                                ordenBancariaMO.MonedaCortoLocal = await _reader.IsDBNullAsync(Constante._18, cancelToken) ? String.Empty : _reader.GetString(Constante._18);
                                ordenBancariaMO.ImporteLocal = await _reader.IsDBNullAsync(Constante._19, cancelToken) ? 0 : _reader.GetDecimal(Constante._19);
                                ordenBancariaMO.MonedaForanea = await _reader.IsDBNullAsync(Constante._20, cancelToken) ? String.Empty : _reader.GetString(Constante._20);
                                ordenBancariaMO.MonedaCortoForanea = await _reader.IsDBNullAsync(Constante._21, cancelToken) ? String.Empty : _reader.GetString(Constante._21);
                                ordenBancariaMO.ImporteForanea = await _reader.IsDBNullAsync(Constante._22, cancelToken) ? 0 : _reader.GetDecimal(Constante._22);
                                ordenBancariaMO.Liberador = await _reader.IsDBNullAsync(Constante._23, cancelToken) ? String.Empty : _reader.GetString(Constante._23);
                                ordenBancariaMO.PreAprobador = await _reader.IsDBNullAsync(Constante._24, cancelToken) ? String.Empty : _reader.GetString(Constante._24);
                                ordenBancariaMO.UsuarioCreacion = await _reader.IsDBNullAsync(Constante._25, cancelToken) ? String.Empty : _reader.GetString(Constante._25);
                                DateTime? fechaCreacion = await _reader.IsDBNullAsync(Constante._26, cancelToken) ? (DateTime?)null : _reader.GetDateTime(Constante._26);
                                ordenBancariaMO.FechaCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortDateString();
                                ordenBancariaMO.HoraCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortTimeString();
                                listaOrdenesBancarias.Add(ordenBancariaMO);
                            }

                            objetoOrdenBancariaMO.Codigo = Constante.CODIGO_OK;
                        }

                        _reader.Close();
                        _con.Close();
                        objetoOrdenBancariaMO.Codigo = _reader != null ? objetoOrdenBancariaMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoOrdenBancariaMO.Mensaje = _reader != null ? objetoOrdenBancariaMO.Mensaje: Constante.MENSAJE_RECARGAR_PAGINA;
                        objetoOrdenBancariaMO.ListaOrdenesBancarias = listaOrdenesBancarias;
                        String mensaje = _reader != null ? Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_POR_APROBAR_ASYNC_OK : Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_POR_APROBAR_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_BUSCAR_ORDENES_BANCARIAS_POR_APROBAR_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_BUSCAR_ORDENES_BANCARIAS_POR_APROBAR_ASYNC, Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_POR_APROBAR_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoOrdenBancariaMO;
        }

        public async Task<ObjetoOrdenBancariaMO> ListarOrdenesBancariasAprobadasAsync(CancellationToken cancelToken, String idUsuario, Int32 pagina, Int32 filas, String idSociedad, String idBanco, String idTipoOrden)
        {
            ObjetoOrdenBancariaMO objetoOrdenBancariaMO = new ObjetoOrdenBancariaMO();
            List<OrdenBancariaMO> listaOrdenesBancarias = null;
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_LISTAR_ORDENES_BANCARIAS_APROBADAS, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_USUARIO, System.Data.SqlDbType.NChar, Constante._6);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idUsuario;
                        
                        SqlParameter par2 = _cmd.Parameters.Add(Constante.PAGINA, System.Data.SqlDbType.Int);
                        par2.Direction = System.Data.ParameterDirection.Input;
                        par2.Value = pagina;

                        SqlParameter par3 = _cmd.Parameters.Add(Constante.FILAS, System.Data.SqlDbType.Int);
                        par3.Direction = System.Data.ParameterDirection.Input;
                        par3.Value = filas;

                        SqlParameter par4 = _cmd.Parameters.Add(Constante.ID_SOCIEDAD, System.Data.SqlDbType.NChar, 4);
                        par4.Direction = System.Data.ParameterDirection.Input;
                        par4.Value = idSociedad;

                        SqlParameter par5 = _cmd.Parameters.Add(Constante.ID_BANCO, System.Data.SqlDbType.NChar, 5);
                        par5.Direction = System.Data.ParameterDirection.Input;
                        par5.Value = idBanco;

                        SqlParameter par6 = _cmd.Parameters.Add(Constante.ID_TIPO_ORDEN, System.Data.SqlDbType.NVarChar, 5);
                        par6.Direction = System.Data.ParameterDirection.Input;
                        par6.Value = idTipoOrden;

                        SqlParameter par7 = _cmd.Parameters.Add(Constante.TOTAL_REGISTROS, System.Data.SqlDbType.Int);
                        par7.Direction = System.Data.ParameterDirection.Output;
                        
                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult, cancelToken);

                        if (_reader != null && !_reader.HasRows)
                        {
                            objetoOrdenBancariaMO.Codigo = Constante.CODIGO_OMISION;
                            objetoOrdenBancariaMO.Mensaje = Constante.MENSAJE_SIN_RESULTADOS;
                        }
                        else if (_reader != null && _reader.HasRows)
                        {
                            listaOrdenesBancarias = new List<OrdenBancariaMO>();

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                OrdenBancariaMO ordenBancariaMO = new OrdenBancariaMO();
                                ordenBancariaMO.IdOrdenBancaria = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                ordenBancariaMO.Banco = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                ordenBancariaMO.BancoCorto = await _reader.IsDBNullAsync(Constante._2, cancelToken) ? String.Empty : _reader.GetString(Constante._2);
                                ordenBancariaMO.IdTipoOrden = await _reader.IsDBNullAsync(Constante._3, cancelToken) ? String.Empty : _reader.GetString(Constante._3);
                                ordenBancariaMO.TipoOrden = await _reader.IsDBNullAsync(Constante._4, cancelToken) ? String.Empty : _reader.GetString(Constante._4);
                                ordenBancariaMO.TipoOrdenCorto = await _reader.IsDBNullAsync(Constante._5, cancelToken) ? String.Empty : _reader.GetString(Constante._5);
                                ordenBancariaMO.IdEstadoOrden = await _reader.IsDBNullAsync(Constante._6, cancelToken) ? String.Empty : _reader.GetString(Constante._6);
                                ordenBancariaMO.EstadoOrden = await _reader.IsDBNullAsync(Constante._7, cancelToken) ? String.Empty : _reader.GetString(Constante._7);
                                ordenBancariaMO.IdSociedad = await _reader.IsDBNullAsync(Constante._8, cancelToken) ? String.Empty : _reader.GetString(Constante._8);
                                ordenBancariaMO.Sociedad = await _reader.IsDBNullAsync(Constante._9, cancelToken) ? String.Empty : _reader.GetString(Constante._9);
                                ordenBancariaMO.SociedadCorto = await _reader.IsDBNullAsync(Constante._10, cancelToken) ? String.Empty : _reader.GetString(Constante._10);
                                ordenBancariaMO.IdSap = await _reader.IsDBNullAsync(Constante._11, cancelToken) ? String.Empty : _reader.GetString(Constante._11);
                                ordenBancariaMO.Anio = await _reader.IsDBNullAsync(Constante._12, cancelToken) ? String.Empty : _reader.GetString(Constante._12);
                                ordenBancariaMO.MomentoOrden = await _reader.IsDBNullAsync(Constante._13, cancelToken) ? String.Empty : _reader.GetString(Constante._13);
                                ordenBancariaMO.FechaOrden = await _reader.IsDBNullAsync(Constante._14, cancelToken) ? String.Empty : _reader.GetString(Constante._14);
                                ordenBancariaMO.NombreArchivo = await _reader.IsDBNullAsync(Constante._15, cancelToken) ? String.Empty : _reader.GetString(Constante._15);
                                ordenBancariaMO.RutaArchivo = await _reader.IsDBNullAsync(Constante._16, cancelToken) ? String.Empty : _reader.GetString(Constante._16);
                                ordenBancariaMO.MonedaLocal = await _reader.IsDBNullAsync(Constante._17, cancelToken) ? String.Empty : _reader.GetString(Constante._17);
                                ordenBancariaMO.MonedaCortoLocal = await _reader.IsDBNullAsync(Constante._18, cancelToken) ? String.Empty : _reader.GetString(Constante._18);
                                ordenBancariaMO.ImporteLocal = await _reader.IsDBNullAsync(Constante._19, cancelToken) ? 0 : _reader.GetDecimal(Constante._19);
                                ordenBancariaMO.MonedaForanea = await _reader.IsDBNullAsync(Constante._20, cancelToken) ? String.Empty : _reader.GetString(Constante._20);
                                ordenBancariaMO.MonedaCortoForanea = await _reader.IsDBNullAsync(Constante._21, cancelToken) ? String.Empty : _reader.GetString(Constante._21);
                                ordenBancariaMO.ImporteForanea = await _reader.IsDBNullAsync(Constante._22, cancelToken) ? 0 : _reader.GetDecimal(Constante._22);
                                ordenBancariaMO.Liberador = await _reader.IsDBNullAsync(Constante._23, cancelToken) ? String.Empty : _reader.GetString(Constante._23);
                                ordenBancariaMO.PreAprobador = await _reader.IsDBNullAsync(Constante._24, cancelToken) ? String.Empty : _reader.GetString(Constante._24);
                                ordenBancariaMO.Aprobador = await _reader.IsDBNullAsync(Constante._25, cancelToken) ? String.Empty : _reader.GetString(Constante._25);
                                ordenBancariaMO.Comentarios = await _reader.IsDBNullAsync(Constante._26, cancelToken) ? String.Empty : _reader.GetString(Constante._26);
                                ordenBancariaMO.UsuarioCreacion = await _reader.IsDBNullAsync(Constante._27, cancelToken) ? String.Empty : _reader.GetString(Constante._27);
                                DateTime? fechaCreacion = await _reader.IsDBNullAsync(Constante._28, cancelToken) ? (DateTime?)null : _reader.GetDateTime(Constante._28);
                                ordenBancariaMO.FechaCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortDateString();
                                ordenBancariaMO.HoraCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortTimeString();
                                ordenBancariaMO.UsuarioEdicion = await _reader.IsDBNullAsync(Constante._29, cancelToken) ? String.Empty : _reader.GetString(Constante._29);
                                DateTime? fechaEdicion = await _reader.IsDBNullAsync(Constante._30, cancelToken) ? (DateTime?)null : _reader.GetDateTime(Constante._30);
                                ordenBancariaMO.FechaEdicion = fechaEdicion == null ? String.Empty : fechaEdicion.Value.ToShortDateString();
                                ordenBancariaMO.HoraEdicion = fechaEdicion == null ? String.Empty : fechaEdicion.Value.ToShortTimeString();
                                listaOrdenesBancarias.Add(ordenBancariaMO);
                            }

                            objetoOrdenBancariaMO.Codigo = Constante.CODIGO_OK;
                        }

                        _reader.Close();
                        _con.Close();
                        objetoOrdenBancariaMO.Codigo = _reader != null ? objetoOrdenBancariaMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoOrdenBancariaMO.Mensaje = _reader != null ? objetoOrdenBancariaMO.Mensaje: Constante.MENSAJE_RECARGAR_PAGINA;
                        objetoOrdenBancariaMO.ListaOrdenesBancarias = listaOrdenesBancarias;
                        objetoOrdenBancariaMO.TotalRegistros = par7.Value == null ? 0 : Convert.ToInt32(par7.Value);
                        String mensaje = _reader != null ? Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_APROBADAS_ASYNC_OK : Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_APROBADAS_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_APROBADAS_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_APROBADAS_ASYNC, Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_APROBADAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoOrdenBancariaMO;
        }

        public async Task<ObjetoOrdenBancariaDetalleMO> ListarOrdenesBancariasDetalleAprobadasAsync(CancellationToken cancelToken, String idSociedad, String idSap, String anio, String momentoOrden, String idTipoOrden)
        {
            ObjetoOrdenBancariaDetalleMO objetoOrdenBancariaDetalleMO = new ObjetoOrdenBancariaDetalleMO();
            List<OrdenBancariaDetalleMO> listaOrdenesBancariasDetalle = null;
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_LISTAR_ORDENES_BANCARIAS_DETALLE_APROBADAS, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        
                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_SOCIEDAD, System.Data.SqlDbType.NChar, Constante._4);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idSociedad;

                        SqlParameter par2 = _cmd.Parameters.Add(Constante.ID_SAP, System.Data.SqlDbType.NVarChar, Constante._10);
                        par2.Direction = System.Data.ParameterDirection.Input;
                        par2.Value = idSap;

                        SqlParameter par3 = _cmd.Parameters.Add(Constante.ANIO, System.Data.SqlDbType.NChar, Constante._4);
                        par3.Direction = System.Data.ParameterDirection.Input;
                        par3.Value = anio;

                        SqlParameter par4 = _cmd.Parameters.Add(Constante.MOMENTO_ORDEN, System.Data.SqlDbType.NChar, Constante._8);
                        par4.Direction = System.Data.ParameterDirection.Input;
                        par4.Value = momentoOrden;

                        SqlParameter par5 = _cmd.Parameters.Add(Constante.ID_TIPO_ORDEN, System.Data.SqlDbType.NVarChar, Constante._5);
                        par5.Direction = System.Data.ParameterDirection.Input;
                        par5.Value = idTipoOrden;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult, cancelToken);

                        if (_reader != null && !_reader.HasRows)
                        {
                            objetoOrdenBancariaDetalleMO.Codigo = Constante.CODIGO_OMISION;
                            objetoOrdenBancariaDetalleMO.Mensaje = Constante.MENSAJE_SIN_RESULTADOS;
                        }
                        else if (_reader != null && _reader.HasRows)
                        {
                            listaOrdenesBancariasDetalle = new List<OrdenBancariaDetalleMO>();

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                OrdenBancariaDetalleMO ordenBancariaDetalleMO = new OrdenBancariaDetalleMO();
                                ordenBancariaDetalleMO.IdOrdenBancariaDetalle = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                ordenBancariaDetalleMO.TipoTransferencia = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                ordenBancariaDetalleMO.FormaPago = await _reader.IsDBNullAsync(Constante._2, cancelToken) ? String.Empty : _reader.GetString(Constante._2);
                                ordenBancariaDetalleMO.SubTipoPago = await _reader.IsDBNullAsync(Constante._3, cancelToken) ? String.Empty : _reader.GetString(Constante._3);
                                ordenBancariaDetalleMO.Referencia1 = await _reader.IsDBNullAsync(Constante._4, cancelToken) ? String.Empty : _reader.GetString(Constante._4);
                                ordenBancariaDetalleMO.Referencia2 = await _reader.IsDBNullAsync(Constante._5, cancelToken) ? String.Empty : _reader.GetString(Constante._5);
                                ordenBancariaDetalleMO.MonedaCargo = await _reader.IsDBNullAsync(Constante._6, cancelToken) ? String.Empty : _reader.GetString(Constante._6);
                                ordenBancariaDetalleMO.MonedaCargoCorto = await _reader.IsDBNullAsync(Constante._7, cancelToken) ? String.Empty : _reader.GetString(Constante._7);
                                ordenBancariaDetalleMO.CuentaCargo = await _reader.IsDBNullAsync(Constante._8, cancelToken) ? String.Empty : _reader.GetString(Constante._8);
                                ordenBancariaDetalleMO.MontoCargo = await _reader.IsDBNullAsync(Constante._9, cancelToken) ? 0 : _reader.GetDecimal(Constante._9);
                                ordenBancariaDetalleMO.MonedaAbono = await _reader.IsDBNullAsync(Constante._10, cancelToken) ? String.Empty : _reader.GetString(Constante._10);
                                ordenBancariaDetalleMO.MonedaAbonoCorto = await _reader.IsDBNullAsync(Constante._11, cancelToken) ? String.Empty : _reader.GetString(Constante._11);
                                ordenBancariaDetalleMO.CuentaAbono = await _reader.IsDBNullAsync(Constante._12, cancelToken) ? String.Empty : _reader.GetString(Constante._12);
                                ordenBancariaDetalleMO.CuentaGasto = await _reader.IsDBNullAsync(Constante._13, cancelToken) ? String.Empty : _reader.GetString(Constante._13);
                                ordenBancariaDetalleMO.MontoAbono = await _reader.IsDBNullAsync(Constante._14, cancelToken) ? 0 : _reader.GetDecimal(Constante._14);
                                ordenBancariaDetalleMO.TipoCambio = await _reader.IsDBNullAsync(Constante._15, cancelToken) ? 0 : _reader.GetDecimal(Constante._15);
                                ordenBancariaDetalleMO.ModuloRaiz = await _reader.IsDBNullAsync(Constante._16, cancelToken) ? 0 : _reader.GetInt32(Constante._16);
                                ordenBancariaDetalleMO.DigitoControl = await _reader.IsDBNullAsync(Constante._17, cancelToken) ? 0 : _reader.GetInt32(Constante._17);
                                ordenBancariaDetalleMO.Indicador = await _reader.IsDBNullAsync(Constante._18, cancelToken) ? String.Empty : _reader.GetString(Constante._18);
                                ordenBancariaDetalleMO.NroOperacion = await _reader.IsDBNullAsync(Constante._19, cancelToken) ? String.Empty : _reader.GetString(Constante._19);
                                ordenBancariaDetalleMO.Beneficiario = await _reader.IsDBNullAsync(Constante._20, cancelToken) ? String.Empty : _reader.GetString(Constante._20);
                                ordenBancariaDetalleMO.TipoDocumento = await _reader.IsDBNullAsync(Constante._21, cancelToken) ? String.Empty : _reader.GetString(Constante._21);
                                ordenBancariaDetalleMO.TipoDocumentoCorto = await _reader.IsDBNullAsync(Constante._22, cancelToken) ? String.Empty : _reader.GetString(Constante._22);
                                ordenBancariaDetalleMO.NroDocumento = await _reader.IsDBNullAsync(Constante._23, cancelToken) ? String.Empty : _reader.GetString(Constante._23);
                                ordenBancariaDetalleMO.Correo = await _reader.IsDBNullAsync(Constante._24, cancelToken) ? String.Empty : _reader.GetString(Constante._24);
                                ordenBancariaDetalleMO.NombreBanco = await _reader.IsDBNullAsync(Constante._25, cancelToken) ? String.Empty : _reader.GetString(Constante._25);
                                ordenBancariaDetalleMO.RucBanco = await _reader.IsDBNullAsync(Constante._26, cancelToken) ? String.Empty : _reader.GetString(Constante._26);
                                ordenBancariaDetalleMO.NroFactura = await _reader.IsDBNullAsync(Constante._27, cancelToken) ? String.Empty : _reader.GetString(Constante._27);
                                ordenBancariaDetalleMO.FechaFactura = await _reader.IsDBNullAsync(Constante._28, cancelToken) ? String.Empty : _reader.GetString(Constante._28);
                                ordenBancariaDetalleMO.FechaFinFactura = await _reader.IsDBNullAsync(Constante._29, cancelToken) ? String.Empty : _reader.GetString(Constante._29);
                                ordenBancariaDetalleMO.SignoFactura = await _reader.IsDBNullAsync(Constante._30, cancelToken) ? String.Empty : _reader.GetString(Constante._30);
                                ordenBancariaDetalleMO.IdRespuesta = await _reader.IsDBNullAsync(Constante._31, cancelToken) ? String.Empty : _reader.GetString(Constante._31);
                                ordenBancariaDetalleMO.Respuesta = await _reader.IsDBNullAsync(Constante._32, cancelToken) ? String.Empty : _reader.GetString(Constante._32);
                                ordenBancariaDetalleMO.NroOrden = await _reader.IsDBNullAsync(Constante._33, cancelToken) ? String.Empty : _reader.GetString(Constante._33);
                                ordenBancariaDetalleMO.NroConvenio = await _reader.IsDBNullAsync(Constante._34, cancelToken) ? String.Empty : _reader.GetString(Constante._34);
                                ordenBancariaDetalleMO.UsuarioCreacion = await _reader.IsDBNullAsync(Constante._35, cancelToken) ? String.Empty : _reader.GetString(Constante._35);
                                DateTime? fechaCreacion = await _reader.IsDBNullAsync(Constante._36, cancelToken) ? (DateTime?)null : _reader.GetDateTime(Constante._36);
                                ordenBancariaDetalleMO.FechaCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortDateString();
                                ordenBancariaDetalleMO.HoraCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortTimeString();
                                listaOrdenesBancariasDetalle.Add(ordenBancariaDetalleMO);
                            }

                            objetoOrdenBancariaDetalleMO.Codigo = Constante.CODIGO_OK;
                        }

                        _reader.Close();
                        _con.Close();
                        objetoOrdenBancariaDetalleMO.Codigo = _reader != null ? objetoOrdenBancariaDetalleMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoOrdenBancariaDetalleMO.Mensaje = _reader != null ? objetoOrdenBancariaDetalleMO.Mensaje: Constante.MENSAJE_RECARGAR_PAGINA;
                        objetoOrdenBancariaDetalleMO.ListaOrdenesBancariasDetalle = listaOrdenesBancariasDetalle;
                        String mensaje = _reader != null ? Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_APROBADAS_ASYNC_OK : Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_APROBADAS_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DETALLE_APROBADAS_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DETALLE_APROBADAS_ASYNC, Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_APROBADAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoOrdenBancariaDetalleMO;
        }

        public async Task<ObjetoOrdenBancariaMO> BuscarOrdenesBancariasAprobadasAsync(CancellationToken cancelToken, String idUsuario, String idSap, String fechaInicio, String fechaFin)
        {
            ObjetoOrdenBancariaMO objetoOrdenBancariaMO = new ObjetoOrdenBancariaMO();
            List<OrdenBancariaMO> listaOrdenesBancarias = null;
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_BUSCAR_ORDENES_BANCARIAS_APROBADAS, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_USUARIO, System.Data.SqlDbType.NChar, Constante._6);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idUsuario;
                        
                        SqlParameter par2 = _cmd.Parameters.Add(Constante.ID_SAP, System.Data.SqlDbType.NVarChar, Constante._10);
                        par2.Direction = System.Data.ParameterDirection.Input;
                        par2.Value = idSap;

                        SqlParameter par3 = _cmd.Parameters.Add(Constante.FECHA_INICIO, System.Data.SqlDbType.NChar, Constante._10);
                        par3.Direction = System.Data.ParameterDirection.Input;
                        par3.Value = fechaInicio;

                        SqlParameter par4 = _cmd.Parameters.Add(Constante.FECHA_FIN, System.Data.SqlDbType.NChar, Constante._10);
                        par4.Direction = System.Data.ParameterDirection.Input;
                        par4.Value = fechaFin;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult, cancelToken);

                        if (_reader != null && !_reader.HasRows)
                        {
                            objetoOrdenBancariaMO.Codigo = Constante.CODIGO_OMISION;
                            objetoOrdenBancariaMO.Mensaje = Constante.MENSAJE_SIN_RESULTADOS;
                        }
                        else if (_reader != null && _reader.HasRows)
                        {
                            listaOrdenesBancarias = new List<OrdenBancariaMO>();

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                OrdenBancariaMO ordenBancariaMO = new OrdenBancariaMO();
                                ordenBancariaMO.IdOrdenBancaria = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                ordenBancariaMO.Banco = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                ordenBancariaMO.BancoCorto = await _reader.IsDBNullAsync(Constante._2, cancelToken) ? String.Empty : _reader.GetString(Constante._2);
                                ordenBancariaMO.IdTipoOrden = await _reader.IsDBNullAsync(Constante._3, cancelToken) ? String.Empty : _reader.GetString(Constante._3);
                                ordenBancariaMO.TipoOrden = await _reader.IsDBNullAsync(Constante._4, cancelToken) ? String.Empty : _reader.GetString(Constante._4);
                                ordenBancariaMO.TipoOrdenCorto = await _reader.IsDBNullAsync(Constante._5, cancelToken) ? String.Empty : _reader.GetString(Constante._5);
                                ordenBancariaMO.IdEstadoOrden = await _reader.IsDBNullAsync(Constante._6, cancelToken) ? String.Empty : _reader.GetString(Constante._6);
                                ordenBancariaMO.EstadoOrden = await _reader.IsDBNullAsync(Constante._7, cancelToken) ? String.Empty : _reader.GetString(Constante._7);
                                ordenBancariaMO.IdSociedad = await _reader.IsDBNullAsync(Constante._8, cancelToken) ? String.Empty : _reader.GetString(Constante._8);
                                ordenBancariaMO.Sociedad = await _reader.IsDBNullAsync(Constante._9, cancelToken) ? String.Empty : _reader.GetString(Constante._9);
                                ordenBancariaMO.SociedadCorto = await _reader.IsDBNullAsync(Constante._10, cancelToken) ? String.Empty : _reader.GetString(Constante._10);
                                ordenBancariaMO.IdSap = await _reader.IsDBNullAsync(Constante._11, cancelToken) ? String.Empty : _reader.GetString(Constante._11);
                                ordenBancariaMO.Anio = await _reader.IsDBNullAsync(Constante._12, cancelToken) ? String.Empty : _reader.GetString(Constante._12);
                                ordenBancariaMO.MomentoOrden = await _reader.IsDBNullAsync(Constante._13, cancelToken) ? String.Empty : _reader.GetString(Constante._13);
                                ordenBancariaMO.FechaOrden = await _reader.IsDBNullAsync(Constante._14, cancelToken) ? String.Empty : _reader.GetString(Constante._14);
                                ordenBancariaMO.NombreArchivo = await _reader.IsDBNullAsync(Constante._15, cancelToken) ? String.Empty : _reader.GetString(Constante._15);
                                ordenBancariaMO.RutaArchivo = await _reader.IsDBNullAsync(Constante._16, cancelToken) ? String.Empty : _reader.GetString(Constante._16);
                                ordenBancariaMO.MonedaLocal = await _reader.IsDBNullAsync(Constante._17, cancelToken) ? String.Empty : _reader.GetString(Constante._17);
                                ordenBancariaMO.MonedaCortoLocal = await _reader.IsDBNullAsync(Constante._18, cancelToken) ? String.Empty : _reader.GetString(Constante._18);
                                ordenBancariaMO.ImporteLocal = await _reader.IsDBNullAsync(Constante._19, cancelToken) ? 0 : _reader.GetDecimal(Constante._19);
                                ordenBancariaMO.MonedaForanea = await _reader.IsDBNullAsync(Constante._20, cancelToken) ? String.Empty : _reader.GetString(Constante._20);
                                ordenBancariaMO.MonedaCortoForanea = await _reader.IsDBNullAsync(Constante._21, cancelToken) ? String.Empty : _reader.GetString(Constante._21);
                                ordenBancariaMO.ImporteForanea = await _reader.IsDBNullAsync(Constante._22, cancelToken) ? 0 : _reader.GetDecimal(Constante._22);
                                ordenBancariaMO.Liberador = await _reader.IsDBNullAsync(Constante._23, cancelToken) ? String.Empty : _reader.GetString(Constante._23);
                                ordenBancariaMO.PreAprobador = await _reader.IsDBNullAsync(Constante._24, cancelToken) ? String.Empty : _reader.GetString(Constante._24);
                                ordenBancariaMO.Aprobador = await _reader.IsDBNullAsync(Constante._25, cancelToken) ? String.Empty : _reader.GetString(Constante._25);
                                ordenBancariaMO.Comentarios = await _reader.IsDBNullAsync(Constante._26, cancelToken) ? String.Empty : _reader.GetString(Constante._26);
                                ordenBancariaMO.UsuarioCreacion = await _reader.IsDBNullAsync(Constante._27, cancelToken) ? String.Empty : _reader.GetString(Constante._27);
                                DateTime? fechaCreacion = await _reader.IsDBNullAsync(Constante._28, cancelToken) ? (DateTime?)null : _reader.GetDateTime(Constante._28);
                                ordenBancariaMO.FechaCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortDateString();
                                ordenBancariaMO.HoraCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortTimeString();
                                ordenBancariaMO.UsuarioEdicion = await _reader.IsDBNullAsync(Constante._29, cancelToken) ? String.Empty : _reader.GetString(Constante._29);
                                DateTime? fechaEdicion = await _reader.IsDBNullAsync(Constante._30, cancelToken) ? (DateTime?)null : _reader.GetDateTime(Constante._30);
                                ordenBancariaMO.FechaEdicion = fechaEdicion == null ? String.Empty : fechaEdicion.Value.ToShortDateString();
                                ordenBancariaMO.HoraEdicion = fechaEdicion == null ? String.Empty : fechaEdicion.Value.ToShortTimeString();
                                listaOrdenesBancarias.Add(ordenBancariaMO);
                            }

                            objetoOrdenBancariaMO.Codigo = Constante.CODIGO_OK;
                        }

                        _reader.Close();
                        _con.Close();
                        objetoOrdenBancariaMO.Codigo = _reader != null ? objetoOrdenBancariaMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoOrdenBancariaMO.Mensaje = _reader != null ? objetoOrdenBancariaMO.Mensaje: Constante.MENSAJE_RECARGAR_PAGINA;
                        objetoOrdenBancariaMO.ListaOrdenesBancarias = listaOrdenesBancarias;
                        String mensaje = _reader != null ? Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_APROBADAS_ASYNC_OK : Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_APROBADAS_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_BUSCAR_ORDENES_BANCARIAS_APROBADAS_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_BUSCAR_ORDENES_BANCARIAS_APROBADAS_ASYNC, Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_APROBADAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoOrdenBancariaMO;
        }

        public async Task<ObjetoOrdenBancariaMO> ListarOrdenesBancariasAnuladasAsync(CancellationToken cancelToken, String idUsuario, Int32 pagina, Int32 filas, String idSociedad, String idBanco, String idTipoOrden)
        {
            ObjetoOrdenBancariaMO objetoOrdenBancariaMO = new ObjetoOrdenBancariaMO();
            List<OrdenBancariaMO> listaOrdenesBancarias = null;
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_LISTAR_ORDENES_BANCARIAS_ANULADAS, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        
                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_USUARIO, System.Data.SqlDbType.NChar, Constante._6);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idUsuario;
                        
                        SqlParameter par2 = _cmd.Parameters.Add(Constante.PAGINA, System.Data.SqlDbType.Int);
                        par2.Direction = System.Data.ParameterDirection.Input;
                        par2.Value = pagina;

                        SqlParameter par3 = _cmd.Parameters.Add(Constante.FILAS, System.Data.SqlDbType.Int);
                        par3.Direction = System.Data.ParameterDirection.Input;
                        par3.Value = filas;

                        SqlParameter par4 = _cmd.Parameters.Add(Constante.ID_SOCIEDAD, System.Data.SqlDbType.NChar, 4);
                        par4.Direction = System.Data.ParameterDirection.Input;
                        par4.Value = idSociedad;

                        SqlParameter par5 = _cmd.Parameters.Add(Constante.ID_BANCO, System.Data.SqlDbType.NChar, 5);
                        par5.Direction = System.Data.ParameterDirection.Input;
                        par5.Value = idBanco;

                        SqlParameter par6 = _cmd.Parameters.Add(Constante.ID_TIPO_ORDEN, System.Data.SqlDbType.NVarChar, 5);
                        par6.Direction = System.Data.ParameterDirection.Input;
                        par6.Value = idTipoOrden;

                        SqlParameter par7 = _cmd.Parameters.Add(Constante.TOTAL_REGISTROS, System.Data.SqlDbType.Int);
                        par7.Direction = System.Data.ParameterDirection.Output;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult, cancelToken);

                        if (_reader != null && !_reader.HasRows)
                        {
                            objetoOrdenBancariaMO.Codigo = Constante.CODIGO_OMISION;
                            objetoOrdenBancariaMO.Mensaje = Constante.MENSAJE_SIN_RESULTADOS;
                        }
                        else if (_reader != null && _reader.HasRows)
                        {
                            listaOrdenesBancarias = new List<OrdenBancariaMO>();

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                OrdenBancariaMO ordenBancariaMO = new OrdenBancariaMO();
                                ordenBancariaMO.IdOrdenBancaria = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                ordenBancariaMO.Banco = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                ordenBancariaMO.BancoCorto = await _reader.IsDBNullAsync(Constante._2, cancelToken) ? String.Empty : _reader.GetString(Constante._2);
                                ordenBancariaMO.IdTipoOrden = await _reader.IsDBNullAsync(Constante._3, cancelToken) ? String.Empty : _reader.GetString(Constante._3);
                                ordenBancariaMO.TipoOrden = await _reader.IsDBNullAsync(Constante._4, cancelToken) ? String.Empty : _reader.GetString(Constante._4);
                                ordenBancariaMO.TipoOrdenCorto = await _reader.IsDBNullAsync(Constante._5, cancelToken) ? String.Empty : _reader.GetString(Constante._5);
                                ordenBancariaMO.IdEstadoOrden = await _reader.IsDBNullAsync(Constante._6, cancelToken) ? String.Empty : _reader.GetString(Constante._6);
                                ordenBancariaMO.EstadoOrden = await _reader.IsDBNullAsync(Constante._7, cancelToken) ? String.Empty : _reader.GetString(Constante._7);
                                ordenBancariaMO.IdSociedad = await _reader.IsDBNullAsync(Constante._8, cancelToken) ? String.Empty : _reader.GetString(Constante._8);
                                ordenBancariaMO.Sociedad = await _reader.IsDBNullAsync(Constante._9, cancelToken) ? String.Empty : _reader.GetString(Constante._9);
                                ordenBancariaMO.SociedadCorto = await _reader.IsDBNullAsync(Constante._10, cancelToken) ? String.Empty : _reader.GetString(Constante._10);
                                ordenBancariaMO.IdSap = await _reader.IsDBNullAsync(Constante._11, cancelToken) ? String.Empty : _reader.GetString(Constante._11);
                                ordenBancariaMO.Anio = await _reader.IsDBNullAsync(Constante._12, cancelToken) ? String.Empty : _reader.GetString(Constante._12);
                                ordenBancariaMO.MomentoOrden = await _reader.IsDBNullAsync(Constante._13, cancelToken) ? String.Empty : _reader.GetString(Constante._13);
                                ordenBancariaMO.FechaOrden = await _reader.IsDBNullAsync(Constante._14, cancelToken) ? String.Empty : _reader.GetString(Constante._14);
                                ordenBancariaMO.NombreArchivo = await _reader.IsDBNullAsync(Constante._15, cancelToken) ? String.Empty : _reader.GetString(Constante._15);
                                ordenBancariaMO.RutaArchivo = await _reader.IsDBNullAsync(Constante._16, cancelToken) ? String.Empty : _reader.GetString(Constante._16);
                                ordenBancariaMO.MonedaLocal = await _reader.IsDBNullAsync(Constante._17, cancelToken) ? String.Empty : _reader.GetString(Constante._17);
                                ordenBancariaMO.MonedaCortoLocal = await _reader.IsDBNullAsync(Constante._18, cancelToken) ? String.Empty : _reader.GetString(Constante._18);
                                ordenBancariaMO.ImporteLocal = await _reader.IsDBNullAsync(Constante._19, cancelToken) ? 0 : _reader.GetDecimal(Constante._19);
                                ordenBancariaMO.MonedaForanea = await _reader.IsDBNullAsync(Constante._20, cancelToken) ? String.Empty : _reader.GetString(Constante._20);
                                ordenBancariaMO.MonedaCortoForanea = await _reader.IsDBNullAsync(Constante._21, cancelToken) ? String.Empty : _reader.GetString(Constante._21);
                                ordenBancariaMO.ImporteForanea = await _reader.IsDBNullAsync(Constante._22, cancelToken) ? 0 : _reader.GetDecimal(Constante._22);
                                ordenBancariaMO.Liberador = await _reader.IsDBNullAsync(Constante._23, cancelToken) ? String.Empty : _reader.GetString(Constante._23);
                                ordenBancariaMO.PreAprobador = await _reader.IsDBNullAsync(Constante._24, cancelToken) ? String.Empty : _reader.GetString(Constante._24);
                                ordenBancariaMO.Anulador = await _reader.IsDBNullAsync(Constante._25, cancelToken) ? String.Empty : _reader.GetString(Constante._25);
                                ordenBancariaMO.Comentarios = await _reader.IsDBNullAsync(Constante._26, cancelToken) ? String.Empty : _reader.GetString(Constante._26);
                                ordenBancariaMO.UsuarioCreacion = await _reader.IsDBNullAsync(Constante._27, cancelToken) ? String.Empty : _reader.GetString(Constante._27);
                                DateTime? fechaCreacion = await _reader.IsDBNullAsync(Constante._28, cancelToken) ? (DateTime?)null : _reader.GetDateTime(Constante._28);
                                ordenBancariaMO.FechaCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortDateString();
                                ordenBancariaMO.HoraCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortTimeString();
                                ordenBancariaMO.UsuarioEdicion = await _reader.IsDBNullAsync(Constante._29, cancelToken) ? String.Empty : _reader.GetString(Constante._29);
                                DateTime? fechaEdicion = await _reader.IsDBNullAsync(Constante._30, cancelToken) ? (DateTime?)null : _reader.GetDateTime(Constante._30);
                                ordenBancariaMO.FechaEdicion = fechaEdicion == null ? String.Empty : fechaEdicion.Value.ToShortDateString();
                                ordenBancariaMO.HoraEdicion = fechaEdicion == null ? String.Empty : fechaEdicion.Value.ToShortTimeString();
                                listaOrdenesBancarias.Add(ordenBancariaMO);
                            }

                            objetoOrdenBancariaMO.Codigo = Constante.CODIGO_OK;
                        }

                        _reader.Close();
                        _con.Close();
                        objetoOrdenBancariaMO.Codigo = _reader != null ? objetoOrdenBancariaMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoOrdenBancariaMO.Mensaje = _reader != null ? objetoOrdenBancariaMO.Mensaje: Constante.MENSAJE_RECARGAR_PAGINA;
                        objetoOrdenBancariaMO.ListaOrdenesBancarias = listaOrdenesBancarias;
                        objetoOrdenBancariaMO.TotalRegistros = par7.Value == null ? 0 : Convert.ToInt32(par7.Value);
                        String mensaje = _reader != null ? Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_ANULADAS_ASYNC_OK : Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_ANULADAS_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_ANULADAS_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_ANULADAS_ASYNC, Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_ANULADAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoOrdenBancariaMO;
        }

        public async Task<ObjetoOrdenBancariaDetalleMO> ListarOrdenesBancariasDetalleAnuladasAsync(CancellationToken cancelToken, String idSociedad, String idSap, String anio, String momentoOrden, String idTipoOrden)
        {
            ObjetoOrdenBancariaDetalleMO objetoOrdenBancariaDetalleMO = new ObjetoOrdenBancariaDetalleMO();
            List<OrdenBancariaDetalleMO> listaOrdenesBancariasDetalle = null;
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_LISTAR_ORDENES_BANCARIAS_DETALLE_ANULADAS, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        
                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_SOCIEDAD, System.Data.SqlDbType.NChar, Constante._4);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idSociedad;

                        SqlParameter par2 = _cmd.Parameters.Add(Constante.ID_SAP, System.Data.SqlDbType.NVarChar, Constante._10);
                        par2.Direction = System.Data.ParameterDirection.Input;
                        par2.Value = idSap;

                        SqlParameter par3 = _cmd.Parameters.Add(Constante.ANIO, System.Data.SqlDbType.NChar, Constante._4);
                        par3.Direction = System.Data.ParameterDirection.Input;
                        par3.Value = anio;

                        SqlParameter par4 = _cmd.Parameters.Add(Constante.MOMENTO_ORDEN, System.Data.SqlDbType.NChar, Constante._8);
                        par4.Direction = System.Data.ParameterDirection.Input;
                        par4.Value = momentoOrden;

                        SqlParameter par5 = _cmd.Parameters.Add(Constante.ID_TIPO_ORDEN, System.Data.SqlDbType.NVarChar, Constante._5);
                        par5.Direction = System.Data.ParameterDirection.Input;
                        par5.Value = idTipoOrden;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult, cancelToken);

                        if (_reader != null && !_reader.HasRows)
                        {
                            objetoOrdenBancariaDetalleMO.Codigo = Constante.CODIGO_OMISION;
                            objetoOrdenBancariaDetalleMO.Mensaje = Constante.MENSAJE_SIN_RESULTADOS;
                        }
                        else if (_reader != null && _reader.HasRows)
                        {
                            listaOrdenesBancariasDetalle = new List<OrdenBancariaDetalleMO>();

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                OrdenBancariaDetalleMO ordenBancariaDetalleMO = new OrdenBancariaDetalleMO();
                                ordenBancariaDetalleMO.IdOrdenBancariaDetalle = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                ordenBancariaDetalleMO.TipoTransferencia = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                ordenBancariaDetalleMO.FormaPago = await _reader.IsDBNullAsync(Constante._2, cancelToken) ? String.Empty : _reader.GetString(Constante._2);
                                ordenBancariaDetalleMO.SubTipoPago = await _reader.IsDBNullAsync(Constante._3, cancelToken) ? String.Empty : _reader.GetString(Constante._3);
                                ordenBancariaDetalleMO.Referencia1 = await _reader.IsDBNullAsync(Constante._4, cancelToken) ? String.Empty : _reader.GetString(Constante._4);
                                ordenBancariaDetalleMO.Referencia2 = await _reader.IsDBNullAsync(Constante._5, cancelToken) ? String.Empty : _reader.GetString(Constante._5);
                                ordenBancariaDetalleMO.MonedaCargo = await _reader.IsDBNullAsync(Constante._6, cancelToken) ? String.Empty : _reader.GetString(Constante._6);
                                ordenBancariaDetalleMO.MonedaCargoCorto = await _reader.IsDBNullAsync(Constante._7, cancelToken) ? String.Empty : _reader.GetString(Constante._7);
                                ordenBancariaDetalleMO.CuentaCargo = await _reader.IsDBNullAsync(Constante._8, cancelToken) ? String.Empty : _reader.GetString(Constante._8);
                                ordenBancariaDetalleMO.MontoCargo = await _reader.IsDBNullAsync(Constante._9, cancelToken) ? 0 : _reader.GetDecimal(Constante._9);
                                ordenBancariaDetalleMO.MonedaAbono = await _reader.IsDBNullAsync(Constante._10, cancelToken) ? String.Empty : _reader.GetString(Constante._10);
                                ordenBancariaDetalleMO.MonedaAbonoCorto = await _reader.IsDBNullAsync(Constante._11, cancelToken) ? String.Empty : _reader.GetString(Constante._11);
                                ordenBancariaDetalleMO.CuentaAbono = await _reader.IsDBNullAsync(Constante._12, cancelToken) ? String.Empty : _reader.GetString(Constante._12);
                                ordenBancariaDetalleMO.CuentaGasto = await _reader.IsDBNullAsync(Constante._13, cancelToken) ? String.Empty : _reader.GetString(Constante._13);
                                ordenBancariaDetalleMO.MontoAbono = await _reader.IsDBNullAsync(Constante._14, cancelToken) ? 0 : _reader.GetDecimal(Constante._14);
                                ordenBancariaDetalleMO.TipoCambio = await _reader.IsDBNullAsync(Constante._15, cancelToken) ? 0 : _reader.GetDecimal(Constante._15);
                                ordenBancariaDetalleMO.ModuloRaiz = await _reader.IsDBNullAsync(Constante._16, cancelToken) ? 0 : _reader.GetInt32(Constante._16);
                                ordenBancariaDetalleMO.DigitoControl = await _reader.IsDBNullAsync(Constante._17, cancelToken) ? 0 : _reader.GetInt32(Constante._17);
                                ordenBancariaDetalleMO.Indicador = await _reader.IsDBNullAsync(Constante._18, cancelToken) ? String.Empty : _reader.GetString(Constante._18);
                                ordenBancariaDetalleMO.NroOperacion = await _reader.IsDBNullAsync(Constante._19, cancelToken) ? String.Empty : _reader.GetString(Constante._19);
                                ordenBancariaDetalleMO.Beneficiario = await _reader.IsDBNullAsync(Constante._20, cancelToken) ? String.Empty : _reader.GetString(Constante._20);
                                ordenBancariaDetalleMO.TipoDocumento = await _reader.IsDBNullAsync(Constante._21, cancelToken) ? String.Empty : _reader.GetString(Constante._21);
                                ordenBancariaDetalleMO.TipoDocumentoCorto = await _reader.IsDBNullAsync(Constante._22, cancelToken) ? String.Empty : _reader.GetString(Constante._22);
                                ordenBancariaDetalleMO.NroDocumento = await _reader.IsDBNullAsync(Constante._23, cancelToken) ? String.Empty : _reader.GetString(Constante._23);
                                ordenBancariaDetalleMO.Correo = await _reader.IsDBNullAsync(Constante._24, cancelToken) ? String.Empty : _reader.GetString(Constante._24);
                                ordenBancariaDetalleMO.NombreBanco = await _reader.IsDBNullAsync(Constante._25, cancelToken) ? String.Empty : _reader.GetString(Constante._25);
                                ordenBancariaDetalleMO.RucBanco = await _reader.IsDBNullAsync(Constante._26, cancelToken) ? String.Empty : _reader.GetString(Constante._26);
                                ordenBancariaDetalleMO.NroFactura = await _reader.IsDBNullAsync(Constante._27, cancelToken) ? String.Empty : _reader.GetString(Constante._27);
                                ordenBancariaDetalleMO.FechaFactura = await _reader.IsDBNullAsync(Constante._28, cancelToken) ? String.Empty : _reader.GetString(Constante._28);
                                ordenBancariaDetalleMO.FechaFinFactura = await _reader.IsDBNullAsync(Constante._29, cancelToken) ? String.Empty : _reader.GetString(Constante._29);
                                ordenBancariaDetalleMO.SignoFactura = await _reader.IsDBNullAsync(Constante._30, cancelToken) ? String.Empty : _reader.GetString(Constante._30);
                                ordenBancariaDetalleMO.UsuarioCreacion = await _reader.IsDBNullAsync(Constante._31, cancelToken) ? String.Empty : _reader.GetString(Constante._31);
                                DateTime? fechaCreacion = await _reader.IsDBNullAsync(Constante._32, cancelToken) ? (DateTime?)null : _reader.GetDateTime(Constante._32);
                                ordenBancariaDetalleMO.FechaCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortDateString();
                                ordenBancariaDetalleMO.HoraCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortTimeString();
                                listaOrdenesBancariasDetalle.Add(ordenBancariaDetalleMO);
                            }

                            objetoOrdenBancariaDetalleMO.Codigo = Constante.CODIGO_OK;
                        }

                        _reader.Close();
                        _con.Close();
                        objetoOrdenBancariaDetalleMO.Codigo = _reader != null ? objetoOrdenBancariaDetalleMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoOrdenBancariaDetalleMO.Mensaje = _reader != null ? objetoOrdenBancariaDetalleMO.Mensaje: Constante.MENSAJE_RECARGAR_PAGINA;
                        objetoOrdenBancariaDetalleMO.ListaOrdenesBancariasDetalle = listaOrdenesBancariasDetalle;
                        String mensaje = _reader != null ? Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_ANULADAS_ASYNC_OK : Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_ANULADAS_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DETALLE_ANULADAS_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DETALLE_ANULADAS_ASYNC, Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_ANULADAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoOrdenBancariaDetalleMO;
        }

        public async Task<ObjetoOrdenBancariaMO> BuscarOrdenesBancariasAnuladasAsync(CancellationToken cancelToken, String idUsuario, String idSap, String fechaInicio, String fechaFin)
        {
            ObjetoOrdenBancariaMO objetoOrdenBancariaMO = new ObjetoOrdenBancariaMO();
            List<OrdenBancariaMO> listaOrdenesBancarias = null;
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_BUSCAR_ORDENES_BANCARIAS_ANULADAS, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        
                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_USUARIO, System.Data.SqlDbType.NChar, Constante._6);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idUsuario;

                        SqlParameter par2 = _cmd.Parameters.Add(Constante.ID_SAP, System.Data.SqlDbType.NVarChar, Constante._10);
                        par2.Direction = System.Data.ParameterDirection.Input;
                        par2.Value = idSap;

                        SqlParameter par3 = _cmd.Parameters.Add(Constante.FECHA_INICIO, System.Data.SqlDbType.NChar, Constante._10);
                        par3.Direction = System.Data.ParameterDirection.Input;
                        par3.Value = fechaInicio;

                        SqlParameter par4 = _cmd.Parameters.Add(Constante.FECHA_FIN, System.Data.SqlDbType.NChar, Constante._10);
                        par4.Direction = System.Data.ParameterDirection.Input;
                        par4.Value = fechaFin;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult, cancelToken);

                        if (_reader != null && !_reader.HasRows)
                        {
                            objetoOrdenBancariaMO.Codigo = Constante.CODIGO_OMISION;
                            objetoOrdenBancariaMO.Mensaje = Constante.MENSAJE_SIN_RESULTADOS;
                        }
                        else if (_reader != null && _reader.HasRows)
                        {
                            listaOrdenesBancarias = new List<OrdenBancariaMO>();

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                OrdenBancariaMO ordenBancariaMO = new OrdenBancariaMO();
                                ordenBancariaMO.IdOrdenBancaria = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                ordenBancariaMO.Banco = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                ordenBancariaMO.BancoCorto = await _reader.IsDBNullAsync(Constante._2, cancelToken) ? String.Empty : _reader.GetString(Constante._2);
                                ordenBancariaMO.IdTipoOrden = await _reader.IsDBNullAsync(Constante._3, cancelToken) ? String.Empty : _reader.GetString(Constante._3);
                                ordenBancariaMO.TipoOrden = await _reader.IsDBNullAsync(Constante._4, cancelToken) ? String.Empty : _reader.GetString(Constante._4);
                                ordenBancariaMO.TipoOrdenCorto = await _reader.IsDBNullAsync(Constante._5, cancelToken) ? String.Empty : _reader.GetString(Constante._5);
                                ordenBancariaMO.IdEstadoOrden = await _reader.IsDBNullAsync(Constante._6, cancelToken) ? String.Empty : _reader.GetString(Constante._6);
                                ordenBancariaMO.EstadoOrden = await _reader.IsDBNullAsync(Constante._7, cancelToken) ? String.Empty : _reader.GetString(Constante._7);
                                ordenBancariaMO.IdSociedad = await _reader.IsDBNullAsync(Constante._8, cancelToken) ? String.Empty : _reader.GetString(Constante._8);
                                ordenBancariaMO.Sociedad = await _reader.IsDBNullAsync(Constante._9, cancelToken) ? String.Empty : _reader.GetString(Constante._9);
                                ordenBancariaMO.SociedadCorto = await _reader.IsDBNullAsync(Constante._10, cancelToken) ? String.Empty : _reader.GetString(Constante._10);
                                ordenBancariaMO.IdSap = await _reader.IsDBNullAsync(Constante._11, cancelToken) ? String.Empty : _reader.GetString(Constante._11);
                                ordenBancariaMO.Anio = await _reader.IsDBNullAsync(Constante._12, cancelToken) ? String.Empty : _reader.GetString(Constante._12);
                                ordenBancariaMO.MomentoOrden = await _reader.IsDBNullAsync(Constante._13, cancelToken) ? String.Empty : _reader.GetString(Constante._13);
                                ordenBancariaMO.FechaOrden = await _reader.IsDBNullAsync(Constante._14, cancelToken) ? String.Empty : _reader.GetString(Constante._14);
                                ordenBancariaMO.NombreArchivo = await _reader.IsDBNullAsync(Constante._15, cancelToken) ? String.Empty : _reader.GetString(Constante._15);
                                ordenBancariaMO.RutaArchivo = await _reader.IsDBNullAsync(Constante._16, cancelToken) ? String.Empty : _reader.GetString(Constante._16);
                                ordenBancariaMO.MonedaLocal = await _reader.IsDBNullAsync(Constante._17, cancelToken) ? String.Empty : _reader.GetString(Constante._17);
                                ordenBancariaMO.MonedaCortoLocal = await _reader.IsDBNullAsync(Constante._18, cancelToken) ? String.Empty : _reader.GetString(Constante._18);
                                ordenBancariaMO.ImporteLocal = await _reader.IsDBNullAsync(Constante._19, cancelToken) ? 0 : _reader.GetDecimal(Constante._19);
                                ordenBancariaMO.MonedaForanea = await _reader.IsDBNullAsync(Constante._20, cancelToken) ? String.Empty : _reader.GetString(Constante._20);
                                ordenBancariaMO.MonedaCortoForanea = await _reader.IsDBNullAsync(Constante._21, cancelToken) ? String.Empty : _reader.GetString(Constante._21);
                                ordenBancariaMO.ImporteForanea = await _reader.IsDBNullAsync(Constante._22, cancelToken) ? 0 : _reader.GetDecimal(Constante._22);
                                ordenBancariaMO.Liberador = await _reader.IsDBNullAsync(Constante._23, cancelToken) ? String.Empty : _reader.GetString(Constante._23);
                                ordenBancariaMO.PreAprobador = await _reader.IsDBNullAsync(Constante._24, cancelToken) ? String.Empty : _reader.GetString(Constante._24);
                                ordenBancariaMO.Anulador = await _reader.IsDBNullAsync(Constante._25, cancelToken) ? String.Empty : _reader.GetString(Constante._25);
                                ordenBancariaMO.Comentarios = await _reader.IsDBNullAsync(Constante._26, cancelToken) ? String.Empty : _reader.GetString(Constante._26);
                                ordenBancariaMO.UsuarioCreacion = await _reader.IsDBNullAsync(Constante._27, cancelToken) ? String.Empty : _reader.GetString(Constante._27);
                                DateTime? fechaCreacion = await _reader.IsDBNullAsync(Constante._28, cancelToken) ? (DateTime?)null : _reader.GetDateTime(Constante._28);
                                ordenBancariaMO.FechaCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortDateString();
                                ordenBancariaMO.HoraCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortTimeString();
                                ordenBancariaMO.UsuarioEdicion = await _reader.IsDBNullAsync(Constante._29, cancelToken) ? String.Empty : _reader.GetString(Constante._29);
                                DateTime? fechaEdicion = await _reader.IsDBNullAsync(Constante._30, cancelToken) ? (DateTime?)null : _reader.GetDateTime(Constante._30);
                                ordenBancariaMO.FechaEdicion = fechaEdicion == null ? String.Empty : fechaEdicion.Value.ToShortDateString();
                                ordenBancariaMO.HoraEdicion = fechaEdicion == null ? String.Empty : fechaEdicion.Value.ToShortTimeString();
                                listaOrdenesBancarias.Add(ordenBancariaMO);
                            }

                            objetoOrdenBancariaMO.Codigo = Constante.CODIGO_OK;
                        }

                        _reader.Close();
                        _con.Close();
                        objetoOrdenBancariaMO.Codigo = _reader != null ? objetoOrdenBancariaMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoOrdenBancariaMO.Mensaje = _reader != null ? objetoOrdenBancariaMO.Mensaje: Constante.MENSAJE_RECARGAR_PAGINA;
                        objetoOrdenBancariaMO.ListaOrdenesBancarias = listaOrdenesBancarias;
                        String mensaje = _reader != null ? Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_ANULADAS_ASYNC_OK : Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_ANULADAS_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_BUSCAR_ORDENES_BANCARIAS_ANULADAS_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_BUSCAR_ORDENES_BANCARIAS_ANULADAS_ASYNC, Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_ANULADAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoOrdenBancariaMO;
        }

        public async Task<ObjetoOrdenBancariaMO> ConsultarEstadoOrdenBancariaAsync(CancellationToken cancelToken, String idSociedad, String idSap, String anio, String momentoOrden, String idTipoOrden)
        {
            ObjetoOrdenBancariaMO objetoOrdenBancariaMO = new ObjetoOrdenBancariaMO();
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_CONSULTAR_ESTADO_ORDEN_BANCARIA, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        
                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_SOCIEDAD, System.Data.SqlDbType.NChar, Constante._4);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idSociedad;

                        SqlParameter par2 = _cmd.Parameters.Add(Constante.ID_SAP, System.Data.SqlDbType.NVarChar, Constante._10);
                        par2.Direction = System.Data.ParameterDirection.Input;
                        par2.Value = idSap;

                        SqlParameter par3 = _cmd.Parameters.Add(Constante.ANIO, System.Data.SqlDbType.NChar, Constante._4);
                        par3.Direction = System.Data.ParameterDirection.Input;
                        par3.Value = anio;

                        SqlParameter par4 = _cmd.Parameters.Add(Constante.MOMENTO_ORDEN, System.Data.SqlDbType.NChar, Constante._8);
                        par4.Direction = System.Data.ParameterDirection.Input;
                        par4.Value = momentoOrden;

                        SqlParameter par5 = _cmd.Parameters.Add(Constante.ID_TIPO_ORDEN, System.Data.SqlDbType.NVarChar, Constante._5);
                        par5.Direction = System.Data.ParameterDirection.Input;
                        par5.Value = idTipoOrden;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult, cancelToken);

                        if (_reader != null && !_reader.HasRows)
                        {
                            objetoOrdenBancariaMO.Codigo = Constante.CODIGO_NO_OK;
                            objetoOrdenBancariaMO.Mensaje = Constante.MENSAJE_SIN_RESULTADOS;
                        }
                        else if (_reader != null && _reader.HasRows)
                        {
                            if (await _reader.ReadAsync(cancelToken))
                            {
                                objetoOrdenBancariaMO.IdEstadoOrden = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                objetoOrdenBancariaMO.EsHorarioBanco = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? false : _reader.GetBoolean(Constante._1);
                            }

                            objetoOrdenBancariaMO.Codigo = Constante.CODIGO_OK;
                        }

                        _reader.Close();
                        _con.Close();
                        objetoOrdenBancariaMO.Codigo = _reader != null ? objetoOrdenBancariaMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoOrdenBancariaMO.Mensaje = _reader != null ? objetoOrdenBancariaMO.Mensaje : Constante.MENSAJE_RECARGAR_PAGINA;
                        String mensaje = _reader != null ? Constante.MENSAJE_CONSULTAR_ESTADO_ORDEN_BANCARIA_ASYNC_OK : Constante.MENSAJE_CONSULTAR_ESTADO_ORDEN_BANCARIA_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_CONSULTAR_ESTADO_ORDEN_BANCARIA_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_CONSULTAR_ESTADO_ORDEN_BANCARIA_ASYNC, Constante.MENSAJE_CONSULTAR_ESTADO_ORDEN_BANCARIA_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoOrdenBancariaMO;
        }

        public async Task<ObjetoOrdenBancariaMO> ActualizarEstadoOrdenBancariaAsync(CancellationToken cancelToken, String idSociedad, String idSap, String anio, String momentoOrden, String idTipoOrden, String idUsuario, String idEstadoOrden, String comentarios)
        {
            ObjetoOrdenBancariaMO objetoOrdenBancariaMO = new ObjetoOrdenBancariaMO();
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_ACTUALIZAR_ESTADO_ORDEN_BANCARIA, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_SOCIEDAD, System.Data.SqlDbType.NChar, Constante._4);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idSociedad;

                        SqlParameter par2 = _cmd.Parameters.Add(Constante.ID_SAP, System.Data.SqlDbType.NVarChar, Constante._10);
                        par2.Direction = System.Data.ParameterDirection.Input;
                        par2.Value = idSap;

                        SqlParameter par3 = _cmd.Parameters.Add(Constante.ANIO, System.Data.SqlDbType.NChar, Constante._4);
                        par3.Direction = System.Data.ParameterDirection.Input;
                        par3.Value = anio;

                        SqlParameter par4 = _cmd.Parameters.Add(Constante.MOMENTO_ORDEN, System.Data.SqlDbType.NChar, Constante._8);
                        par4.Direction = System.Data.ParameterDirection.Input;
                        par4.Value = momentoOrden;

                        SqlParameter par5 = _cmd.Parameters.Add(Constante.ID_TIPO_ORDEN, System.Data.SqlDbType.NVarChar, Constante._5);
                        par5.Direction = System.Data.ParameterDirection.Input;
                        par5.Value = idTipoOrden;

                        SqlParameter par6 = _cmd.Parameters.Add(Constante.ID_USUARIO, System.Data.SqlDbType.NChar, Constante._6);
                        par6.Direction = System.Data.ParameterDirection.Input;
                        par6.Value = idUsuario;

                        SqlParameter par7 = _cmd.Parameters.Add(Constante.ID_ESTADO_ORDEN, System.Data.SqlDbType.NChar, Constante._2);
                        par7.Direction = System.Data.ParameterDirection.InputOutput;
                        par7.Value = idEstadoOrden;

                        SqlParameter par8 = _cmd.Parameters.Add(Constante.COMENTARIOS, System.Data.SqlDbType.NVarChar, Constante._300);
                        par8.Direction = System.Data.ParameterDirection.Input;
                        par8.Value = comentarios;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleRow, cancelToken);
                        
                        if (_reader != null && _reader.HasRows)
                        {
                            if (await _reader.ReadAsync(cancelToken))
                            {
                                objetoOrdenBancariaMO.Codigo = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? 0 : _reader.GetInt32(Constante._0);
                                objetoOrdenBancariaMO.Mensaje = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                            }
                        }

                        _reader.Close();
                        _con.Close();
                        objetoOrdenBancariaMO.Codigo = _reader != null ? objetoOrdenBancariaMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoOrdenBancariaMO.Mensaje = _reader != null ? objetoOrdenBancariaMO.Mensaje : Constante.MENSAJE_RECARGAR_PAGINA;
                        objetoOrdenBancariaMO.IdEstadoOrden = par7.Value == null ? String.Empty : par7.Value.ToString();
                        String mensaje = objetoOrdenBancariaMO.Codigo == Constante.CODIGO_OK ? Constante.MENSAJE_ACTUALIZAR_ESTADO_ORDEN_BANCARIA_ASYNC_OK : Constante.MENSAJE_ACTUALIZAR_ESTADO_ORDEN_BANCARIA_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_ACTUALIZAR_ESTADO_ORDEN_BANCARIA_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_ACTUALIZAR_ESTADO_ORDEN_BANCARIA_ASYNC, Constante.MENSAJE_ACTUALIZAR_ESTADO_ORDEN_BANCARIA_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoOrdenBancariaMO;
        }

        public async Task<ObjetoOrdenBancariaMO> ConsultarEstadoMasivoOrdenesBancariasAsync(CancellationToken cancelToken, String cadena)
        {
            ObjetoOrdenBancariaMO objetoOrdenBancariaMO = new ObjetoOrdenBancariaMO();
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_CONSULTAR_ESTADO_MASIVO_ORDENES_BANCARIAS, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        
                        SqlParameter par1 = _cmd.Parameters.Add(Constante.CADENA, System.Data.SqlDbType.NVarChar, Constante._4000);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = cadena;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult, cancelToken);

                        if (_reader != null && !_reader.HasRows)
                        {
                            objetoOrdenBancariaMO.Codigo = Constante.CODIGO_NO_OK;
                            objetoOrdenBancariaMO.Mensaje = Constante.MENSAJE_SIN_RESULTADOS;
                        }
                        else if (_reader != null && _reader.HasRows)
                        {
                            if (await _reader.ReadAsync(cancelToken))
                            {
                                objetoOrdenBancariaMO.EsEstadoLiberado = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? false : _reader.GetBoolean(Constante._0);
                                objetoOrdenBancariaMO.EsEstadoPreAprobado = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? false : _reader.GetBoolean(Constante._1);
                                objetoOrdenBancariaMO.EsHorarioBanco = await _reader.IsDBNullAsync(Constante._2, cancelToken) ? false : _reader.GetBoolean(Constante._2);
                            }

                            objetoOrdenBancariaMO.Codigo = Constante.CODIGO_OK;
                        }

                        _reader.Close();
                        _con.Close();
                        objetoOrdenBancariaMO.Codigo = _reader != null ? objetoOrdenBancariaMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoOrdenBancariaMO.Mensaje = _reader != null ? objetoOrdenBancariaMO.Mensaje : Constante.MENSAJE_RECARGAR_PAGINA;
                        String mensaje = _reader != null ? Constante.MENSAJE_CONSULTAR_ESTADO_MASIVO_ORDENES_BANCARIAS_ASYNC_OK : Constante.MENSAJE_CONSULTAR_ESTADO_MASIVO_ORDENES_BANCARIAS_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_CONSULTAR_ESTADO_MASIVO_ORDENES_BANCARIAS_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_CONSULTAR_ESTADO_MASIVO_ORDENES_BANCARIAS_ASYNC, Constante.MENSAJE_CONSULTAR_ESTADO_MASIVO_ORDENES_BANCARIAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoOrdenBancariaMO;
        }

        public async Task<ObjetoOrdenBancariaMO> ActualizarEstadoMasivoOrdenesBancariasAsync(CancellationToken cancelToken, String idSociedad, String idSap, String anio, String momentoOrden, String idTipoOrden, String idUsuario, String idEstadoOrden, String comentarios)
        {
            ObjetoOrdenBancariaMO objetoOrdenBancariaMO = new ObjetoOrdenBancariaMO();
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_ACTUALIZAR_ESTADO_MASIVO_ORDEN_BANCARIA, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_SOCIEDAD, System.Data.SqlDbType.NChar, Constante._4);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idSociedad;

                        SqlParameter par2 = _cmd.Parameters.Add(Constante.ID_SAP, System.Data.SqlDbType.NVarChar, Constante._10);
                        par2.Direction = System.Data.ParameterDirection.Input;
                        par2.Value = idSap;

                        SqlParameter par3 = _cmd.Parameters.Add(Constante.ANIO, System.Data.SqlDbType.NChar, Constante._4);
                        par3.Direction = System.Data.ParameterDirection.Input;
                        par3.Value = anio;

                        SqlParameter par4 = _cmd.Parameters.Add(Constante.MOMENTO_ORDEN, System.Data.SqlDbType.NChar, Constante._8);
                        par4.Direction = System.Data.ParameterDirection.Input;
                        par4.Value = momentoOrden;

                        SqlParameter par5 = _cmd.Parameters.Add(Constante.ID_TIPO_ORDEN, System.Data.SqlDbType.NVarChar, Constante._5);
                        par5.Direction = System.Data.ParameterDirection.Input;
                        par5.Value = idTipoOrden;

                        SqlParameter par6 = _cmd.Parameters.Add(Constante.ID_USUARIO, System.Data.SqlDbType.NChar, Constante._6);
                        par6.Direction = System.Data.ParameterDirection.Input;
                        par6.Value = idUsuario;

                        SqlParameter par7 = _cmd.Parameters.Add(Constante.ID_ESTADO_ORDEN, System.Data.SqlDbType.NChar, Constante._2);
                        par7.Direction = System.Data.ParameterDirection.InputOutput;
                        par7.Value = idEstadoOrden;

                        SqlParameter par8 = _cmd.Parameters.Add(Constante.COMENTARIOS, System.Data.SqlDbType.NVarChar, Constante._300);
                        par8.Direction = System.Data.ParameterDirection.Input;
                        par8.Value = comentarios;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleRow, cancelToken);
                        
                        if (_reader != null && _reader.HasRows)
                        {
                            if (await _reader.ReadAsync(cancelToken))
                            {
                                objetoOrdenBancariaMO.Codigo = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? 0 : _reader.GetInt32(Constante._0);
                                objetoOrdenBancariaMO.Mensaje = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                            }
                        }

                        _reader.Close();
                        _con.Close();
                        objetoOrdenBancariaMO.Codigo = _reader != null ? objetoOrdenBancariaMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoOrdenBancariaMO.Mensaje = _reader != null ? objetoOrdenBancariaMO.Mensaje : Constante.MENSAJE_RECARGAR_PAGINA;
                        objetoOrdenBancariaMO.IdEstadoOrden = par7.Value == null ? String.Empty : par7.Value.ToString();
                        String mensaje = objetoOrdenBancariaMO.Codigo == Constante.CODIGO_OK ? Constante.MENSAJE_ACTUALIZAR_ESTADO_MASIVO_ORDENES_BANCARIAS_ASYNC_OK : Constante.MENSAJE_ACTUALIZAR_ESTADO_MASIVO_ORDENES_BANCARIAS_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_ACTUALIZAR_ESTADO_MASIVO_ORDENES_BANCARIAS_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_ACTUALIZAR_ESTADO_MASIVO_ORDENES_BANCARIAS_ASYNC, Constante.MENSAJE_ACTUALIZAR_ESTADO_MASIVO_ORDENES_BANCARIAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoOrdenBancariaMO;
        }

        public async Task<ObjetoFlujoAprobacionMO> ListarFlujoAprobacionAsync(CancellationToken cancelToken, String idSociedad, String idSap, String anio, String momentoOrden, String idTipoOrden)
        {
            ObjetoFlujoAprobacionMO objetoFluoAprobacionMO = new ObjetoFlujoAprobacionMO();
            List<FlujoAprobacionMO> listaFlujoAprobacion = null;
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_LISTAR_FLUJO_APROBACION, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        
                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_SOCIEDAD, System.Data.SqlDbType.NChar, Constante._4);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idSociedad;

                        SqlParameter par2 = _cmd.Parameters.Add(Constante.ID_SAP, System.Data.SqlDbType.NVarChar, Constante._10);
                        par2.Direction = System.Data.ParameterDirection.Input;
                        par2.Value = idSap;

                        SqlParameter par3 = _cmd.Parameters.Add(Constante.ANIO, System.Data.SqlDbType.NChar, Constante._4);
                        par3.Direction = System.Data.ParameterDirection.Input;
                        par3.Value = anio;

                        SqlParameter par4 = _cmd.Parameters.Add(Constante.MOMENTO_ORDEN, System.Data.SqlDbType.NChar, Constante._8);
                        par4.Direction = System.Data.ParameterDirection.Input;
                        par4.Value = momentoOrden;

                        SqlParameter par5 = _cmd.Parameters.Add(Constante.ID_TIPO_ORDEN, System.Data.SqlDbType.NVarChar, Constante._5);
                        par5.Direction = System.Data.ParameterDirection.Input;
                        par5.Value = idTipoOrden;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult, cancelToken);

                        if (_reader != null && !_reader.HasRows)
                        {
                            objetoFluoAprobacionMO.Codigo = Constante.CODIGO_OMISION;
                            objetoFluoAprobacionMO.Mensaje = Constante.MENSAJE_SIN_RESULTADOS;
                        }
                        else if (_reader != null && _reader.HasRows)
                        {
                            listaFlujoAprobacion = new List<FlujoAprobacionMO>();

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                FlujoAprobacionMO flujoAprobacionMO = new FlujoAprobacionMO();
                                flujoAprobacionMO.IdFlujoAprobacion = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                flujoAprobacionMO.EstadoFlujo = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                flujoAprobacionMO.EstadoFlujoCorto = await _reader.IsDBNullAsync(Constante._2, cancelToken) ? String.Empty : _reader.GetString(Constante._2);
                                flujoAprobacionMO.Comentarios = await _reader.IsDBNullAsync(Constante._3, cancelToken) ? String.Empty : _reader.GetString(Constante._3);
                                flujoAprobacionMO.UsuarioCreacion = await _reader.IsDBNullAsync(Constante._4, cancelToken) ? String.Empty : _reader.GetString(Constante._4);
                                DateTime? fechaRegistro = await _reader.IsDBNullAsync(Constante._5, cancelToken) ? (DateTime?)null : _reader.GetDateTime(Constante._5);
                                flujoAprobacionMO.FechaCreacion = fechaRegistro == null ? String.Empty : fechaRegistro.Value.ToShortDateString();
                                flujoAprobacionMO.HoraCreacion = fechaRegistro == null ? String.Empty : fechaRegistro.Value.ToShortTimeString();
                                listaFlujoAprobacion.Add(flujoAprobacionMO);
                            }

                            objetoFluoAprobacionMO.Codigo = Constante.CODIGO_OK;
                        }

                        _reader.Close();
                        _con.Close();
                        objetoFluoAprobacionMO.Codigo = _reader != null ? objetoFluoAprobacionMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoFluoAprobacionMO.Mensaje = _reader != null ? objetoFluoAprobacionMO.Mensaje: Constante.MENSAJE_RECARGAR_PAGINA;
                        objetoFluoAprobacionMO.ListaFlujoAprobacion = listaFlujoAprobacion;
                        String mensaje = _reader != null ? Constante.MENSAJE_LISTAR_FLUJO_APROBACION_ASYNC_OK : Constante.MENSAJE_LISTAR_FLUJO_APROBACION_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_LISTAR_FLUJO_APROBACION_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_LISTAR_FLUJO_APROBACION_ASYNC, Constante.MENSAJE_LISTAR_FLUJO_APROBACION_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoFluoAprobacionMO;
        }

        public async Task<ObjetoOrdenBancariaMO> ListarOrdenesBancariasAprobadasTesoreriaAsync(CancellationToken cancelToken, String idUsuario, Int32 pagina, Int32 filas, String idSociedad, String idBanco, String idTipoOrden)
        {
            ObjetoOrdenBancariaMO objetoOrdenBancariaMO = new ObjetoOrdenBancariaMO();
            List<OrdenBancariaMO> listaOrdenesBancarias = null;
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_LISTAR_ORDENES_BANCARIAS_APROBADAS_TESORERIA, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_USUARIO, System.Data.SqlDbType.NChar, Constante._6);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idUsuario;
                        
                        SqlParameter par2 = _cmd.Parameters.Add(Constante.PAGINA, System.Data.SqlDbType.Int);
                        par2.Direction = System.Data.ParameterDirection.Input;
                        par2.Value = pagina;

                        SqlParameter par3 = _cmd.Parameters.Add(Constante.FILAS, System.Data.SqlDbType.Int);
                        par3.Direction = System.Data.ParameterDirection.Input;
                        par3.Value = filas;

                        SqlParameter par4 = _cmd.Parameters.Add(Constante.ID_SOCIEDAD, System.Data.SqlDbType.NChar, 4);
                        par4.Direction = System.Data.ParameterDirection.Input;
                        par4.Value = idSociedad;

                        SqlParameter par5 = _cmd.Parameters.Add(Constante.ID_BANCO, System.Data.SqlDbType.NChar, 5);
                        par5.Direction = System.Data.ParameterDirection.Input;
                        par5.Value = idBanco;

                        SqlParameter par6 = _cmd.Parameters.Add(Constante.ID_TIPO_ORDEN, System.Data.SqlDbType.NVarChar, 5);
                        par6.Direction = System.Data.ParameterDirection.Input;
                        par6.Value = idTipoOrden;

                        SqlParameter par7 = _cmd.Parameters.Add(Constante.TOTAL_REGISTROS, System.Data.SqlDbType.Int);
                        par7.Direction = System.Data.ParameterDirection.Output;
                        
                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult, cancelToken);

                        if (_reader != null && !_reader.HasRows)
                        {
                            objetoOrdenBancariaMO.Codigo = Constante.CODIGO_OMISION;
                            objetoOrdenBancariaMO.Mensaje = Constante.MENSAJE_SIN_RESULTADOS;
                        }
                        else if (_reader != null && _reader.HasRows)
                        {
                            listaOrdenesBancarias = new List<OrdenBancariaMO>();

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                OrdenBancariaMO ordenBancariaMO = new OrdenBancariaMO();
                                ordenBancariaMO.IdOrdenBancaria = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                ordenBancariaMO.Banco = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                ordenBancariaMO.BancoCorto = await _reader.IsDBNullAsync(Constante._2, cancelToken) ? String.Empty : _reader.GetString(Constante._2);
                                ordenBancariaMO.IdTipoOrden = await _reader.IsDBNullAsync(Constante._3, cancelToken) ? String.Empty : _reader.GetString(Constante._3);
                                ordenBancariaMO.TipoOrden = await _reader.IsDBNullAsync(Constante._4, cancelToken) ? String.Empty : _reader.GetString(Constante._4);
                                ordenBancariaMO.TipoOrdenCorto = await _reader.IsDBNullAsync(Constante._5, cancelToken) ? String.Empty : _reader.GetString(Constante._5);
                                ordenBancariaMO.IdEstadoOrden = await _reader.IsDBNullAsync(Constante._6, cancelToken) ? String.Empty : _reader.GetString(Constante._6);
                                ordenBancariaMO.EstadoOrden = await _reader.IsDBNullAsync(Constante._7, cancelToken) ? String.Empty : _reader.GetString(Constante._7);
                                ordenBancariaMO.IdSociedad = await _reader.IsDBNullAsync(Constante._8, cancelToken) ? String.Empty : _reader.GetString(Constante._8);
                                ordenBancariaMO.Sociedad = await _reader.IsDBNullAsync(Constante._9, cancelToken) ? String.Empty : _reader.GetString(Constante._9);
                                ordenBancariaMO.SociedadCorto = await _reader.IsDBNullAsync(Constante._10, cancelToken) ? String.Empty : _reader.GetString(Constante._10);
                                ordenBancariaMO.IdSap = await _reader.IsDBNullAsync(Constante._11, cancelToken) ? String.Empty : _reader.GetString(Constante._11);
                                ordenBancariaMO.Anio = await _reader.IsDBNullAsync(Constante._12, cancelToken) ? String.Empty : _reader.GetString(Constante._12);
                                ordenBancariaMO.MomentoOrden = await _reader.IsDBNullAsync(Constante._13, cancelToken) ? String.Empty : _reader.GetString(Constante._13);
                                ordenBancariaMO.FechaOrden = await _reader.IsDBNullAsync(Constante._14, cancelToken) ? String.Empty : _reader.GetString(Constante._14);
                                ordenBancariaMO.NombreArchivo = await _reader.IsDBNullAsync(Constante._15, cancelToken) ? String.Empty : _reader.GetString(Constante._15);
                                ordenBancariaMO.RutaArchivo = await _reader.IsDBNullAsync(Constante._16, cancelToken) ? String.Empty : _reader.GetString(Constante._16);
                                ordenBancariaMO.MonedaLocal = await _reader.IsDBNullAsync(Constante._17, cancelToken) ? String.Empty : _reader.GetString(Constante._17);
                                ordenBancariaMO.MonedaCortoLocal = await _reader.IsDBNullAsync(Constante._18, cancelToken) ? String.Empty : _reader.GetString(Constante._18);
                                ordenBancariaMO.ImporteLocal = await _reader.IsDBNullAsync(Constante._19, cancelToken) ? 0 : _reader.GetDecimal(Constante._19);
                                ordenBancariaMO.MonedaForanea = await _reader.IsDBNullAsync(Constante._20, cancelToken) ? String.Empty : _reader.GetString(Constante._20);
                                ordenBancariaMO.MonedaCortoForanea = await _reader.IsDBNullAsync(Constante._21, cancelToken) ? String.Empty : _reader.GetString(Constante._21);
                                ordenBancariaMO.ImporteForanea = await _reader.IsDBNullAsync(Constante._22, cancelToken) ? 0 : _reader.GetDecimal(Constante._22);
                                ordenBancariaMO.Liberador = await _reader.IsDBNullAsync(Constante._23, cancelToken) ? String.Empty : _reader.GetString(Constante._23);
                                ordenBancariaMO.PreAprobador = await _reader.IsDBNullAsync(Constante._24, cancelToken) ? String.Empty : _reader.GetString(Constante._24);
                                ordenBancariaMO.Aprobador = await _reader.IsDBNullAsync(Constante._25, cancelToken) ? String.Empty : _reader.GetString(Constante._25);
                                ordenBancariaMO.Comentarios = await _reader.IsDBNullAsync(Constante._26, cancelToken) ? String.Empty : _reader.GetString(Constante._26);
                                ordenBancariaMO.UsuarioCreacion = await _reader.IsDBNullAsync(Constante._27, cancelToken) ? String.Empty : _reader.GetString(Constante._27);
                                DateTime? fechaCreacion = await _reader.IsDBNullAsync(Constante._28, cancelToken) ? (DateTime?)null : _reader.GetDateTime(Constante._28);
                                ordenBancariaMO.FechaCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortDateString();
                                ordenBancariaMO.HoraCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortTimeString();
                                ordenBancariaMO.UsuarioEdicion = await _reader.IsDBNullAsync(Constante._29, cancelToken) ? String.Empty : _reader.GetString(Constante._29);
                                DateTime? fechaEdicion = await _reader.IsDBNullAsync(Constante._30, cancelToken) ? (DateTime?)null : _reader.GetDateTime(Constante._30);
                                ordenBancariaMO.FechaEdicion = fechaEdicion == null ? String.Empty : fechaEdicion.Value.ToShortDateString();
                                ordenBancariaMO.HoraEdicion = fechaEdicion == null ? String.Empty : fechaEdicion.Value.ToShortTimeString();
                                listaOrdenesBancarias.Add(ordenBancariaMO);
                            }

                            objetoOrdenBancariaMO.Codigo = Constante.CODIGO_OK;
                        }

                        _reader.Close();
                        _con.Close();
                        objetoOrdenBancariaMO.Codigo = _reader != null ? objetoOrdenBancariaMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoOrdenBancariaMO.Mensaje = _reader != null ? objetoOrdenBancariaMO.Mensaje: Constante.MENSAJE_RECARGAR_PAGINA;
                        objetoOrdenBancariaMO.ListaOrdenesBancarias = listaOrdenesBancarias;
                        objetoOrdenBancariaMO.TotalRegistros = par7.Value == null ? 0 : Convert.ToInt32(par7.Value);
                        String mensaje = _reader != null ? Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_APROBADAS_ASYNC_OK : Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_APROBADAS_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_APROBADAS_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_APROBADAS_ASYNC, Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_APROBADAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoOrdenBancariaMO;
        }

        public async Task<ObjetoOrdenBancariaDetalleMO> ListarOrdenesBancariasDetalleAprobadasTesoreriaAsync(CancellationToken cancelToken, String idSociedad, String idSap, String anio, String momentoOrden, String idTipoOrden)
        {
            ObjetoOrdenBancariaDetalleMO objetoOrdenBancariaDetalleMO = new ObjetoOrdenBancariaDetalleMO();
            List<OrdenBancariaDetalleMO> listaOrdenesBancariasDetalle = null;
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_LISTAR_ORDENES_BANCARIAS_DETALLE_APROBADAS_TESORERIA, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        
                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_SOCIEDAD, System.Data.SqlDbType.NChar, Constante._4);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idSociedad;

                        SqlParameter par2 = _cmd.Parameters.Add(Constante.ID_SAP, System.Data.SqlDbType.NVarChar, Constante._10);
                        par2.Direction = System.Data.ParameterDirection.Input;
                        par2.Value = idSap;

                        SqlParameter par3 = _cmd.Parameters.Add(Constante.ANIO, System.Data.SqlDbType.NChar, Constante._4);
                        par3.Direction = System.Data.ParameterDirection.Input;
                        par3.Value = anio;

                        SqlParameter par4 = _cmd.Parameters.Add(Constante.MOMENTO_ORDEN, System.Data.SqlDbType.NChar, Constante._8);
                        par4.Direction = System.Data.ParameterDirection.Input;
                        par4.Value = momentoOrden;

                        SqlParameter par5 = _cmd.Parameters.Add(Constante.ID_TIPO_ORDEN, System.Data.SqlDbType.NVarChar, Constante._5);
                        par5.Direction = System.Data.ParameterDirection.Input;
                        par5.Value = idTipoOrden;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult, cancelToken);

                        if (_reader != null && !_reader.HasRows)
                        {
                            objetoOrdenBancariaDetalleMO.Codigo = Constante.CODIGO_OMISION;
                            objetoOrdenBancariaDetalleMO.Mensaje = Constante.MENSAJE_SIN_RESULTADOS;
                        }
                        else if (_reader != null && _reader.HasRows)
                        {
                            listaOrdenesBancariasDetalle = new List<OrdenBancariaDetalleMO>();

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                OrdenBancariaDetalleMO ordenBancariaDetalleMO = new OrdenBancariaDetalleMO();
                                ordenBancariaDetalleMO.IdOrdenBancariaDetalle = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                ordenBancariaDetalleMO.TipoTransferencia = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                ordenBancariaDetalleMO.FormaPago = await _reader.IsDBNullAsync(Constante._2, cancelToken) ? String.Empty : _reader.GetString(Constante._2);
                                ordenBancariaDetalleMO.SubTipoPago = await _reader.IsDBNullAsync(Constante._3, cancelToken) ? String.Empty : _reader.GetString(Constante._3);
                                ordenBancariaDetalleMO.Referencia1 = await _reader.IsDBNullAsync(Constante._4, cancelToken) ? String.Empty : _reader.GetString(Constante._4);
                                ordenBancariaDetalleMO.Referencia2 = await _reader.IsDBNullAsync(Constante._5, cancelToken) ? String.Empty : _reader.GetString(Constante._5);
                                ordenBancariaDetalleMO.MonedaCargo = await _reader.IsDBNullAsync(Constante._6, cancelToken) ? String.Empty : _reader.GetString(Constante._6);
                                ordenBancariaDetalleMO.MonedaCargoCorto = await _reader.IsDBNullAsync(Constante._7, cancelToken) ? String.Empty : _reader.GetString(Constante._7);
                                ordenBancariaDetalleMO.CuentaCargo = await _reader.IsDBNullAsync(Constante._8, cancelToken) ? String.Empty : _reader.GetString(Constante._8);
                                ordenBancariaDetalleMO.MontoCargo = await _reader.IsDBNullAsync(Constante._9, cancelToken) ? 0 : _reader.GetDecimal(Constante._9);
                                ordenBancariaDetalleMO.MonedaAbono = await _reader.IsDBNullAsync(Constante._10, cancelToken) ? String.Empty : _reader.GetString(Constante._10);
                                ordenBancariaDetalleMO.MonedaAbonoCorto = await _reader.IsDBNullAsync(Constante._11, cancelToken) ? String.Empty : _reader.GetString(Constante._11);
                                ordenBancariaDetalleMO.CuentaAbono = await _reader.IsDBNullAsync(Constante._12, cancelToken) ? String.Empty : _reader.GetString(Constante._12);
                                ordenBancariaDetalleMO.CuentaGasto = await _reader.IsDBNullAsync(Constante._13, cancelToken) ? String.Empty : _reader.GetString(Constante._13);
                                ordenBancariaDetalleMO.MontoAbono = await _reader.IsDBNullAsync(Constante._14, cancelToken) ? 0 : _reader.GetDecimal(Constante._14);
                                ordenBancariaDetalleMO.TipoCambio = await _reader.IsDBNullAsync(Constante._15, cancelToken) ? 0 : _reader.GetDecimal(Constante._15);
                                ordenBancariaDetalleMO.ModuloRaiz = await _reader.IsDBNullAsync(Constante._16, cancelToken) ? 0 : _reader.GetInt32(Constante._16);
                                ordenBancariaDetalleMO.DigitoControl = await _reader.IsDBNullAsync(Constante._17, cancelToken) ? 0 : _reader.GetInt32(Constante._17);
                                ordenBancariaDetalleMO.Indicador = await _reader.IsDBNullAsync(Constante._18, cancelToken) ? String.Empty : _reader.GetString(Constante._18);
                                ordenBancariaDetalleMO.NroOperacion = await _reader.IsDBNullAsync(Constante._19, cancelToken) ? String.Empty : _reader.GetString(Constante._19);
                                ordenBancariaDetalleMO.Beneficiario = await _reader.IsDBNullAsync(Constante._20, cancelToken) ? String.Empty : _reader.GetString(Constante._20);
                                ordenBancariaDetalleMO.TipoDocumento = await _reader.IsDBNullAsync(Constante._21, cancelToken) ? String.Empty : _reader.GetString(Constante._21);
                                ordenBancariaDetalleMO.TipoDocumentoCorto = await _reader.IsDBNullAsync(Constante._22, cancelToken) ? String.Empty : _reader.GetString(Constante._22);
                                ordenBancariaDetalleMO.NroDocumento = await _reader.IsDBNullAsync(Constante._23, cancelToken) ? String.Empty : _reader.GetString(Constante._23);
                                ordenBancariaDetalleMO.Correo = await _reader.IsDBNullAsync(Constante._24, cancelToken) ? String.Empty : _reader.GetString(Constante._24);
                                ordenBancariaDetalleMO.NombreBanco = await _reader.IsDBNullAsync(Constante._25, cancelToken) ? String.Empty : _reader.GetString(Constante._25);
                                ordenBancariaDetalleMO.RucBanco = await _reader.IsDBNullAsync(Constante._26, cancelToken) ? String.Empty : _reader.GetString(Constante._26);
                                ordenBancariaDetalleMO.NroFactura = await _reader.IsDBNullAsync(Constante._27, cancelToken) ? String.Empty : _reader.GetString(Constante._27);
                                ordenBancariaDetalleMO.FechaFactura = await _reader.IsDBNullAsync(Constante._28, cancelToken) ? String.Empty : _reader.GetString(Constante._28);
                                ordenBancariaDetalleMO.FechaFinFactura = await _reader.IsDBNullAsync(Constante._29, cancelToken) ? String.Empty : _reader.GetString(Constante._29);
                                ordenBancariaDetalleMO.SignoFactura = await _reader.IsDBNullAsync(Constante._30, cancelToken) ? String.Empty : _reader.GetString(Constante._30);
                                ordenBancariaDetalleMO.IdRespuesta = await _reader.IsDBNullAsync(Constante._31, cancelToken) ? String.Empty : _reader.GetString(Constante._31);
                                ordenBancariaDetalleMO.Respuesta = await _reader.IsDBNullAsync(Constante._32, cancelToken) ? String.Empty : _reader.GetString(Constante._32);
                                ordenBancariaDetalleMO.NroOrden = await _reader.IsDBNullAsync(Constante._33, cancelToken) ? String.Empty : _reader.GetString(Constante._33);
                                ordenBancariaDetalleMO.NroConvenio = await _reader.IsDBNullAsync(Constante._34, cancelToken) ? String.Empty : _reader.GetString(Constante._34);
                                ordenBancariaDetalleMO.UsuarioCreacion = await _reader.IsDBNullAsync(Constante._35, cancelToken) ? String.Empty : _reader.GetString(Constante._35);
                                DateTime? fechaCreacion = await _reader.IsDBNullAsync(Constante._36, cancelToken) ? (DateTime?)null : _reader.GetDateTime(Constante._36);
                                ordenBancariaDetalleMO.FechaCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortDateString();
                                ordenBancariaDetalleMO.HoraCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortTimeString();
                                listaOrdenesBancariasDetalle.Add(ordenBancariaDetalleMO);
                            }

                            objetoOrdenBancariaDetalleMO.Codigo = Constante.CODIGO_OK;
                        }

                        _reader.Close();
                        _con.Close();
                        objetoOrdenBancariaDetalleMO.Codigo = _reader != null ? objetoOrdenBancariaDetalleMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoOrdenBancariaDetalleMO.Mensaje = _reader != null ? objetoOrdenBancariaDetalleMO.Mensaje: Constante.MENSAJE_RECARGAR_PAGINA;
                        objetoOrdenBancariaDetalleMO.ListaOrdenesBancariasDetalle = listaOrdenesBancariasDetalle;
                        String mensaje = _reader != null ? Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_APROBADAS_ASYNC_OK : Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_APROBADAS_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DETALLE_APROBADAS_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DETALLE_APROBADAS_ASYNC, Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_APROBADAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoOrdenBancariaDetalleMO;
        }

        public async Task<ObjetoOrdenBancariaMO> BuscarOrdenesBancariasAprobadasTesoreriaAsync(CancellationToken cancelToken, String idUsuario, String idSap, String fechaInicio, String fechaFin)
        {
            ObjetoOrdenBancariaMO objetoOrdenBancariaMO = new ObjetoOrdenBancariaMO();
            List<OrdenBancariaMO> listaOrdenesBancarias = null;
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_BUSCAR_ORDENES_BANCARIAS_APROBADAS_TESORERIA, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_USUARIO, System.Data.SqlDbType.NChar, Constante._6);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idUsuario;
                        
                        SqlParameter par2 = _cmd.Parameters.Add(Constante.ID_SAP, System.Data.SqlDbType.NVarChar, Constante._10);
                        par2.Direction = System.Data.ParameterDirection.Input;
                        par2.Value = idSap;

                        SqlParameter par3 = _cmd.Parameters.Add(Constante.FECHA_INICIO, System.Data.SqlDbType.NChar, Constante._10);
                        par3.Direction = System.Data.ParameterDirection.Input;
                        par3.Value = fechaInicio;

                        SqlParameter par4 = _cmd.Parameters.Add(Constante.FECHA_FIN, System.Data.SqlDbType.NChar, Constante._10);
                        par4.Direction = System.Data.ParameterDirection.Input;
                        par4.Value = fechaFin;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult, cancelToken);

                        if (_reader != null && !_reader.HasRows)
                        {
                            objetoOrdenBancariaMO.Codigo = Constante.CODIGO_OMISION;
                            objetoOrdenBancariaMO.Mensaje = Constante.MENSAJE_SIN_RESULTADOS;
                        }
                        else if (_reader != null && _reader.HasRows)
                        {
                            listaOrdenesBancarias = new List<OrdenBancariaMO>();

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                OrdenBancariaMO ordenBancariaMO = new OrdenBancariaMO();
                                ordenBancariaMO.IdOrdenBancaria = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                ordenBancariaMO.Banco = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                ordenBancariaMO.BancoCorto = await _reader.IsDBNullAsync(Constante._2, cancelToken) ? String.Empty : _reader.GetString(Constante._2);
                                ordenBancariaMO.IdTipoOrden = await _reader.IsDBNullAsync(Constante._3, cancelToken) ? String.Empty : _reader.GetString(Constante._3);
                                ordenBancariaMO.TipoOrden = await _reader.IsDBNullAsync(Constante._4, cancelToken) ? String.Empty : _reader.GetString(Constante._4);
                                ordenBancariaMO.TipoOrdenCorto = await _reader.IsDBNullAsync(Constante._5, cancelToken) ? String.Empty : _reader.GetString(Constante._5);
                                ordenBancariaMO.IdEstadoOrden = await _reader.IsDBNullAsync(Constante._6, cancelToken) ? String.Empty : _reader.GetString(Constante._6);
                                ordenBancariaMO.EstadoOrden = await _reader.IsDBNullAsync(Constante._7, cancelToken) ? String.Empty : _reader.GetString(Constante._7);
                                ordenBancariaMO.IdSociedad = await _reader.IsDBNullAsync(Constante._8, cancelToken) ? String.Empty : _reader.GetString(Constante._8);
                                ordenBancariaMO.Sociedad = await _reader.IsDBNullAsync(Constante._9, cancelToken) ? String.Empty : _reader.GetString(Constante._9);
                                ordenBancariaMO.SociedadCorto = await _reader.IsDBNullAsync(Constante._10, cancelToken) ? String.Empty : _reader.GetString(Constante._10);
                                ordenBancariaMO.IdSap = await _reader.IsDBNullAsync(Constante._11, cancelToken) ? String.Empty : _reader.GetString(Constante._11);
                                ordenBancariaMO.Anio = await _reader.IsDBNullAsync(Constante._12, cancelToken) ? String.Empty : _reader.GetString(Constante._12);
                                ordenBancariaMO.MomentoOrden = await _reader.IsDBNullAsync(Constante._13, cancelToken) ? String.Empty : _reader.GetString(Constante._13);
                                ordenBancariaMO.FechaOrden = await _reader.IsDBNullAsync(Constante._14, cancelToken) ? String.Empty : _reader.GetString(Constante._14);
                                ordenBancariaMO.NombreArchivo = await _reader.IsDBNullAsync(Constante._15, cancelToken) ? String.Empty : _reader.GetString(Constante._15);
                                ordenBancariaMO.RutaArchivo = await _reader.IsDBNullAsync(Constante._16, cancelToken) ? String.Empty : _reader.GetString(Constante._16);
                                ordenBancariaMO.MonedaLocal = await _reader.IsDBNullAsync(Constante._17, cancelToken) ? String.Empty : _reader.GetString(Constante._17);
                                ordenBancariaMO.MonedaCortoLocal = await _reader.IsDBNullAsync(Constante._18, cancelToken) ? String.Empty : _reader.GetString(Constante._18);
                                ordenBancariaMO.ImporteLocal = await _reader.IsDBNullAsync(Constante._19, cancelToken) ? 0 : _reader.GetDecimal(Constante._19);
                                ordenBancariaMO.MonedaForanea = await _reader.IsDBNullAsync(Constante._20, cancelToken) ? String.Empty : _reader.GetString(Constante._20);
                                ordenBancariaMO.MonedaCortoForanea = await _reader.IsDBNullAsync(Constante._21, cancelToken) ? String.Empty : _reader.GetString(Constante._21);
                                ordenBancariaMO.ImporteForanea = await _reader.IsDBNullAsync(Constante._22, cancelToken) ? 0 : _reader.GetDecimal(Constante._22);
                                ordenBancariaMO.Liberador = await _reader.IsDBNullAsync(Constante._23, cancelToken) ? String.Empty : _reader.GetString(Constante._23);
                                ordenBancariaMO.PreAprobador = await _reader.IsDBNullAsync(Constante._24, cancelToken) ? String.Empty : _reader.GetString(Constante._24);
                                ordenBancariaMO.Aprobador = await _reader.IsDBNullAsync(Constante._25, cancelToken) ? String.Empty : _reader.GetString(Constante._25);
                                ordenBancariaMO.Comentarios = await _reader.IsDBNullAsync(Constante._26, cancelToken) ? String.Empty : _reader.GetString(Constante._26);
                                ordenBancariaMO.UsuarioCreacion = await _reader.IsDBNullAsync(Constante._27, cancelToken) ? String.Empty : _reader.GetString(Constante._27);
                                DateTime? fechaCreacion = await _reader.IsDBNullAsync(Constante._28, cancelToken) ? (DateTime?)null : _reader.GetDateTime(Constante._28);
                                ordenBancariaMO.FechaCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortDateString();
                                ordenBancariaMO.HoraCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortTimeString();
                                ordenBancariaMO.UsuarioEdicion = await _reader.IsDBNullAsync(Constante._29, cancelToken) ? String.Empty : _reader.GetString(Constante._29);
                                DateTime? fechaEdicion = await _reader.IsDBNullAsync(Constante._30, cancelToken) ? (DateTime?)null : _reader.GetDateTime(Constante._30);
                                ordenBancariaMO.FechaEdicion = fechaEdicion == null ? String.Empty : fechaEdicion.Value.ToShortDateString();
                                ordenBancariaMO.HoraEdicion = fechaEdicion == null ? String.Empty : fechaEdicion.Value.ToShortTimeString();
                                listaOrdenesBancarias.Add(ordenBancariaMO);
                            }

                            objetoOrdenBancariaMO.Codigo = Constante.CODIGO_OK;
                        }

                        _reader.Close();
                        _con.Close();
                        objetoOrdenBancariaMO.Codigo = _reader != null ? objetoOrdenBancariaMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoOrdenBancariaMO.Mensaje = _reader != null ? objetoOrdenBancariaMO.Mensaje: Constante.MENSAJE_RECARGAR_PAGINA;
                        objetoOrdenBancariaMO.ListaOrdenesBancarias = listaOrdenesBancarias;
                        String mensaje = _reader != null ? Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_APROBADAS_ASYNC_OK : Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_APROBADAS_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_BUSCAR_ORDENES_BANCARIAS_APROBADAS_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_BUSCAR_ORDENES_BANCARIAS_APROBADAS_ASYNC, Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_APROBADAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoOrdenBancariaMO;
        }

        public async Task<ObjetoOrdenBancariaMO> ListarOrdenesBancariasDiariasAsync(CancellationToken cancelToken, String idUsuario, Int32 pagina, Int32 filas, String idSociedad, String idBanco, String idTipoOrden, String idEstadoOrden)
        {
            ObjetoOrdenBancariaMO objetoOrdenBancariaMO = new ObjetoOrdenBancariaMO();
            List<OrdenBancariaMO> listaOrdenesBancarias = null;
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_LISTAR_ORDENES_BANCARIAS_DIARIAS, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_USUARIO, System.Data.SqlDbType.NChar, Constante._6);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idUsuario;
                        
                        SqlParameter par2 = _cmd.Parameters.Add(Constante.PAGINA, System.Data.SqlDbType.Int);
                        par2.Direction = System.Data.ParameterDirection.Input;
                        par2.Value = pagina;

                        SqlParameter par3 = _cmd.Parameters.Add(Constante.FILAS, System.Data.SqlDbType.Int);
                        par3.Direction = System.Data.ParameterDirection.Input;
                        par3.Value = filas;

                        SqlParameter par4 = _cmd.Parameters.Add(Constante.ID_SOCIEDAD, System.Data.SqlDbType.NChar, Constante._4);
                        par4.Direction = System.Data.ParameterDirection.Input;
                        par4.Value = idSociedad;

                        SqlParameter par5 = _cmd.Parameters.Add(Constante.ID_BANCO, System.Data.SqlDbType.NChar, Constante._5);
                        par5.Direction = System.Data.ParameterDirection.Input;
                        par5.Value = idBanco;

                        SqlParameter par6 = _cmd.Parameters.Add(Constante.ID_TIPO_ORDEN, System.Data.SqlDbType.NVarChar, Constante._5);
                        par6.Direction = System.Data.ParameterDirection.Input;
                        par6.Value = idTipoOrden;

                        SqlParameter par7 = _cmd.Parameters.Add(Constante.ID_ESTADO_ORDEN, System.Data.SqlDbType.NChar, Constante._2);
                        par7.Direction = System.Data.ParameterDirection.Input;
                        par7.Value = idEstadoOrden;

                        SqlParameter par8 = _cmd.Parameters.Add(Constante.TOTAL_REGISTROS, System.Data.SqlDbType.Int);
                        par8.Direction = System.Data.ParameterDirection.Output;
                        
                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult, cancelToken);

                        if (_reader != null && !_reader.HasRows)
                        {
                            objetoOrdenBancariaMO.Codigo = Constante.CODIGO_OMISION;
                            objetoOrdenBancariaMO.Mensaje = Constante.MENSAJE_SIN_RESULTADOS;
                        }
                        else if (_reader != null && _reader.HasRows)
                        {
                            listaOrdenesBancarias = new List<OrdenBancariaMO>();

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                OrdenBancariaMO ordenBancariaMO = new OrdenBancariaMO();
                                ordenBancariaMO.IdOrdenBancaria = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                ordenBancariaMO.Banco = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                ordenBancariaMO.BancoCorto = await _reader.IsDBNullAsync(Constante._2, cancelToken) ? String.Empty : _reader.GetString(Constante._2);
                                ordenBancariaMO.IdTipoOrden = await _reader.IsDBNullAsync(Constante._3, cancelToken) ? String.Empty : _reader.GetString(Constante._3);
                                ordenBancariaMO.TipoOrden = await _reader.IsDBNullAsync(Constante._4, cancelToken) ? String.Empty : _reader.GetString(Constante._4);
                                ordenBancariaMO.TipoOrdenCorto = await _reader.IsDBNullAsync(Constante._5, cancelToken) ? String.Empty : _reader.GetString(Constante._5);
                                ordenBancariaMO.IdEstadoOrden = await _reader.IsDBNullAsync(Constante._6, cancelToken) ? String.Empty : _reader.GetString(Constante._6);
                                ordenBancariaMO.EstadoOrden = await _reader.IsDBNullAsync(Constante._7, cancelToken) ? String.Empty : _reader.GetString(Constante._7);
                                ordenBancariaMO.IdSociedad = await _reader.IsDBNullAsync(Constante._8, cancelToken) ? String.Empty : _reader.GetString(Constante._8);
                                ordenBancariaMO.Sociedad = await _reader.IsDBNullAsync(Constante._9, cancelToken) ? String.Empty : _reader.GetString(Constante._9);
                                ordenBancariaMO.SociedadCorto = await _reader.IsDBNullAsync(Constante._10, cancelToken) ? String.Empty : _reader.GetString(Constante._10);
                                ordenBancariaMO.IdSap = await _reader.IsDBNullAsync(Constante._11, cancelToken) ? String.Empty : _reader.GetString(Constante._11);
                                ordenBancariaMO.Anio = await _reader.IsDBNullAsync(Constante._12, cancelToken) ? String.Empty : _reader.GetString(Constante._12);
                                ordenBancariaMO.MomentoOrden = await _reader.IsDBNullAsync(Constante._13, cancelToken) ? String.Empty : _reader.GetString(Constante._13);
                                ordenBancariaMO.FechaOrden = await _reader.IsDBNullAsync(Constante._14, cancelToken) ? String.Empty : _reader.GetString(Constante._14);
                                ordenBancariaMO.NombreArchivo = await _reader.IsDBNullAsync(Constante._15, cancelToken) ? String.Empty : _reader.GetString(Constante._15);
                                ordenBancariaMO.RutaArchivo = await _reader.IsDBNullAsync(Constante._16, cancelToken) ? String.Empty : _reader.GetString(Constante._16);
                                ordenBancariaMO.MonedaLocal = await _reader.IsDBNullAsync(Constante._17, cancelToken) ? String.Empty : _reader.GetString(Constante._17);
                                ordenBancariaMO.MonedaCortoLocal = await _reader.IsDBNullAsync(Constante._18, cancelToken) ? String.Empty : _reader.GetString(Constante._18);
                                ordenBancariaMO.ImporteLocal = await _reader.IsDBNullAsync(Constante._19, cancelToken) ? 0 : _reader.GetDecimal(Constante._19);
                                ordenBancariaMO.MonedaForanea = await _reader.IsDBNullAsync(Constante._20, cancelToken) ? String.Empty : _reader.GetString(Constante._20);
                                ordenBancariaMO.MonedaCortoForanea = await _reader.IsDBNullAsync(Constante._21, cancelToken) ? String.Empty : _reader.GetString(Constante._21);
                                ordenBancariaMO.ImporteForanea = await _reader.IsDBNullAsync(Constante._22, cancelToken) ? 0 : _reader.GetDecimal(Constante._22);
                                ordenBancariaMO.Liberador = await _reader.IsDBNullAsync(Constante._23, cancelToken) ? String.Empty : _reader.GetString(Constante._23);
                                ordenBancariaMO.PreAprobador = await _reader.IsDBNullAsync(Constante._24, cancelToken) ? String.Empty : _reader.GetString(Constante._24);
                                ordenBancariaMO.Aprobador = await _reader.IsDBNullAsync(Constante._25, cancelToken) ? String.Empty : _reader.GetString(Constante._25);
                                ordenBancariaMO.Comentarios = await _reader.IsDBNullAsync(Constante._26, cancelToken) ? String.Empty : _reader.GetString(Constante._26);
                                ordenBancariaMO.UsuarioCreacion = await _reader.IsDBNullAsync(Constante._27, cancelToken) ? String.Empty : _reader.GetString(Constante._27);
                                DateTime? fechaCreacion = await _reader.IsDBNullAsync(Constante._28, cancelToken) ? (DateTime?)null : _reader.GetDateTime(Constante._28);
                                ordenBancariaMO.FechaCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortDateString();
                                ordenBancariaMO.HoraCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortTimeString();
                                ordenBancariaMO.UsuarioEdicion = await _reader.IsDBNullAsync(Constante._29, cancelToken) ? String.Empty : _reader.GetString(Constante._29);
                                DateTime? fechaEdicion = await _reader.IsDBNullAsync(Constante._30, cancelToken) ? (DateTime?)null : _reader.GetDateTime(Constante._30);
                                ordenBancariaMO.FechaEdicion = fechaEdicion == null ? String.Empty : fechaEdicion.Value.ToShortDateString();
                                ordenBancariaMO.HoraEdicion = fechaEdicion == null ? String.Empty : fechaEdicion.Value.ToShortTimeString();
                                listaOrdenesBancarias.Add(ordenBancariaMO);
                            }

                            objetoOrdenBancariaMO.Codigo = Constante.CODIGO_OK;
                        }

                        _reader.Close();
                        _con.Close();
                        objetoOrdenBancariaMO.Codigo = _reader != null ? objetoOrdenBancariaMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoOrdenBancariaMO.Mensaje = _reader != null ? objetoOrdenBancariaMO.Mensaje: Constante.MENSAJE_RECARGAR_PAGINA;
                        objetoOrdenBancariaMO.ListaOrdenesBancarias = listaOrdenesBancarias;
                        objetoOrdenBancariaMO.TotalRegistros = par8.Value == null ? 0 : Convert.ToInt32(par8.Value);
                        String mensaje = _reader != null ? Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DIARIAS_ASYNC_OK : Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DIARIAS_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DIARIAS_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DIARIAS_ASYNC, Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DIARIAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoOrdenBancariaMO;
        }

        public async Task<ObjetoOrdenBancariaDetalleMO> ListarOrdenesBancariasDetalleDiariasAsync(CancellationToken cancelToken, String idSociedad, String idSap, String anio, String momentoOrden, String idTipoOrden)
        {
            ObjetoOrdenBancariaDetalleMO objetoOrdenBancariaDetalleMO = new ObjetoOrdenBancariaDetalleMO();
            List<OrdenBancariaDetalleMO> listaOrdenesBancariasDetalle = null;
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_LISTAR_ORDENES_BANCARIAS_DETALLE_DIARIAS, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        
                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_SOCIEDAD, System.Data.SqlDbType.NChar, Constante._4);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idSociedad;

                        SqlParameter par2 = _cmd.Parameters.Add(Constante.ID_SAP, System.Data.SqlDbType.NVarChar, Constante._10);
                        par2.Direction = System.Data.ParameterDirection.Input;
                        par2.Value = idSap;

                        SqlParameter par3 = _cmd.Parameters.Add(Constante.ANIO, System.Data.SqlDbType.NChar, Constante._4);
                        par3.Direction = System.Data.ParameterDirection.Input;
                        par3.Value = anio;

                        SqlParameter par4 = _cmd.Parameters.Add(Constante.MOMENTO_ORDEN, System.Data.SqlDbType.NChar, Constante._8);
                        par4.Direction = System.Data.ParameterDirection.Input;
                        par4.Value = momentoOrden;

                        SqlParameter par5 = _cmd.Parameters.Add(Constante.ID_TIPO_ORDEN, System.Data.SqlDbType.NVarChar, Constante._5);
                        par5.Direction = System.Data.ParameterDirection.Input;
                        par5.Value = idTipoOrden;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult, cancelToken);

                        if (_reader != null && !_reader.HasRows)
                        {
                            objetoOrdenBancariaDetalleMO.Codigo = Constante.CODIGO_OMISION;
                            objetoOrdenBancariaDetalleMO.Mensaje = Constante.MENSAJE_SIN_RESULTADOS;
                        }
                        else if (_reader != null && _reader.HasRows)
                        {
                            listaOrdenesBancariasDetalle = new List<OrdenBancariaDetalleMO>();

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                OrdenBancariaDetalleMO ordenBancariaDetalleMO = new OrdenBancariaDetalleMO();
                                ordenBancariaDetalleMO.IdOrdenBancariaDetalle = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                ordenBancariaDetalleMO.TipoTransferencia = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                ordenBancariaDetalleMO.FormaPago = await _reader.IsDBNullAsync(Constante._2, cancelToken) ? String.Empty : _reader.GetString(Constante._2);
                                ordenBancariaDetalleMO.SubTipoPago = await _reader.IsDBNullAsync(Constante._3, cancelToken) ? String.Empty : _reader.GetString(Constante._3);
                                ordenBancariaDetalleMO.Referencia1 = await _reader.IsDBNullAsync(Constante._4, cancelToken) ? String.Empty : _reader.GetString(Constante._4);
                                ordenBancariaDetalleMO.Referencia2 = await _reader.IsDBNullAsync(Constante._5, cancelToken) ? String.Empty : _reader.GetString(Constante._5);
                                ordenBancariaDetalleMO.MonedaCargo = await _reader.IsDBNullAsync(Constante._6, cancelToken) ? String.Empty : _reader.GetString(Constante._6);
                                ordenBancariaDetalleMO.MonedaCargoCorto = await _reader.IsDBNullAsync(Constante._7, cancelToken) ? String.Empty : _reader.GetString(Constante._7);
                                ordenBancariaDetalleMO.CuentaCargo = await _reader.IsDBNullAsync(Constante._8, cancelToken) ? String.Empty : _reader.GetString(Constante._8);
                                ordenBancariaDetalleMO.MontoCargo = await _reader.IsDBNullAsync(Constante._9, cancelToken) ? 0 : _reader.GetDecimal(Constante._9);
                                ordenBancariaDetalleMO.MonedaAbono = await _reader.IsDBNullAsync(Constante._10, cancelToken) ? String.Empty : _reader.GetString(Constante._10);
                                ordenBancariaDetalleMO.MonedaAbonoCorto = await _reader.IsDBNullAsync(Constante._11, cancelToken) ? String.Empty : _reader.GetString(Constante._11);
                                ordenBancariaDetalleMO.CuentaAbono = await _reader.IsDBNullAsync(Constante._12, cancelToken) ? String.Empty : _reader.GetString(Constante._12);
                                ordenBancariaDetalleMO.CuentaGasto = await _reader.IsDBNullAsync(Constante._13, cancelToken) ? String.Empty : _reader.GetString(Constante._13);
                                ordenBancariaDetalleMO.MontoAbono = await _reader.IsDBNullAsync(Constante._14, cancelToken) ? 0 : _reader.GetDecimal(Constante._14);
                                ordenBancariaDetalleMO.TipoCambio = await _reader.IsDBNullAsync(Constante._15, cancelToken) ? 0 : _reader.GetDecimal(Constante._15);
                                ordenBancariaDetalleMO.ModuloRaiz = await _reader.IsDBNullAsync(Constante._16, cancelToken) ? 0 : _reader.GetInt32(Constante._16);
                                ordenBancariaDetalleMO.DigitoControl = await _reader.IsDBNullAsync(Constante._17, cancelToken) ? 0 : _reader.GetInt32(Constante._17);
                                ordenBancariaDetalleMO.Indicador = await _reader.IsDBNullAsync(Constante._18, cancelToken) ? String.Empty : _reader.GetString(Constante._18);
                                ordenBancariaDetalleMO.NroOperacion = await _reader.IsDBNullAsync(Constante._19, cancelToken) ? String.Empty : _reader.GetString(Constante._19);
                                ordenBancariaDetalleMO.Beneficiario = await _reader.IsDBNullAsync(Constante._20, cancelToken) ? String.Empty : _reader.GetString(Constante._20);
                                ordenBancariaDetalleMO.TipoDocumento = await _reader.IsDBNullAsync(Constante._21, cancelToken) ? String.Empty : _reader.GetString(Constante._21);
                                ordenBancariaDetalleMO.TipoDocumentoCorto = await _reader.IsDBNullAsync(Constante._22, cancelToken) ? String.Empty : _reader.GetString(Constante._22);
                                ordenBancariaDetalleMO.NroDocumento = await _reader.IsDBNullAsync(Constante._23, cancelToken) ? String.Empty : _reader.GetString(Constante._23);
                                ordenBancariaDetalleMO.Correo = await _reader.IsDBNullAsync(Constante._24, cancelToken) ? String.Empty : _reader.GetString(Constante._24);
                                ordenBancariaDetalleMO.NombreBanco = await _reader.IsDBNullAsync(Constante._25, cancelToken) ? String.Empty : _reader.GetString(Constante._25);
                                ordenBancariaDetalleMO.RucBanco = await _reader.IsDBNullAsync(Constante._26, cancelToken) ? String.Empty : _reader.GetString(Constante._26);
                                ordenBancariaDetalleMO.NroFactura = await _reader.IsDBNullAsync(Constante._27, cancelToken) ? String.Empty : _reader.GetString(Constante._27);
                                ordenBancariaDetalleMO.FechaFactura = await _reader.IsDBNullAsync(Constante._28, cancelToken) ? String.Empty : _reader.GetString(Constante._28);
                                ordenBancariaDetalleMO.FechaFinFactura = await _reader.IsDBNullAsync(Constante._29, cancelToken) ? String.Empty : _reader.GetString(Constante._29);
                                ordenBancariaDetalleMO.SignoFactura = await _reader.IsDBNullAsync(Constante._30, cancelToken) ? String.Empty : _reader.GetString(Constante._30);
                                ordenBancariaDetalleMO.IdRespuesta = await _reader.IsDBNullAsync(Constante._31, cancelToken) ? String.Empty : _reader.GetString(Constante._31);
                                ordenBancariaDetalleMO.Respuesta = await _reader.IsDBNullAsync(Constante._32, cancelToken) ? String.Empty : _reader.GetString(Constante._32);
                                ordenBancariaDetalleMO.NroOrden = await _reader.IsDBNullAsync(Constante._33, cancelToken) ? String.Empty : _reader.GetString(Constante._33);
                                ordenBancariaDetalleMO.NroConvenio = await _reader.IsDBNullAsync(Constante._34, cancelToken) ? String.Empty : _reader.GetString(Constante._34);
                                ordenBancariaDetalleMO.UsuarioCreacion = await _reader.IsDBNullAsync(Constante._35, cancelToken) ? String.Empty : _reader.GetString(Constante._35);
                                DateTime? fechaCreacion = await _reader.IsDBNullAsync(Constante._36, cancelToken) ? (DateTime?)null : _reader.GetDateTime(Constante._36);
                                ordenBancariaDetalleMO.FechaCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortDateString();
                                ordenBancariaDetalleMO.HoraCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortTimeString();
                                listaOrdenesBancariasDetalle.Add(ordenBancariaDetalleMO);
                            }

                            objetoOrdenBancariaDetalleMO.Codigo = Constante.CODIGO_OK;
                        }

                        _reader.Close();
                        _con.Close();
                        objetoOrdenBancariaDetalleMO.Codigo = _reader != null ? objetoOrdenBancariaDetalleMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoOrdenBancariaDetalleMO.Mensaje = _reader != null ? objetoOrdenBancariaDetalleMO.Mensaje: Constante.MENSAJE_RECARGAR_PAGINA;
                        objetoOrdenBancariaDetalleMO.ListaOrdenesBancariasDetalle = listaOrdenesBancariasDetalle;
                        String mensaje = _reader != null ? Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_DIARIAS_ASYNC_OK : Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_DIARIAS_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DETALLE_DIARIAS_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_LISTAR_ORDENES_BANCARIAS_DETALLE_DIARIAS_ASYNC, Constante.MENSAJE_LISTAR_ORDENES_BANCARIAS_DETALLE_DIARIAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoOrdenBancariaDetalleMO;
        }

        public async Task<ObjetoOrdenBancariaMO> BuscarOrdenesBancariasDiariasAsync(CancellationToken cancelToken, String idUsuario, String idSap)
        {
            ObjetoOrdenBancariaMO objetoOrdenBancariaMO = new ObjetoOrdenBancariaMO();
            List<OrdenBancariaMO> listaOrdenesBancarias = null;
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_BUSCAR_ORDENES_BANCARIAS_DIARIAS, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_USUARIO, System.Data.SqlDbType.NChar, Constante._6);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idUsuario;
                        
                        SqlParameter par2 = _cmd.Parameters.Add(Constante.ID_SAP, System.Data.SqlDbType.NVarChar, Constante._10);
                        par2.Direction = System.Data.ParameterDirection.Input;
                        par2.Value = idSap;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult, cancelToken);

                        if (_reader != null && !_reader.HasRows)
                        {
                            objetoOrdenBancariaMO.Codigo = Constante.CODIGO_OMISION;
                            objetoOrdenBancariaMO.Mensaje = Constante.MENSAJE_SIN_RESULTADOS;
                        }
                        else if (_reader != null && _reader.HasRows)
                        {
                            listaOrdenesBancarias = new List<OrdenBancariaMO>();

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                OrdenBancariaMO ordenBancariaMO = new OrdenBancariaMO();
                                ordenBancariaMO.IdOrdenBancaria = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                ordenBancariaMO.Banco = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                ordenBancariaMO.BancoCorto = await _reader.IsDBNullAsync(Constante._2, cancelToken) ? String.Empty : _reader.GetString(Constante._2);
                                ordenBancariaMO.IdTipoOrden = await _reader.IsDBNullAsync(Constante._3, cancelToken) ? String.Empty : _reader.GetString(Constante._3);
                                ordenBancariaMO.TipoOrden = await _reader.IsDBNullAsync(Constante._4, cancelToken) ? String.Empty : _reader.GetString(Constante._4);
                                ordenBancariaMO.TipoOrdenCorto = await _reader.IsDBNullAsync(Constante._5, cancelToken) ? String.Empty : _reader.GetString(Constante._5);
                                ordenBancariaMO.IdEstadoOrden = await _reader.IsDBNullAsync(Constante._6, cancelToken) ? String.Empty : _reader.GetString(Constante._6);
                                ordenBancariaMO.EstadoOrden = await _reader.IsDBNullAsync(Constante._7, cancelToken) ? String.Empty : _reader.GetString(Constante._7);
                                ordenBancariaMO.IdSociedad = await _reader.IsDBNullAsync(Constante._8, cancelToken) ? String.Empty : _reader.GetString(Constante._8);
                                ordenBancariaMO.Sociedad = await _reader.IsDBNullAsync(Constante._9, cancelToken) ? String.Empty : _reader.GetString(Constante._9);
                                ordenBancariaMO.SociedadCorto = await _reader.IsDBNullAsync(Constante._10, cancelToken) ? String.Empty : _reader.GetString(Constante._10);
                                ordenBancariaMO.IdSap = await _reader.IsDBNullAsync(Constante._11, cancelToken) ? String.Empty : _reader.GetString(Constante._11);
                                ordenBancariaMO.Anio = await _reader.IsDBNullAsync(Constante._12, cancelToken) ? String.Empty : _reader.GetString(Constante._12);
                                ordenBancariaMO.MomentoOrden = await _reader.IsDBNullAsync(Constante._13, cancelToken) ? String.Empty : _reader.GetString(Constante._13);
                                ordenBancariaMO.FechaOrden = await _reader.IsDBNullAsync(Constante._14, cancelToken) ? String.Empty : _reader.GetString(Constante._14);
                                ordenBancariaMO.NombreArchivo = await _reader.IsDBNullAsync(Constante._15, cancelToken) ? String.Empty : _reader.GetString(Constante._15);
                                ordenBancariaMO.RutaArchivo = await _reader.IsDBNullAsync(Constante._16, cancelToken) ? String.Empty : _reader.GetString(Constante._16);
                                ordenBancariaMO.MonedaLocal = await _reader.IsDBNullAsync(Constante._17, cancelToken) ? String.Empty : _reader.GetString(Constante._17);
                                ordenBancariaMO.MonedaCortoLocal = await _reader.IsDBNullAsync(Constante._18, cancelToken) ? String.Empty : _reader.GetString(Constante._18);
                                ordenBancariaMO.ImporteLocal = await _reader.IsDBNullAsync(Constante._19, cancelToken) ? 0 : _reader.GetDecimal(Constante._19);
                                ordenBancariaMO.MonedaForanea = await _reader.IsDBNullAsync(Constante._20, cancelToken) ? String.Empty : _reader.GetString(Constante._20);
                                ordenBancariaMO.MonedaCortoForanea = await _reader.IsDBNullAsync(Constante._21, cancelToken) ? String.Empty : _reader.GetString(Constante._21);
                                ordenBancariaMO.ImporteForanea = await _reader.IsDBNullAsync(Constante._22, cancelToken) ? 0 : _reader.GetDecimal(Constante._22);
                                ordenBancariaMO.Liberador = await _reader.IsDBNullAsync(Constante._23, cancelToken) ? String.Empty : _reader.GetString(Constante._23);
                                ordenBancariaMO.PreAprobador = await _reader.IsDBNullAsync(Constante._24, cancelToken) ? String.Empty : _reader.GetString(Constante._24);
                                ordenBancariaMO.Aprobador = await _reader.IsDBNullAsync(Constante._25, cancelToken) ? String.Empty : _reader.GetString(Constante._25);
                                ordenBancariaMO.Comentarios = await _reader.IsDBNullAsync(Constante._26, cancelToken) ? String.Empty : _reader.GetString(Constante._26);
                                ordenBancariaMO.UsuarioCreacion = await _reader.IsDBNullAsync(Constante._27, cancelToken) ? String.Empty : _reader.GetString(Constante._27);
                                DateTime? fechaCreacion = await _reader.IsDBNullAsync(Constante._28, cancelToken) ? (DateTime?)null : _reader.GetDateTime(Constante._28);
                                ordenBancariaMO.FechaCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortDateString();
                                ordenBancariaMO.HoraCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortTimeString();
                                ordenBancariaMO.UsuarioEdicion = await _reader.IsDBNullAsync(Constante._29, cancelToken) ? String.Empty : _reader.GetString(Constante._29);
                                DateTime? fechaEdicion = await _reader.IsDBNullAsync(Constante._30, cancelToken) ? (DateTime?)null : _reader.GetDateTime(Constante._30);
                                ordenBancariaMO.FechaEdicion = fechaEdicion == null ? String.Empty : fechaEdicion.Value.ToShortDateString();
                                ordenBancariaMO.HoraEdicion = fechaEdicion == null ? String.Empty : fechaEdicion.Value.ToShortTimeString();
                                listaOrdenesBancarias.Add(ordenBancariaMO);
                            }

                            objetoOrdenBancariaMO.Codigo = Constante.CODIGO_OK;
                        }

                        _reader.Close();
                        _con.Close();
                        objetoOrdenBancariaMO.Codigo = _reader != null ? objetoOrdenBancariaMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoOrdenBancariaMO.Mensaje = _reader != null ? objetoOrdenBancariaMO.Mensaje: Constante.MENSAJE_RECARGAR_PAGINA;
                        objetoOrdenBancariaMO.ListaOrdenesBancarias = listaOrdenesBancarias;
                        String mensaje = _reader != null ? Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_DIARIAS_ASYNC_OK : Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_DIARIAS_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_BUSCAR_ORDENES_BANCARIAS_DIARIAS_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_ORDENBANCARIA_RE, Constante.METODO_BUSCAR_ORDENES_BANCARIAS_DIARIAS_ASYNC, Constante.MENSAJE_BUSCAR_ORDENES_BANCARIAS_DIARIAS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoOrdenBancariaMO;
        }
    }
}