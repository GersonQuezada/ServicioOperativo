using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Dto.Oficial
{
    public class ResultadoResumenRegistroIngresadoDto
    {
        public int SucursalId { get; set; }
        public string Sucursal { get; set; }
        public string Oficial { get; set; }
        public int CantidadRegistros { get; set; }
        public DateTime? FechaIni { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}