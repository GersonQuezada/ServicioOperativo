using Credimujer.Op.Domail.Models.Entities;
using Credimujer.Op.Dto.Base;
using Credimujer.Op.Repository.Implementations.Data;
using Credimujer.Op.Repository.Implementations.Data.Base;
using Credimujer.Op.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Repository.Implementations
{
    public class AnilloGrupalRepository : BaseRepository<AnilloGrupalEntity>, IAnilloGrupalRepository
    {
        private readonly DataContext _context;

        public AnilloGrupalRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<DropdownDto>> Lista()
        {
            return await _context.AnilloGrupal.Where(p => p.EstadoFila)
                .Select(p => new DropdownDto
                {
                    Id = p.Id,
                    Code = p.Codigo,
                    Description = p.Descripcion,
                    Value = p.BancoComunalCodigo
                }).ToListAsync();
        }

        public async Task<List<AnilloGrupalEntity>> ListarPorBancoComunal(int bancoComunalId)
        {
            return await _context.AnilloGrupal.Where(p => p.EstadoFila && p.BancoComunalId == bancoComunalId)
                .Select(s => new AnilloGrupalEntity()
                {
                    Id = s.Id,
                    Codigo = s.Codigo,
                    BancoComunalCodigo = s.BancoComunalCodigo,
                    BancoComunalId = s.BancoComunalId,
                    Descripcion = s.Descripcion,
                    Ciclo = s.Ciclo,
                    EstadoId = s.EstadoId,
                    Correlativo = s.Correlativo,
                    BancoComunal = s.BancoComunal
                }).ToListAsync();
        }
    }
}