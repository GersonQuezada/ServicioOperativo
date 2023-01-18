using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Credimujer.Op.Common.Base;
using Credimujer.Op.Domail.Models.Entities;
using Credimujer.Op.Dto.Base;
using Credimujer.Op.Dto.Oficial;
using Credimujer.Op.Dto.PreSolicitud.GenerarPreSolicitud;
using Credimujer.Op.Dto.PreSolicitud.Reporte;
using Credimujer.Op.Model.Oficial;
using Credimujer.Op.Model.PreSolicitud.GenerarPresolicitud;
using Credimujer.Op.Repository.Interfaces.Data;

namespace Credimujer.Op.Repository.Interfaces
{
    public interface IPreSolicitudRepository : IBaseRepository<PreSolicitudEntity>
    {
        Task<List<ListaPreSolicitudPorBancoDto>> ObtenerPorPreSolicitudCabecera
            (int id, List<string> listaEstadoCodigo, List<string> listaEstadoCodigoPreSolicitudCabecera);

        Task<List<ListaPreSolicitudPorBancoDto>> ObtenerSinCabeceraPreSolicitud
            (FiltroPorBancoComunalModel model);

        Task<List<PreSolicitudEntity>> ObtenerPorBancoComunal
            (int bancoComunalId, int? anilloGrupalId);

        Task<PreSolicitudPorIdDto> ObtenerPorIdYEstado(int id, string estadoCodigo);

        Task<List<ListaPreSolicitudParaValidacionDto>> ListaPreSolicitudPorSocia(
           int sociaId
           );

        Task<List<PreSolicitudEntity>> ObtenerSinPresolicitudCabeceraPorSocia(int sociaId);

        Task<List<ListaPreSolicitudDto>> ObtenerPresolicitudNoAceptadas(int preSolicitudId);

        Task<List<ListaPreSolicitudPorBancoDto>> ObtenerPresolicitudMotivoRetiro
            (int bancoComunalRetiradoId, int? anilloGrupalRetirado);

        Task<ResponseDto<PaginationResultDTO<ResultadoResumenRegistroIngresadoDto>>>
            ObtenerPorUsuarioySucursal(BuscarResumenIngresoModel model);
    }
}