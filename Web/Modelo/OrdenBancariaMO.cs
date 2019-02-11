using System;

namespace Modelo
{
    public class OrdenBancariaMO
    {
        public String IdOrdenBancaria { get; set; }
        public String Banco { get; set; }
        public String BancoCorto { get; set; }
        public String IdTipoOrden { get; set; }
        public String TipoOrden { get; set; }
        public String TipoOrdenCorto { get; set; }
        public String IdEstadoOrden { get; set; }
        public String EstadoOrden { get; set; }
        public String IdSociedad { get; set; }
        public String Sociedad { get; set; }
        public String SociedadCorto { get; set; }
        public String IdSap { get; set; }
        public String Anio { get; set; }
        public String MomentoOrden { get; set; }
        public String FechaOrden { get; set; }
        public String NombreArchivo { get; set; }
        public String RutaArchivo { get; set; }
        public String MonedaLocal { get; set; }
        public String MonedaCortoLocal { get; set; }
        public Decimal ImporteLocal { get; set; }
        public String MonedaForanea { get; set; }
        public String MonedaCortoForanea { get; set; }
        public Decimal ImporteForanea { get; set; }
        public String Liberador { get; set; }
        public String PreAprobador { get; set; }
        public String Aprobador { get; set; }
        public String Anulador { get; set; }
        public String Comentarios { get; set; }
        public String UsuarioCreacion { get; set; }
        public String FechaCreacion { get; set; }
        public String HoraCreacion { get; set; }
        public String UsuarioEdicion { get; set; }
        public String FechaEdicion { get; set; }
        public String HoraEdicion { get; set; }
    }
}