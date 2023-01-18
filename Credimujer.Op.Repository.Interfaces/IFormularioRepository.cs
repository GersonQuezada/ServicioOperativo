using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Credimujer.Op.Domail.Models.Entities;
using Credimujer.Op.Dto.Socia.Validacion;
using Credimujer.Op.Repository.Interfaces.Data;

namespace Credimujer.Op.Repository.Interfaces
{
    public interface IFormularioRepository : IBaseRepository<FormularioEntity>
    {
        Task<FormularioEntity> ObtenerPorCodigoPorSocia(int sociaId);

        Task<List<FormularioEntity>> ObtenerPorBancoComunalyAnillo(int bancoComunalId, int? anilloGrupalId);

        Task<int> TotalBancoComunalyAnillo(int bancoComunalId, int? anilloGrupalId);

        Task<List<FormularioEntity>> ObtenerPorSocia(int sociaId);

        Task<List<SociasPorBancoComunalyAnilloDto>> ObtenerPorBancoComunalyAnilloGrupal(int bancoComunalId, int? anilloGrupalId);
    }
}