using Credimujer.Op.Domail.Models.Entities;
using Credimujer.Op.Repository.Interfaces.Configuration.Base;
using Microsoft.EntityFrameworkCore;

namespace Credimujer.Op.Repository.Implementations.Configurations
{
    public class DistritoConfiguration : IEntityConfiguration<DistritoEntity>
    {
        public void Configure(
            Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<DistritoEntity> entityBuilder)
        {
            entityBuilder.ToTable("DISTRITO");
            entityBuilder.HasKey(c => new { c.Codigo, c.DepartamentoCodigo, c.ProvinciaCodigo });
            entityBuilder.Property(c => c.Codigo).HasColumnName("VC_CODIGO");
            entityBuilder.Property(c => c.DepartamentoCodigo).HasColumnName("VC_DEPARTAMENTO_CODIGO");
            entityBuilder.Property(c => c.ProvinciaCodigo).HasColumnName("VC_PROVINCIA_CODIGO");
            entityBuilder.Property(c => c.Descripcion).HasColumnName("VC_DESCRIPCION");

            entityBuilder.HasOne(c => c.Departamento).WithMany(m => m.Distritos).HasForeignKey(f => f.DepartamentoCodigo);
            entityBuilder.HasOne(c => c.Provincia).WithMany(m => m.Distritos).HasForeignKey(f => new { f.ProvinciaCodigo, f.DepartamentoCodigo });
        }
    }
}