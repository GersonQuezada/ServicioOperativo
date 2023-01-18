using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Dto.Socia.Busqueda
{
    public class ListaSociaMotivoBajaDto
    {
        public string BancoComunal { get; set; }

        public string Dni { get; set; }

        public string Socia { get; set; }

        public string TipoBaja { get; set; }

        public string Motivo { get; set; }
        public DateTime? FechaBaja { get; set; }
    }
}