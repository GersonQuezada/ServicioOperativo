using Credimujer.Op.Common.Base;
using Credimujer.Op.Dto.Base;
using Credimujer.Op.Dto.Oficial;
using Credimujer.Op.Dto.PreSolicitud.EstadoPreSolicitud;
using Credimujer.Op.Model.Oficial;
using System.Threading.Tasks;

namespace Credimujer.Op.Application.Interfaces
{
    public interface IOficialApplication
    {
        Task<ResponseDto<PaginationResultDTO<ResultadoResumenRegistroIngresadoDto>>> BuscarResumenDeRegistrosIngresedos(BuscarResumenIngresoModel model);

        Task<ResponseDto> ListaOficialPorSucursal(string sucursalCodigo);

        Task<ResponseDto> Catalogo(CatalogoModel model);

        Task<ResponseDto<PaginationResultDTO<PresolicitudIngresadosDto>>> ObtenerPresolicitudesRegistradas(BuscarPresolicitudPorUsuarioModel model);

        Task<ResponseDto> ReporteResumenDeRegistrosIngresados(ReportePresolicitudPorUsuarioModel model);

        Task<ResponseDto> ReporteResumenDeRegistrosIngresadosExcel
            (ReportePresolicitudPorUsuarioModel model);
    }
}