using System;
using System.Collections.Generic;

namespace Modelo
{
    public class ObjetoFlujoAprobacionMO
    {
        public Int32 Codigo { get; set; }
        public String Mensaje { get; set; }
        public List<FlujoAprobacionMO> ListaFlujoAprobacion { get; set; }
    }
}