using Credimujer.Op.Dto.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Dto.Oficial
{
    public class ReporteRegistrosIngresadosDto
    {
        public Cabecera Cabecera { get; set; }
        public List<Detalle> Detalle { get; set; }
    }

    public class Cabecera
    {
        public string Sucursal { get; set; }
        public string Oficial { get; set; }
        public int CantidadRegistros { get; set; }
    }

    public class Detalle
    {
        public int Id { get; set; }
        public string BancoComunal { get; set; }
        public string AnilloGrupal { get; set; }
        public decimal Monto { get; set; }
        public int Plazo { get; set; }
        public string Estado { get; set; }
        public string EstadoCodigo { get; set; }

        public DateTime? FechaDesembolso { get; set; }

        public List<string> DetalleTipoCredito { get; set; }
        public List<DropdownDto> TipoReporte { get; set; }
        public List<string> AbreviaturaTipoCredito { get; set; }
    }
}