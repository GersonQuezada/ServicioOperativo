using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Model.Socia.Actualizar
{
    public class InformacionSociaModel
    {
        public int Id { get; set; }
        public string Celular { get; set; }
        public string Telefono { get; set; }
        public int? EntidadBancariaId { get; set; }
        public string NroCuenta { get; set; }
        public string Actividad { get; set; }
        public string Actividad2 { get; set; }
        public string Actividad3 { get; set; }
        public int CargoBancoComunalId { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string FechaNacimiento { get; set; }
        public string NroDni { get; set; }

        public string Ubicacion { get; set; }
        public string Direccion { get; set; }
        public string Referencia { get; set; }

        public string UbicacionNegocio { get; set; }
        public string DireccionNegocio { get; set; }
        public string ReferenciaNegocio { get; set; }

    }
}