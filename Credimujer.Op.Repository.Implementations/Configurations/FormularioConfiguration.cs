using Credimujer.Op.Domail.Models.Entities;
using Credimujer.Op.Repository.Implementations.Configurations.Base;
using Microsoft.EntityFrameworkCore;

namespace Credimujer.Op.Repository.Implementations.Configurations
{
    public class FormularioConfiguration : EntityConfiguration<FormularioEntity>
    {
        public FormularioConfiguration(ModelBuilder modelBuilder)
        {
            var entityBuilder = modelBuilder.Entity<FormularioEntity>();
            entityBuilder.ToTable("ASO_FORMULARIO");
            entityBuilder.HasKey(c => c.Id);
            entityBuilder.Property(c => c.Id).HasColumnName("IN_ID");
            entityBuilder.Property(c => c.SociaId).HasColumnName("IN_SOCIA_ID");
            entityBuilder.Property(c => c.EstadoCivilId).HasColumnName("IN_ESTADO_CIVIL_ID");
            entityBuilder.Property(c => c.GradoInstruccionId).HasColumnName("IN_GRADO_INSTRUCCION_ID");
            entityBuilder.Property(c => c.Celular).HasColumnName("VC_NROCELULAR");
            entityBuilder.Property(c => c.NroDependiente).HasColumnName("VC_NRODEPENDIENTES");
            entityBuilder.Property(c => c.ActividadEconomica).HasColumnName("VC_ACTIVIDAD_ECONOMICA");
            entityBuilder.Property(c => c.ActividadEconomica2).HasColumnName("VC_ACTIVIDAD_ECONOMICA_2");
            entityBuilder.Property(c => c.ActividadEconomica3).HasColumnName("VC_ACTIVIDAD_ECONOMICA_3");
            entityBuilder.Property(c => c.Ubicacion).HasColumnName("VC_UBICACION");
            entityBuilder.Property(c => c.Direccion).HasColumnName("VC_DIRECCION");
            entityBuilder.Property(c => c.Referencia).HasColumnName("VC_REFERENCIA");
            entityBuilder.Property(c => c.SituacionDomicilioId).HasColumnName("IN_SITUACION_DOMICILIO_ID");
            entityBuilder.Property(c => c.TieneCtaAhorro).HasColumnName("IN_TIENE_CTA_AHORRO_ID");
            entityBuilder.Property(c => c.EntidadBancariaId).HasColumnName("IN_ENTIDAD_BANCARIA_ID");
            entityBuilder.Property(c => c.NroCuenta).HasColumnName("VC_NRO_CUENTA");
            entityBuilder.Property(c => c.Representante).HasColumnName("VC_REPRESENTANTE");
            entityBuilder.Property(c => c.UbicacionNegocio).HasColumnName("VC_UBICACION_NEGOCIO");
            entityBuilder.Property(c => c.DireccionNegocio).HasColumnName("VC_DIRECCION_NEGOCIO");
            entityBuilder.Property(c => c.ReferenciaNegocio).HasColumnName("VC_REFERENCIA_NEGOCIO");

            entityBuilder.Property(c => c.BancoComunalId).HasColumnName("IN_BANCO_COMUNAL_ID");
            entityBuilder.Property(c => c.AnilloGrupalId).HasColumnName("IN_ID_ANILLO_GRUPAL");
            entityBuilder.Property(c => c.FechaNacimiento).HasColumnName("DT_FECHA_NACIMIENTO");

            entityBuilder.Property(c => c.CargoBancoComunalId).HasColumnName("IN_CARGO_BANCO_COMUNAL_ID");
            entityBuilder.Property(c => c.Telefono).HasColumnName("VC_TELEFONO");
            entityBuilder.Property(c => c.TipoDocumentoId).HasColumnName("IN_TIPO_DOCU");

            entityBuilder.HasOne(c => c.Socia).WithMany(m => m.Formulario).HasForeignKey(f => f.SociaId);
            entityBuilder.HasOne(c => c.EstadoCivil).WithMany(m => m.SociaEstadoCivil).HasForeignKey(f => f.EstadoCivilId);
            entityBuilder.HasOne(c => c.GradoInstruccion).WithMany(m => m.SociaGradoInstruccion).HasForeignKey(f => f.GradoInstruccionId);
            entityBuilder.HasOne(c => c.SituacionDomicilio).WithMany(m => m.SociaSituacionDomicilio).HasForeignKey(f => f.SituacionDomicilioId);
            entityBuilder.HasOne(c => c.EntidadBancaria).WithMany(m => m.SociaEntidadBancaria).HasForeignKey(f => f.EntidadBancariaId);

            entityBuilder.HasOne(c => c.BancoComunal).WithMany(m => m.Formulario)
                .HasForeignKey(f => f.BancoComunalId);
            entityBuilder.HasOne(c => c.AnilloGrupal).WithMany(m => m.Formulario)
                .HasForeignKey(f => f.AnilloGrupalId);

            entityBuilder.HasOne(c => c.CargoBancoComunal).WithMany(m => m.SociaCargoBancoComunal)
                .HasForeignKey(f => f.CargoBancoComunalId);

            entityBuilder.HasOne(c => c.TipoDocumento).WithMany(m => m.FormularioTipoDocumento).HasForeignKey(f => f.TipoDocumentoId);

            Configure(entityBuilder);
        }
    }
}