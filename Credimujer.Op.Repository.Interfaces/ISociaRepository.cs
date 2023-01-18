using System.Collections.Generic;
using Credimujer.Op.Common.Base;
using Credimujer.Op.Domail.Models.Entities;
using Credimujer.Op.Dto.Base;
using Credimujer.Op.Dto.Socia.Busqueda;
using Credimujer.Op.Model.Socia.Busqueda;
using Credimujer.Op.Repository.Interfaces.Data;
using System.Threading.Tasks;
using Credimujer.Op.Dto.Socia.Registro;
using Credimujer.Op.Dto.Socia.Actualizar;
using Credimujer.Op.Dto.PreSolicitud.GenerarPreSolicitud;
using Credimujer.Op.Dto.Socia.Reporte;
using Credimujer.Op.Domail.Models.Entities.Procedure;
using System;

namespace Credimujer.Op.Repository.Interfaces
{
    public interface ISociaRepository : IBaseRepository<SociaEntity>
    {
        Task<SociaEntity> ObtenerPorId(int sociaId);

        Task<List<SociaEntity>> ObtenerPorListaId(List<int> listaSociaId);

        Task<ResponseDto<PaginationResultDTO<ListaSociaParaAprobarDto>>> ListadoSociaParaAprobar(FiltroSociaParaAprobarModel model, List<string> sucursal);

        Task<ActualizarDatoPersonalDto> ObtenerPorIdParaActualizarDatoPersonal(int idSocia);

        Task<SociaEntity> ObtenerConFormularioPorIdYEstado(int idSocia, string estado);

        Task<ResponseDto<PaginationResultDTO<ListaSociaPorBancoComunalDto>>> ObtnerPorBancoComunalYAnilloGrupal(FiltroBusquedaPorBancoComunalModel model);

        Task<InformacionSociaDto> ObtenerInformacionPorId(int idSocia);

        Task<SociaNuevaPorDniDto> ObtenerSinFormularioPorDniyListaSucursal(string nroDni, List<string> listaSucursalCodigo);

        Task<List<ListaSociaPorBCyAGDto>> ObtenerPorBancoComunalYAnilloGrupal(int bancoComunalId, int? anilloGrupalId);

        Task<ReportePorSociaDto> ObtenerDatosPorId(int sociaId);

        ObtenerTipoDeudaPorDni ObtenerTipoDeuda(string nroDni);

        ObtenerTipoRiesgoPorDni ObtenerTipoRiesgo(string nroDni);

        ObtenerCapacidadPagoEntity ObtenerCapacidadPago(string nroDni);

        Task<List<ReporteListaSociaDto._Detalle>> ObtenerListaSociaReporte(int bancoComunalId);

        Task<ResponseDto<PaginationResultDTO<ListaSociaPorSucursalDto>>> ObtenerPorSucursalyDatos
            (FiltroBusquedaPorSucursalDatoModel model);

        List<ListadoProductoSociaEntity> ListadoProducto(string codigoSocia);

        List<ListadoProductoSociaEntity> ListadoProductoPorNumDocumento(string numeroDocumento);

        List<ListadoCreditoCabeceraEntity> ListadoCreditoCabecera(string numcredito);

        List<ListadoCreditoDetalleEntity> ListadoCreditoDetalle(string numcredito);

        List<BusquedaSociaEntity> BusquedaSocia(string dniNombres);

        Tuple<List<ListaSociaMotivoBajaDto>, int> ListadoSociaConMotivoBaja(FiltroSociaConMotivoBajaModel model);

        Task<SociaEntity> ObtenerSistemaExternoIdyEstado(int idSocia);

        Task<bool> ExsiteCelular(string celular, int? sociaId);

        Task<bool> ExsiteTelefono(string telefono, int? sociaId);

        Task<SociaEntity> ObtenerSociaConEstado(int sociaId);

        Task<bool> ExisteCuentaBancaria(int bancoId, string nroCta, int? sociaId);
        Task<SociaNuevaPorDniDto> ObtenerPorDniyListaSucursal(string nroDni, List<string> listaSucursalCodigo);
    }
}