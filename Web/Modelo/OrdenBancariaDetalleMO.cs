using System;

namespace Modelo
{
    public class OrdenBancariaDetalleMO
    {
        public String IdOrdenBancariaDetalle { get; set; }
        public String TipoTransferencia { get; set; }
        public String FormaPago { get; set; }
        public String SubTipoPago { get; set; }
        public String Referencia1 { get; set; }
        public String Referencia2 { get; set; }
        public String MonedaCargo { get; set; }
        public String MonedaCargoCorto { get; set; }
        public String CuentaCargo { get; set; }
        public Decimal MontoCargo { get; set; }
        public String MonedaAbono { get; set; }
        public String MonedaAbonoCorto { get; set; }
        public String CuentaAbono { get; set; }
        public String CuentaGasto { get; set; }
        public Decimal MontoAbono { get; set; }
        public Decimal TipoCambio { get; set; }
        public Int32 ModuloRaiz { get; set; }
        public Int32 DigitoControl { get; set; }
        public String Indicador { get; set; }
        public String NroOperacion { get; set; }
        public String Beneficiario { get; set; }
        public String TipoDocumento { get; set; }
        public String TipoDocumentoCorto { get; set; }
        public String NroDocumento { get; set; }
        public String Correo { get; set; }
        public String NombreBanco { get; set; }
        public String RucBanco { get; set; }
        public String NroFactura { get; set; }
        public String FechaFactura { get; set; }
        public String FechaFinFactura { get; set; }
        public String SignoFactura { get; set; }
        public String IdRespuesta { get; set; }
        public String Respuesta { get; set; }
        public String NroOrden { get; set; }
        public String NroConvenio { get; set; }
        public String UsuarioCreacion { get; set; }
        public String FechaCreacion { get; set; }
        public String HoraCreacion { get; set; }
    }
}