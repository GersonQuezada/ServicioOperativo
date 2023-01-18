using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Dto.Base
{
    public class CatalogDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public  int DetailId { get; set; }
        public string DetailCode { get; set; }

    }
}
