using System.Text;
using Credimujer.Op.Domail.Models.Entities;
using Credimujer.Op.Repository.Implementations.Configurations.Base;
using Microsoft.EntityFrameworkCore;

namespace Credimujer.Op.Repository.Implementations.Configurations
{
    public class SociaConfiguration : EntityConfiguration<SociaEntity>
    {
        public SociaConfiguration(ModelBuilder modelBuilder)
        {
            var entityBuilder = modelBuilder.Entity<SociaEntity>();
            entityBuilder.ToTable("ASO_SOCIA");
            entityBuilder.HasKey(c => c.Id);
            entityBuilder.Property(c => c.Id).HasColumnName("IN_ID");
            entityBuilder.Property(c => c.NroDni).HasColumnName("VC_NRODNI");
            entityBuilder.Property(c => c.ApellidoPaterno).HasColumnName("VC_APELLIDOPAT");
            entityBuilder.Property(c => c.ApellidoMaterno).HasColumnName("VC_APELLIDOMAT");
            entityBuilder.Property(c => c.Nombre).HasColumnName("VC_NOMBRES");
            entityBuilder.Property(c => c.Celular).HasColumnName("VC_CELULAR");
            entityBuilder.Property(c => c.Telefono).HasColumnName("VC_TLF_FIJO");
            entityBuilder.Property(c => c.EntidadBancario).HasColumnName("VC_ENTIDAD_BANCO");
            entityBuilder.Property(c => c.NroCuenta).HasColumnName("VC_NRO_CTA");
            entityBuilder.Property(c => c.EstadoId).HasColumnName("IN_ESTADO");
            entityBuilder.Property(c => c.CodigoCliente).HasColumnName("VC_COD_CLI");
            entityBuilder.Property(c => c.SucursalId).HasColumnName("IN_SUCURSAL_ID");
            entityBuilder.Property(c => c.BancoComunalId).HasColumnName("IN_BANCO_COMUNAL_ID");
            entityBuilder.Property(c => c.SociaId_SistemaExterno).HasColumnName("IN_ID_SISTEMA_EXTERNO");
            entityBuilder.Property(c => c.CargoBancoComunalId).HasColumnName("IN_CARGO_BANCO_COMUNAL_ID");
            entityBuilder.Property(c => c.TipoDocumentoId).HasColumnName("IN_TIPO_DOCU");
            entityBuilder.Property(c => c.Reingresante).HasColumnName("BT_REINGRESANTE");
            entityBuilder.Property(c => c.DatoIncompleto).HasColumnName("VC_DATO_INCOMPLETO");

            entityBuilder.HasOne(c => c.Estado).WithMany(m => m.SociaEstado).HasForeignKey(f => f.EstadoId);
            entityBuilder.HasOne(c => c.Sucursal).WithMany(m => m.SociaSucursal).HasForeignKey(f => f.SucursalId);
            entityBuilder.HasOne(c => c.BancoComunal).WithMany(m => m.Socia).HasForeignKey(f => f.BancoComunalId);
            entityBuilder.HasOne(c => c.TipoDocumento).WithMany(m => m.SociaTipoDocumento).HasForeignKey(f => f.TipoDocumentoId);
            Configure(entityBuilder);
        }
    }
}