using Credimujer.Op.Model.Base;
using System;

namespace Credimujer.Op.Model.Oficial
{
    public class BuscarPresolicitudPorUsuarioModel : SortModel
    {
        public int SucursalId { get; set; }
        public string Usuario { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}