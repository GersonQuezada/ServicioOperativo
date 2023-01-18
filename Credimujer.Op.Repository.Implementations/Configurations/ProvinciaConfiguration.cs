using Credimujer.Op.Domail.Models.Entities;
using Credimujer.Op.Repository.Interfaces.Configuration.Base;
using Microsoft.EntityFrameworkCore;

namespace Credimujer.Op.Repository.Implementations.Configurations
{
    public class ProvinciaConfiguration : IEntityConfiguration<ProvinciaEntity>
    {
        public void Configure(
            Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ProvinciaEntity> entityBuilder)
        {
            entityBuilder.ToTable("PROVINCIA");
            entityBuilder.HasKey(c => new { c.Codigo, c.DepartamentoCodigo });

            entityBuilder.Property(c => c.Codigo).HasColumnName("VC_CODIGO");
            entityBuilder.Property(c => c.DepartamentoCodigo).HasColumnName("VC_DEPARTAMENTO_CODIGO");
            entityBuilder.Property(c => c.Descripcion).HasColumnName("VC_DESCRIPCION");

            entityBuilder.HasOne(c => c.Departamento).WithMany(m => m.Provincias).HasForeignKey(f => f.DepartamentoCodigo);
        }
    }
}