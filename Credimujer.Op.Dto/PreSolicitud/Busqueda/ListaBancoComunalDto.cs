using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Dto.PreSolicitud.Busqueda
{
    public class ListaBancoComunalDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool TienePeriodoGracia { get; set; }
        public int SucursalId { get; set; }
        public int? SecuenciaMaxAnilloGrupal { get; set; }
    }
}