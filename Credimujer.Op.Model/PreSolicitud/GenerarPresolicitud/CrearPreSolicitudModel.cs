using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Model.PreSolicitud.GenerarPresolicitud
{
    public class CrearPreSolicitudModel
    {
        public int SociaId { get; set; }
        public int? TipoCreditoId { get; set; }
        public decimal Monto { get; set; }
        public int TasaInteresId { get; set; }
        public int Plazo { get; set; }
        public int? PlazoGracia { get; set; }
        public int? EntidadBancariaId { get; set; }
        public string NroCuenta { get; set; }
        public int? AsistenciaId { get; set; }
        public int? NivelRiesgoId { get; set; }
        public int? SubTipoCreditoId { get; set; }
        public bool Msv { get; set; }
        public bool Ahorrista { get; set; }
        public bool Retirada { get; set; }

        public string MotivoCodigo { get; set; }
        public bool CobraMedianteDj { get; set; }
        public bool NuevoAnilloGrupal { get; set; }
        public bool NuevoBancoComunal { get; set; }
        public int? AnilloGrupalId { get; set; }
        public int? BancoComunalId { get; set; }
        public int? SociaDjId { get; set; }
        public decimal? CapacidadPago { get; set; }
        public string TipoDispositivoCodigo { get; set; }
        public int? TipoDeudaId { get; set; }
    }
}