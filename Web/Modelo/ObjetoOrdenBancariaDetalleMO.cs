using System;
using System.Collections.Generic;

namespace Modelo
{
    public class ObjetoOrdenBancariaDetalleMO
    {
        public Int32 Codigo { get; set; }
        public String Mensaje { get; set; }
        public List<OrdenBancariaDetalleMO> ListaOrdenesBancariasDetalle { get; set; }
    }
}