using System.Text;
using Credimujer.Op.Domail.Models.Entities;
using Credimujer.Op.Repository.Implementations.Configurations.Base;
using Microsoft.EntityFrameworkCore;

namespace Credimujer.Op.Repository.Implementations.Configurations
{
    public class PreSolicitudConfiguration : EntityConfiguration<PreSolicitudEntity>
    {
        public PreSolicitudConfiguration(ModelBuilder modelBuilder)
        {
            var entityBuilder = modelBuilder.Entity<PreSolicitudEntity>();
            entityBuilder.ToTable("ASO_PRE_SOLICITUD");
            entityBuilder.HasKey(c => c.Id);
            entityBuilder.Property(c => c.Id).HasColumnName("IN_ID");
            entityBuilder.Property(c => c.Nro).HasColumnName("IN_NRO");
            entityBuilder.Property(c => c.SociaId).HasColumnName("IN_SOCIA_ID");
            //entityBuilder.Property(c => c.EntidadBancariaId).HasColumnName("IN_ENTIDAD_BANCARIA_ID");
            entityBuilder.Property(c => c.Plazo).HasColumnName("IN_PLAZO");
            entityBuilder.Property(c => c.PlazoGracia).HasColumnName("IN_PLAZO_GRACIA");
            entityBuilder.Property(c => c.Monto).HasColumnName("DC_MONTO");
            entityBuilder.Property(c => c.EstadoId).HasColumnName("IN_ESTADO");
            entityBuilder.Property(c => c.TipoCreditoId).HasColumnName("IN_TIPO_CREDITO_ID");
            entityBuilder.Property(c => c.PreSolicitudCabeceraId).HasColumnName("IN_PRESOLICITUD_ID");
            entityBuilder.Property(c => c.TasaInteresId).HasColumnName("IN_TASA_INTERES_ID");
            entityBuilder.Property(c => c.AsistenciaId).HasColumnName("IN_ASISTENCIA_ID");
            entityBuilder.Property(c => c.NivelRiesgoId).HasColumnName("IN_NIVEL_RIESGO");
            entityBuilder.Property(c => c.BancoDesembolsoId).HasColumnName("IN_BANCO_DESEMBOLSO_ID");
            entityBuilder.Property(c => c.NroCuenta).HasColumnName("NRO_CUENTA");
            entityBuilder.Property(c => c.SubTipoCreditoId).HasColumnName("IN_SUBTIPO_CREDITO_ID");
            entityBuilder.Property(c => c.MSV).HasColumnName("BT_MSV");
            entityBuilder.Property(c => c.Comentario).HasColumnName("VC_COMENTARIO");
            entityBuilder.Property(c => c.MotivoRetiroId).HasColumnName("IN_MOTIVO_RETIRO_ID");
            entityBuilder.Property(c => c.CobraMedianteDj).HasColumnName("BT_COBRA_MEDIANTE_DJ");
            entityBuilder.Property(c => c.AnilloGrupalRetiroId).HasColumnName("IN_ANILLO_GRUPAL_RETIRADO_ID");
            entityBuilder.Property(c => c.BancoComunalRetiradoId).HasColumnName("IN_BANCO_COMUNAL_RETIRADO_ID");
            entityBuilder.Property(c => c.SociaDjId).HasColumnName("IN_SOCIA_DJ_ID");
            entityBuilder.Property(c => c.CapacidadPago).HasColumnName("DC_CAPACIDAD_PAGO");
            entityBuilder.Property(c => c.SistemaExternoSociaPorRetiro).HasColumnName("IN_SISTEMA_EXTERNO_SOCIA");
            entityBuilder.Property(c => c.SistemaOrigenId).HasColumnName("IN_SISTEMA_ORIGEN");
            entityBuilder.Property(c => c.TipoDispositivoId).HasColumnName("IN_DISPOSITIVO_ID");
            entityBuilder.Property(c => c.TipoDeudaId).HasColumnName("IN_TIPO_DEUDA_ID");
            
            //entityBuilder.HasOne(c => c.EntidadBancaria).WithMany(m => m.PreSolicitudEntidadBancaria).HasForeignKey(f => f.EntidadBancariaId);
            entityBuilder.HasOne(c => c.Estado).WithMany(m => m.PreSolicitudEstado).HasForeignKey(f => f.EstadoId);
            entityBuilder.HasOne(c => c.TipoCredito).WithMany(m => m.PreSolicitudTipoCredito).HasForeignKey(f => f.TipoCreditoId);
            entityBuilder.HasOne(c => c.SubTipoCredito).WithMany(m => m.PreSolicitudSubTipoCredito).HasForeignKey(f => f.SubTipoCreditoId);
            entityBuilder.HasOne(c => c.Socia).WithMany(m => m.ListaPreSolicitud).HasForeignKey(f => f.SociaId);
            entityBuilder.HasOne(c => c.SociaDj).WithMany(m => m.ListaPreSolicitudDj).HasForeignKey(f => f.SociaDjId);
            entityBuilder.HasOne(c => c.PreSolicitudCabecera).WithMany(m => m.PreSolicitud).HasForeignKey(f => f.PreSolicitudCabeceraId);

            entityBuilder.HasOne(c => c.TasaInteres).WithMany(m => m.PreSolicitudTasaInteres).HasForeignKey(f => f.TasaInteresId);
            entityBuilder.HasOne(c => c.Asistencia).WithMany(m => m.PreSolicitudAsistencia).HasForeignKey(f => f.AsistenciaId);
            entityBuilder.HasOne(c => c.NivelRiesgo).WithMany(m => m.PreSolicitudNivelRiesgo).HasForeignKey(f => f.NivelRiesgoId);
            entityBuilder.HasOne(c => c.BancoDesembolso).WithMany(m => m.PreSolicitudBancoDesembolso).HasForeignKey(f => f.BancoDesembolsoId);
            entityBuilder.HasOne(c => c.MotivoRetiro).WithMany(m => m.PreSolicitudMotivoRetiro).HasForeignKey(f => f.MotivoRetiroId);
            entityBuilder.HasOne(c => c.SistemaOrigen).WithMany(m => m.PreSolicitudSistemaOrigen).HasForeignKey(f => f.SistemaOrigenId);
            entityBuilder.HasOne(c => c.TipoDispositivo).WithMany(m => m.PreSolicitudTipoDispositivo).HasForeignKey(f => f.TipoDispositivoId);
            Configure(entityBuilder);
        }
    }
}