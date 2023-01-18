using Credimujer.Op.Domail.Models.Entities;
using Credimujer.Op.Repository.Implementations.Configurations.Base;
using Microsoft.EntityFrameworkCore;

namespace Credimujer.Op.Repository.Implementations.Configurations
{
    public class CatalogoDetalleConfiguration : EntityConfiguration<CatalogoDetalleEntity>
    {
        public CatalogoDetalleConfiguration(ModelBuilder modelBuilder)
        {
            var entityBuilder = modelBuilder.Entity<CatalogoDetalleEntity>();
            entityBuilder.ToTable("ASO_DETALLE_CATALOGO");
            entityBuilder.HasKey(c => c.Id);
            entityBuilder.Property(c => c.Id).HasColumnName("IN_ID");
            entityBuilder.Property(c => c.CatalogoId).HasColumnName("IN_ID_CATALOGO");
            entityBuilder.Property(c => c.Codigo).HasColumnName("VC_CODIGO");
            entityBuilder.Property(c => c.Valor).HasColumnName("VC_VALOR");
            entityBuilder.Property(c => c.Descripcion).HasColumnName("VC_DESCRIPCION");
            entityBuilder.Property(c => c.Orden).HasColumnName("IN_ORDEN");
            entityBuilder.Property(c => c.Abreviatura).HasColumnName("VC_ABREVIATURA");
            entityBuilder.HasOne(c => c.Catalogo).WithMany(m => m.CatalogoDetalle).HasForeignKey(f => f.CatalogoId);

            Configure(entityBuilder);
        }
    }
}