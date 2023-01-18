using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Model.PreSolicitud
{
    public class CatalogModel
    {
        public bool TipoCredito { get; set; }
        public bool SubTipoCredito { get; set; }
        public bool TasaInteres { get; set; }
        public bool TasaInteresRebatir { get; set; }
        public bool EntidadFinanciera { get; set; }
        public bool Asistencia { get; set; }
        public bool NivelRiesgo { get; set; }
        public bool AnilloGrupal { get; set; }
        public bool MotivoRetiro { get; set; }
        public bool ObtenerTipoPresolicitudAGenerar { get; set; }
        public bool MostrarControl { get; set; }
        public bool TipoDeuda { get; set; }
    }
}