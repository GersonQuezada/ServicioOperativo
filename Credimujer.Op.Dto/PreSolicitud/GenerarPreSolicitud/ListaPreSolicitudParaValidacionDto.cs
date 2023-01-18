using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Dto.PreSolicitud.GenerarPreSolicitud
{
    public class ListaPreSolicitudParaValidacionDto
    {
        public int Id { get; set; }
        public string NroDni { get; set; }
        public string BancoComunal { get; set; }
        public string AnilloGrupal { get; set; }
        public string BancoComunalCodigo { get; set; }
        public string AnilloGrupalCodigo { get; set; }
        public string TipoCreditoCodigo { get; set; }
        public string TipoCredito { get; set; }
        public string SubTipoCreditoCodigo { get; set; }
        public string SubTipoCredito { get; set; }
        public string EstadoCodigo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int? PreSolicitudCabecera { get; set; }
        public int SociaId { get; set; }
    }
}