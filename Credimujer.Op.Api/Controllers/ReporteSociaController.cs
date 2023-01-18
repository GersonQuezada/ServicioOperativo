using Autofac;
using Credimujer.Op.Application.Implementations.Socia;
using Credimujer.Op.Application.Interfaces;
using Credimujer.Op.Common;
using Credimujer.Op.Common.Base;
using Credimujer.Op.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Api.Controllers
{
    [Authorize(AuthenticationSchemes = AuthenticateScheme.PersonalOperativo)]
    [Route("ReporteSocia")]
    [ApiController]
    public class ReporteSociaController
    {
        private readonly Lazy<ISociaReporteApplication> _reporteSociaApplication;

        public ReporteSociaController(ILifetimeScope lifetimeScope)
        {
            _reporteSociaApplication = new Lazy<ISociaReporteApplication>(() => lifetimeScope.Resolve<ISociaReporteApplication>());
        }

        private ISociaReporteApplication ReporteSociaApplication => _reporteSociaApplication.Value;

        [HttpPost("ReporteDatoSocia")]
        public async Task<JsonResult> ReporteDatoSocia([FromBody] int id)
        {
            ResponseDto response;
            try
            {
                response = await ReporteSociaApplication.ReportePorSocia(id);
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

        [HttpGet("ReporteListaSociaPorBanCoComunal/{bancoComunalId}")]
        public async Task<JsonResult> ReporteListaSociaPorBanCoComunal(int bancoComunalId)
        {
            ResponseDto response;
            try
            {
                response = await ReporteSociaApplication.ReporteListaSociaPorBanCoComunal(bancoComunalId);
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

        [HttpPost("ReporteExcelListaSociaPorBanCoComunal")]
        public async Task<JsonResult> ReporteExcelListaSociaPorBanCoComunal([FromBody] int bancoComunalId)
        {
            ResponseDto response;
            try
            {
                response = await ReporteSociaApplication.ReporteExcelListaSociaPorBanCoComunal(bancoComunalId);
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