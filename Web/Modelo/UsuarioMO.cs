using System;

namespace Modelo
{
    public class UsuarioMO
    {
        public String IdUsuario { get; set; }
        public String Usuario { get; set; }
        public String Clave { get; set; }
        public String ApePaterno { get; set; }
        public String ApeMaterno { get; set; }
        public String Nombres { get; set; }
        public String Correo { get; set; }
        public String Celular { get; set; }
        public String Estado { get; set; }
        public String UsuarioCreacion { get; set; }
        public String FechaCreacion { get; set; }
        public String HoraCreacion { get; set; }
    }
}