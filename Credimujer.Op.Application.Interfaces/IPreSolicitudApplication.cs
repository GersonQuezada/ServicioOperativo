using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Credimujer.Op.Common.Base;
using Credimujer.Op.Model.PreSolicitud;
using Credimujer.Op.Model.PreSolicitud.GenerarPresolicitud;

namespace Credimujer.Op.Application.Interfaces
{
    public interface IPreSolicitudApplication
    {
        Task<ResponseDto> Catalog(CatalogModel model);

        Task<ResponseDto> BusquedaBancoComunal(string descripcion, string sucursal);

        Task<ResponseDto> ObtnerCuentaBancariaSocia(int sociaId);

        Task<ResponseDto> BuscarPreSolicitudPorBanco(FiltroPorBancoComunalModel model);

        Task<ResponseDto> RechazarPresolicitud(int id);

        Task<ResponseDto> ActualizarPreSolicitud(ActualizarPreSolicitudModel model);

        Task<ResponseDto> CrearPreSolicitudCabecera(FiltroCrearPreSolicitudCabeceraModel model);

        Task<ResponseDto> ObtenerPreSolicitudRegistrado(int id);

        Task<ResponseDto> ListaEstadoPreSolicitud(FiltroCrearPreSolicitudCabeceraModel model);

        Task<ResponseDto> ObtenerDetallePreSolicitud(int id);

        Task<ResponseDto> ObtenerDescripcionBancoComunal(string bancoComunalcodigo);

        Task<ResponseDto> ActualizarEstadoPreSolicitudCab(ActualizarEstadoPreSolicitudCabModel model);

        Task<ResponseDto> CrearPreSolicitud(CrearPreSolicitudModel model);

        Task<ResponseDto> ActualizarEstadoPorAprobarPreSolicitudCab(ActualizarEstadoPreSolicitudCabModel model);

        Task<ResponseDto> BusquedaBancoComunalconSucursal(string descripcion, int sucursalId);

        Task<ResponseDto> MaximoCorrelativoAnillo(int bancoComunalId);
    }
}