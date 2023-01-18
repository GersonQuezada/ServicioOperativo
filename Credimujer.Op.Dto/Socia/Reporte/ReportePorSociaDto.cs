using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Dto.Socia.Reporte
{
    public class ReportePorSociaDto
    {
        public string Usuario { get; set; }
        public string Sucursal { get; set; }
        public string ApePaterno { get; set; }
        public string ApeMaterno { get; set; }
        public string Nombre { get; set; }
        public string Dni { get; set; }
        public DateTime? FechaNac { get; set; }
        public int Edad { get; set; }
        public string ActividadEconomica { get; set; }
        public string Celular { get; set; }
        public string GradoInstruccion { get; set; }
        public string NivelRiesgo { get; set; }//
        public string TipoDeuda { get; set; }
        public string Departamento { get; set; }
        public string Provincia { get; set; }
        public string Distrito { get; set; }
        public string Localidad { get; set; }
        public string Direccion { get; set; }
        public string Referencia { get; set; }
        public string BancoComunal { get; set; }
        public string EntidadFinanciera { get; set; }
        public int? IdSocia { get; set; }
        public string Cargo { get; set; }//
        public string NroCtaAhorro { get; set; }
        public string Ubicacion { get; set; }
        public string EstadoCivil { get; set; }
        //public string UbicacionNegocio { get; set; }
    }
}