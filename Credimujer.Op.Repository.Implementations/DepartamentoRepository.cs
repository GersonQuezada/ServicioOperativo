using Credimujer.Op.Domail.Models.Entities;
using Credimujer.Op.Dto.Base;
using Credimujer.Op.Repository.Implementations.Data;
using Credimujer.Op.Repository.Implementations.Data.Base;
using Credimujer.Op.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Credimujer.Op.Repository.Implementations
{
    public class DepartamentoRepository : BaseRepository<DepartamentoEntity>, IDepartamentoRepository
    {
        private readonly DataContext _context;

        public DepartamentoRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<DropdownDto>> ListarDropdown()
        {
            return await _context.Departamento.Select(s => new DropdownDto()
            {
                Code = s.Codigo,
                Description = s.Descripcion
            }).ToListAsync();
        }
    }
}
