using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Dto.PreSolicitud.Reporte
{
    public class ListaPreSolicitudDto
    {
        public int? IdSocia { get; set; }
        public string Nombre { get; set; }
        public string Dni { get; set; }
        public string ActividadEconomica { get; set; }
        public string Observacion { get; set; }
        public string EstadoPreSolicitud { get; set; }
        public string CodigoEstadoPreSolicitud { get; set; }
    }
}
