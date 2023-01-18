using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Model.Socia
{
    public class CatalogoModel
    {
        public bool ObtenerEstadoCivil { get; set; }
        public bool ObtenerGradoInstruccion { get; set; }
        public bool ObtenerSituacionDomicilio { get; set; }
        public bool ObtenerAfirmacion { get; set; }
        public bool ObtenerEntidadFinanciera { get; set; }
        public bool ObtenerDepartamento { get; set; }
        public bool ObtenerSucursal { get; set; }
        public bool ObtenerAnilloGrupal { get; set; }
        public bool ObtenerCargoBancoComunal { get; set; }
        public bool ObtenerTipoPresolicitudAGenerar { get; set; }
        public bool ObtenerTipoDocumento { get; set; }
        public bool ObtenerEstadoSocia { get; set; }
    }
}