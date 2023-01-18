using Credimujer.Op.Common.Base;
using Credimujer.Op.Dto.Base;
using Credimujer.Op.Dto.PreSolicitud.Reporte;
using Credimujer.Op.Model.PreSolicitud.Reporte;
using System.Threading.Tasks;

namespace Credimujer.Op.Application.Interfaces
{
    public interface IReporteApplication
    {
        Task<ResponseDto> Catalog(CatalogModel model);

        Task<ResponseDto> ReporteCtaExterna(int id);

        Task<ResponseDto> ReporteParalelo(int id);

        Task<ResponseDto> ReporteAnilloGrupal(int id);

        Task<ResponseDto> ReporteParaleloAnilloGrupal(int id);

        Task<ResponseDto<PaginationResultDTO<ListaPreSocilitudCabeceraDto>>> RelacionPresolicitudCabecera(FiltroRelacionPreSolicitudModel model);

        Task<ResponseDto> ReporteCtaExternaExcel(int id);

        Task<ResponseDto> ReporteParaleloExcel(int id);

        Task<ResponseDto> ReporteAnilloGrupalExcel(int id);

        Task<ResponseDto> ReporteParaleloAnilloGrupalExcel(int id);

        Task<ResponseDto> ReporteParaleloPromocional(int id);

        Task<ResponseDto> ReporteParaleloPromocionalExcel(int id);
    }
}