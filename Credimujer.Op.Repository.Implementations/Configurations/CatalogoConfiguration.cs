using Credimujer.Op.Domail.Models.Entities;
using Credimujer.Op.Repository.Implementations.Configurations.Base;
using Microsoft.EntityFrameworkCore;

namespace Credimujer.Op.Repository.Implementations.Configurations
{
    public class CatalogoConfiguration : EntityConfiguration<CatalogoEntity>
    {
        public CatalogoConfiguration(ModelBuilder modelBuilder)
        {
            var entityBuilder = modelBuilder.Entity<CatalogoEntity>();
            entityBuilder.ToTable("ASO_CATALOGO");
            entityBuilder.HasKey(c => c.Id);
            entityBuilder.Property(c => c.Id).HasColumnName("IN_ID");
            entityBuilder.Property(c => c.Codigo).HasColumnName("VC_CODIGO");
            entityBuilder.Property(c => c.Descripcion).HasColumnName("VC_NOMBRE");

            Configure(entityBuilder);
        }
    }
}
