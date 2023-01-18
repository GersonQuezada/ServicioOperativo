using Credimujer.Op.Domail.Models.Base;
using System;

namespace Credimujer.Op.Domail.Models.Entities
{
    public class FormularioEntity : BaseEntity
    {
        public int Id { get; set; }
        public int SociaId { get; set; }
        public int EstadoCivilId { get; set; }
        public int GradoInstruccionId { get; set; }
        public string Celular { get; set; }
        public string NroDependiente { get; set; }
        public string ActividadEconomica { get; set; }
        public string ActividadEconomica2 { get; set; }
        public string ActividadEconomica3 { get; set; }
        public string Ubicacion { get; set; }
        public string Direccion { get; set; }
        public string Referencia { get; set; }
        public int SituacionDomicilioId { get; set; }
        public int? TieneCtaAhorro { get; set; }
        public int? EntidadBancariaId { get; set; }
        public string NroCuenta { get; set; }
        public string Representante { get; set; }
        public string UbicacionNegocio { get; set; }
        public string DireccionNegocio { get; set; }
        public string ReferenciaNegocio { get; set; }
        public int ? BancoComunalId { get; set; }
        public int? AnilloGrupalId { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int? CargoBancoComunalId { get; set; }

        public string Telefono { get; set; }
        public int TipoDocumentoId { get; set; }

        public virtual SociaEntity Socia { get; set; }
        public virtual CatalogoDetalleEntity EstadoCivil { get; set; }
        public virtual CatalogoDetalleEntity GradoInstruccion { get; set; }
        public virtual CatalogoDetalleEntity SituacionDomicilio { get; set; }
        public virtual CatalogoDetalleEntity EntidadBancaria { get; set; }
        public virtual BancoComunalEntity BancoComunal { get; set; }
        public virtual AnilloGrupalEntity AnilloGrupal { get; set; }
        public virtual CatalogoDetalleEntity CargoBancoComunal { get; set; }
        public virtual CatalogoDetalleEntity TipoDocumento { get; set; }
    }
}