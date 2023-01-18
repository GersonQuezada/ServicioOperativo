using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Credimujer.Op.Domail.Models.Entities;
using Credimujer.Op.Domail.Models.Entities.Procedure;
using Credimujer.Op.Repository.Implementations.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Credimujer.Op.Repository.Implementations.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ListadoProductoSociaEntity> SPListadoProductoSocia { get; set; }
        public virtual DbSet<ListadoProductoSociaEntity> SPListadoProductoSociaDni { get; set; }
        public virtual DbSet<ListadoCreditoCabeceraEntity> SPListadoCreditoCabecera { get; set; }
        public virtual DbSet<ListadoCreditoDetalleEntity> SPListadoCreditoDetalle { get; set; }
        public virtual DbSet<ObtenerTipoDeudaPorDni> ObtenerTipoDeudaPorDni { get; set; }
        public virtual DbSet<SociaMotivoBajasEntity> SPSociaMotivoBaja { get; set; }
        public virtual DbSet<ObtenerTipoRiesgoPorDni> SPObtenerTipoRiesgo { get; set; }

        public virtual DbSet<ObtenerCapacidadPagoEntity> SPObtenerCapacidadPagos { get; set; }
        public virtual DbSet<BusquedaSociaEntity> SPBusquedaSocia { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new SociaConfiguration(builder));
            builder.ApplyConfiguration(new DepartamentoConfiguration());
            builder.ApplyConfiguration(new ProvinciaConfiguration());
            builder.ApplyConfiguration(new DistritoConfiguration());
            builder.ApplyConfiguration(new FormularioConfiguration(builder));
            builder.ApplyConfiguration(new CatalogoConfiguration(builder));
            builder.ApplyConfiguration(new CatalogoDetalleConfiguration(builder));
            builder.ApplyConfiguration(new PreSolicitudConfiguration(builder));
            builder.ApplyConfiguration(new BancoComunalConfiguration(builder));
            builder.ApplyConfiguration(new AnilloGrupalConfiguration(builder));
            builder.ApplyConfiguration(new PreSolicitudCabeceraConfiguration(builder));
        }

        public DbSet<SociaEntity> Socia { get; set; }
        public DbSet<DepartamentoEntity> Departamento { get; set; }
        public DbSet<ProvinciaEntity> Provincia { get; set; }
        public DbSet<DistritoEntity> Distrito { get; set; }
        public DbSet<FormularioEntity> Formulario { get; set; }
        public DbSet<CatalogoEntity> Catalogo { get; set; }
        public DbSet<CatalogoDetalleEntity> CatalogoDetalle { get; set; }
        public DbSet<PreSolicitudEntity> PreSolicitud { get; set; }
        public DbSet<BancoComunalEntity> BancoComunal { get; set; }
        public DbSet<AnilloGrupalEntity> AnilloGrupal { get; set; }
        public DbSet<PreSolicitudCabeceraEntity> PreSolicitudCabecera { get; set; }
    }
}