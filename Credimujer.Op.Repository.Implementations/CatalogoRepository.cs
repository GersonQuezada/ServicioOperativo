using Credimujer.Op.Domail.Models.Entities;
using Credimujer.Op.Repository.Implementations.Data;
using Credimujer.Op.Repository.Implementations.Data.Base;
using Credimujer.Op.Repository.Interfaces;

namespace Credimujer.Op.Repository.Implementations
{
    public class CatalogoRepository : BaseRepository<CatalogoEntity>, ICatalogoRepository
    {
        private readonly DataContext _context;
        public CatalogoRepository(DataContext context) : base(context)
        {
            this._context = context;
        }
    }
}
