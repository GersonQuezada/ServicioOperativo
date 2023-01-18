using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Model.Service.Iam
{
    public class ActualizarPasswordModel
    {
        public string Usuario { get; set; }
        public string Contrasenia { get; set; }
        public string NuevaContrasenia { get; set; }
    }
}
