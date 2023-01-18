using Autofac;
using Credimujer.Op.Application.Implementations.PreSolicitud;
using Credimujer.Op.Application.Interfaces;
using Credimujer.Op.Common;
using Credimujer.Op.Common.Base;
using Credimujer.Op.Common.Exceptions;
using Credimujer.Op.Dto.Base;
using Credimujer.Op.Dto.Oficial;
using Credimujer.Op.Dto.PreSolicitud.EstadoPreSolicitud;
using Credimujer.Op.Model.Oficial;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Api.Controllers
{
    [Authorize(AuthenticationSchemes = AuthenticateScheme.PersonalOperativo)]
    [Route("Oficial")]
    [ApiController]
    public class OficialController
    {
        private readonly Lazy<IOficialApplication> _oficialApplication;

        public OficialController(ILifetimeScope lifetimeScope)
        {
            _oficialApplication = new Lazy<IOficialApplication>(() => lifetimeScope.Resolve<IOficialApplication>());
        }

        private IOficialApplication OficialApplication => _oficialApplication.Value;

        [HttpGet("catalogo")]
        public async Task<JsonResult> Catalogo([FromQuery] CatalogoModel model)
        {
            ResponseDto response;
            try
            {
                response = await OficialApplication.Catalogo(model);
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

        [HttpPost("BuscarResumenDeRegistrosIngresedos")]
        public async Task<JsonResult> BuscarResumenDeRegistrosIngresedos([FromBody] BuscarResumenIngresoModel model)
        {
            ResponseDto<PaginationResultDTO<ResultadoResumenRegistroIngresadoDto>> response;
            try
            {
                response = await OficialApplication.BuscarResumenDeRegistrosIngresedos(model);
            }
            catch (FunctionalException ex)
            {
                response = new ResponseDto<PaginationResultDTO<ResultadoResumenRegistroIngresadoDto>> { Status = ex.FuntionalCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (TechnicalException ex)
            {
                response = new ResponseDto<PaginationResultDTO<ResultadoResumenRegistroIngresadoDto>> { Status = ex.ErrorCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (Exception ex)
            {
                response = new ResponseDto<PaginationResultDTO<ResultadoResumenRegistroIngresadoDto>> { Status = Constants.SystemStatusCode.TechnicalError, Message = ex.StackTrace.ToString() };
            }
            return new JsonResult(response);
        }

        [HttpGet("ListaOficialPorSucursal/{sucursalCodigo}")]
        public async Task<JsonResult> ListaOficialPorSucursal(string sucursalCodigo)
        {
            ResponseDto response;
            try
            {
                response = await OficialApplication.ListaOficialPorSucursal(sucursalCodigo);
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

        [HttpPost("ObtenerPresolicitudesRegistradas")]
        public async Task<JsonResult> ObtenerPresolicitudesRegistradas([FromBody] BuscarPresolicitudPorUsuarioModel model)
        {
            ResponseDto<PaginationResultDTO<PresolicitudIngresadosDto>> response;
            try
            {
                response = await OficialApplication.ObtenerPresolicitudesRegistradas(model);
            }
            catch (FunctionalException ex)
            {
                response = new ResponseDto<PaginationResultDTO<PresolicitudIngresadosDto>> { Status = ex.FuntionalCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (TechnicalException ex)
            {
                response = new ResponseDto<PaginationResultDTO<PresolicitudIngresadosDto>> { Status = ex.ErrorCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (Exception ex)
            {
                response = new ResponseDto<PaginationResultDTO<PresolicitudIngresadosDto>> { Status = Constants.SystemStatusCode.TechnicalError, Message = ex.StackTrace.ToString() };
            }
            return new JsonResult(response);
        }

        [HttpPost("ReporteResumenDeRegistrosIngresados")]
        public async Task<JsonResult> ReporteResumenDeRegistrosIngresados(ReportePresolicitudPorUsuarioModel model)
        {
            ResponseDto response;
            try
            {
                response = await OficialApplication.ReporteResumenDeRegistrosIngresados(model);
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

        [HttpPost("ReporteResumenDeRegistrosIngresadosExcel")]
        public async Task<JsonResult> ReporteResumenDeRegistrosIngresadosExcel(ReportePresolicitudPorUsuarioModel model)
        {
            ResponseDto response;
            try
            {
                response = await OficialApplication.ReporteResumenDeRegistrosIngresadosExcel(model);
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