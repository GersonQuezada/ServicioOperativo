using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Model.Service.Iam
{
    public class ActualizarUsuarioConDniModel
    {
        public string Dni { get; set; }
        public string DniNuevo { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
    }
}