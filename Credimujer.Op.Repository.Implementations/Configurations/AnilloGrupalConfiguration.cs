using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Credimujer.Op.Domail.Models.Entities;
using Credimujer.Op.Repository.Implementations.Configurations.Base;
using Microsoft.EntityFrameworkCore;

namespace Credimujer.Op.Repository.Implementations.Configurations
{
    public class AnilloGrupalConfiguration : EntityConfiguration<AnilloGrupalEntity>
    {
        public AnilloGrupalConfiguration(ModelBuilder modelBuilder)
        {
            var entityBuilder = modelBuilder.Entity<AnilloGrupalEntity>();
            entityBuilder.ToTable("ASO_ANILLO_GRUPAL");
            entityBuilder.HasKey(c => c.Id);
            entityBuilder.Property(c => c.Id).HasColumnName("IN_ID");
            entityBuilder.Property(c => c.Codigo).HasColumnName("VC_CODIGO");
            entityBuilder.Property(c => c.BancoComunalCodigo).HasColumnName("VC_BANCO_COMUNAL_CODIGO");
            entityBuilder.Property(c => c.Descripcion).HasColumnName("VC_DESCRIPCION");
            entityBuilder.Property(c => c.Ciclo).HasColumnName("IN_CICLO");
            entityBuilder.Property(c => c.EstadoId).HasColumnName("ESTADO_ID");
            entityBuilder.Property(c => c.Correlativo).HasColumnName("IN_CORRELATIVO");
            entityBuilder.Property(c => c.BancoComunalId).HasColumnName("IN_BANCO_COMUNAL_ID");
            entityBuilder.HasOne(c => c.BancoComunal).WithMany(m => m.AnilloGrupal)
                .HasForeignKey(f => f.BancoComunalId);
            entityBuilder.HasOne(c => c.Estado).WithMany(m => m.AnilloGrupalEstado).HasForeignKey(f => f.EstadoId);
            Configure(entityBuilder);
        }
    }
}