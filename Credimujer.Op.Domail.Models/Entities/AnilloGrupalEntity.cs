using Credimujer.Op.Domail.Models.Base;
using System.Collections.Generic;

namespace Credimujer.Op.Domail.Models.Entities
{
    public class AnilloGrupalEntity : BaseEntity
    {
        public AnilloGrupalEntity()
        {
            Formulario = new List<FormularioEntity>();
            PreSolicitudCabecera = new List<PreSolicitudCabeceraEntity>();
        }

        public int Id { get; set; }
        public string Codigo { get; set; }
        public string BancoComunalCodigo { get; set; }
        public int BancoComunalId { get; set; }
        public string Descripcion { get; set; }
        public int? Ciclo { get; set; }
        public int EstadoId { get; set; }
        public int Correlativo { get; set; }
        public virtual BancoComunalEntity BancoComunal { get; set; }
        public virtual CatalogoDetalleEntity Estado { get; set; }
        public virtual ICollection<FormularioEntity> Formulario { get; set; }
        public virtual ICollection<PreSolicitudCabeceraEntity> PreSolicitudCabecera { get; set; }
    }
}