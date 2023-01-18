using System.Collections.Generic;
using System.Threading.Tasks;
using Credimujer.Op.Domail.Models.Entities;
using Credimujer.Op.Dto.Base;
using Credimujer.Op.Repository.Interfaces.Data;

namespace Credimujer.Op.Repository.Interfaces
{
    public interface ICatalogoDetalleRepository : IBaseRepository<CatalogoDetalleEntity>
    {
        Task<List<DropdownDto>> ListarPorCatalogoCodigoParaDropDown(string codigo);

        Task<DropdownDto> ObtenerPorCodigoConEstadoActivo(string codigo);

        Task<List<CatalogDto>> ObtenerPorListaCodigoCatalogo(List<string> listaCodigo);

        Task<List<DropdownDto>> ObtenerPorListaCodigoyActivoInactivo(List<string> listCodigo);

        Task<List<DropdownDto>> ObtenerPorValoryActivoInactivo(string codigo);

        Task<List<DropdownDto>> ObtenerPorListCodigo(List<string> listCodigo);
    }
}