using System;

namespace Modelo
{
    public class FlujoAprobacionMO
    {
        public String IdFlujoAprobacion { get; set; }
        public String EstadoFlujo { get; set; }
        public String EstadoFlujoCorto { get; set; }
        public String Comentarios { get; set; }
        public String UsuarioCreacion { get; set; }
        public String FechaCreacion { get; set; }
        public String HoraCreacion { get; set; }
    }
}