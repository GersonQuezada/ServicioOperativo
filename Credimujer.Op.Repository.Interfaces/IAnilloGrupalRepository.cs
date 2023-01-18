using Credimujer.Op.Domail.Models.Entities;
using Credimujer.Op.Dto.Base;
using Credimujer.Op.Repository.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Repository.Interfaces
{
    public interface IAnilloGrupalRepository : IBaseRepository<AnilloGrupalEntity>
    {
        Task<List<DropdownDto>> Lista();

        Task<List<AnilloGrupalEntity>> ListarPorBancoComunal(int bancoComunalId);
    }
}