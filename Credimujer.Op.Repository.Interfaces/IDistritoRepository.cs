using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Credimujer.Op.Domail.Models.Entities;
using Credimujer.Op.Repository.Interfaces.Data;

namespace Credimujer.Op.Repository.Interfaces
{
    public interface IDistritoRepository : IBaseRepository<DistritoEntity>
    {
        Task<List<DistritoEntity>> Lista();

        Task<DistritoEntity> ObtenerPorCodigo(string departamentoCodigo, string provinciaCodigo
            , string distritoCodigo);
    }
}