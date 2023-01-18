using Credimujer.Op.Model.Base;
using System;

namespace Credimujer.Op.Model.Oficial
{
    public class BuscarResumenIngresoModel : SortModel
    {
        public string SucursalCodigo { get; set; }
        public string usuario { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}