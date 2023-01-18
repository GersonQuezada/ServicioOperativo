using Credimujer.Op.Model.Base;

namespace Credimujer.Op.Model.Socia.Busqueda
{
    public class FiltroSociaParaAprobarModel :  SortModel
    {
        public string Dni { get; set; }
        public string ApellidoNombre { get; set; }
        public string SucursalCodigo { get; set; }
        public int? BancoComunalId { get; set; }
        public int? AnilloGrupalId { get; set; }
        public int? EstadoSociaId { get; set; }
    }
}
