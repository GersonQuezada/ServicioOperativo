using Credimujer.Op.Common.Base;
using Credimujer.Op.Dto.Base;
using Credimujer.Op.Dto.Socia.Busqueda;
using Credimujer.Op.Model.Socia.Busqueda;
using System.Threading.Tasks;
using Credimujer.Op.Dto.Socia.Registro;
using Credimujer.Op.Model.Socia;
using Credimujer.Op.Model.Socia.Registrar;
using Credimujer.Op.Model.Service.Iam;
using Credimujer.Op.Model.Socia.Actualizar;

namespace Credimujer.Op.Application.Interfaces
{
    public interface ISociaApplication
    {
        Task<ResponseDto> Catalogo(CatalogoModel model);

        Task<ResponseDto> RegistrarSocia(NuevaSociaModel model);

        Task<ResponseDto<PaginationResultDTO<ListaSociaParaAprobarDto>>> BusquedaSocia(
            FiltroSociaParaAprobarModel model);

        Task<ResponseDto> ObtenerSociaPorId(int sociaId);

        Task<ResponseDto> ObtenerSociaPorDni(string dni);

        Task<ResponseDto> ActualizarDatoPersonal(ActualizarDatoPersonalDto model);

        Task<ResponseDto> AprobarAccesoSocia(RegistrarUsuarioModel model);

        Task<ResponseDto> BusquedaBancoComunal(string descripcion, string sucursal);

        Task<ResponseDto> BusquedaBancoComunalSinRestriccion(string descripcion, string sucursal);

        Task<ResponseDto<PaginationResultDTO<ListaSociaPorBancoComunalDto>>> BusquedaPorBancoComunal(FiltroBusquedaPorBancoComunalModel model);

        Task<ResponseDto<PaginationResultDTO<ListaSociaPorBancoComunalDto>>> BusquedaPorBancoComunalSinRestriccion(FiltroBusquedaPorBancoComunalModel model);

        Task<ResponseDto> ActualizarInformacionSocia(InformacionSociaModel model);

        Task<ResponseDto> ObtenerInformacionSociaPorId(int id);

        Task<ResponseDto> EliminarSocia(int id);

        Task<ResponseDto<PaginationResultDTO<ListaSociaPorSucursalDto>>> BusquedaSociaPorSucursal(FiltroBusquedaPorSucursalDatoModel model);

        ResponseDto BusquedaSociaExterno(FiltroBusquedaPorSucursalDatoModel model);

        Task<ResponseDto> ListaHistorialSolicitud(string codigoSocia);

        ResponseDto ListaHistorialSolicitudPorNumeroDocumento(string numeroDocumento);

        Task<ResponseDto> ListaCredito(string numeroCredito);

        Task<ResponseDto<PaginationResultDTO<ListaSociaMotivoBajaDto>>> ListadoSociaConMotivoBaja(FiltroSociaConMotivoBajaModel model);

        Task<ResponseDto> ExisteSociaAsignadaAlCargo(int sociaId, int cargoBancoComunalId);

        Task<ResponseDto> ExisteCargoDisponible(int bancoComunalId, int cargoBancoComunalId);
        Task<ResponseDto> BusquedaBancoComunalPorId(int id);
        Task<ResponseDto> ObtenerSociaPorDniParaActFormulario(string dni);
    }
}