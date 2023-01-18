using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Dto.Socia.Registro
{
    public class ActualizarDatoPersonalDto
    {
        public int Id { get; set; }
        public string Dni { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Nombre { get; set; }
    }
}
