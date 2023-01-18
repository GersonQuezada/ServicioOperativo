using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Credimujer.Op.Domail.Models.Entities;
using Credimujer.Op.Repository.Implementations.Data;
using Credimujer.Op.Repository.Implementations.Data.Base;
using Credimujer.Op.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Credimujer.Op.Repository.Implementations
{
    public class DistritoRepository : BaseRepository<DistritoEntity>, IDistritoRepository
    {
        private readonly DataContext _context;

        public DistritoRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<DistritoEntity>> Lista()
        {
            return await _context.Distrito
                .Select(p => new DistritoEntity()
                {
                    Codigo = p.Codigo,
                    DepartamentoCodigo = p.DepartamentoCodigo,
                    ProvinciaCodigo = p.ProvinciaCodigo,
                    Descripcion = p.Descripcion,
                    Departamento = p.Departamento,
                    Provincia = p.Provincia,
                }).ToListAsync();
        }

        public async Task<DistritoEntity> ObtenerPorCodigo(string departamentoCodigo, string provinciaCodigo
            , string distritoCodigo)
        {
            return await _context.Distrito.Where(p => p.DepartamentoCodigo == departamentoCodigo
            && p.ProvinciaCodigo == provinciaCodigo && p.Codigo == distritoCodigo)
                .Select(p => new DistritoEntity()
                {
                    Codigo = p.Codigo,
                    DepartamentoCodigo = p.DepartamentoCodigo,
                    ProvinciaCodigo = p.ProvinciaCodigo,
                    Descripcion = p.Descripcion,
                    Departamento = p.Departamento,
                    Provincia = p.Provincia,
                }).FirstOrDefaultAsync();
        }
    }
}