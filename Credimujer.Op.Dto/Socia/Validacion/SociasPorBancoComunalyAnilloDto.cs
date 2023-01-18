using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Dto.Socia.Validacion
{
    public class SociasPorBancoComunalyAnilloDto
    {
        public int SociaId { get; set; }
        public string Nombre { get; set; }
        public int? BancoComunalId { get; set; }
        public int? AnilloGrupalId { get; set; }
        public int? CargoBancoComunalId { get; set; }
    }
}