using System;
using System.Collections.Generic;

namespace Modelo
{
    public class ObjetoRespuestaMO
    {
        public Int32 Codigo { get; set; }
        public String Mensaje { get; set; }
        public RespuestaMO RespuestaMO { get; set; }
        public List<RespuestaDetalleMO> ListaRespuestas { get; set; }
    }
}