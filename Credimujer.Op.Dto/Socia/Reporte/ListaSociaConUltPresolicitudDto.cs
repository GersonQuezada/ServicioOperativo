using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Dto.Socia.Reporte
{
    public class ListaSociaConUltPresolicitudDto
    {
        public int SociaId { get; set; }
        public string NivelRiesgo { get; set; }
        public decimal? CapacidadPago { get; set; }
        public string TipoDeuda { get; set; }
        public int Id { get; set; }
    }
}
