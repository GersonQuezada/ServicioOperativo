using System.Collections.Generic;

namespace Credimujer.Op.Domail.Models.Entities
{
    public class DepartamentoEntity
    {
        public DepartamentoEntity()
        {
            Provincias = new List<ProvinciaEntity>();
            Distritos = new List<DistritoEntity>();
        }

        public string Codigo { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<ProvinciaEntity> Provincias { get; set; }
        public virtual ICollection<DistritoEntity> Distritos { get; set; }
    }
}