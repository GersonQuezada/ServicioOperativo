using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Credimujer.Op.Domail.Models.Entities;
using Credimujer.Op.Repository.Implementations.Data;
using Credimujer.Op.Repository.Implementations.Data.Base;
using Credimujer.Op.Repository.Interfaces;

namespace Credimujer.Op.Repository.Implementations
{
    public class ProvinciaRepository : BaseRepository<ProvinciaEntity>, IProvinciaRepository
    {
        private readonly DataContext _context;

        public ProvinciaRepository(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}
