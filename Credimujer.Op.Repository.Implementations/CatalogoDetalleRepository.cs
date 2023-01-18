using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Credimujer.Op.Domail.Models.Entities;
using Credimujer.Op.Dto.Base;
using Credimujer.Op.Repository.Implementations.Data;
using Credimujer.Op.Repository.Implementations.Data.Base;
using Credimujer.Op.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Credimujer.Op.Repository.Implementations
{
    public class CatalogoDetalleRepository : BaseRepository<CatalogoDetalleEntity>, ICatalogoDetalleRepository
    {
        private readonly DataContext _context;

        public CatalogoDetalleRepository(DataContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<List<DropdownDto>> ListarPorCatalogoCodigoParaDropDown(string codigo)
        {
            return await _context.CatalogoDetalle.Where(p => p.EstadoFila
                                                             && p.Catalogo.Codigo == codigo

                )
                .OrderBy(o => o.Orden)
                .Select(s => new DropdownDto()
                {
                    Id = s.Id,
                    Code = s.Codigo,
                    Description = s.Descripcion,
                    Value = s.Valor
                })
                .ToListAsync();
        }

        public async Task<DropdownDto> ObtenerPorCodigoConEstadoActivo(string codigo)
        {
            return await _context.CatalogoDetalle.Where(p => p.EstadoFila && p.Codigo == codigo)
                .Select(s => new DropdownDto()
                {
                    Id = s.Id,
                    Code = s.Codigo,
                    Description = s.Descripcion
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<DropdownDto>> ObtenerPorValoryActivoInactivo(string codigo)
        {
            return await _context.CatalogoDetalle.Where(p => p.Valor == codigo)
                .Select(s => new DropdownDto()
                {
                    Id = s.Id,
                    Code = s.Codigo,
                    Description = s.Descripcion
                })
                .ToListAsync();
        }

        public async Task<List<DropdownDto>> ObtenerPorListaCodigoyActivoInactivo(List<string> listCodigo)
        {
            return await _context.CatalogoDetalle.Where(p => listCodigo.Contains(p.Codigo))
                .Select(s => new DropdownDto()
                {
                    Id = s.Id,
                    Code = s.Codigo,
                    Description = s.Descripcion
                })
                .ToListAsync();
        }

        public async Task<List<CatalogDto>> ObtenerPorListaCodigoCatalogo(List<string> listaCodigo)
        {
            return await _context.CatalogoDetalle.Where(p => p.EstadoFila &&
                                                             listaCodigo.Contains(p.Catalogo.Codigo))
                .Select(s => new CatalogDto()
                {
                    Id = s.CatalogoId,
                    Code = s.Catalogo.Codigo,
                    DetailId = s.Id,
                    DetailCode = s.Codigo
                }).ToListAsync();
        }

        public async Task<List<DropdownDto>> ObtenerPorListCodigo(List<string> listCodigo)
        {
            return await _context.CatalogoDetalle.Where(p => p.EstadoFila && listCodigo.Contains(p.Codigo))
                .Select(s => new DropdownDto()
                {
                    Id = s.Id,
                    Code = s.Codigo,
                    Description = s.Descripcion
                })
                .ToListAsync();
        }
    }
}