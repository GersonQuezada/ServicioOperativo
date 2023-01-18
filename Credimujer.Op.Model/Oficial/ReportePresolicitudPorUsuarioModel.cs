using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Model.Oficial
{
    public class ReportePresolicitudPorUsuarioModel
    {
        public int SucursalId { get; set; }
        public string Oficial { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}