using Credimujer.Op.Domail.Models.Entities;
using Credimujer.Op.Repository.Implementations.Configurations.Base;
using Microsoft.EntityFrameworkCore;

namespace Credimujer.Op.Repository.Implementations.Configurations
{
    public class PreSolicitudCabeceraConfiguration : EntityConfiguration<PreSolicitudCabeceraEntity>
    {
        public PreSolicitudCabeceraConfiguration(ModelBuilder modelBuilder)
        {
            var entityBuilder = modelBuilder.Entity<PreSolicitudCabeceraEntity>();
            entityBuilder.ToTable("ASO_PRE_SOLICITUD_CABECERA");
            entityBuilder.HasKey(c => c.Id);
            entityBuilder.Property(c => c.Id).HasColumnName("IN_ID");
            entityBuilder.Property(c => c.Monto).HasColumnName("DC_MONTO");
            entityBuilder.Property(c => c.Plazo).HasColumnName("IN_PLAZO");
            entityBuilder.Property(c => c.EstadoId).HasColumnName("IN_ESTADO");
            entityBuilder.Property(c => c.Observacion).HasColumnName("VC_OBS");
            entityBuilder.Property(c => c.BancoComunalId).HasColumnName("IN_BANCO_COMUNAL_ID");
            entityBuilder.Property(c => c.AnilloGrupalId).HasColumnName("IN_ID_ANILLO_GRUPAL");
            entityBuilder.Property(c => c.FechaDesembolso).HasColumnName("DT_FECHA_DESEMBOLSO");
            entityBuilder.Property(c => c.TipoId).HasColumnName("IN_ID_TIPO");

            entityBuilder.HasOne(c => c.Estado).WithMany(m => m.PreSolicitudCabeceraEstado).HasForeignKey(f => f.EstadoId);
            entityBuilder.HasOne(c => c.BancoComunal).WithMany(m => m.PreSolicitudCabecera)
                .HasForeignKey(f => f.BancoComunalId);
            entityBuilder.HasOne(c => c.AnilloGrupal).WithMany(m => m.PreSolicitudCabecera)
                .HasForeignKey(f => f.AnilloGrupalId);
            entityBuilder.HasOne(f => f.Tipo).WithMany(m => m.PreSolicitudCabeceraTipo).HasForeignKey(f => f.TipoId);

            Configure(entityBuilder);
        }
    }
}