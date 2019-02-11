using System;
using System.Collections.Generic;

namespace Modelo
{
    public class ObjetoFiltroMO
    {
        public Int32 Codigo { get; set; }
        public String Mensaje { get; set; }
        public List<FiltroMO> ListaFiltros { get; set; }
    }
}