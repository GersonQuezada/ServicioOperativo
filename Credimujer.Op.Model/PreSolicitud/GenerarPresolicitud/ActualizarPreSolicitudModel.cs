using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Model.PreSolicitud.GenerarPresolicitud
{
    public class ActualizarPreSolicitudModel
    {
        public int Id { get; set; }
        public int TipoCreditoId { get; set; }
        public int SubTipoCreditoId { get; set; }
        public decimal Monto { get; set; }
        public int TasaInteresId { get; set; }
        public int Plazo { get; set; }
        public int? PlazoGracia { get; set; }
        public int EntidadBancariaId { get; set; }
        public string NroCuenta { get; set; }
        public int? AsistenciaId { get; set; }
        public int? NivelRiesgoId { get; set; }
        public bool Msv { get; set; }
        public bool CobraMedianteDj { get; set; }
        public int? SociaDjId { get; set; }
        public decimal? CapacidadPago { get; set; }
    }
}