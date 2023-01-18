using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Credimujer.Op.Application.Interfaces;
using Credimujer.Op.Common;
using Credimujer.Op.Common.Base;
using Credimujer.Op.Common.Exceptions;
using Credimujer.Op.Model.PreSolicitud;
using Credimujer.Op.Model.PreSolicitud.GenerarPresolicitud;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Credimujer.Op.Api.Controllers
{
    [Authorize(AuthenticationSchemes = AuthenticateScheme.PersonalOperativo)]
    [Route("Presolicitud")]
    [ApiController]
    public class PreSolicitudController
    {
        private readonly Lazy<IPreSolicitudApplication> _preSolicitud;
        private readonly Lazy<ICommonApplication> _commonApplication;

        public PreSolicitudController(ILifetimeScope lifetimeScope)
        {
            _preSolicitud = new Lazy<IPreSolicitudApplication>(() => lifetimeScope.Resolve<IPreSolicitudApplication>());
            _commonApplication = new Lazy<ICommonApplication>(() => lifetimeScope.Resolve<ICommonApplication>());
        }

        private IPreSolicitudApplication PreSolicitudApplication => _preSolicitud.Value;
        private ICommonApplication CommonApplication => _commonApplication.Value;

        [HttpGet("catalogo")]
        public async Task<JsonResult> Catalogo([FromQuery] CatalogModel model)
        {
            ResponseDto response;
            try
            {
                response = await PreSolicitudApplication.Catalog(model);
            }
            catch (FunctionalException ex)
            {
                response = new ResponseDto { Status = ex.FuntionalCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (TechnicalException ex)
            {
                response = new ResponseDto { Status = ex.ErrorCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (Exception ex)
            {
                response = new ResponseDto { Status = Constants.SystemStatusCode.TechnicalError, Message = ex.StackTrace.ToString() };
            }
            return new JsonResult(response);
        }

        [HttpGet("ObtenerDescripcionBancoComunal/{bancoComunalCodigo}")]
        public async Task<JsonResult> ObtenerDescripcionBancoComunal(string bancoComunalCodigo)
        {
            ResponseDto response;
            try
            {
                response = await PreSolicitudApplication.ObtenerDescripcionBancoComunal(bancoComunalCodigo);
            }
            catch (FunctionalException ex)
            {
                response = new ResponseDto { Status = ex.FuntionalCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (TechnicalException ex)
            {
                response = new ResponseDto { Status = ex.ErrorCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (Exception ex)
            {
                response = new ResponseDto { Status = Constants.SystemStatusCode.TechnicalError, Message = ex.StackTrace.ToString() };
            }
            return new JsonResult(response);
        }

        [HttpPost("BuscarPreSolicitudPorBanco")]
        public async Task<JsonResult> BuscarPreSolicitudPorBanco([FromBody] FiltroPorBancoComunalModel model)
        {
            ResponseDto response;
            try
            {
                response = await PreSolicitudApplication.BuscarPreSolicitudPorBanco(model);
            }
            catch (FunctionalException ex)
            {
                response = new ResponseDto { Status = ex.FuntionalCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (TechnicalException ex)
            {
                response = new ResponseDto { Status = ex.ErrorCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (Exception ex)
            {
                response = new ResponseDto { Status = Constants.SystemStatusCode.TechnicalError, Message = ex.StackTrace.ToString() };
            }
            return new JsonResult(response);
        }

        [HttpPost("RechazarPresolicitud")]
        public async Task<JsonResult> RechazarPresolicitud([FromBody] int id)
        {
            ResponseDto response;
            try
            {
                response = await PreSolicitudApplication.RechazarPresolicitud(id);
            }
            catch (FunctionalException ex)
            {
                response = new ResponseDto { Status = ex.FuntionalCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (TechnicalException ex)
            {
                response = new ResponseDto { Status = ex.ErrorCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (Exception ex)
            {
                response = new ResponseDto { Status = Constants.SystemStatusCode.TechnicalError, Message = ex.StackTrace.ToString() };
            }
            return new JsonResult(response);
        }

        [HttpPost("ObtenerPreSolicitudRegistrado")]
        public async Task<JsonResult> ObtenerPreSolicitudRegistrado([FromBody] int id)
        {
            ResponseDto response;
            try
            {
                response = await PreSolicitudApplication.ObtenerPreSolicitudRegistrado(id);
            }
            catch (FunctionalException ex)
            {
                response = new ResponseDto { Status = ex.FuntionalCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (TechnicalException ex)
            {
                response = new ResponseDto { Status = ex.ErrorCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (Exception ex)
            {
                response = new ResponseDto { Status = Constants.SystemStatusCode.TechnicalError, Message = ex.StackTrace.ToString() };
            }
            return new JsonResult(response);
        }

        [HttpPost("ActualizarPreSolicitud")]
        public async Task<JsonResult> ActualizarPreSolicitud([FromBody] ActualizarPreSolicitudModel model)
        {
            ResponseDto response;
            try
            {
                response = await PreSolicitudApplication.ActualizarPreSolicitud(model);
            }
            catch (FunctionalException ex)
            {
                response = new ResponseDto { Status = ex.FuntionalCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (TechnicalException ex)
            {
                response = new ResponseDto { Status = ex.ErrorCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (Exception ex)
            {
                response = new ResponseDto { Status = Constants.SystemStatusCode.TechnicalError, Message = ex.StackTrace.ToString() };
            }
            return new JsonResult(response);
        }

        [HttpPost("CrearPreSolicitud")]
        public async Task<JsonResult> CrearPreSolicitud([FromBody] CrearPreSolicitudModel model)
        {
            ResponseDto response;
            try
            {
                response = await PreSolicitudApplication.CrearPreSolicitud(model);
            }
            catch (FunctionalException ex)
            {
                response = new ResponseDto { Status = ex.FuntionalCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (TechnicalException ex)
            {
                response = new ResponseDto { Status = ex.ErrorCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (Exception ex)
            {
                response = new ResponseDto { Status = Constants.SystemStatusCode.TechnicalError, Message = ex.StackTrace.ToString() };
            }
            return new JsonResult(response);
        }

        [HttpPost("ObtenerDetallePreSolicitud")]
        public async Task<JsonResult> ObtenerDetallePreSolicitud([FromBody] int id)
        {
            ResponseDto response;
            try
            {
                response = await PreSolicitudApplication.ObtenerDetallePreSolicitud(id);
            }
            catch (FunctionalException ex)
            {
                response = new ResponseDto { Status = ex.FuntionalCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (TechnicalException ex)
            {
                response = new ResponseDto { Status = ex.ErrorCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (Exception ex)
            {
                response = new ResponseDto { Status = Constants.SystemStatusCode.TechnicalError, Message = ex.StackTrace.ToString() };
            }
            return new JsonResult(response);
        }

        [HttpPost("CrearPreSolicitudCabecera")]
        public async Task<JsonResult> CrearPreSolicitudCabecera([FromBody] FiltroCrearPreSolicitudCabeceraModel model)
        {
            ResponseDto response;
            try
            {
                response = await PreSolicitudApplication.CrearPreSolicitudCabecera(model);
            }
            catch (FunctionalException ex)
            {
                response = new ResponseDto { Status = ex.FuntionalCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (TechnicalException ex)
            {
                response = new ResponseDto { Status = ex.ErrorCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (Exception ex)
            {
                response = new ResponseDto { Status = Constants.SystemStatusCode.TechnicalError, Message = ex.StackTrace.ToString() };
            }
            return new JsonResult(response);
        }

        [HttpPost("ListaEstadoPreSolicitud")]
        public async Task<JsonResult> ListaEstadoPreSolicitud([FromBody] FiltroCrearPreSolicitudCabeceraModel model)
        {
            ResponseDto response;
            try
            {
                response = await PreSolicitudApplication.ListaEstadoPreSolicitud(model);
            }
            catch (FunctionalException ex)
            {
                response = new ResponseDto { Status = ex.FuntionalCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (TechnicalException ex)
            {
                response = new ResponseDto { Status = ex.ErrorCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (Exception ex)
            {
                response = new ResponseDto { Status = Constants.SystemStatusCode.TechnicalError, Message = ex.StackTrace.ToString() };
            }
            return new JsonResult(response);
        }

        [HttpPost("ActualizarEstadoPreSolicitudCab")]
        public async Task<JsonResult> ActualizarEstadoPreSolicitudCab([FromBody] ActualizarEstadoPreSolicitudCabModel model)
        {
            ResponseDto response;
            try
            {
                response = await PreSolicitudApplication.ActualizarEstadoPreSolicitudCab(model);
            }
            catch (FunctionalException ex)
            {
                response = new ResponseDto { Status = ex.FuntionalCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (TechnicalException ex)
            {
                response = new ResponseDto { Status = ex.ErrorCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (Exception ex)
            {
                response = new ResponseDto { Status = Constants.SystemStatusCode.TechnicalError, Message = ex.StackTrace.ToString() };
            }
            return new JsonResult(response);
        }

        [HttpGet("BusquedaBancoComunal/{descripcion}/{sucursal}")]
        [HttpGet("BusquedaBancoComunal/{descripcion}")]
        public async Task<JsonResult> BusquedaBancoComunal(string descripcion, string sucursal)
        {
            ResponseDto response;
            try
            {
                response = await PreSolicitudApplication.BusquedaBancoComunal(descripcion, sucursal);
            }
            catch (FunctionalException ex)
            {
                response = new ResponseDto { Status = ex.FuntionalCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (TechnicalException ex)
            {
                response = new ResponseDto { Status = ex.ErrorCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (Exception ex)
            {
                response = new ResponseDto { Status = Constants.SystemStatusCode.TechnicalError, Message = ex.StackTrace.ToString() };
            }
            return new JsonResult(response);
        }

        [HttpGet("BusquedaBancoComunalySucursal/{descripcion}/{sucursalId}")]
        public async Task<JsonResult> BusquedaBancoComunalconSucursal(string descripcion, int sucursalId)
        {
            ResponseDto response;
            try
            {
                response = await PreSolicitudApplication.BusquedaBancoComunalconSucursal(descripcion, sucursalId);
            }
            catch (FunctionalException ex)
            {
                response = new ResponseDto { Status = ex.FuntionalCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (TechnicalException ex)
            {
                response = new ResponseDto { Status = ex.ErrorCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (Exception ex)
            {
                response = new ResponseDto { Status = Constants.SystemStatusCode.TechnicalError, Message = ex.StackTrace.ToString() };
            }
            return new JsonResult(response);
        }

        [HttpGet("ObtnerCuentaBancariaSocia")]
        public async Task<JsonResult> ObtnerCuentaBancariaSocia(int id)
        {
            ResponseDto response;
            try
            {
                response = await PreSolicitudApplication.ObtnerCuentaBancariaSocia(id);
            }
            catch (FunctionalException ex)
            {
                response = new ResponseDto { Status = ex.FuntionalCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (TechnicalException ex)
            {
                response = new ResponseDto { Status = ex.ErrorCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (Exception ex)
            {
                response = new ResponseDto { Status = Constants.SystemStatusCode.TechnicalError, Message = ex.StackTrace.ToString() };
            }
            return new JsonResult(response);
        }

        [HttpPost("ActualizarEstadoPorAprobarPreSolicitudCab")]
        public async Task<JsonResult> ActualizarEstadoPorAprobarPreSolicitudCab([FromBody] ActualizarEstadoPreSolicitudCabModel model)
        {
            ResponseDto response;
            try
            {
                response = await PreSolicitudApplication.ActualizarEstadoPorAprobarPreSolicitudCab(model);
            }
            catch (FunctionalException ex)
            {
                response = new ResponseDto { Status = ex.FuntionalCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (TechnicalException ex)
            {
                response = new ResponseDto { Status = ex.ErrorCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (Exception ex)
            {
                response = new ResponseDto { Status = Constants.SystemStatusCode.TechnicalError, Message = ex.StackTrace.ToString() };
            }
            return new JsonResult(response);
        }

        [HttpGet("MaximoCorrelativoAnillo")]
        public async Task<JsonResult> MaximoCorrelativoAnillo(int id)
        {
            ResponseDto response;
            try
            {
                response = await PreSolicitudApplication.MaximoCorrelativoAnillo(id);
            }
            catch (FunctionalException ex)
            {
                response = new ResponseDto { Status = ex.FuntionalCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (TechnicalException ex)
            {
                response = new ResponseDto { Status = ex.ErrorCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (Exception ex)
            {
                response = new ResponseDto { Status = Constants.SystemStatusCode.TechnicalError, Message = ex.StackTrace.ToString() };
            }
            return new JsonResult(response);
        }
    }
}