using Autofac;
using Credimujer.Op.Application.Interfaces;
using Credimujer.Op.Common;
using Credimujer.Op.Common.Base;
using Credimujer.Op.Common.Exceptions;
using Credimujer.Op.Dto.Base;
using Credimujer.Op.Dto.Socia.Busqueda;
using Credimujer.Op.Dto.Socia.Registro;
using Credimujer.Op.Model.Service.Iam;
using Credimujer.Op.Model.Socia;
using Credimujer.Op.Model.Socia.Actualizar;
using Credimujer.Op.Model.Socia.Busqueda;
using Credimujer.Op.Model.Socia.Registrar;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Credimujer.Op.Api.Controllers
{
    [Authorize(AuthenticationSchemes = AuthenticateScheme.PersonalOperativo)]
    [Route("Socia")]
    [ApiController]
    public class SociaController
    {
        private readonly Lazy<ISociaApplication> _sociaApplication;
        private readonly Lazy<ICommonApplication> _commonApplication;

        public SociaController(ILifetimeScope lifetimeScope)
        {
            _sociaApplication = new Lazy<ISociaApplication>(() => lifetimeScope.Resolve<ISociaApplication>());
            _commonApplication = new Lazy<ICommonApplication>(() => lifetimeScope.Resolve<ICommonApplication>());
        }

        private ISociaApplication SociaApplication => _sociaApplication.Value;
        private ICommonApplication CommonApplication => _commonApplication.Value;

        [HttpGet("catalogo")]
        public async Task<JsonResult> Catalogo([FromQuery] CatalogoModel model)
        {
            ResponseDto response;
            try
            {
                response = await SociaApplication.Catalogo(model);
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

        [HttpPost("RegistrarSocia")]
        public async Task<JsonResult> RegistrarSocia([FromBody][CustomizeValidator(RuleSet = "NuevaSocia")] NuevaSociaModel model)
        {
            ResponseDto response;
            try
            {
                response = await SociaApplication.RegistrarSocia(model);
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

        [HttpPost("AprobarAccesoSocia")]
        public async Task<JsonResult> AprobarAccesoSocia([FromBody] RegistrarUsuarioModel model)
        {
            ResponseDto response;
            try
            {
                response = await SociaApplication.AprobarAccesoSocia(model);
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

        [HttpPost("BusquedaSocia")]
        public async Task<JsonResult> BusquedaSocia([FromBody] FiltroSociaParaAprobarModel model)
        {
            ResponseDto<PaginationResultDTO<ListaSociaParaAprobarDto>> response;
            try
            {
                response = await SociaApplication.BusquedaSocia(model);
            }
            catch (FunctionalException ex)
            {
                response = new ResponseDto<PaginationResultDTO<ListaSociaParaAprobarDto>> { Status = ex.FuntionalCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
                //Logger.Warning(ex.TransactionId, ex.Message, ex);
            }
            catch (TechnicalException ex)
            {
                response = new ResponseDto<PaginationResultDTO<ListaSociaParaAprobarDto>> { Status = ex.ErrorCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
                //Logger.Error(ex.TransactionId, ex.Message, ex);
            }
            catch (Exception ex)
            {
                response = new ResponseDto<PaginationResultDTO<ListaSociaParaAprobarDto>> { Status = Constants.SystemStatusCode.TechnicalError, Message = ex.Message };
                //Logger.Error(response.TransactionId, ex.Message, ex);
            }

            return new JsonResult(response);
        }

        [HttpGet("ObtenerSociaPorId/{sociaId}")]
        public async Task<JsonResult> ObtenerSociaPorId(int sociaId)
        {
            ResponseDto response;
            try
            {
                response = await SociaApplication.ObtenerSociaPorId(sociaId);
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

        [HttpGet("ObtenerSociaPorDni")]
        public async Task<JsonResult> ObtenerSociaPorDni(string nroDni)
        {
            ResponseDto response;
            try
            {
                response = await SociaApplication.ObtenerSociaPorDni(nroDni);
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

        [HttpGet("ObtenerSociaPorDniParaActFormulario")]
        public async Task<JsonResult> ObtenerSociaPorDniParaActFormulario(string nroDni)
        {
            ResponseDto response;
            try
            {
                response = await SociaApplication.ObtenerSociaPorDniParaActFormulario(nroDni);
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

        

        [HttpPost("ActualizarDatoPersonal")]
        public async Task<JsonResult> ActualizarDatoPersonal([FromBody] ActualizarDatoPersonalDto model)
        {
            ResponseDto response;
            try
            {
                response = await SociaApplication.ActualizarDatoPersonal(model);
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

        [HttpGet("ObtenerProvincia/{codigoDepartamento}")]
        public async Task<JsonResult> ObtenerProvincia(string codigoDepartamento)
        {
            ResponseDto response;
            try
            {
                response = await CommonApplication.ObtenerProvincia(codigoDepartamento);
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

        [HttpGet("ObtenerDistrito/{codigoDepartamento}/{codigoProv}")]
        public async Task<JsonResult> ObtenerDistrito(string codigoDepartamento, string codigoProv)
        {
            ResponseDto response;
            try
            {
                response = await CommonApplication.ObtenerDistrito(codigoDepartamento, codigoProv);
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
                response = await SociaApplication.BusquedaBancoComunal(descripcion, sucursal);
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

        [HttpGet("BusquedaBancoComunalSinRestriccion/{descripcion}/{sucursal}")]
        [HttpGet("BusquedaBancoComunalSinRestriccion/{descripcion}")]
        public async Task<JsonResult> BusquedaBancoComunalSinRestriccion(string descripcion, string sucursal)
        {
            ResponseDto response;
            try
            {
                response = await SociaApplication.BusquedaBancoComunalSinRestriccion(descripcion, sucursal);
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

        [HttpPost("BusquedaPorBancoComunal")]
        public async Task<JsonResult> BusquedaPorBancoComunal([FromBody] FiltroBusquedaPorBancoComunalModel model)
        {
            ResponseDto<PaginationResultDTO<ListaSociaPorBancoComunalDto>> response;
            try
            {
                response = await SociaApplication.BusquedaPorBancoComunal(model);
            }
            catch (FunctionalException ex)
            {
                response = new ResponseDto<PaginationResultDTO<ListaSociaPorBancoComunalDto>> { Status = ex.FuntionalCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (TechnicalException ex)
            {
                response = new ResponseDto<PaginationResultDTO<ListaSociaPorBancoComunalDto>> { Status = ex.ErrorCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (Exception ex)
            {
                response = new ResponseDto<PaginationResultDTO<ListaSociaPorBancoComunalDto>> { Status = Constants.SystemStatusCode.TechnicalError, Message = ex.StackTrace.ToString() };
            }
            return new JsonResult(response);
        }

        [HttpPost("BusquedaPorBancoComunalSinRestriccion")]
        public async Task<JsonResult> BusquedaPorBancoComunalSinRestriccion([FromBody] FiltroBusquedaPorBancoComunalModel model)
        {
            ResponseDto<PaginationResultDTO<ListaSociaPorBancoComunalDto>> response;
            try
            {
                response = await SociaApplication.BusquedaPorBancoComunalSinRestriccion(model);
            }
            catch (FunctionalException ex)
            {
                response = new ResponseDto<PaginationResultDTO<ListaSociaPorBancoComunalDto>> { Status = ex.FuntionalCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (TechnicalException ex)
            {
                response = new ResponseDto<PaginationResultDTO<ListaSociaPorBancoComunalDto>> { Status = ex.ErrorCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (Exception ex)
            {
                response = new ResponseDto<PaginationResultDTO<ListaSociaPorBancoComunalDto>> { Status = Constants.SystemStatusCode.TechnicalError, Message = ex.StackTrace.ToString() };
            }
            return new JsonResult(response);
        }

        [HttpGet("ObtenerInformacionSociaPorId/{id}")]
        public async Task<JsonResult> ObtenerInformacionSociaPorId(int id)
        {
            ResponseDto response;
            try
            {
                response = await SociaApplication.ObtenerInformacionSociaPorId(id);
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

        [HttpPost("ActualizarInformacionSocia")]
        public async Task<JsonResult> ActualizarInformacionSocia([FromBody] InformacionSociaModel model)
        {
            ResponseDto response;
            try
            {
                response = await SociaApplication.ActualizarInformacionSocia(model);
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

        [HttpPost("EliminarSocia")]
        public async Task<JsonResult> EliminarSocia([FromBody] int id)
        {
            ResponseDto response;
            try
            {
                response = await SociaApplication.EliminarSocia(id);
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

        [HttpPost("BusquedaSociaPorSucursal")]
        public async Task<JsonResult> BusquedaSociaPorSucursal([FromBody] FiltroBusquedaPorSucursalDatoModel model)
        {
            ResponseDto<PaginationResultDTO<ListaSociaPorSucursalDto>> response;
            try
            {
                response = await SociaApplication.BusquedaSociaPorSucursal(model);
            }
            catch (FunctionalException ex)
            {
                response = new ResponseDto<PaginationResultDTO<ListaSociaPorSucursalDto>> { Status = ex.FuntionalCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (TechnicalException ex)
            {
                response = new ResponseDto<PaginationResultDTO<ListaSociaPorSucursalDto>> { Status = ex.ErrorCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (Exception ex)
            {
                response = new ResponseDto<PaginationResultDTO<ListaSociaPorSucursalDto>> { Status = Constants.SystemStatusCode.TechnicalError, Message = ex.StackTrace.ToString() };
            }
            return new JsonResult(response);
        }

        [HttpPost("BusquedaSociaExterno")]
        public JsonResult BusquedaSociaExterno([FromBody] FiltroBusquedaPorSucursalDatoModel model)
        {
            ResponseDto response;
            try
            {
                response = SociaApplication.BusquedaSociaExterno(model);
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

        [HttpGet("ListaHistorialSolicitud/{codigoSocia}")]
        public async Task<JsonResult> ListaHistorialSolicitud(string codigoSocia)
        {
            ResponseDto response;
            try
            {
                response = await SociaApplication.ListaHistorialSolicitud(codigoSocia);
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

        [HttpGet("ListaHistorialSolicitudPorNumeroDocu/{numeroDocumento}")]
        public JsonResult ListaHistorialSolicitudPorNumeroDocu(string numeroDocumento)
        {
            ResponseDto response;
            try
            {
                response = SociaApplication.ListaHistorialSolicitudPorNumeroDocumento(numeroDocumento);
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

        [HttpGet("ListaCredito/{numeroCredito}")]
        public async Task<JsonResult> ListaCredito(string numeroCredito)
        {
            ResponseDto response;
            try
            {
                response = await SociaApplication.ListaCredito(numeroCredito);
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

        [HttpPost("ListadoSociaConMotivoBaja")]
        public async Task<JsonResult> ListadoSociaConMotivoBaja([FromBody] FiltroSociaConMotivoBajaModel model)
        {
            ResponseDto<PaginationResultDTO<ListaSociaMotivoBajaDto>> response;
            try
            {
                response = await SociaApplication.ListadoSociaConMotivoBaja(model);
            }
            catch (FunctionalException ex)
            {
                response = new ResponseDto<PaginationResultDTO<ListaSociaMotivoBajaDto>> { Status = ex.FuntionalCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (TechnicalException ex)
            {
                response = new ResponseDto<PaginationResultDTO<ListaSociaMotivoBajaDto>> { Status = ex.ErrorCode, Message = ex.Message, Data = ex.Data, TransactionId = ex.TransactionId };
            }
            catch (Exception ex)
            {
                response = new ResponseDto<PaginationResultDTO<ListaSociaMotivoBajaDto>> { Status = Constants.SystemStatusCode.TechnicalError, Message = ex.StackTrace.ToString() };
            }
            return new JsonResult(response);
        }

        [HttpGet("ExisteSociaAsignadaAlCargo")]
        public async Task<JsonResult> ExisteSociaAsignadaAlCargo([FromQuery] int sociaId, int cargoBancoComunalId)
        {
            ResponseDto response;
            try
            {
                if (sociaId == 0 || cargoBancoComunalId == 0)
                {
                    throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, "Debe ingresar sociaId y cargobancoComunalId");
                }
                response = await SociaApplication.ExisteSociaAsignadaAlCargo(sociaId, cargoBancoComunalId);
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

        [HttpGet("ExisteCargoDisponible")]
        public async Task<JsonResult> ExisteCargoDisponible([FromQuery] int bancoComunalId, int cargoBancoComunalId)
        {
            ResponseDto response;
            try
            {
                if (bancoComunalId == 0 || cargoBancoComunalId == 0)
                {
                    throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, "Debe ingresar bancoComunalId y cargobancoComunalId");
                }
                response = await SociaApplication.ExisteCargoDisponible(bancoComunalId, cargoBancoComunalId);
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

        
        [HttpPost("BusquedaBancoComunalPorId")]
        public async Task<JsonResult> BusquedaBancoComunalPorId([FromBody] int id)
        {
            ResponseDto response;
            try
            {
                response = await SociaApplication.BusquedaBancoComunalPorId(id);
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