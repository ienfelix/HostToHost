using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Comun;
using Microsoft.Extensions.Configuration;
using Modelo;
using Newtonsoft.Json;
using Repositorio;

namespace Negocio
{
    public class ServicioNE
    {
        private Bitacora _bitacora = null;
        private ServicioRE _servicioRE = null;
        private HttpClient _httpClient = null;

        public ServicioNE(IConfiguration configuration)
        {
            _bitacora = _bitacora ?? new Bitacora(configuration);
            _servicioRE = _servicioRE ?? new ServicioRE(configuration);
            _httpClient = _httpClient ?? new HttpClient();
            _httpClient.BaseAddress = new Uri(Constante.URL_HOSTTOHOST_SERVICIO);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constante.APPLICATION_JSON));
        }

        public async Task<String> EnviarRespuestaProcesoHaciaSapAsync(CancellationToken cancelToken, String idSociedad, String idSap, String anio, String momentoOrden, String tipoOrden)
        {
            String respuesta = String.Empty;
            try
            {
                ObjetoRespuestaMO objetoRespuestaMO = await _servicioRE.EnviarRespuestaProcesoHaciaSapAsync(cancelToken, idSociedad, idSap, anio, momentoOrden, tipoOrden);
                
                if (objetoRespuestaMO.Codigo == Constante.CODIGO_OK)
                {
                    respuesta += String.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}", objetoRespuestaMO.RespuestaMO.IdEstadoOrden, Constante.DELIMITADOR_BARRA, objetoRespuestaMO.RespuestaMO.EstadoOrden, Constante.DELIMITADOR_BARRA, objetoRespuestaMO.RespuestaMO.Usuario, Constante.DELIMITADOR_BARRA, objetoRespuestaMO.RespuestaMO.Fecha, Constante.DELIMITADOR_BARRA, objetoRespuestaMO.RespuestaMO.Hora, Constante.DELIMITADOR_NUMERAL);

                    foreach (var item in objetoRespuestaMO.ListaRespuestas)
                    {
                        respuesta += String.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}", item.Beneficiario, Constante.DELIMITADOR_BARRA, item.Referencia1, Constante.DELIMITADOR_BARRA, item.Importe, Constante.DELIMITADOR_BARRA, item.IdRespuesta, Constante.DELIMITADOR_BARRA, item.Respuesta, Constante.DELIMITADOR_NUMERAL);
                    }

                    respuesta = respuesta.Substring(Constante._0, respuesta.Length - Constante._1);
                }
                String mensaje = objetoRespuestaMO.Codigo == Constante.CODIGO_OK ? Constante.MENSAJE_ENVIAR_RESPUESTA_PROCESO_HACIA_SAP_ASYNC_OK : Constante.MENSAJE_ENVIAR_RESPUESTA_PROCESO_HACIA_SAP_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_SERVICIO_NE, Constante.METODO_ENVIAR_RESPUESTA_PROCESO_HACIA_SAP_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_SERVICIO_NE, Constante.METODO_ENVIAR_RESPUESTA_PROCESO_HACIA_SAP_ASYNC, Constante.MENSAJE_ENVIAR_RESPUESTA_PROCESO_HACIA_SAP_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return respuesta;
        }

        public async Task<RespuestaDetalleMO> EnviarEstadoProcesoHaciaSapAsync(CancellationToken cancelToken, String idSociedad, String anio, String momentoOrden, String idEstadoOrden, String idSap, String usuario, String tipoOrden)
        {
            RespuestaDetalleMO respuestaDetalleMO = null;
            Boolean esEnviado = false;
            try
            {
                String parametros = String.Format("{0}/{1}/{2}/{3}/{4}/{5}/{6}", idSociedad, anio, momentoOrden, idEstadoOrden, idSap, usuario, tipoOrden);
                HttpResponseMessage response = await _httpClient.GetAsync(parametros, cancelToken);
                
                if (response.IsSuccessStatusCode)
                {
                    var respuesta = await response.Content.ReadAsStringAsync();
                    respuestaDetalleMO = JsonConvert.DeserializeObject<RespuestaDetalleMO>(respuesta);
                    esEnviado = true;
                }

                String mensaje = esEnviado == true ? Constante.MENSAJE_ENVIAR_ESTADO_PROCESO_HACIA_SAP_ASYNC_OK : Constante.MENSAJE_ENVIAR_ESTADO_PROCESO_HACIA_SAP_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_SERVICIO_NE, Constante.METODO_ENVIAR_ESTADO_PROCESO_HACIA_SAP_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_SERVICIO_NE, Constante.METODO_ENVIAR_ESTADO_PROCESO_HACIA_SAP_ASYNC, Constante.MENSAJE_ENVIAR_ESTADO_PROCESO_HACIA_SAP_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return respuestaDetalleMO;
        }
    }
}