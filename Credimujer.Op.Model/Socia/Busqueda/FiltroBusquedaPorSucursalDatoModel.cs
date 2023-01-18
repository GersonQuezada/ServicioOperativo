using Credimujer.Op.Model.Base;

namespace Credimujer.Op.Model.Socia.Busqueda
{
    public class FiltroBusquedaPorSucursalDatoModel : SortModel
    {
        public string SucursalCodigo { get; set; }
        public string Dni { get; set; }
        public string Nombre { get; set; }
    }
}