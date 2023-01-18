using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Dto.Socia.Reporte
{
    public class ReporteListaSociaDto
    {
        public ReporteListaSociaDto()
        {
            Cabecera = new _Cabecera();
            Detalle = new List<_Detalle>();
        }

        public _Cabecera Cabecera { get; set; }
        public List<_Detalle> Detalle { get; set; }

        public class _Cabecera
        {
            //public string SubTipoPreSolicitud { get; set; }
            public string Sucursal { get; set; }

            public string BancoComunal { get; set; }
            public int Ciclo { get; set; }
            public DateTime Fecha { get; set; }
            public string Usuario { get; set; }
        }

        public class _Detalle
        {
            public int Id { get; set; }
            public int? IdSocia { get; set; }
            public string Nombre { get; set; }
            public string TipoDocumento { get; set; }
            public string NroDni { get; set; }
            public DateTime? FechaNacimiento { get; set; }
            public string Producto { get; set; }
            public string Cargo { get; set; }
            public int Edad { get; set; }
            public string GradoInstruccion { get; set; }
            public string Riesgo { get; set; } //
            public string TipoDeuda { get; set; }//
            public string NroDependiente { get; set; }
            public string Celular { get; set; }
            public string Telefono { get; set; }
            public string TipoVivienda { get; set; }
            public string ActividadEconomica { get; set; }
            public string EntidadFinanciera { get; set; }
            public string NroCuenta { get; set; }

            public string Departamento { get; set; }
            public string Provincia { get; set; }
            public string Distrito { get; set; }
            public string Localidad { get; set; }
            public string Direccion { get; set; }
            public string Referencia { get; set; }

            public string DepartamentoNegocio { get; set; }
            public string ProvinciaNegocio { get; set; }
            public string DistritoNegocio { get; set; }
            public string LocalidadNegocio { get; set; }
            public string DireccionNegocio { get; set; }
            public string ReferenciaNegocio { get; set; }


            public string Ubicacion { get; set; }
            public string UbicacionNegocio { get; set; }
            public DateTime? UltimaModificacion { get; set; }

            public string Reingreso { get; set; }
            public string EstadoCivil { get; set; }
        }
    }
}