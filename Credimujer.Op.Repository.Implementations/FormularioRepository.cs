using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Credimujer.Op.Common;
using Credimujer.Op.Domail.Models.Entities;
using Credimujer.Op.Dto.Socia.Validacion;
using Credimujer.Op.Repository.Implementations.Data;
using Credimujer.Op.Repository.Implementations.Data.Base;
using Credimujer.Op.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Credimujer.Op.Repository.Implementations
{
    public class FormularioRepository : BaseRepository<FormularioEntity>, IFormularioRepository
    {
        private readonly DataContext _context;

        public FormularioRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<FormularioEntity> ObtenerPorCodigoPorSocia(int sociaId)
        {
            return await _context.Formulario.Where(p => p.EstadoFila && p.SociaId == sociaId
            && p.Socia.Estado.Codigo == Constants.Core.Catalogo.DetEstadoSocia.Activa)
                .FirstOrDefaultAsync();
        }

        public async Task<List<FormularioEntity>> ObtenerPorBancoComunalyAnillo(int bancoComunalId, int? anilloGrupalId)
        {
            var query = _context.Formulario.Where(p => p.EstadoFila && p.BancoComunalId == bancoComunalId);
            if (anilloGrupalId.HasValue)
            {
                query = query.Where(p => p.AnilloGrupalId == anilloGrupalId);
            }
            return await query.ToListAsync();
        }

        public async Task<int> TotalBancoComunalyAnillo(int bancoComunalId, int? anilloGrupalId)
        {
            var query = _context.Formulario.Where(p => p.EstadoFila && p.BancoComunalId == bancoComunalId
            && p.AnilloGrupalId == anilloGrupalId
            && p.Socia.Estado.Codigo == Constants.Core.Catalogo.DetEstadoSocia.Activa
            );
            //if (!string.IsNullOrEmpty(anilloGrupalCodigo))
            //{
            //    query = query.Where(p => p.AnilloGrupalCodigo == anilloGrupalCodigo);
            //}
            return await query.CountAsync();
        }

        public async Task<List<FormularioEntity>> ObtenerPorSocia(int sociaId)
        {
            var query = _context.Formulario.Where(p => p.EstadoFila && p.SociaId == sociaId);
            return await query.ToListAsync();
        }

        public async Task<List<SociasPorBancoComunalyAnilloDto>> ObtenerPorBancoComunalyAnilloGrupal(int bancoComunalId, int? anilloGrupalId)
        {
            var query = _context.Formulario.Where(p => p.EstadoFila && p.BancoComunalId == bancoComunalId);
            if (anilloGrupalId.HasValue)
                query = query.Where(p => p.AnilloGrupalId == anilloGrupalId);

            return await query.Select(s => new SociasPorBancoComunalyAnilloDto()
            {
                SociaId = s.SociaId,
                CargoBancoComunalId = s.CargoBancoComunalId,
                BancoComunalId = s.BancoComunalId,
                AnilloGrupalId = s.AnilloGrupalId,
                Nombre = s.Socia.Nombre + " " + s.Socia.ApellidoPaterno + " " + s.Socia.ApellidoMaterno
            }).ToListAsync();
        }
    }
}