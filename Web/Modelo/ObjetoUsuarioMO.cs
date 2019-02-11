using System;
using System.Collections.Generic;

namespace Modelo
{
    public class ObjetoUsuarioMO
    {
        public Int32 Codigo { get; set; }
        public String Mensaje { get; set; }
        public String  IdUsuario { get; set; }
        public Int32 TotalRegistros { get; set; }
        public List<UsuarioMO> ListaUsuarios { get; set; }
    }
}