using Credimujer.Op.Domail.Models.Entities;
using Credimujer.Op.Dto.Base;
using Credimujer.Op.Repository.Interfaces.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Credimujer.Op.Repository.Interfaces
{
    public interface IDepartamentoRepository : IBaseRepository<DepartamentoEntity>
    {
        Task<List<DropdownDto>> ListarDropdown();
    }
}
