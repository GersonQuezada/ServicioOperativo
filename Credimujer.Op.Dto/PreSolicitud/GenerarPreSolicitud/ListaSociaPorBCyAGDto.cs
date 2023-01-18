using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Dto.PreSolicitud.GenerarPreSolicitud
{
    public class ListaSociaPorBCyAGDto
    {
        public int Id { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Nombre { get; set; }
        public string Dni { get; set; }
        public int? SociaId_SistemaExterno { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public FormularioSocia Formulario { get; set; }
        public bool Reingresante { get; set; }
        public bool DatoIncompleto { get; set; }
        public class FormularioSocia
        {
            public int Id { get; set; }
            public string BancoComunal { get; set; }
            public string AnilloGrupal { get; set; }
        }
    }
}