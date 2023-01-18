using Autofac;
using Credimujer.Op.Application.Interfaces;
using Credimujer.Op.Common;
using Credimujer.Op.Common.Base;
using Credimujer.Op.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Credimujer.Op.Model.Service.Iam;

namespace Credimujer.Op.Api.Controllers
{
    [Authorize(AuthenticationSchemes = AuthenticateScheme.PersonalOperativo)]
    [Route("Perfil")]
    [ApiController]
    public class PerfilController
    {
        private readonly Lazy<IPerfilApplication> _perfilApplication;
        //private readonly Lazy<ICommonApplication> _commonApplication;

        public PerfilController(ILifetimeScope lifetimeScope)
        {
            _perfilApplication = new Lazy<IPerfilApplication>(() => lifetimeScope.Resolve<IPerfilApplication>());
            //_commonApplication = new Lazy<ICommonApplication>(() => lifetimeScope.Resolve<ICommonApplication>());
        }

        private IPerfilApplication PerfilApplication => _perfilApplication.Value;
        [HttpGet("ObtenerDatoUsuario")]
        public async Task<JsonResult> ObtenerDatoUsuario()
        {
            ResponseDto response;
            try
            {
                response = await PerfilApplication.ObtenerDatoUsuario();
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
        [HttpPost("ActualizarCelular")]
        public async Task<JsonResult> ActualizarCelular([FromBody] ActualizarCelularUsuarioModel model)
        {
            ResponseDto response;
            try
            {
                response = await PerfilApplication.ActualizarCelular(model);
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
        [HttpPost("ActualizarContraseniaUsuario")]
        public async Task<JsonResult> ActualizarContraseniaUsuario([FromBody] ActualizarPasswordModel model)
        {
            ResponseDto response;
            try
            {
                response = await PerfilApplication.ActualizarContraseniaUsuario(model);
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
