using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Domail.Models.Entities
{
    public class ProvinciaEntity
    {
        public ProvinciaEntity()
        {
            Distritos = new List<DistritoEntity>();
        }

        public string Codigo { get; set; }
        public string DepartamentoCodigo { get; set; }
        public string Descripcion { get; set; }
        public virtual DepartamentoEntity Departamento { get; set; }
        public virtual ICollection<DistritoEntity> Distritos { get; set; }
    }
}