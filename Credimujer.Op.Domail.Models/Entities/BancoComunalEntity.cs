using System.Collections;
using System.Collections.Generic;
using Credimujer.Op.Domail.Models.Base;

namespace Credimujer.Op.Domail.Models.Entities
{
    public class BancoComunalEntity : BaseEntity
    {
        public BancoComunalEntity()
        {
            AnilloGrupal = new List<AnilloGrupalEntity>();
            Formulario = new List<FormularioEntity>();
            Socia = new List<SociaEntity>();
            PreSolicitudCabecera = new List<PreSolicitudCabeceraEntity>();
        }

        public int Id { get; set; }
        public string Codigo { get; set; }
        public int SucursalId { get; set; }
        public string Descripcion { get; set; }
        public int Ciclo { get; set; }
        public int EstadoId { get; set; }
        public bool PeriodoGracia { get; set; }
        public virtual CatalogoDetalleEntity Sucursal { get; set; }
        public virtual CatalogoDetalleEntity Estado { get; set; }
        public virtual ICollection<AnilloGrupalEntity> AnilloGrupal { get; set; }
        public virtual ICollection<FormularioEntity> Formulario { get; set; }
        public virtual ICollection<SociaEntity> Socia { get; set; }
        public virtual ICollection<PreSolicitudCabeceraEntity> PreSolicitudCabecera { get; set; }
    }
}