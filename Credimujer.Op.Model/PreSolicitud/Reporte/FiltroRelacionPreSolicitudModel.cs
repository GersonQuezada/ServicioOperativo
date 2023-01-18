using Credimujer.Op.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Model.PreSolicitud.Reporte
{
    public class FiltroRelacionPreSolicitudModel : SortModel
    {
        public int BancoComunalId { get; set; }
        public int? AnilloGrupalId { get; set; }
        public int? Nro { get; set; }
    }
}