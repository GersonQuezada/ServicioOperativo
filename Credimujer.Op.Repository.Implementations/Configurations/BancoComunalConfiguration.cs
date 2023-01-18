using Credimujer.Op.Domail.Models.Entities;
using Credimujer.Op.Repository.Implementations.Configurations.Base;
using Microsoft.EntityFrameworkCore;

namespace Credimujer.Op.Repository.Implementations.Configurations
{
    public class BancoComunalConfiguration : EntityConfiguration<BancoComunalEntity>
    {
        public BancoComunalConfiguration(ModelBuilder modelBuilder)
        {
            var entityBuilder = modelBuilder.Entity<BancoComunalEntity>();
            entityBuilder.ToTable("ASO_BANCO_COMUNAL");
            entityBuilder.HasKey(c => c.Id);
            entityBuilder.Property(c => c.Id).HasColumnName("IN_ID");
            entityBuilder.Property(c => c.Codigo).HasColumnName("VC_CODIGO");
            entityBuilder.Property(c => c.SucursalId).HasColumnName("IN_SUCURSAL_ID");
            entityBuilder.Property(c => c.Descripcion).HasColumnName("VC_DESCRIPCION");
            entityBuilder.Property(c => c.Ciclo).HasColumnName("IN_CICLO");
            entityBuilder.Property(c => c.EstadoId).HasColumnName("ESTADO_ID");
            entityBuilder.Property(c => c.PeriodoGracia).HasColumnName("BT_CON_PERIODO_GRACIA");

            entityBuilder.HasOne(c => c.Sucursal).WithMany(m => m.BancoComunalSucursal)
                .HasForeignKey(f => f.SucursalId);
            entityBuilder.HasOne(c => c.Estado).WithMany(m => m.BancoComunalEstado).HasForeignKey(f => f.EstadoId);
            Configure(entityBuilder);
        }
    }
}