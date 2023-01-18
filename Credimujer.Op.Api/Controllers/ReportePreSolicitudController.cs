using Autofac;
using Credimujer.Op.Application.Interfaces;
using Credimujer.Op.Common;
using Credimujer.Op.Common.Base;
using Credimujer.Op.Common.Exceptions;
using Credimujer.Op.Dto.Base;
using Credimujer.Op.Dto.PreSolicitud.Reporte;
using Credimujer.Op.Model.PreSolicitud.Reporte;
using Credimujer.Op.Model.Socia.Busqueda;
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
    [Route("ReportePresolicitud")]
    [ApiController]
    public class ReportePreSolicitudController
    {
        private readonly Lazy<IReporteApplication> _reporteApplication;

        public ReportePreSolicitudController(ILifetimeScope lifetimeScope)
        {
            _reporteApplication = new Lazy<IReporteApplication>(() => lifetimeScope.Resolve<IReporteApplication>());
        }

        private IReporteApplication ReporteApplication => _reporteApplication.Value;

        [HttpGet("catalogo")]
        public async Task<JsonResult> Catalogo([FromQuery] CatalogModel model)
        {
            ResponseDto response;
            try
            {
                response = await ReporteApplication.Catalog(model);
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

        [HttpPost("RelacionPresolicitudCabecera")]
        public async Task<JsonResult> RelacionPresolicitudCabecera([FromBody] FiltroRelacionPreSolicitudModel model)
        {
            ResponseDto<PaginationResultDTO<ListaPreSocilitudCabeceraDto>> response;
            try
            {
                response = await ReporteApplication.RelacionPresolicitudCabecera(model);
            }
            catch (FunctionalException ex)
            {
                response = new ResponseDto<PaginationResultDTO<ListaPreSocilitudCabeceraDto>> { Status = ex.FuntionalCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
                //Logger.Warning(ex.TransactionId, ex.Message, ex);
            }
            catch (TechnicalException ex)
            {
                response = new ResponseDto<PaginationResultDTO<ListaPreSocilitudCabeceraDto>> { Status = ex.ErrorCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
                //Logger.Error(ex.TransactionId, ex.Message, ex);
            }
            catch (Exception ex)
            {
                response = new ResponseDto<PaginationResultDTO<ListaPreSocilitudCabeceraDto>> { Status = Constants.SystemStatusCode.TechnicalError, Message = ex.Message };
                //Logger.Error(response.TransactionId, ex.Message, ex);
            }

            return new JsonResult(response);
        }

        [HttpPost("ReporteCtaExterna")]
        public async Task<JsonResult> ReporteCtaExterna([FromBody] int id)
        {
            ResponseDto response;
            try
            {
                response = await ReporteApplication.ReporteCtaExterna(id);
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

        [HttpPost("ReporteCtaExternaExcel")]
        public async Task<JsonResult> ReporteCtaExternaExcel([FromBody] int id)
        {
            ResponseDto response;
            try
            {
                response = await ReporteApplication.ReporteCtaExternaExcel(id);
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

        [HttpPost("ReporteParalelo")]
        public async Task<JsonResult> ReporteParalelo([FromBody] int id)
        {
            ResponseDto response;
            try
            {
                response = await ReporteApplication.ReporteParalelo(id);
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

        [HttpPost("ReporteParaleloExcel")]
        public async Task<JsonResult> ReporteParaleloExcel([FromBody] int id)
        {
            ResponseDto response;
            try
            {
                response = await ReporteApplication.ReporteParaleloExcel(id);
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

        [HttpPost("ReporteAnilloGrupal")]
        public async Task<JsonResult> ReporteAnilloGrupal([FromBody] int id)
        {
            ResponseDto response;
            try
            {
                response = await ReporteApplication.ReporteAnilloGrupal(id);
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

        [HttpPost("ReporteAnilloGrupalExcel")]
        public async Task<JsonResult> ReporteAnilloGrupalExcel([FromBody] int id)
        {
            ResponseDto response;
            try
            {
                response = await ReporteApplication.ReporteAnilloGrupalExcel(id);
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

        [HttpPost("ReporteParaleloAnilloGrupal")]
        public async Task<JsonResult> ReporteParaleloAnilloGrupal([FromBody] int id)
        {
            ResponseDto response;
            try
            {
                response = await ReporteApplication.ReporteParaleloAnilloGrupal(id);
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

        [HttpPost("ReporteParaleloAnilloGrupalExcel")]
        public async Task<JsonResult> ReporteParaleloAnilloGrupalExcel([FromBody] int id)
        {
            ResponseDto response;
            try
            {
                response = await ReporteApplication.ReporteParaleloAnilloGrupalExcel(id);
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

        [HttpPost("ReporteParaleloPromocional")]
        public async Task<JsonResult> ReporteParaleloPromocional([FromBody] int id)
        {
            ResponseDto response;
            try
            {
                response = await ReporteApplication.ReporteParaleloPromocional(id);
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

        [HttpPost("ReporteParaleloPromocionalExcel")]
        public async Task<JsonResult> ReporteParaleloPromocionalExcel([FromBody] int id)
        {
            ResponseDto response;
            try
            {
                response = await ReporteApplication.ReporteParaleloPromocionalExcel(id);
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