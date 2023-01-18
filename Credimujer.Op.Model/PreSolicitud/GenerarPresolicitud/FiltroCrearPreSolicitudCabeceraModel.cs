using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Model.PreSolicitud.GenerarPresolicitud
{
    public class FiltroCrearPreSolicitudCabeceraModel
    {
        public int BancoComunalId { get; set; }
        public int? AnilloGrupalId { get; set; }
        //public string AnilloGrupalCodigo
        //{
        //    set => _AnilloGrupalCodigo = value;
        //    get => _AnilloGrupalCodigo == "" ? null : _AnilloGrupalCodigo;
        //}

        public string FechaDesembolso { get; set; }
        public string TipoCodigo { get; set; }

        private string _AnilloGrupalCodigo;
    }
}