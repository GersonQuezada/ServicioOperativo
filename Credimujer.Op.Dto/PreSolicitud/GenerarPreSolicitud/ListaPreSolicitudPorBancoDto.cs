using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Dto.PreSolicitud.GenerarPreSolicitud
{
    public class ListaPreSolicitudPorBancoDto
    {
        public int? Id { get; set; }
        public string BancoComunal { get; set; }
        public string AnilloGrupal { get; set; }
        public int SociaId { get; set; }
        public string Nombre { get; set; }
        public string Dni { get; set; }
        public decimal Monto { get; set; }
        public decimal Plazo { get; set; }
        public string EntidadBancaria { get; set; }
        public string Estado { get; set; }
        public string EstadoCodigo { get; set; }
        public int? Nro { get; set; }

        public int? SociaIdSistemaExterno { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string TipoCredito { get; set; }
        public int? SociaDj { get; set; }

        public bool TienePeriodoGracia { get; set; }
        public int? NivelRiesgoId { get; set; }
        public decimal? CapacidadPago { get; set; }
        public bool EsOrigenSistemaSocia { get; set; }
        public string TasaInteres { get; set; }

        public bool Reingresante { get; set; }
        public bool DatoIncompleto { get; set; }
        public int? TipoDeudaId { get; set; }
    }
}