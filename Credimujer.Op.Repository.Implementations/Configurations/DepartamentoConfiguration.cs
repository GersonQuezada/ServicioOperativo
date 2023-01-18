using Credimujer.Op.Domail.Models.Entities;
using Credimujer.Op.Repository.Interfaces.Configuration.Base;
using Microsoft.EntityFrameworkCore;

namespace Credimujer.Op.Repository.Implementations.Configurations
{
    public class DepartamentoConfiguration : IEntityConfiguration<DepartamentoEntity>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<DepartamentoEntity> entityBuilder)
        {
            entityBuilder.ToTable("DEPARTAMENTO");
            entityBuilder.HasKey(c => c.Codigo);
            entityBuilder.Property(c => c.Codigo).HasColumnName("VC_CODIGO");
            entityBuilder.Property(c => c.Descripcion).HasColumnName("VC_DESCRIPCION");
        }
    }
}