using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Credimujer.Op.Domail.Models.Entities;
using Credimujer.Op.Dto.Base;
using Credimujer.Op.Dto.PreSolicitud.Busqueda;
using Credimujer.Op.Dto.PreSolicitud.Common;
using Credimujer.Op.Dto.Socia.Reporte;
using Credimujer.Op.Repository.Interfaces.Data;

namespace Credimujer.Op.Repository.Interfaces
{
    public interface IBancoComunalRepository : IBaseRepository<BancoComunalEntity>
    {
        Task<List<DropdownDto>> BusquedaPorDescripcion(string descripcion, List<string> sucursal);

        Task<string> ObtenerSucursalPorCodigo(int? bancoComunalId);

        Task<List<ListaBancoComunalDto>> ListarPorDescripcion(string descripcion, List<string> sucursal);

        Task<BancoComunalDto> ObtenerPorCodigo(int id);

        Task<BancoComunalyAnilloGrupalDto> ObtenerBancoyAnillo(int bancoComunalId, int? anilloGrupalId);

        Task<BancoComunalEntity> ObtenerPorId(int bancoComunalId);
    }
}