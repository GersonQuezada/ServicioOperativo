using Credimujer.Op.Common.Base;
using Credimujer.Op.Domail.Models.Entities;
using Credimujer.Op.Dto.Base;
using Credimujer.Op.Dto.Oficial;
using Credimujer.Op.Dto.PreSolicitud.EstadoPreSolicitud;
using Credimujer.Op.Dto.PreSolicitud.Reporte;
using Credimujer.Op.Dto.Socia.Reporte;
using Credimujer.Op.Model.Oficial;
using Credimujer.Op.Model.PreSolicitud.Reporte;
using Credimujer.Op.Repository.Interfaces.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Credimujer.Op.Repository.Interfaces
{
    public interface IPreSolicitudCabeceraRepository : IBaseRepository<PreSolicitudCabeceraEntity>
    {
        Task<List<ListaPreSolicitudCabeceraDto>> ObtenerPorBancoComunalYEstado(int bancoComunalId,
            int? anilloGrupalId, string estadoCodigo = "");

        Task<PreSolicitudCabeceraEntity> ObtenerConDetalle(int id, string estadoCodigo);

        Task<ReporteCtaExternaDto> ReporteCtaExterna(int id, List<int?> listSubTipoCredito);

        Task<ReporteParaleloDto> ReporteParalelo(int id,
           List<int?> listSubTipoCredito
           );

        Task<ResponseDto<PaginationResultDTO<ListaPreSocilitudCabeceraDto>>> ListaRelacion
            (FiltroRelacionPreSolicitudModel model);

        Task<bool> ExistePreSolicitudAprobadaPorSociaId(int sociaId);

        Task<ResponseDto<PaginationResultDTO<PresolicitudIngresadosDto>>> ObtenerPorUsuario(BuscarPresolicitudPorUsuarioModel model);

        Task<ReporteRegistrosIngresadosDto> ReporteRegistrosIngresados(ReportePresolicitudPorUsuarioModel model);

        Task<ReporteParaleloPromocionalDto> ReporteParaleloPromocional(int id,
            List<int?> listSubTipoCredito
            );

        Task<ReporteCtaExternaDto> ReporteParaleloPromocional(int id);

        Task<List<ListaSociaConUltPresolicitudDto>> ObtenerUltPresolicitudPorListaSocia(
            List<int> listaSocia);

        Task<TipoDeudaNivelRiesgoSociaDto> ObtenerTipoDeudaNivelRiesgoPorSocia(int sociaId);
    }
}