using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Dto.Socia.Busqueda
{
    public class ListaSociaParaAprobarDto
    {
        public int Id { get; set; }
        public string Dni { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Nombre { get; set; }
        public string Estado { get; set; }
        public string EstadoCodigo { get; set; }
        public string BancoComunal { get; set; }
        public string AnilloGrupal { get; set; }
        public string BancoComunalCodigo { get; set; }
        public string CargoBancoComunalCodigo { get; set; }
        public int? BancoComunalId { get; set; }
    }
}