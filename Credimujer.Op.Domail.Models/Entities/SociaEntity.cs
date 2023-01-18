using Credimujer.Op.Domail.Models.Base;
using System.Collections.Generic;

namespace Credimujer.Op.Domail.Models.Entities
{
    public class SociaEntity : BaseEntity
    {
        public SociaEntity()
        {
            Formulario = new List<FormularioEntity>();
            ListaPreSolicitud = new List<PreSolicitudEntity>();
            ListaPreSolicitudDj = new List<PreSolicitudEntity>();
        }

        public int Id { get; set; }
        public string NroDni { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Nombre { get; set; }
        public string Celular { get; set; }
        public string Telefono { get; set; }
        public string EntidadBancario { get; set; }
        public string NroCuenta { get; set; }
        public int SucursalId { get; set; }

        //public string Departamento { get; set; }
        public int EstadoId { get; set; }

        public string CodigoCliente { get; set; }
        public int? BancoComunalId { get; set; }
        public int? SociaId_SistemaExterno { get; set; }
        public int? CargoBancoComunalId { get; set; }
        public int TipoDocumentoId { get; set; }
        public bool? Reingresante { get; set; }
        public bool? DatoIncompleto { get; set; }
        public virtual ICollection<FormularioEntity> Formulario { get; set; }
        public virtual CatalogoDetalleEntity Estado { get; set; }
        public virtual ICollection<PreSolicitudEntity> ListaPreSolicitud { get; set; }
        public virtual CatalogoDetalleEntity Sucursal { get; set; }
        public virtual BancoComunalEntity BancoComunal { get; set; }
        public virtual CatalogoDetalleEntity TipoDocumento { get; set; }
        public virtual ICollection<PreSolicitudEntity> ListaPreSolicitudDj { get; set; }
    }
}