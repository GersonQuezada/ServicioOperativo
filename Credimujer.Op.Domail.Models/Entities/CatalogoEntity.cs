using Credimujer.Op.Domail.Models.Base;
using System.Collections.Generic;

namespace Credimujer.Op.Domail.Models.Entities
{
    public class CatalogoEntity : BaseEntity
    {
        public CatalogoEntity()
        {
            CatalogoDetalle = new List<CatalogoDetalleEntity>();
        }

        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public virtual ICollection<CatalogoDetalleEntity> CatalogoDetalle { get; set; }
    }
}
