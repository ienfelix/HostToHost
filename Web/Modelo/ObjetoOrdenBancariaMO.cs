using System;
using System.Collections.Generic;

namespace Modelo
{
    public class ObjetoOrdenBancariaMO
    {
        public Int32 Codigo { get; set; }
        public String Mensaje { get; set; }
        public String IdEstadoOrden { get; set; }
        public Int32 TotalRegistros { get; set; }
        public Boolean EsEstadoLiberado { get; set; }
        public Boolean EsEstadoPreAprobado { get; set; }
        public Boolean EsHorarioBanco { get; set; }
        public List<OrdenBancariaMO> ListaOrdenesBancarias { get; set; }
    }
}