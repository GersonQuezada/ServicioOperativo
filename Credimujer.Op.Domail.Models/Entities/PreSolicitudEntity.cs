using Credimujer.Op.Domail.Models.Base;

namespace Credimujer.Op.Domail.Models.Entities
{
    public class PreSolicitudEntity : BaseEntity
    {
        public int Id { get; set; }
        public int Nro { get; set; }
        public int SociaId { get; set; }

        //public int? EntidadBancariaId { get; set; }
        public int Plazo { get; set; }

        public int? PlazoGracia { get; set; }
        public decimal Monto { get; set; }
        public int EstadoId { get; set; }
        public int? TipoCreditoId { get; set; }
        public int? PreSolicitudCabeceraId { get; set; }
        public int? TasaInteresId { get; set; }
        public int? AsistenciaId { get; set; }
        public int? NivelRiesgoId { get; set; }
        public int? BancoDesembolsoId { get; set; }
        public int? SubTipoCreditoId { get; set; }
        public string NroCuenta { get; set; }
        public bool MSV { get; set; }
        public string Comentario { get; set; }
        public int? MotivoRetiroId { get; set; }
        public bool CobraMedianteDj { get; set; }
        public int? BancoComunalRetiradoId { get; set; }
        public int? AnilloGrupalRetiroId { get; set; }
        public int? SociaDjId { get; set; }
        public decimal? CapacidadPago { get; set; }
        public int? SistemaExternoSociaPorRetiro { get; set; }
        public int? SistemaOrigenId { get; set; } //el registro por cual sistema fue registrado
        public int? TipoDispositivoId { get; set; }
        public int? TipoDeudaId { get; set; }
        public virtual CatalogoDetalleEntity Estado { get; set; }

        //public virtual CatalogoDetalleEntity EntidadBancaria { get; set; }
        public virtual CatalogoDetalleEntity TipoCredito { get; set; }

        public virtual CatalogoDetalleEntity SubTipoCredito { get; set; }
        public virtual CatalogoDetalleEntity MotivoRetiro { get; set; }
        public virtual SociaEntity Socia { get; set; }
        public virtual PreSolicitudCabeceraEntity PreSolicitudCabecera { get; set; }
        public virtual CatalogoDetalleEntity TasaInteres { get; set; }
        public virtual CatalogoDetalleEntity Asistencia { get; set; }
        public virtual CatalogoDetalleEntity NivelRiesgo { get; set; }
        public virtual CatalogoDetalleEntity BancoDesembolso { get; set; }
        public virtual SociaEntity SociaDj { get; set; }
        public virtual CatalogoDetalleEntity SistemaOrigen { get; set; }
        public virtual CatalogoDetalleEntity TipoDispositivo { get; set; }
        public virtual CatalogoDetalleEntity TipoDeuda { get; set; }
    }
}