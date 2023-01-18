using Credimujer.Op.Dto.Base;
using System;
using System.Collections.Generic;

namespace Credimujer.Op.Dto.PreSolicitud.Reporte
{
    public class ListaPreSocilitudCabeceraDto
    {
        public int Nro { get; set; }
        public string BancoComunal { get; set; }
        public string AnilloGrupal { get; set; }
        public int Ciclo { get; set; }
        public DateTime? FechaDesembolso { get; set; }
        public decimal Monto { get; set; }
        public int Plazo { get; set; }

        public string TipoProducto { get; set; } = "";
        public string Estado { get; set; }
        public string EstadoCodigo { get; set; }
        public List<string> DetalleTipoCredito { get; set; }
        public List<string> DetalleTipoCreditoAbreviatura { get; set; }
        public List<DropdownDto> TipoReporte { get; set; }
    }
}