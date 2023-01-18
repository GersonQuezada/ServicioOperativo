using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Dto.PreSolicitud.Reporte
{
    public class ReporteParaleloPromocionalDto
    {
        public _Cabecera Cabecera { get; set; }
        public List<_Detalle> Detalle { get; set; }

        public class _Cabecera
        {
            //public string SubTipoPreSolicitud { get; set; }
            public int Id { get; set; }

            public string Sucursal { get; set; }

            public DateTime Fecha { get; set; }
            public string Usuario { get; set; }
            public string BancoComunal { get; set; }
            public string BancoComunalCodigo { get; set; }
            public int Ciclo { get; set; }

            public DateTime? FechaDesembolso { get; set; }
            public string OficialCredito { get; set; }
            public decimal Monto { get; set; }
        }

        public class _Detalle
        {
            public int? IdSocia { get; set; }
            public string Nombre { get; set; }
            public string Dni { get; set; }
            public string Cargo { get; set; }
            public string NivelRiesgo { get; set; }
            public decimal? CapacidadPago { get; set; }
            public string TipoDeuda { get; set; }
            public string ActividadEconomica { get; set; }
            public string SubTipoCredito { get; set; }

            //public bool AfiliadaSmv { get; set; }
            public string AfiliadaSmv { get; set; }

            public string CobraConDj { get; set; }
            public decimal Monto { get; set; }
            public int Plazo { get; set; }
            public string TasaInteres { get; set; }
            public int? PlazoGracia { get; set; }
            public string Asistencia { get; set; }
            public string EntidadFinanciera { get; set; }
            public string NroCuenta { get; set; }
            public string EstadoPreSolicitud { get; set; }
            public string CodigoEstadoPreSolicitud { get; set; }
        }
    }
}