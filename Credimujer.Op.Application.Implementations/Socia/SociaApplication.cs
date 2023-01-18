using Autofac;
using Credimujer.Op.Application.Interfaces;
using Credimujer.Op.Common;
using Credimujer.Op.Common.Base;
using Credimujer.Op.Common.Exceptions;
using Credimujer.Op.Common.Resources;
using Credimujer.Op.Domail.Models.Entities;
using Credimujer.Op.Dto.Base;
using Credimujer.Op.Dto.Socia;
using Credimujer.Op.Dto.Socia.Busqueda;
using Credimujer.Op.Dto.Socia.Registro;
using Credimujer.Op.Model.Service.Iam;
using Credimujer.Op.Model.Socia;
using Credimujer.Op.Model.Socia.Actualizar;
using Credimujer.Op.Model.Socia.Busqueda;
using Credimujer.Op.Model.Socia.Registrar;
using Credimujer.Op.Repository.Interfaces;
using Credimujer.Op.Repository.Interfaces.Data;
using Credimujer.Op.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Credimujer.Op.Application.Implementations.Socia
{
    public class SociaApplication : ISociaApplication
    {
        private readonly Lazy<IUnitOfWork> _unitOfWork;
        private readonly AppSetting _setting;
        private readonly Lazy<IHttpContextAccessor> _httpContext;
        private readonly Lazy<IIamService> _iamService;

        public SociaApplication(IOptions<AppSetting> settings, ILifetimeScope lifetimeScope)
        {
            _setting = settings.Value;
            _unitOfWork = new Lazy<IUnitOfWork>(() => lifetimeScope.Resolve<IUnitOfWork>());
            _httpContext = new Lazy<IHttpContextAccessor>(() => lifetimeScope.Resolve<IHttpContextAccessor>());
            _iamService = new Lazy<IIamService>(() => lifetimeScope.Resolve<IIamService>());
        }

        private ClaimsPrincipal UserIdentity => _httpContext.Value.HttpContext.User;

        #region Interface private

        private IUnitOfWork UnitOfWork => _unitOfWork.Value;
        private ISociaRepository SociaRepository => UnitOfWork.Repository<ISociaRepository>();

        private ICatalogoDetalleRepository CatalogoDetalleRepository =>
            UnitOfWork.Repository<ICatalogoDetalleRepository>();

        private IDepartamentoRepository DepartamentoRepository => UnitOfWork.Repository<IDepartamentoRepository>();

        private IIamService IamService => UnitOfWork.Repository<IIamService>();
        private IBancoComunalRepository BancoComunalRepository => UnitOfWork.Repository<IBancoComunalRepository>();
        private IAnilloGrupalRepository AnilloGrupalRepository => UnitOfWork.Repository<IAnilloGrupalRepository>();
        private IFormularioRepository FormularioRepository => UnitOfWork.Repository<IFormularioRepository>();
        private IPreSolicitudCabeceraRepository PreSolicitudCabeceraRepository => UnitOfWork.Repository<IPreSolicitudCabeceraRepository>();

        private IPreSolicitudRepository PreSolicitudRepository => UnitOfWork.Repository<IPreSolicitudRepository>();

        #endregion Interface private

        public async Task<ResponseDto> Catalogo(CatalogoModel model)
        {
            var comunDto = new CatalogoDto();
            if (model.ObtenerEstadoCivil)
                comunDto.EstadoCivil =
                    await CatalogoDetalleRepository.ListarPorCatalogoCodigoParaDropDown(Constants.Core.Catalogo.EstadoCivil);
            if (model.ObtenerGradoInstruccion)
                comunDto.GradoInstruccion =
                    await CatalogoDetalleRepository.ListarPorCatalogoCodigoParaDropDown(Constants.Core.Catalogo
                        .GradoInstruccion);
            if (model.ObtenerAfirmacion)
                comunDto.Afirmacion =
                    await CatalogoDetalleRepository.ListarPorCatalogoCodigoParaDropDown(Constants.Core.Catalogo.Afirmacion);
            if (model.ObtenerDepartamento)
                comunDto.Departamento = await DepartamentoRepository.ListarDropdown();
            if (model.ObtenerEntidadFinanciera)
                comunDto.EntidadFinanciera =
                    await CatalogoDetalleRepository.ListarPorCatalogoCodigoParaDropDown(Constants.Core.Catalogo
                        .EntidadFinanciera);
            if (model.ObtenerSituacionDomicilio)
                comunDto.SituacionDomicilio =
                    await CatalogoDetalleRepository.ListarPorCatalogoCodigoParaDropDown(Constants.Core.Catalogo
                        .SituacionDomicilio);
            if (model.ObtenerSucursal)
                comunDto.Sucursal =
                    await CatalogoDetalleRepository.ListarPorCatalogoCodigoParaDropDown(Constants.Core.Catalogo
                        .Sucursal);
            if (model.ObtenerAnilloGrupal)
                comunDto.AnilloGrupal = await AnilloGrupalRepository.Lista();
            if (model.ObtenerCargoBancoComunal)
                comunDto.CargoBancoComunal =
                    await CatalogoDetalleRepository.ListarPorCatalogoCodigoParaDropDown(Constants.Core.Catalogo.CargoBancoComunal);
            if (model.ObtenerTipoDocumento)
                comunDto.TipoDocumento =
                    await CatalogoDetalleRepository.ListarPorCatalogoCodigoParaDropDown(Constants.Core.Catalogo.TipoDocumentoIdentidad);
            if (model.ObtenerEstadoSocia)
                comunDto.EstadoSocia =
                    await CatalogoDetalleRepository.ListarPorCatalogoCodigoParaDropDown(Constants.Core.Catalogo.EstadoSocia);
            return new ResponseDto()
            {
                Data = comunDto
            };
        }

        public async Task<ResponseDto> RegistrarSocia(NuevaSociaModel model)
        {
            var sucursalId = await ObtnerSucursalId(model.SucursalCodigo);

            //if (model.TipoDocumentoId == 0)
            //{
            //    throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
            //       "Debe seleccionar el tipo documento.");
            //}
            if (!string.IsNullOrEmpty(model.Celular))
                if (await SociaRepository.ExsiteCelular(model.Celular, model.SociaId))
                    throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                   "El número de celular ya se encuentra registrado.");
            if (!string.IsNullOrEmpty(model.Telefono))
                if (await SociaRepository.ExsiteTelefono(model.Telefono, model.SociaId))
                    throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                   "El número de teléfono ya se encuentra registrado.");

            if (await SociaRepository.ExisteCuentaBancaria(model.EntidadBancariaId ?? 0, model.NroCuenta, model.SociaId))
            {
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                      "Cuenta bancaria ya se encuentra registrado.");
            }
            //if (model.BancoComunalId <= 0)
            //    throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
            //     "Seleccionar banco comunal.");
            if (!model.SociaId.HasValue)
            {
                var resultExist = await SociaRepository.Any(p => p.EstadoFila && p.NroDni == model.Dni);
                if (resultExist)
                    throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                        "Socia ya se encuentra Registrada");
            }
            var fechaNac = DateTime.ParseExact(model.FechaNacimiento, "dd/MM/yyyy", null);
            var edad = obtenerEdad(fechaNac);
            if (edad >= 90)
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                   $"Ingresar correctamente la fecha nacimiento porque supera los {edad} años.");

            var estado =
                await CatalogoDetalleRepository.ObtenerPorCodigoConEstadoActivo(Constants.Core.Catalogo.DetEstadoSocia
                    .Activa);

            var entity = new SociaEntity();

            if (model.SociaId.HasValue)
            {
                entity = await SetSocia(model, estado.Id, sucursalId, fechaNac);

                UnitOfWork.Set<SociaEntity>().Update(entity);
            }
            else
            {
                entity = new SociaEntity()
                {
                    NroDni = model.Dni,
                    Nombre = model.Nombre,
                    ApellidoPaterno = model.ApellidoPaterno,
                    ApellidoMaterno = model.ApellidoMaterno,
                    Celular = string.IsNullOrEmpty(model.Celular) ? null : model.Celular,
                    EstadoId = estado.Id,
                    SucursalId = sucursalId,
                    BancoComunalId = model.BancoComunalId,
                    CargoBancoComunalId = model.CargoBancoComunalId,
                    Telefono = string.IsNullOrEmpty(model.Telefono) ? null : model.Telefono,
                    TipoDocumentoId = model.TipoDocumentoId,

                    Formulario = new List<FormularioEntity>()
                    {
                        new()
                        {
                            EstadoCivilId = model.EstadoCivilId,
                            NroDependiente = model.NroDependiente,
                            ActividadEconomica = model.ActividadEconomica,
                            ActividadEconomica2 = string.IsNullOrEmpty(model.ActividadEconomica2)
                                ? null
                                : model.ActividadEconomica2,
                            ActividadEconomica3 = string.IsNullOrEmpty(model.ActividadEconomica3)
                                ? null
                                : model.ActividadEconomica3,
                            Ubicacion = model.Ubicacion,
                            Direccion = model.Direccion,
                            Referencia = model.Referencia,
                            NroCuenta = model.NroCuenta,
                            Representante = model.Representante,
                            UbicacionNegocio = model.UbicacionNegocio,
                            DireccionNegocio = model.DireccionNegocio,
                            ReferenciaNegocio = model.ReferenciaNegocio,
                            GradoInstruccionId = model.GradoInstruccionId,
                            SituacionDomicilioId = model.SituacionDomicilioId,
                            EntidadBancariaId = model.EntidadBancariaId,
                            FechaNacimiento = fechaNac,
                            Celular = string.IsNullOrEmpty(model.Celular)?null: model.Celular,
                            CargoBancoComunalId = model.CargoBancoComunalId,
                            BancoComunalId = model.BancoComunalId,
                            Telefono=string.IsNullOrEmpty(model.Telefono)?null:model.Telefono,
                            TipoDocumentoId= model.TipoDocumentoId
                        }
                    }
                };

                await UnitOfWork.Set<SociaEntity>().AddAsync(entity);
            }

            await UnitOfWork.SaveChangesAsync();
            if (!model.SociaId.HasValue || entity.Reingresante==true || entity.DatoIncompleto==true)
            {
                var crearUsuario = new RegistrarUsuarioModel()
                {
                    Nombre = model.Nombre,
                    ApellidoPaterno = model.ApellidoPaterno,
                    ApellidoMaterno = model.ApellidoMaterno,
                    Celular = model.Celular,
                    Dni = model.Dni
                };
                var response = await IamService.RegistrarUsuarioTipoSocia(crearUsuario);
                if (response.Status == Constants.SystemStatusCode.Ok)
                    await UnitOfWork.SaveChangesAsync();
            }
            else
            {
                await UnitOfWork.SaveChangesAsync();
            }

            return new ResponseDto() { Message = CommonResource.register_ok };
        }

        public async Task<ResponseDto<PaginationResultDTO<ListaSociaParaAprobarDto>>> BusquedaSocia(FiltroSociaParaAprobarModel model)
        {
            //var estado = await CatalogoDetalleRepository.ObtenerPorCodigoConEstadoActivo(Constants.Core.Catalogo.DetEstadoSocia.Activa);

            ResponseDto<PaginationResultDTO<ListaSociaParaAprobarDto>> result;

            var listaSucursal = ObtenerSucursal();
            //model.EstadoSociaId=estado.Id;
            result = await SociaRepository.ListadoSociaParaAprobar(model, listaSucursal);

            return result;
        }

        public async Task<ResponseDto> AprobarAccesoSocia(RegistrarUsuarioModel model)
        {
            var listaEstado = await CatalogoDetalleRepository.ListarPorCatalogoCodigoParaDropDown(Constants.Core.Catalogo.EstadoSocia);
            var deBajaId = listaEstado.First(p => p.Code == Constants.Core.Catalogo.DetEstadoSocia.Debaja).Id;
            var obtenerSocia = await SociaRepository.FirstOrDefault(p => p.Id == model.Id && p.EstadoFila);
            
            if ((obtenerSocia.Reingresante==true || obtenerSocia.DatoIncompleto==true) && obtenerSocia.EstadoId == deBajaId)
            {
                return new ResponseDto
                {

                    Status = 2,
                    Message = "Debe Completar sus los datos de la socia para poder registrar su presolicitud.",
                    Data = obtenerSocia.NroDni
                };
            }

            if (obtenerSocia == null)
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, "No se encontro registro.");
            if(obtenerSocia.TipoDocumentoId==0)
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, "Socia no tiene registrado el tipo de documento.");
            var formulario = await FormularioRepository.ObtenerPorSocia(model.Id);
            
            if (formulario.Any() && formulario.Count() > 1)
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, "La socia cuenta con varios formulario, comunicarse con sistemas.");

            model.Nombre = obtenerSocia.Nombre;
            model.ApellidoPaterno = obtenerSocia.ApellidoPaterno;
            model.ApellidoMaterno = obtenerSocia.ApellidoMaterno;
            model.Celular = obtenerSocia.Celular;
            model.Dni = obtenerSocia.NroDni;

            

            

            obtenerSocia.EstadoId = listaEstado.First(f => f.Code == model.Estado).Id;
            obtenerSocia.BancoComunalId = model.BancoComunalId;
            obtenerSocia.CargoBancoComunalId = model.CargoBancoComunalId;
            UnitOfWork.Set<SociaEntity>().Update(obtenerSocia);

            if (formulario.Any())
            {
                var entityFormulario = formulario.First();
                entityFormulario.BancoComunalId = model.BancoComunalId;
                entityFormulario.CargoBancoComunalId = model.CargoBancoComunalId;
                UnitOfWork.Set<FormularioEntity>().Update(entityFormulario);
            }
            if (listaEstado.First(f => f.Code == model.Estado).Code == Constants.Core.Catalogo.DetEstadoSocia.Activa)
            {
                var response = await IamService.RegistrarUsuarioTipoSocia(model);
                if (response.Status == Constants.SystemStatusCode.Ok)
                    await UnitOfWork.SaveChangesAsync();

                return response;
            }
            else
            {
                await UnitOfWork.SaveChangesAsync();
            }

            return new ResponseDto() { Message = "Se registro correctamente." };
        }

        public async Task<ResponseDto> ObtenerSociaPorId(int sociaId)
        {
            var result = await SociaRepository.ObtenerPorIdParaActualizarDatoPersonal(sociaId);
            if (result == null)
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, "No se encontro registro.");

            return new ResponseDto()
            {
                Data = result
            };
        }

        public async Task<ResponseDto> ObtenerSociaPorDni(string dni)
        {
            var listaSucursal = ObtenerSucursal();
            var result = await SociaRepository.ObtenerSinFormularioPorDniyListaSucursal(dni, listaSucursal);
            if (result == null)
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, "No se encontró socia.");

            return new ResponseDto()
            {
                Data = result
            };
        }

        public async Task<ResponseDto> ObtenerSociaPorDniParaActFormulario(string dni)
        {
            var listaSucursal = ObtenerSucursal();
            var result = await SociaRepository.ObtenerPorDniyListaSucursal(dni, listaSucursal);
            if (result == null)
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, "No se encontró socia.");

            return new ResponseDto()
            {
                Data = result
            };
        }

        public async Task<ResponseDto> ActualizarDatoPersonal(ActualizarDatoPersonalDto model)
        {
            var result = await SociaRepository.ObtenerSociaConEstado(model.Id);
            if (result == null)
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, "No se actulizó porque socia no se encuentra registrada.");
            //if (result.Estado.Codigo == Constants.Core.Catalogo.DetEstadoSocia.Rechazada)
            //    throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, "No se puede actualizar porque tiene estado rechazada");
            if (result.Estado.Codigo == Constants.Core.Catalogo.DetEstadoSocia.Activa)
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, "No se puede actualizar porque tiene estado con acceso");
            result.NroDni = model.Dni;
            result.Nombre = model.Nombre;
            result.ApellidoPaterno = model.ApellidoPaterno;
            result.ApellidoMaterno = model.ApellidoMaterno;
            UnitOfWork.Set<SociaEntity>().Update(result);
            await UnitOfWork.SaveChangesAsync();
            return new ResponseDto() { Message = CommonResource.update_ok };
        }

        public async Task<ResponseDto> BusquedaBancoComunal(string descripcion, string sucursal)
        {
            var listSucursal = ValidarSucursalSeleccionado(sucursal);
            if (!string.IsNullOrEmpty(sucursal))
                listSucursal = listSucursal.Where(p => p == sucursal).ToList();
            var result = await BancoComunalRepository.BusquedaPorDescripcion(descripcion, listSucursal);
            return new ResponseDto()
            {
                Data = result
            };
        }

        public async Task<ResponseDto> BusquedaBancoComunalSinRestriccion(string descripcion, string sucursal)
        {
            var result = await BancoComunalRepository.BusquedaPorDescripcion(descripcion, new List<string> { sucursal });
            return new ResponseDto()
            {
                Data = result
            };
        }

        public async Task<ResponseDto<PaginationResultDTO<ListaSociaPorBancoComunalDto>>> BusquedaPorBancoComunal(FiltroBusquedaPorBancoComunalModel model)
        {
            if (string.IsNullOrEmpty(model.Sucursal))
            {
                model.ListaSucursalCodigo = ObtenerSucursal();
            }
            else
            {
                model.ListaSucursalCodigo = ObtenerSucursal().Where(p => p == model.Sucursal).ToList();
            }
            if (!model.ListaSucursalCodigo.Any())
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, "Usuario no tiene asignado la sucursal ingresada.");
            ResponseDto<PaginationResultDTO<ListaSociaPorBancoComunalDto>> response;
            response = await SociaRepository.ObtnerPorBancoComunalYAnilloGrupal(model);
            return response;
        }

        public async Task<ResponseDto<PaginationResultDTO<ListaSociaPorBancoComunalDto>>> BusquedaPorBancoComunalSinRestriccion(FiltroBusquedaPorBancoComunalModel model)
        {
            ResponseDto<PaginationResultDTO<ListaSociaPorBancoComunalDto>> response;
            response = await SociaRepository.ObtnerPorBancoComunalYAnilloGrupal(model);
            return response;
        }

        public async Task<ResponseDto> ObtenerInformacionSociaPorId(int id)
        {
            //var listaSucursalCodigo = ObtenerSucursal();
            var result = await SociaRepository.ObtenerInformacionPorId(id);
            if (result == null)
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, "No se encontro registro.");
            return new ResponseDto()
            {
                Data = result
            };
        }

        public async Task<ResponseDto> ActualizarInformacionSocia(InformacionSociaModel model)
        {
            try
            {
                await UnitOfWork.BeginTransaction();

                var result = await SociaRepository.ObtenerConFormularioPorIdYEstado(model.Id, string.Empty);
                if (result == null)
                    throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                        "Socia no se encuentra registrada.");

                if (result.Formulario.FirstOrDefault() == null)
                    throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                        "Socia no cuenta con formulario registrado");

                var dniActual = result.NroDni;

                result.Celular = model.Celular;
                result.Telefono = model.Telefono;

                result.Formulario.First().EntidadBancariaId = model.EntidadBancariaId;
                result.Formulario.First().NroCuenta = model.NroCuenta;
                result.Formulario.First().ActividadEconomica = model.Actividad;
                result.Formulario.First().ActividadEconomica2 = string.IsNullOrEmpty(model.Actividad2) ? null : model.Actividad2;
                result.Formulario.First().ActividadEconomica3 = string.IsNullOrEmpty(model.Actividad3) ? null : model.Actividad3;
                result.Formulario.First().Celular = model.Celular;
                result.Formulario.First().Telefono = model.Telefono;
                result.Formulario.First().CargoBancoComunalId = model.CargoBancoComunalId;

                if (!result.SociaId_SistemaExterno.HasValue)
                {
                    var fechaNac = DateTime.ParseExact(model.FechaNacimiento, "dd/MM/yyyy", null);
                    var edad = obtenerEdad(fechaNac);
                    if (edad >= 90)
                        throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                            $"Ingresar correctamente la fecha nacimiento porque supera los {edad} años.");
                    result.Formulario.First().FechaNacimiento = fechaNac;
                }

                result.Formulario.First().Direccion = model.Direccion;
                result.Formulario.First().Referencia = model.Referencia;
                result.Formulario.First().Ubicacion = model.Ubicacion;

                result.Formulario.First().DireccionNegocio = model.DireccionNegocio;
                result.Formulario.First().ReferenciaNegocio = model.ReferenciaNegocio;
                result.Formulario.First().UbicacionNegocio = model.UbicacionNegocio;

                result.NroDni = result.SociaId_SistemaExterno.HasValue ? result.NroDni : model.NroDni;
                result.ApellidoPaterno = model.ApellidoPaterno;
                result.ApellidoMaterno = model.ApellidoMaterno;
                result.Nombre = model.Nombre;

                if (result.Formulario.First().BancoComunalId == null)
                {
                    throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, "El formulario de la socia no cuenta con banco comunal.");
                }

                var bancoComunalId = result.Formulario.First()?.BancoComunalId;
                var anilloGrupalId = result.Formulario.First()?.AnilloGrupalId;
                var resultData = await FormularioRepository
                    .ObtenerPorBancoComunalyAnilloGrupal(bancoComunalId ?? 0, anilloGrupalId);
                var sinCargo = await CatalogoDetalleRepository.ObtenerPorCodigoConEstadoActivo(Constants.Core.Catalogo.DetCargoBancoComunal.SinCargo);
                if (resultData.Any() && resultData.Any(a => a.CargoBancoComunalId == model.CargoBancoComunalId
                                                        && a.SociaId != model.Id
                                                        && a.CargoBancoComunalId != sinCargo.Id))
                {
                    var data = resultData.First(a => a.CargoBancoComunalId == model.CargoBancoComunalId).Nombre;
                    throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, $"El cargo ya se encuentra asignado por {data}");
                    //return new ResponseDto() { Message = $"El cargo ya se encuentra asignado por {data}" };
                }

                UnitOfWork.Set<SociaEntity>().Update(result);
                var resultService = await IamService.ActualizarCuentaUsuarioConDni(new ActualizarUsuarioConDniModel()
                {
                    DniNuevo = model.NroDni,
                    Dni = result.SociaId_SistemaExterno.HasValue ? result.NroDni : dniActual,
                    Nombre = model.Nombre,
                    ApellidoPaterno = model.ApellidoPaterno,
                    ApellidoMaterno = model.ApellidoMaterno,
                });
                if (resultService.Status != Constants.SystemStatusCode.Ok)
                {
                    await UnitOfWork.Rollback();
                    throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, resultService.Message);
                }
                await UnitOfWork.SaveChangeTransaction();
                return new ResponseDto { Status = Constants.SystemStatusCode.Ok, Message = CommonResource.update_ok };
            }
            catch (FunctionalException ex)
            {
                throw new FunctionalException(ex.Message);
            }
            catch (Exception ex)
            {
                await UnitOfWork.Rollback();
                throw new TechnicalException(ex.Message);
            }
        }

        public async Task<ResponseDto> EliminarSocia(int id)
        {
            var listCatalogo = new List<DropdownDto>();

            var existePresolicitudAprobada = await PreSolicitudCabeceraRepository.ExistePreSolicitudAprobadaPorSociaId(id);
            if (existePresolicitudAprobada)
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, "Socia ya tiene una prosilicitud aprobada");

            var result = await SociaRepository.ObtenerConFormularioPorIdYEstado(id, string.Empty);
            if (result.SociaId_SistemaExterno.HasValue)
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, "No se puede eliminar socia, ya se encuentra registrada en el sistema externo.");
            var resultPresolicitud = await PreSolicitudRepository.ObtenerSinPresolicitudCabeceraPorSocia(id);
            if (resultPresolicitud.Any())
            {
                listCatalogo = await CatalogoDetalleRepository.ObtenerPorListCodigo(
                        new List<string> {
                        Constants.Core.Catalogo.PreSolicitudEstado.Rechazada,
                        Constants.Core.Catalogo.PreSolicitudEstado.Retirada
                        }
                 );
                resultPresolicitud = resultPresolicitud.Where(p => !listCatalogo.Select(s => s.Id).ToList().Contains(p.EstadoId)).ToList();
            }
            try
            {
                await UnitOfWork.BeginTransaction();

                result.EstadoFila = false;
                if (result.Formulario.Any())
                    result.Formulario.FirstOrDefault().EstadoFila = false;
                UnitOfWork.Set<SociaEntity>().Update(result);

                if (resultPresolicitud.Any())
                {
                    resultPresolicitud.ForEach(f =>
                    {
                        f.EstadoId = listCatalogo.First(x => x.Code == Constants.Core.Catalogo.PreSolicitudEstado.Rechazada).Id;
                        f.Comentario = "Socia ha sido eliminada.";
                    });
                    UnitOfWork.Set<PreSolicitudEntity>().UpdateRange(resultPresolicitud);
                }
                var resultService = await IamService.EliminarSocia(new UsuarioModel()
                {
                    usuario = result.NroDni,
                });
                if (resultService.Status != Constants.SystemStatusCode.Ok)
                {
                    await UnitOfWork.Rollback();
                    return new ResponseDto { Status = Constants.SystemStatusCode.FunctionalError, Message = resultService.Message };
                }
                await UnitOfWork.SaveChangeTransaction();
                return new ResponseDto { Status = Constants.SystemStatusCode.Ok, Message = CommonResource.delete_ok };
            }
            catch (Exception ex)
            {
                await UnitOfWork.Rollback();
                throw new TechnicalException(ex.Message);
            }
        }

        public async Task<ResponseDto<PaginationResultDTO<ListaSociaPorSucursalDto>>> BusquedaSociaPorSucursal(FiltroBusquedaPorSucursalDatoModel model)
        {
            ResponseDto<PaginationResultDTO<ListaSociaPorSucursalDto>> result;

            ValidarSucursalSeleccionado(model.SucursalCodigo);

            result = await SociaRepository.ObtenerPorSucursalyDatos(model);

            return result;
        }

        public ResponseDto BusquedaSociaExterno(FiltroBusquedaPorSucursalDatoModel model)
        {
            var descripcion = string.IsNullOrEmpty(model.Dni) ? model.Nombre : model.Dni;
            var result = SociaRepository.BusquedaSocia(descripcion);

            var response = new ResponseDto();
            response.Data = result;
            return response;
        }

        public async Task<ResponseDto> ListaHistorialSolicitud(string codigoSocia)
        {
            var socia = await SociaRepository.FirstOrDefault(p => p.EstadoFila && p.CodigoCliente == codigoSocia);
            if (socia == null)
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, "socia debe tener su codigo interno.");

            if (!string.IsNullOrEmpty(codigoSocia))
            {
                var result = SociaRepository.ListadoProducto(codigoSocia);
                if (result.Any())
                {
                    result.ForEach(f => { f.Socia = socia.ApellidoPaterno + " " + socia.ApellidoMaterno + " " + socia.Nombre; });
                }
                else
                {
                    throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, "socia no cuenta con historial.");
                }
                return new ResponseDto
                {
                    Data = result.OrderByDescending(o => o.FechaDesembolso).ToList()
                };
            }
            return new ResponseDto();
        }

        public ResponseDto ListaHistorialSolicitudPorNumeroDocumento(string numeroDocumento)
        {
            if (!string.IsNullOrEmpty(numeroDocumento))
            {
                var result = SociaRepository.ListadoProductoPorNumDocumento(numeroDocumento);
                if (!result.Any())
                {
                    throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, "socia no cuenta con historial.");
                }
                return new ResponseDto
                {
                    Data = result.OrderByDescending(o => o.FechaDesembolso).ToList()
                };
            }
            return new ResponseDto();
        }

        public async Task<ResponseDto> ListaCredito(string numeroCredito)
        {
            var cabecera = SociaRepository.ListadoCreditoCabecera(numeroCredito);
            var detalle = SociaRepository.ListadoCreditoDetalle(numeroCredito);
            if (cabecera != null)
            {
                //var guid = Guid.NewGuid();
                cabecera[0].Id = numeroCredito;
            }
            if (detalle.Any())
            {
                detalle = detalle.OrderBy(o => o.FechaVencimiento).ToList();
            }
            return new ResponseDto
            {
                Data = {
                        Cabecera=cabecera.FirstOrDefault(),
                        Detalle=detalle
                    }
            };
        }

        public async Task<ResponseDto<PaginationResultDTO<ListaSociaMotivoBajaDto>>> ListadoSociaConMotivoBaja(FiltroSociaConMotivoBajaModel model)
        {
            ResponseDto<PaginationResultDTO<ListaSociaMotivoBajaDto>> result = new ResponseDto<PaginationResultDTO<ListaSociaMotivoBajaDto>>();
            var resultSearch = SociaRepository.ListadoSociaConMotivoBaja(model);

            var paginationResult = new PaginationResultDTO<ListaSociaMotivoBajaDto>
            {
                CurrentPage = model.Page,
                PageSize = model.PageSize,
                RowCount = resultSearch.Item2
            };
            var pageCount = (double)paginationResult.RowCount / paginationResult.PageSize;
            paginationResult.PageCount = (int)Math.Ceiling(pageCount);

            paginationResult.Results = resultSearch.Item1;

            result.Data = paginationResult;

            return result;
        }

        public async Task<ResponseDto> ExisteSociaAsignadaAlCargo(int sociaId, int cargoBancoComunalId)
        {
            var listaCargoBancoComunal = await CatalogoDetalleRepository.ListarPorCatalogoCodigoParaDropDown(
                Constants.Core.Catalogo.CargoBancoComunal
            );

            var cargoBancoComunalCodigo = listaCargoBancoComunal.Where(p => p.Id == cargoBancoComunalId).FirstOrDefault().Code;
            if (cargoBancoComunalCodigo == null)
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, "Debe ingresar un cargo banco comunal.");

            if (cargoBancoComunalCodigo == Constants.Core.Catalogo.DetCargoBancoComunal.SinCargo)
                return new ResponseDto();

            var socia = await SociaRepository.ObtenerSociaConEstado(sociaId);

            if (socia == null)
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, "Socia no se encuentra registrada.");

            if (socia.Estado.Codigo != Constants.Core.Catalogo.DetEstadoSocia.Activa)
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, "Socia no se encuentra con acceso.");

            var formulario = await FormularioRepository.ObtenerPorCodigoPorSocia(sociaId);

            if (formulario?.BancoComunalId == null)
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, "Socia no cuenta con formulario registrado o no tiene registrado el banco comunal.");

            var bancoComunalId = formulario.BancoComunalId;
            var anilloGrupalId = formulario.AnilloGrupalId;

            var result = await FormularioRepository.ObtenerPorBancoComunalyAnilloGrupal(bancoComunalId ?? 0, anilloGrupalId);
            var sinCargo = await CatalogoDetalleRepository.ObtenerPorCodigoConEstadoActivo(Constants.Core.Catalogo.DetCargoBancoComunal.SinCargo);
            if (result.Any() && result.Any(a => a.CargoBancoComunalId == cargoBancoComunalId && a.SociaId != socia.Id && a.CargoBancoComunalId != sinCargo.Id))
            {
                //var data = result.First(a => a.CargoBancoComunalId == cargoBancoComunalId).Nombre;
                var data = result.Where(a => a.CargoBancoComunalId == cargoBancoComunalId).Select(s => s.Nombre).ToList();
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, $"El cargo ya se encuentra asignado por {string.Join(",", data)}");
                //return new ResponseDto() { Message = $"El cargo ya se encuentra asignado por {data}" };
            }
            return new ResponseDto();
        }

        public async Task<ResponseDto> ExisteCargoDisponible(int bancoComunalId, int cargoBancoComunalId)
        {
            var result = await FormularioRepository.ObtenerPorBancoComunalyAnilloGrupal(bancoComunalId, null);
            var sinCargo = await CatalogoDetalleRepository.ObtenerPorCodigoConEstadoActivo(Constants.Core.Catalogo.DetCargoBancoComunal.SinCargo);
            if (result.Any() && result.Any(a => a.CargoBancoComunalId == cargoBancoComunalId && a.CargoBancoComunalId != sinCargo.Id))
            {
                var data = result.Where(a => a.CargoBancoComunalId == cargoBancoComunalId).Select(s => s.Nombre).ToList();
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, $"El cargo ya se encuentra asignado por {string.Join(",", data)}");
            }
            return new ResponseDto();
        }
        public async Task<ResponseDto> BusquedaBancoComunalPorId(int id)
        {
            var result = await BancoComunalRepository.ObtenerPorId(id);
            var data = new DropdownDto()
            {
                Id = result.Id,
                Code = result.Codigo,
                Description = result.Descripcion
            };
            return new ResponseDto()
            {
                Data = data
            };
        }

        #region private

        private async Task<SociaEntity> SetSocia(NuevaSociaModel model, int estadoId, int sucursalId, DateTime fechaNac)
        {
            var entity = new SociaEntity();
            if (!model.SociaId.HasValue) return entity;

            entity = await SociaRepository.GetById(model.SociaId ?? 0);
            entity.NroDni = model.Dni;
            entity.Nombre = model.Nombre;
            entity.ApellidoPaterno = model.ApellidoPaterno;
            entity.ApellidoMaterno = model.ApellidoMaterno;
            entity.Celular = string.IsNullOrEmpty(model.Celular) ? null : model.Celular;
            entity.EstadoId = estadoId;
            entity.SucursalId = sucursalId;
            
            entity.Telefono = string.IsNullOrEmpty(model.Telefono) ? null : model.Telefono;
            entity.CargoBancoComunalId = model.CargoBancoComunalId;

            if (entity.DatoIncompleto == true || entity.Reingresante == true)
            {
                //if (model.ConservarId != null)
                //    entity.SociaId_SistemaExterno = !model.ConservarId.Value ? null : entity.SociaId_SistemaExterno;

                if (entity.SociaId_SistemaExterno.HasValue && entity.BancoComunalId.HasValue
                                                           && entity.BancoComunalId != model.BancoComunalId
                   )
                    entity.SociaId_SistemaExterno = null;

                entity.BancoComunalId = model.BancoComunalId;
                entity.Formulario = await FormularioRepository.ObtenerPorSocia(model.SociaId ?? 0);

                foreach (var v in entity.Formulario)
                {
                    v.EstadoCivilId = model.EstadoCivilId;
                    v.NroDependiente = model.NroDependiente;
                    v.ActividadEconomica = model.ActividadEconomica;
                    v.ActividadEconomica2 = string.IsNullOrEmpty(model.ActividadEconomica2)
                        ? null
                        : model.ActividadEconomica2;
                    v.ActividadEconomica3 = string.IsNullOrEmpty(model.ActividadEconomica3)
                        ? null
                        : model.ActividadEconomica3;
                    v.Ubicacion = model.Ubicacion;
                    v.Direccion = model.Direccion;
                    v.Referencia = model.Referencia;
                    v.NroCuenta = model.NroCuenta;
                    v.Representante = model.Representante;
                    v.UbicacionNegocio = model.UbicacionNegocio;
                    v.DireccionNegocio = model.DireccionNegocio;
                    v.ReferenciaNegocio = model.ReferenciaNegocio;
                    v.GradoInstruccionId = model.GradoInstruccionId;
                    v.SituacionDomicilioId = model.SituacionDomicilioId;
                    v.EntidadBancariaId = model.EntidadBancariaId;
                    v.FechaNacimiento = fechaNac;
                    v.Celular = string.IsNullOrEmpty(model.Celular) ? null : model.Celular;
                    v.CargoBancoComunalId = model.CargoBancoComunalId;
                    v.BancoComunalId = model.BancoComunalId;
                    v.Telefono = string.IsNullOrEmpty(model.Telefono) ? null : model.Telefono;
                    v.TipoDocumentoId = model.TipoDocumentoId;
                }

                if (!entity.Formulario.Any())
                {
                    entity.Formulario = new List<FormularioEntity>()
                    {
                        new()
                        {
                            EstadoCivilId = model.EstadoCivilId,
                            NroDependiente = model.NroDependiente,
                            ActividadEconomica = model.ActividadEconomica,
                            ActividadEconomica2 = string.IsNullOrEmpty(model.ActividadEconomica2)
                                ? null
                                : model.ActividadEconomica2,
                            ActividadEconomica3 = string.IsNullOrEmpty(model.ActividadEconomica3)
                                ? null
                                : model.ActividadEconomica3,
                            Ubicacion = model.Ubicacion,
                            Direccion = model.Direccion,
                            Referencia = model.Referencia,
                            NroCuenta = model.NroCuenta,
                            Representante = model.Representante,
                            UbicacionNegocio = model.UbicacionNegocio,
                            DireccionNegocio = model.DireccionNegocio,
                            ReferenciaNegocio = model.ReferenciaNegocio,
                            GradoInstruccionId = model.GradoInstruccionId,
                            SituacionDomicilioId = model.SituacionDomicilioId,
                            EntidadBancariaId = model.EntidadBancariaId,
                            FechaNacimiento = fechaNac,
                            Celular = string.IsNullOrEmpty(model.Celular) ? null : model.Celular,
                            CargoBancoComunalId = model.CargoBancoComunalId,
                            BancoComunalId = model.BancoComunalId,
                            Telefono = string.IsNullOrEmpty(model.Telefono) ? null : model.Telefono,
                            TipoDocumentoId = model.TipoDocumentoId,
                        }
                    };
                }
            }
            else
            {
                entity.BancoComunalId = model.BancoComunalId;
                entity.Formulario = new List<FormularioEntity>()
                {
                    new()
                    {
                        EstadoCivilId = model.EstadoCivilId,
                        NroDependiente = model.NroDependiente,
                        ActividadEconomica = model.ActividadEconomica,
                        ActividadEconomica2 = string.IsNullOrEmpty(model.ActividadEconomica2)
                            ? null
                            : model.ActividadEconomica2,
                        ActividadEconomica3 = string.IsNullOrEmpty(model.ActividadEconomica3)
                            ? null
                            : model.ActividadEconomica3,
                        Ubicacion = model.Ubicacion,
                        Direccion = model.Direccion,
                        Referencia = model.Referencia,
                        NroCuenta = model.NroCuenta,
                        Representante = model.Representante,
                        UbicacionNegocio = model.UbicacionNegocio,
                        DireccionNegocio = model.DireccionNegocio,
                        ReferenciaNegocio = model.ReferenciaNegocio,
                        GradoInstruccionId = model.GradoInstruccionId,
                        SituacionDomicilioId = model.SituacionDomicilioId,
                        EntidadBancariaId = model.EntidadBancariaId,
                        FechaNacimiento = fechaNac,
                        Celular = string.IsNullOrEmpty(model.Celular) ? null : model.Celular,
                        CargoBancoComunalId = model.CargoBancoComunalId,
                        BancoComunalId = model.BancoComunalId,
                        Telefono = string.IsNullOrEmpty(model.Telefono) ? null : model.Telefono,
                        TipoDocumentoId = model.TipoDocumentoId,
                    }
                };
            }

            entity.DatoIncompleto = entity.DatoIncompleto.HasValue ? false : entity.DatoIncompleto;

            return entity;
        }

        private List<string> ValidarSucursalSeleccionado(string sucursalCodigo)
        {
            var listaSucursal = ObtenerSucursal();
            if (string.IsNullOrEmpty(sucursalCodigo))
            {
                return listaSucursal;
            }
            else
            {
                if (!listaSucursal.Any(a => a == sucursalCodigo))
                {
                    throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, "Usuario no tiene acceso" +
                        " a la surusal.");
                }
                else
                {
                    return listaSucursal.Where(p => p == sucursalCodigo).ToList();
                }
            }
        }

        private List<string> ObtenerSucursal()
        {
            var data = UserIdentity.FindFirst(Constants.Core.UserAsociadoClaims.Sucursal);
            if (data == null)
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, "Usuario debe estar asignado a una sucursal.");
            return data != null ? data.Value.Split(",").ToList() : new List<string>();
        }

        private async Task<int> ObtnerSucursalId(string sucursalCodigo)
        {
            var sucursalAsignado = UserIdentity.FindFirst(Constants.Core.UserAsociadoClaims.Sucursal);
            var listaSucursalAsignado = sucursalAsignado != null ? sucursalAsignado.Value.Split(",").ToList() : new List<string>();
            if (listaSucursalAsignado.Any(a => a == sucursalCodigo))
            {
                var obtenerSucursal = await CatalogoDetalleRepository
                    .FirstOrDefault(f => f.EstadoFila && f.Codigo == sucursalCodigo
                    && f.Catalogo.Codigo == Constants.Core.Catalogo.Sucursal
                    );

                if (sucursalCodigo == null)
                {
                    throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                        "Sucursal ingresado no existe o esta deshabilitado.");
                }
                else
                {
                    return obtenerSucursal.Id;
                }
            }
            else
            {
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                    "Usuario no tiene asignado la sucursal ingresada.");
            }
        }

        private int obtenerEdad(DateTime fechaNacimiento)
        {
            return (int)Math.Floor((DateTime.Now - fechaNacimiento).TotalDays / 365.25D);
        }

        #endregion private
    }
}