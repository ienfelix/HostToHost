using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comun;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Modelo;
using Negocio;

namespace HostToHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioController : ControllerBase
    {
        private ServicioNE _servicioNE = null;

        public ServicioController(IConfiguration configuration)
        {
            _servicioNE = _servicioNE ?? new ServicioNE(configuration);
        }

        [HttpGet]
        public ActionResult<String> Get()
        {
            return Constante.MENSAJE_SERVICIO_OK;
        }

        [HttpGet("{id}")]
        public ActionResult<string> Get(string id)
        {
            return "value";

        }

        [HttpGet("{idSociedad}/{idSap}/{anio}/{momentoOrden}/{tipoOrden}")]
        public async Task<ActionResult<String>> GetAction(String idSociedad, String idSap, String anio, String momentoOrden, String tipoOrden)
        {
            String respuesta = null;
            try
            {
                respuesta = await _servicioNE.EnviarRespuestaProcesoHaciaSapAsync(new System.Threading.CancellationToken(false), idSociedad, idSap, anio, momentoOrden, tipoOrden);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return respuesta;
        }
    }
}