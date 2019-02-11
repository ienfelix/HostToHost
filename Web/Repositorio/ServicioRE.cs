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
    public class ServicioRE
    {
        private String _conexionHostToHost = String.Empty;
        private SqlConnection _con = null;
        private SqlCommand _cmd = null;
        private SqlDataReader _reader = null;
        private Bitacora _bitacora = null;

        public ServicioRE(IConfiguration configuration)
        {
            _bitacora = _bitacora ?? new Bitacora(configuration);
            _conexionHostToHost = configuration.GetConnectionString(Constante.CONEXION_HOST_TO_HOST_PRODUCCION) ?? String.Empty;
        }

        public async Task<ObjetoRespuestaMO> EnviarRespuestaProcesoHaciaSapAsync(CancellationToken cancelToken, String idSociedad, String idSap, String anio, String momentoOrden, String tipoOrden)
        {
            ObjetoRespuestaMO objetoRespuestaMO = new ObjetoRespuestaMO();
            RespuestaMO respuestaMO = null;
            List<RespuestaDetalleMO> listaRespuestas = null;
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_ENVIAR_RESPUESTA_PROCESO_HACIA_SAP, _con))
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

                        SqlParameter par5 = _cmd.Parameters.Add(Constante.TIPO_ORDEN, System.Data.SqlDbType.NChar, Constante._3);
                        par5.Direction = System.Data.ParameterDirection.Input;
                        par5.Value = tipoOrden;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.Default, cancelToken);

                        if (_reader != null && !_reader.HasRows)
                        {
                            objetoRespuestaMO.Codigo = Constante.CODIGO_OMISION;
                            objetoRespuestaMO.Mensaje = Constante.MENSAJE_SIN_RESULTADOS;
                        }
                        else if (_reader != null && _reader.HasRows)
                        {
                            listaRespuestas = new List<RespuestaDetalleMO>();

                            if (await _reader.ReadAsync(cancelToken))
                            {
                                respuestaMO = new RespuestaMO();
                                respuestaMO.IdEstadoOrden = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                respuestaMO.EstadoOrden = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                respuestaMO.Usuario = await _reader.IsDBNullAsync(Constante._2, cancelToken) ? String.Empty : _reader.GetString(Constante._2);
                                respuestaMO.Fecha = await _reader.IsDBNullAsync(Constante._3, cancelToken) ? String.Empty : _reader.GetString(Constante._3);
                                respuestaMO.Hora = await _reader.IsDBNullAsync(Constante._4, cancelToken) ? String.Empty : _reader.GetString(Constante._4);
                            }

                            await _reader.NextResultAsync(cancelToken);

                            while (await _reader.ReadAsync(cancelToken))
                            {
                                RespuestaDetalleMO respuestaDetalleMO = new RespuestaDetalleMO();
                                respuestaDetalleMO.Beneficiario = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                respuestaDetalleMO.Referencia1 = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                respuestaDetalleMO.Importe = await _reader.IsDBNullAsync(Constante._2, cancelToken) ? 0 : _reader.GetDecimal(Constante._2);
                                respuestaDetalleMO.IdRespuesta = await _reader.IsDBNullAsync(Constante._3, cancelToken) ? String.Empty : _reader.GetString(Constante._3);
                                respuestaDetalleMO.Respuesta = await _reader.IsDBNullAsync(Constante._4, cancelToken) ? String.Empty : _reader.GetString(Constante._4);
                                listaRespuestas.Add(respuestaDetalleMO);
                            }

                            objetoRespuestaMO.Codigo = Constante.CODIGO_OK;
                        }

                        _reader.Close();
                        _con.Close();
                        objetoRespuestaMO.Codigo = _reader != null ? objetoRespuestaMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoRespuestaMO.Mensaje = _reader != null ? objetoRespuestaMO.Mensaje : Constante.MENSAJE_RECARGAR_PAGINA;
                        objetoRespuestaMO.RespuestaMO = respuestaMO;
                        objetoRespuestaMO.ListaRespuestas = listaRespuestas;
                        String mensaje = _reader != null ? Constante.MENSAJE_ENVIAR_RESPUESTA_PROCESO_HACIA_SAP_ASYNC_OK : Constante.MENSAJE_ENVIAR_RESPUESTA_PROCESO_HACIA_SAP_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_SERVICIO_RE, Constante.METODO_ENVIAR_RESPUESTA_PROCESO_HACIA_SAP_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_SERVICIO_RE, Constante.METODO_ENVIAR_RESPUESTA_PROCESO_HACIA_SAP_ASYNC, Constante.MENSAJE_ENVIAR_RESPUESTA_PROCESO_HACIA_SAP_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoRespuestaMO;
        }
    }
}