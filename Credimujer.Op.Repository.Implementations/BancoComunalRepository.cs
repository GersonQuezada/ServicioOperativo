using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Credimujer.Op.Common;
using Credimujer.Op.Domail.Models.Entities;
using Credimujer.Op.Dto.Base;
using Credimujer.Op.Dto.PreSolicitud.Busqueda;
using Credimujer.Op.Dto.PreSolicitud.Common;
using Credimujer.Op.Dto.Socia.Reporte;
using Credimujer.Op.Repository.Implementations.Data;
using Credimujer.Op.Repository.Implementations.Data.Base;
using Credimujer.Op.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Credimujer.Op.Repository.Implementations
{
    public class BancoComunalRepository : BaseRepository<BancoComunalEntity>, IBancoComunalRepository
    {
        private readonly DataContext _context;

        public BancoComunalRepository(DataContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<List<DropdownDto>> BusquedaPorDescripcion(string descripcion, List<string> sucursal)
        {
            var query = _context.BancoComunal.Where(p => p.EstadoFila && p.Descripcion.Contains(descripcion)
            && p.Estado.Codigo == Constants.Core.Catalogo.DetEstado.Activo
            && sucursal.Contains(p.Sucursal.Codigo)
            );

            return await query.Select(s => new DropdownDto()
            {
                Id = s.Id,
                Code = s.Codigo,
                Value = s.Sucursal.Codigo,
                Description = s.Descripcion
            })
            .Take(30).ToListAsync();
        }

        public async Task<string> ObtenerSucursalPorCodigo(int? bancoComunalId)
        {
            return await _context.BancoComunal.Where(p => p.EstadoFila && p.Id == bancoComunalId
            && p.Estado.Codigo == Constants.Core.Catalogo.DetEstado.Activo
            ).Select(s => s.Sucursal.Codigo).FirstAsync();
        }

        public async Task<List<ListaBancoComunalDto>> ListarPorDescripcion(string descripcion, List<string> sucursal)
        {
            var query = _context.BancoComunal.Where(p => p.EstadoFila && p.Descripcion.Contains(descripcion)
            && sucursal.Contains(p.Sucursal.Codigo)
            && p.Estado.Codigo == Constants.Core.Catalogo.DetEstado.Activo
            );

            return await query.Select(s => new ListaBancoComunalDto()
            {
                Id = s.Id,
                Code = s.Codigo,
                Description = s.Descripcion,
                TienePeriodoGracia = s.PeriodoGracia,
                SucursalId = s.SucursalId,
                SecuenciaMaxAnilloGrupal = s.AnilloGrupal.Max(m => m.Correlativo)
            })
            .Take(30).ToListAsync();
        }

        public async Task<BancoComunalDto> ObtenerPorCodigo(int id)
        {
            var query = _context.BancoComunal.Where(p => p.EstadoFila && p.Id == id
                && p.Estado.Codigo == Constants.Core.Catalogo.DetEstado.Activo
            );
            return await query.Select(s => new BancoComunalDto()
            {
                BancoComunal = s.Descripcion,
                Ciclo = s.Ciclo,
                Sucursal = s.Sucursal.Descripcion
            }).FirstOrDefaultAsync();
        }

        public async Task<BancoComunalyAnilloGrupalDto> ObtenerBancoyAnillo(int bancoComunalId, int? anilloGrupalId)
        {
            if (!anilloGrupalId.HasValue)
            {
                return await _context.BancoComunal.Where(p => p.EstadoFila && p.Id == bancoComunalId)
                    .Select(s => new BancoComunalyAnilloGrupalDto()
                    {
                        BancoComunal = s.Descripcion,
                        AnilloGrupal = string.Empty
                    }).FirstOrDefaultAsync();
            }
            else
            {
                return await _context.BancoComunal.Where(p => p.EstadoFila && p.Id == bancoComunalId)
                        .Select(s => new BancoComunalyAnilloGrupalDto()
                        {
                            BancoComunal = s.Descripcion,
                            AnilloGrupal = s.AnilloGrupal.FirstOrDefault(f => f.EstadoFila && f.Id == anilloGrupalId).Descripcion
                        }).FirstOrDefaultAsync();
            }
        }

        public async Task<BancoComunalEntity> ObtenerPorId(int bancoComunalId)
        {
            return await _context.BancoComunal.Where(p => p.EstadoFila && p.Estado.Codigo == Constants.Core.Catalogo.DetEstado.Activo
            && p.Id == bancoComunalId
            )
                .FirstOrDefaultAsync();
        }
    }
}