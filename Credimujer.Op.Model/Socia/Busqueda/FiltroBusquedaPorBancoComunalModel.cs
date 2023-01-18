using Credimujer.Op.Model.Base;
using System.Collections.Generic;

namespace Credimujer.Op.Model.Socia.Busqueda
{
    public class FiltroBusquedaPorBancoComunalModel : SortModel
    {
        public FiltroBusquedaPorBancoComunalModel()
        {
            this.ListaSucursalCodigo = new List<string>();
        }

        public string Sucursal { get; set; }
        public int? BancoComunalId { get; set; }
        public int? AnilloGrupalId { get; set; }
        public List<string> ListaSucursalCodigo { get; set; }
    }
}