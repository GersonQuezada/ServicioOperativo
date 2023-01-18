using Autofac;
using Credimujer.Op.Application.Interfaces;
using Credimujer.Op.Common;
using Credimujer.Op.Repository.Interfaces;
using Credimujer.Op.Repository.Interfaces.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Credimujer.Op.Common.Base;
using Credimujer.Op.Common.Exceptions;
using Credimujer.Op.Common.Resources;
using Credimujer.Op.Domail.Models.Entities;
using Credimujer.Op.Dto.PreSolicitud.GenerarPreSolicitud;
using Credimujer.Op.Model.PreSolicitud;
using Credimujer.Op.Model.PreSolicitud.GenerarPresolicitud;
using Credimujer.Op.Dto.Base;
using static Credimujer.Op.Common.Constants.Core.Catalogo;

namespace Credimujer.Op.Application.Implementations.PreSolicitud
{
    public class PreSolicitudApplication : IPreSolicitudApplication
    {
        private readonly Lazy<IUnitOfWork> _unitOfWork;
        private readonly AppSetting _setting;
        private readonly Lazy<IHttpContextAccessor> _httpContext;

        public PreSolicitudApplication(IOptions<AppSetting> settings, ILifetimeScope lifetimeScope)
        {
            _setting = settings.Value;
            _unitOfWork = new Lazy<IUnitOfWork>(() => lifetimeScope.Resolve<IUnitOfWork>());
            _httpContext = new Lazy<IHttpContextAccessor>(() => lifetimeScope.Resolve<IHttpContextAccessor>());
        }

        #region Interface Private

        private ClaimsPrincipal UserIdentity => _httpContext.Value.HttpContext.User;
        private IUnitOfWork UnitOfWork => _unitOfWork.Value;
        private ISociaRepository SociaRepository => UnitOfWork.Repository<ISociaRepository>();
        private IPreSolicitudRepository PreSolicitudRepository => UnitOfWork.Repository<IPreSolicitudRepository>();

        private ICatalogoDetalleRepository CatalogoDetalleRepository =>
            UnitOfWork.Repository<ICatalogoDetalleRepository>();

        private IBancoComunalRepository BancoComunalRepository => UnitOfWork.Repository<IBancoComunalRepository>();
        private IAnilloGrupalRepository AnilloGrupalRepository => UnitOfWork.Repository<IAnilloGrupalRepository>();
        private IFormularioRepository FormularioRepository => UnitOfWork.Repository<IFormularioRepository>();

        private IPreSolicitudCabeceraRepository PreSolicitudCabeceraRepository =>
            UnitOfWork.Repository<IPreSolicitudCabeceraRepository>();

        #endregion Interface Private

        public async Task<ResponseDto> Catalog(CatalogModel model)
        {
            var data = new ResponseDto();
            if (model.TipoCredito)
                data.Data.listaTipoCredito =
                    await CatalogoDetalleRepository.ListarPorCatalogoCodigoParaDropDown(Constants.Core.Catalogo
                        .TipoCredito);
            if (model.SubTipoCredito)
                data.Data.listaSubTipoCredito =
                    await CatalogoDetalleRepository.ListarPorCatalogoCodigoParaDropDown(Constants.Core.Catalogo
                        .SubTipoCredito);
            if (model.TasaInteres)
                data.Data.listaTasaInteres = await CatalogoDetalleRepository.ListarPorCatalogoCodigoParaDropDown(Constants.Core.Catalogo
                    .TasaInteres);
            if (model.TasaInteresRebatir)
                data.Data.listaTasaInteresRebatir = await CatalogoDetalleRepository.ListarPorCatalogoCodigoParaDropDown(Constants.Core.Catalogo
                    .TasaInteresRebatir);
            if (model.EntidadFinanciera)
                data.Data.listaEntidadFinanciera = await CatalogoDetalleRepository.ListarPorCatalogoCodigoParaDropDown(Constants.Core.Catalogo
                    .EntidadFinanciera);
            if (model.Asistencia)
                data.Data.listaAsistencia = await CatalogoDetalleRepository.ListarPorCatalogoCodigoParaDropDown(Constants.Core.Catalogo
                    .AsistenciaPreSolicitud);
            if (model.NivelRiesgo)
                data.Data.listaNivelRiesgo = await CatalogoDetalleRepository.ListarPorCatalogoCodigoParaDropDown(Constants.Core.Catalogo
                    .NivelRiesgo);
            if (model.AnilloGrupal)
                data.Data.listaAnilloGrupal = await AnilloGrupalRepository.Lista();
            if (model.MotivoRetiro)
            {
                data.Data.listaMotivoRetiro = await CatalogoDetalleRepository.ListarPorCatalogoCodigoParaDropDown(Constants.Core.Catalogo
                    .MotivoRetiro);
            }
            if (model.ObtenerTipoPresolicitudAGenerar)
                data.Data.listaTipoPresolicitudAGenerar =
                    await CatalogoDetalleRepository.ListarPorCatalogoCodigoParaDropDown(Constants.Core.Catalogo.TipoPresolicitudGenerar);
            if (model.MostrarControl)
                data.Data.mostrarControl = TieneCargoJefatura();
            if(model.TipoDeuda)
                data.Data.listaTipoDeuda = await CatalogoDetalleRepository.ListarPorCatalogoCodigoParaDropDown(Constants.Core.Catalogo
                    .TipoDeuda);
            return data;
        }

        public async Task<ResponseDto> BusquedaBancoComunal(string descripcion, string sucursal)
        {
            var listSucursal = ValidarSucursalSeleccionado(sucursal);
            if (!string.IsNullOrEmpty(sucursal))
                listSucursal = listSucursal.Where(p => p == sucursal).ToList();
            var result = await BancoComunalRepository.ListarPorDescripcion(descripcion, listSucursal);
            return new ResponseDto()
            {
                Data = result
            };
        }

        public async Task<ResponseDto> BusquedaBancoComunalconSucursal(string descripcion, int sucursalId)
        {
            var sucursal = await CatalogoDetalleRepository.FirstOrDefault(f => f.EstadoFila && f.Id == sucursalId);
            var listSucursal = ValidarSucursalSeleccionado(sucursal.Codigo);
            if (!string.IsNullOrEmpty(sucursal.Codigo))
                listSucursal = listSucursal.Where(p => p == sucursal.Codigo).ToList();
            var result = await BancoComunalRepository.ListarPorDescripcion(descripcion, listSucursal);
            return new ResponseDto()
            {
                Data = result
            };
        }

        public async Task<ResponseDto> ObtnerCuentaBancariaSocia(int sociaId)
        {
            var listaSucursal = ObtenerSucursal();
            var formulario = await FormularioRepository.FirstOrDefault(p => p.EstadoFila && p.SociaId == sociaId
                && listaSucursal.Contains(p.Socia.Sucursal.Codigo));

            if (formulario == null)
            {
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, "Debe ingresar una socia que pertenezca a su sucursal asignada.");
            }

            return new ResponseDto()
            {
                Data ={
                       NroCuenta= formulario.NroCuenta,
                       EntidadBancariaId=formulario.EntidadBancariaId
                    }
            };
        }

        public async Task<ResponseDto> ObtenerDescripcionBancoComunal(string bancoComunalcodigo)
        {
            var result =
                await BancoComunalRepository.FirstOrDefault(f => f.EstadoFila && f.Codigo == bancoComunalcodigo);
            return new ResponseDto()
            {
                Data = result.Descripcion
            };
        }

        public async Task<ResponseDto> BuscarPreSolicitudPorBanco(FiltroPorBancoComunalModel model)
        {
            await ValidarAccesoBancoComunalPorSucursalAsignado(model.BancoComunalId);
            var result = await PreSolicitudRepository.ObtenerSinCabeceraPreSolicitud(model);
            var listaSocia = await SociaRepository.ObtenerPorBancoComunalYAnilloGrupal(model.BancoComunalId, model.AnilloGrupalId);
            var listaNivelRiesgo = await CatalogoDetalleRepository.ListarPorCatalogoCodigoParaDropDown(Constants.Core.Catalogo.NivelRiesgo);
            var listaTipoDeuda = await CatalogoDetalleRepository.ListarPorCatalogoCodigoParaDropDown(Constants.Core.Catalogo.TipoDeuda);
            if (listaSocia != null && listaSocia.Any())
            {
                if (!result.Any())
                    result = new List<ListaPreSolicitudPorBancoDto>();

                foreach (var v in listaSocia)
                {
                    var nivelRiesgo = SociaRepository.ObtenerTipoRiesgo(v.Dni)?.RiesgoMalla;
                    var capacidadPago = SociaRepository.ObtenerCapacidadPago(v.Dni);
                    var tipoDeuda = SociaRepository.ObtenerTipoDeuda(v.Dni);
                    if (!result.Where(x => x.EstadoCodigo == Constants.Core.Catalogo.PreSolicitudEstado.Registrado
                                        || x.EstadoCodigo == Constants.Core.Catalogo.PreSolicitudEstado.Ahorrista
                                        || x.EstadoCodigo == Constants.Core.Catalogo.PreSolicitudEstado.Retirada

                                      )
                                      .Select(s => s.SociaId).Distinct().ToList().Contains(v.Id))
                    {
                        result.Add(new ListaPreSolicitudPorBancoDto()
                        {
                            SociaId = v.Id,
                            SociaIdSistemaExterno = v.SociaId_SistemaExterno,
                            Nombre = $"{v.ApellidoPaterno} {v.ApellidoMaterno} {v.Nombre}",
                            BancoComunal = v.Formulario.BancoComunal,
                            AnilloGrupal = v.Formulario.AnilloGrupal,
                            FechaNacimiento = v.FechaNacimiento,
                            NivelRiesgoId = listaNivelRiesgo.FirstOrDefault(f => f.Code == nivelRiesgo)?.Id,
                            CapacidadPago = capacidadPago?.CapacidadPago,
                            Reingresante = v.Reingresante,
                            DatoIncompleto = v.DatoIncompleto,
                            TipoDeudaId = listaTipoDeuda.FirstOrDefault(f=>f.Code==tipoDeuda?.TipoDeuda)?.Id
                        });
                    }
                    else
                    {
                        var data = result.FirstOrDefault(f => f.SociaId == v.Id);
                        data.NivelRiesgoId = listaNivelRiesgo.FirstOrDefault(f => f.Code == nivelRiesgo)?.Id;
                        data.CapacidadPago = capacidadPago?.CapacidadPago;
                        data.TipoDeudaId = listaTipoDeuda.FirstOrDefault(f => f.Code == tipoDeuda?.TipoDeuda)?.Id;
                    }
                }
            }

            var preSolicitudRetirados = new List<ListaPreSolicitudPorBancoDto>();
            preSolicitudRetirados = await PreSolicitudRepository.ObtenerPresolicitudMotivoRetiro(model.BancoComunalId, model.AnilloGrupalId);

            var listaSinPreSolicitud = result
                .Where(p => p.Id == null).ToList();
            var listaRegistrados = result
                .Where(p => p.Id != null && p.EstadoCodigo == Constants.Core.Catalogo.PreSolicitudEstado.Registrado)
                .OrderBy(o => o.SociaIdSistemaExterno).ToList();
            var listaAhorrista = result
                .Where(p => p.Id != null && p.EstadoCodigo == Constants.Core.Catalogo.PreSolicitudEstado.Ahorrista)
                .OrderBy(o => o.SociaIdSistemaExterno).ToList();
            var listaRetiradas = result
                .Where(p => p.Id != null && p.EstadoCodigo == Constants.Core.Catalogo.PreSolicitudEstado.Retirada)
                .OrderBy(o => o.SociaIdSistemaExterno).ToList();
            var listaRechazada = result
                .Where(p => p.Id != null && p.EstadoCodigo == Constants.Core.Catalogo.PreSolicitudEstado.Rechazada)
                .OrderBy(o => o.SociaIdSistemaExterno).ToList();

            var lista3 = listaRegistrados.Concat(listaSinPreSolicitud).Concat(listaAhorrista)
                .Concat(listaRetiradas).Concat(preSolicitudRetirados).Concat(listaRechazada).OrderBy(o => o.SociaIdSistemaExterno);

            return new ResponseDto()
            {
                Data = lista3
            };
        }

        public async Task<ResponseDto> RechazarPresolicitud(int id)
        {
            var listaEstado =
                await CatalogoDetalleRepository.ListarPorCatalogoCodigoParaDropDown(Constants.Core.Catalogo.EstadoPreSolicitud);
            var estadoRechazadaId = listaEstado.First(f => f.Code == Constants.Core.Catalogo
                .PreSolicitudEstado.Rechazada).Id;

            var result = await PreSolicitudRepository.FirstOrDefault(f => f.EstadoFila && f.Id == id);

            if (result == null)
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                    "La solicitud ya fue rechazada ó no existe.");

            var listaMotivo = await CatalogoDetalleRepository
                .ListarPorCatalogoCodigoParaDropDown(Constants.Core.Catalogo.MotivoRetiro);

            var listaMotivoRetiro = listaMotivo.Where(p => new List<string>()
            {
               Constants.Core.Catalogo.DetMotivoRetiro.AnilloBanco,
               Constants.Core.Catalogo.DetMotivoRetiro.BancoAnillo,
               Constants.Core.Catalogo.DetMotivoRetiro.BancoaBanco,
               Constants.Core.Catalogo.DetMotivoRetiro.AnilloaAnillo
            }.Contains(p.Code));

            var motivoCodigo = result.MotivoRetiroId.HasValue ? listaMotivo.FirstOrDefault(f => f.Id == result.MotivoRetiroId).Code : null;

            await UnitOfWork.BeginTransaction();
            if (
                listaMotivoRetiro.Any(a => a.Code == motivoCodigo)
                //motivoCodigo == Constants.Core.Catalogo.DetMotivoRetiro.AnilloBanco
                //|| motivoCodigo == Constants.Core.Catalogo.DetMotivoRetiro.BancoAnillo
                //|| motivoCodigo == Constants.Core.Catalogo.DetMotivoRetiro.BancoaBanco
                //|| motivoCodigo == Constants.Core.Catalogo.DetMotivoRetiro.AnilloaAnillo
                )
            {
                var formularioEntity = await FormularioRepository.FirstOrDefault(f => f.EstadoFila
                    && f.SociaId == result.SociaId
                );

                var existePresolicitudDeLaRetirada = await PreSolicitudRepository.FirstOrDefault(f => f.EstadoFila &&
                f.PreSolicitudCabeceraId == null &&
                f.SociaId == result.SociaId &&
                f.Socia.Formulario.Any(a => a.BancoComunalId == formularioEntity.BancoComunalId
                                            && a.AnilloGrupalId == formularioEntity.AnilloGrupalId)
                && !listaMotivoRetiro.Select(s => s.Id).ToList().Contains(f.MotivoRetiroId ?? 0)
                && f.EstadoId != estadoRechazadaId
                );

                if (existePresolicitudDeLaRetirada != null)
                {
                    var bancoComunalyAnilloGrupal = await BancoComunalRepository.ObtenerBancoyAnillo(result.BancoComunalRetiradoId ?? 0, result.AnilloGrupalRetiroId);
                    var msg = !string.IsNullOrEmpty(bancoComunalyAnilloGrupal.AnilloGrupal) ?
                        $"Socia cuenta con una presolicitud en {bancoComunalyAnilloGrupal.AnilloGrupal}." :
                        $"Socia cuenta con una presolicitud en {bancoComunalyAnilloGrupal.BancoComunal}.";
                    throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, msg);
                }

                var sociaEntity = await SociaRepository.FirstOrDefault(f => f.EstadoFila && f.Id == result.SociaId);
                sociaEntity.BancoComunalId = result.BancoComunalRetiradoId;
                sociaEntity.SociaId_SistemaExterno = result.SistemaExternoSociaPorRetiro;

                UnitOfWork.Set<SociaEntity>().Update(sociaEntity);

                formularioEntity.BancoComunalId = result.BancoComunalRetiradoId ?? 0;
                formularioEntity.AnilloGrupalId = result.AnilloGrupalRetiroId;

                UnitOfWork.Set<FormularioEntity>().Update(formularioEntity);
            }

            result.EstadoId = estadoRechazadaId;
            UnitOfWork.Set<PreSolicitudEntity>().Update(result);
            //await UnitOfWork.SaveChangesAsync();
            await UnitOfWork.SaveChangeTransaction();
            return new ResponseDto() { Message = CommonResource.rechazo_ok };
        }

        public async Task<ResponseDto> ObtenerPreSolicitudRegistrado(int id)
        {
            var result = await PreSolicitudRepository.ObtenerPorIdYEstado(id, Constants.Core.Catalogo.PreSolicitudEstado.Registrado);
            if (result == null)
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                    "Pre Solicitud no esta registrada");
            return new ResponseDto()
            {
                Data = result
            };
        }

        public async Task<ResponseDto> ActualizarPreSolicitud(ActualizarPreSolicitudModel model)
        {
            var listaEstado =
                await CatalogoDetalleRepository.ListarPorCatalogoCodigoParaDropDown(Constants.Core.Catalogo.EstadoPreSolicitud);
            var estadoRegistradoId = listaEstado.First(f => f.Code == Constants.Core.Catalogo
                .PreSolicitudEstado.Registrado).Id;
            var listaNivelRiesgo = await CatalogoDetalleRepository.ListarPorCatalogoCodigoParaDropDown(Constants.Core.Catalogo.NivelRiesgo);
            var result = await PreSolicitudRepository.FirstOrDefault(f => f.EstadoFila && f.Id == model.Id
                && f.EstadoId == estadoRegistradoId
            );

            if (result == null)
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                    "La solicitud está rechazada ó no existe.");

            var listaCatalogo = await CatalogoDetalleRepository.ObtenerPorListaCodigoCatalogo(new List<string>
            {
                Constants.Core.Catalogo.TipoCredito,
            });
            var obtenerTipoCredito = listaCatalogo.Where(p => p.Code == Constants.Core.Catalogo.TipoCredito)
                    .ToList().Where(d => d.DetailId == model.TipoCreditoId).FirstOrDefault();

            var sociaEntity = await SociaRepository.ObtenerPorId(result.SociaId);

            decimal? capacidadPago; int? nivelRiesgoId;
            if (!sociaEntity.SociaId_SistemaExterno.HasValue || sociaEntity.SociaId_SistemaExterno == 0)
            {
                capacidadPago = model.CapacidadPago;
                nivelRiesgoId = model.NivelRiesgoId;
            }
            else
            {
                capacidadPago = SociaRepository.ObtenerCapacidadPago(sociaEntity.NroDni)?.CapacidadPago;
                var nivelRiesgo = SociaRepository.ObtenerTipoRiesgo(sociaEntity.NroDni)?.RiesgoMalla;
                nivelRiesgoId = listaNivelRiesgo.FirstOrDefault(f => f.Code == nivelRiesgo)?.Id;
            }
            result.TipoCreditoId = model.TipoCreditoId;
            result.Monto = model.Monto;
            result.TasaInteresId = model.TasaInteresId;
            result.Plazo = model.Plazo;
            result.BancoDesembolsoId = model.EntidadBancariaId;
            result.NroCuenta = model.NroCuenta;
            result.AsistenciaId = obtenerTipoCredito.DetailCode == Constants.Core.Catalogo.DetTipoCredito.CtaParalela ? null : model.AsistenciaId;
            //result.NivelRiesgoId = obtenerTipoCredito.DetailCode == Constants.Core.Catalogo.DetTipoCredito.CtaParalela ? null : model.NivelRiesgoId;
            result.NivelRiesgoId = nivelRiesgoId;
            result.PlazoGracia = model.PlazoGracia;
            result.SubTipoCreditoId = model.SubTipoCreditoId;
            result.MSV = obtenerTipoCredito.DetailCode == Constants.Core.Catalogo.DetTipoCredito.CtaParalela ? false :
                        await RegistrarMsvDependiendoEdad(result.SociaId) ? model.Msv : false;
            result.CobraMedianteDj = model.CobraMedianteDj;
            result.SociaDjId = model.SociaDjId;
            result.CapacidadPago = capacidadPago;

            UnitOfWork.Set<PreSolicitudEntity>().Update(result);
            await UnitOfWork.SaveChangesAsync();

            if (result.PreSolicitudCabeceraId.HasValue)
                await RechazarPresolicitudCabecera(result.PreSolicitudCabeceraId, listaEstado);

            return new ResponseDto() { Message = CommonResource.update_ok };
        }

        public async Task<ResponseDto> CrearPreSolicitud(CrearPreSolicitudModel model)
        {
            if (model.CapacidadPago.HasValue && model.CapacidadPago > Constants.Core.Catalogo.MontoMaximoCapacidadPago)
            {
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                       $"La capaciada de pago como máximo es {Constants.Core.Catalogo.MontoMaximoCapacidadPago}");
            }
            if (model.Monto < Constants.Core.Catalogo.MontoMinimoPresolicitud
                && !model.Ahorrista && !model.Retirada)
            {
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                       "Monto Mínimo es de S/100 soles.");
            }
            
            
            var formulario = await FormularioRepository.ObtenerPorCodigoPorSocia(model.SociaId);
            if (formulario?.BancoComunalId == null)
            {
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                       "Socia no cuenta con un formulario o no tiene registrado el banco comunal.");
            }
            await ValidarAccesoBancoComunalPorSucursalAsignado(formulario.BancoComunalId);
            var listaCatalogo = await CatalogoDetalleRepository.ObtenerPorListaCodigoCatalogo(new List<string>
            {
                Constants.Core.Catalogo.EstadoPreSolicitud,
                Constants.Core.Catalogo.TipoCredito,
                Constants.Core.Catalogo.SubTipoCredito,
                Constants.Core.Catalogo.MotivoRetiro,
                Constants.Core.Catalogo.Estado,
                Constants.Core.Catalogo.TipoDispositivo,
                Constants.Core.Catalogo.TipoDeuda
            });

            var obtenerSubTipoCreditoMiCelularId = listaCatalogo
                .Where(p => p.Code == SubTipoCredito).ToList().FirstOrDefault(d => d.DetailCode == DetSubTipoCredito.CreditoMicelular)!.DetailId;

            if (obtenerSubTipoCreditoMiCelularId == model.SubTipoCreditoId)
            {
                if (model.Plazo is > 15 or < 1 && !model.Ahorrista && !model.Retirada)
                {
                    throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                        "El rango de plazo es del 1 hasta 15.");
                }
            }
            else
            {
                if ((model.Plazo > 12 || model.Plazo < 1) && !model.Ahorrista && !model.Retirada)
                {
                    throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                        "El rango de plazo es del 1 hasta 12.");
                }
            }

            if (model.SubTipoCreditoId != null && !model.Ahorrista && !model.Retirada &&
                listaCatalogo.FirstOrDefault(f => f.DetailId == model.SubTipoCreditoId).DetailCode == Constants.Core.Catalogo.DetSubTipoCredito.CreditoCampaña
                && model.Plazo > 3
               )
            {
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                          "CUENTA PARALELO Y CRÉDITO CAMPAÑA TIENE PLAZO MÁX. DE 3.");
            }

            var listaPreSolicitud = await PreSolicitudRepository.ListaPreSolicitudPorSocia(model.SociaId);
            listaPreSolicitud = listaPreSolicitud.Where(p => p.EstadoCodigo != Constants.Core.Catalogo.PreSolicitudEstado.Rechazada).ToList();

            var listaPreSolicitudSinCabecera = listaPreSolicitud.Where(p => !p.PreSolicitudCabecera.HasValue).ToList();

            var cantPreSolicitud = listaPreSolicitudSinCabecera.Count(p => p.EstadoCodigo == Constants.Core.Catalogo.PreSolicitudEstado.Registrado);

            if (!model.Ahorrista && !model.Retirada)
            {
                if (cantPreSolicitud >= 3)
                {
                    throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                        "Socia puede registrar 3 PreSolicitudes.");
                }
                else if (listaPreSolicitudSinCabecera.Any(p =>
                             p.EstadoCodigo == Constants.Core.Catalogo.PreSolicitudEstado.Ahorrista))
                {
                    throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                        "Socia no debe ser ahorrista para registrar una nueva solicitud.");
                }
            }
            else
            {
                if (model.Ahorrista)
                    if (listaPreSolicitudSinCabecera.Any(p => p.EstadoCodigo == Constants.Core.Catalogo.PreSolicitudEstado.Registrado
                    || p.EstadoCodigo == Constants.Core.Catalogo.PreSolicitudEstado.Retirada
                    || p.EstadoCodigo == Constants.Core.Catalogo.PreSolicitudEstado.Ahorrista
                    ))
                    {
                        throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                            "Para asignar ahorrista a una socia, debe rechazar todas sus presolicitudes y no debe ser retirada.");
                    }
                if (model.Retirada)
                    if (listaPreSolicitudSinCabecera.Any(p => p.EstadoCodigo == Constants.Core.Catalogo.PreSolicitudEstado.Registrado
                    || p.EstadoCodigo == Constants.Core.Catalogo.PreSolicitudEstado.Retirada
                    || p.EstadoCodigo == Constants.Core.Catalogo.PreSolicitudEstado.Ahorrista
                    ))
                    {
                        throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                            "Para retirar a una socia, debe rechazar todas sus presolicitudes y no debe ser ahorrista.");
                    }
                if (string.IsNullOrEmpty(model.MotivoCodigo) && model.Retirada)
                {
                    throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                           "Debe de seleccionar un motivo de retiro.");
                }
            }

            PreSolicitudEntity entity;
            int? anilloGrupalRetirado = null;
            int? bancoComunalRetirado = null;
            int? sistemaExternoSociaRetirado = null;
            int mesActual = DateTime.Now.Month;
            try
            {
                await UnitOfWork.BeginTransaction();
                if (!model.Ahorrista && !model.Retirada)
                {
                    var cuentaExterma = listaCatalogo.Where(p => p.Code == Constants.Core.Catalogo.TipoCredito)
                        .ToList().Where(d => d.DetailCode == Constants.Core.Catalogo.DetTipoCredito.CtaExterna)
                        .FirstOrDefault();

                    var obtenerTipoCredito = listaCatalogo.Where(p => p.Code == Constants.Core.Catalogo.TipoCredito)
                       .ToList().Where(d => d.DetailId == model.TipoCreditoId).FirstOrDefault();

                    var obtenersubTipoCredito = listaCatalogo.Where(p => p.Code == Constants.Core.Catalogo.SubTipoCredito)
                        .ToList().Where(d => d.DetailId == model.SubTipoCreditoId).FirstOrDefault();

                    if (listaPreSolicitud.Any(a =>
                            a.TipoCreditoCodigo == Constants.Core.Catalogo.DetTipoCredito.CtaExterna))
                    {
                        switch (obtenersubTipoCredito.DetailCode)
                        {
                            case Constants.Core.Catalogo.DetSubTipoCredito.BancoComunal:
                                if (listaPreSolicitud.Any(a => a.FechaCreacion.Month == mesActual &&
                                                            a.SubTipoCreditoCodigo == Constants.Core.Catalogo.DetSubTipoCredito.BancoComunal))
                                {
                                    throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                                       "Socia puede tener una presolicitud tipo cta. externa - banco comunal por mes.");
                                }
                                break;

                            case Constants.Core.Catalogo.DetSubTipoCredito.AnilloGrupal:
                                if (listaPreSolicitud.Any(a => a.FechaCreacion.Month == mesActual &&
                                                            a.SubTipoCreditoCodigo == Constants.Core.Catalogo.DetSubTipoCredito.AnilloGrupal))
                                {
                                    throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                                       "Socia puede tener una presolicitud tipo cta. externa - anillo grupal por mes.");
                                }
                                break;

                            case Constants.Core.Catalogo.DetSubTipoCredito.ExternoPromocional:
                                if (listaPreSolicitud.Any(a => a.FechaCreacion.Month == mesActual &&
                                                             a.SubTipoCreditoCodigo == Constants.Core.Catalogo.DetSubTipoCredito.ExternoPromocional))
                                {
                                    throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                                       "Socia puede tener una presolicitud tipo cta. externa - externo promocional por mes.");
                                }
                                break;
                        }
                        //if (model.TipoCreditoId == cuentaExterma.DetailId)
                        //{
                        //    throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                        //        "Socia ya cuenta con una PreSolicitud tipo crédito 'Cuenta Externa'.");
                        //}
                    }

                    if (listaPreSolicitudSinCabecera.Any(p => p.TipoCreditoCodigo == obtenerTipoCredito.DetailCode
                                                   && p.SubTipoCreditoCodigo == obtenersubTipoCredito.DetailCode && p.SociaId == model.SociaId)
                       )
                        throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                            "Socia ya cuenta con una PreSolicitud con el mismo tipo de crédito.");

                    var estadoRegistradoId = listaCatalogo.Where(f => f.Code == Constants.Core.Catalogo.EstadoPreSolicitud)
                        .ToList().Where(x => x.DetailCode == Constants.Core.Catalogo.PreSolicitudEstado.Registrado)
                        .FirstOrDefault().DetailId;

                    decimal? capacidadPago; int? nivelRiesgoId;
                    int? tipoDeudaId;
                    var socia = await SociaRepository.ObtenerPorId(model.SociaId);
                    var listaNivelRiesgo = await CatalogoDetalleRepository.ListarPorCatalogoCodigoParaDropDown(Constants.Core.Catalogo.NivelRiesgo);
                    var listaTipoDeuda = listaCatalogo.Where(p => p.Code == Constants.Core.Catalogo.TipoDeuda).ToList();
                    if (socia.SociaId_SistemaExterno is null or 0 || (socia.Reingresante!=null && socia.Reingresante.Value))
                    {
                        capacidadPago = model.CapacidadPago;
                        nivelRiesgoId = model.NivelRiesgoId;
                        tipoDeudaId = model.TipoDeudaId;
                    }
                    else
                    {
                        capacidadPago = SociaRepository.ObtenerCapacidadPago(socia.NroDni)?.CapacidadPago;
                        var nivelRiesgo = SociaRepository.ObtenerTipoRiesgo(socia.NroDni)?.RiesgoMalla;
                        nivelRiesgoId = listaNivelRiesgo.FirstOrDefault(f => f.Code == nivelRiesgo)?.Id;
                        tipoDeudaId = listaTipoDeuda.FirstOrDefault(f=>f.DetailCode== SociaRepository.ObtenerTipoDeuda(socia.NroDni)?.TipoDeuda).DetailId;
                    }
                    entity = new PreSolicitudEntity()
                    {
                        Nro = cantPreSolicitud + 1,
                        SociaId = model.SociaId,
                        BancoDesembolsoId = model.EntidadBancariaId,
                        Plazo = model.Plazo,
                        PlazoGracia = model.PlazoGracia,
                        Monto = model.Monto,
                        TipoCreditoId = model.TipoCreditoId,
                        SubTipoCreditoId = model.SubTipoCreditoId,
                        NroCuenta = model.NroCuenta,
                        EstadoId = estadoRegistradoId,
                        AsistenciaId = obtenerTipoCredito.DetailCode == Constants.Core.Catalogo.DetTipoCredito.CtaParalela ? null : model.AsistenciaId,
                        //NivelRiesgoId = obtenerTipoCredito.DetailCode == Constants.Core.Catalogo.DetTipoCredito.CtaParalela ? null : model.NivelRiesgoId,
                        NivelRiesgoId = nivelRiesgoId,
                        TasaInteresId = model.TasaInteresId,
                        MSV = obtenerTipoCredito.DetailCode == Constants.Core.Catalogo.DetTipoCredito.CtaParalela ? false :
                                                               await RegistrarMsvDependiendoEdad(model.SociaId) ? model.Msv : false,
                        CobraMedianteDj = model.CobraMedianteDj,
                        SociaDjId = model.SociaDjId,
                        CapacidadPago = capacidadPago,
                        TipoDeudaId = tipoDeudaId
                    };
                }
                else if (model.Ahorrista)
                {
                    entity = RegistrarPreSocilicitudAhorrista(listaCatalogo, model.SociaId, 0);
                }
                else if (model.Retirada)
                {
                    string comentario = string.Empty;
                    if (model.MotivoCodigo == Constants.Core.Catalogo.DetMotivoRetiro.AnilloBanco)
                    {
                        bancoComunalRetirado = formulario.BancoComunalId;
                        anilloGrupalRetirado = formulario.AnilloGrupalId;
                        var formularioEntity = await ActualizarFormularioAnilloABanco(model.SociaId);
                        UnitOfWork.Set<FormularioEntity>().Update(formularioEntity);
                    }
                    else if (model.MotivoCodigo == Constants.Core.Catalogo.DetMotivoRetiro.BancoAnillo
                        || model.MotivoCodigo == Constants.Core.Catalogo.DetMotivoRetiro.AnilloaAnillo)
                    {
                        bancoComunalRetirado = formulario.BancoComunalId;
                        anilloGrupalRetirado = formulario.AnilloGrupalId;
                        if (model.NuevoAnilloGrupal)
                        {
                            var anilloGrupalEntity = await CrearAnilloGrupal(formulario.BancoComunalId ?? 0, listaCatalogo);
                            await UnitOfWork.Set<AnilloGrupalEntity>().AddAsync(anilloGrupalEntity);
                            await UnitOfWork.SaveChangesAsync();

                            formulario.AnilloGrupalId = anilloGrupalEntity.Id;
                            UnitOfWork.Set<FormularioEntity>().Update(formulario);
                        }
                        else
                        {
                            var formularioEntity = await ActualizarFormularioBancoAAnillo(model.SociaId, model.AnilloGrupalId);
                            UnitOfWork.Set<FormularioEntity>().Update(formularioEntity);
                        }
                    }
                    else if (model.MotivoCodigo == Constants.Core.Catalogo.DetMotivoRetiro.BancoaBanco)
                    {
                        bancoComunalRetirado = formulario.BancoComunalId;
                        anilloGrupalRetirado = formulario.AnilloGrupalId;
                        var sociaEntity = await ActualizarSociaBanco(model.SociaId, model.BancoComunalId);

                        sistemaExternoSociaRetirado = sociaEntity.SociaId_SistemaExterno;

                        sociaEntity.SociaId_SistemaExterno = null;//socia ya no debe contar con el id sistema externo.
                        UnitOfWork.Set<SociaEntity>().Update(sociaEntity);

                        formulario.BancoComunalId = model.BancoComunalId ?? 0;
                        formulario.AnilloGrupalId = null;

                        UnitOfWork.Set<FormularioEntity>().Update(formulario);
                    }

                    entity = RegistrarPreSocilicitudPorRetiro(listaCatalogo, model, 0, bancoComunalRetirado, anilloGrupalRetirado, sistemaExternoSociaRetirado);
                }
                else
                {
                    entity = null;
                }

                var sistemaOrigen = await CatalogoDetalleRepository.ObtenerPorCodigoConEstadoActivo(Constants.Core.Catalogo.SistemaOrigenOficiales);
                var tipoDispositivo = listaCatalogo.Where(p => p.Code == Constants.Core.Catalogo.TipoDispositivo)
                        .ToList().Where(d => d.DetailCode == model.TipoDispositivoCodigo)
                        .FirstOrDefault();

                entity.SistemaOrigenId = sistemaOrigen.Id;
                entity.TipoDispositivoId = tipoDispositivo?.DetailId;

                await UnitOfWork.Set<PreSolicitudEntity>().AddAsync(entity);
                await UnitOfWork.SaveChangeTransaction();
            }
            catch (FunctionalException ex)
            {
                await UnitOfWork.Rollback();
                throw new FunctionalException(ex.Message);
            }
            catch (Exception ex)
            {
                await UnitOfWork.Rollback();
                throw new TechnicalException(ex.Message);
            }

            return new ResponseDto()
            {
                Message = CommonResource.register_ok
            };
        }

        public async Task<ResponseDto> ObtenerDetallePreSolicitud(int id)
        {
            var listaEstado =
                await CatalogoDetalleRepository.ObtenerPorListaCodigoCatalogo(
                   new List<string>() {
                       Constants.Core.Catalogo.EstadoPreSolicitud,
                       Constants.Core.Catalogo.EstadoPreSolicitudCabecera
                       })
                ;

            var estadoRegistradoPreSolicitud = listaEstado
                .Where(p => p.DetailCode == Constants.Core.Catalogo.PreSolicitudEstado.Registrado
                            && p.Code == Constants.Core.Catalogo.EstadoPreSolicitud)
                .Select(s => s.DetailCode)
                .ToList();
            var estadoObservadoPreSolicitudCab = listaEstado
                .Where(p => p.DetailCode == Constants.Core.Catalogo.PreSolicitudCabeceraEstado.Observada
                            && p.Code == Constants.Core.Catalogo.EstadoPreSolicitudCabecera)
                .Select(s => s.DetailCode)
                .ToList();

            var result = await PreSolicitudRepository.ObtenerPorPreSolicitudCabecera
                (id, estadoRegistradoPreSolicitud, estadoObservadoPreSolicitudCab);

            foreach (var v in result)
            {
                var capacidadPago = SociaRepository.ObtenerCapacidadPago(v.Dni);
                v.CapacidadPago = capacidadPago?.CapacidadPago;
            }

            return new ResponseDto()
            {
                Data = result
            };
        }

        //PRE-SOLICITUD CABECERA
        public async Task<ResponseDto> CrearPreSolicitudCabecera(FiltroCrearPreSolicitudCabeceraModel model)
        {
            await ValidarAccesoBancoComunalPorSucursalAsignado(model.BancoComunalId);

            //if (string.IsNullOrEmpty(model.BancoComunalId))
            //    throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
            //        "Debe seleccionar banco comunal.");
            if (string.IsNullOrEmpty(model.TipoCodigo))
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                    "Debe seleccionar tipo.");
            var result =
                await PreSolicitudRepository.ObtenerPorBancoComunal(model.BancoComunalId,
                    model.AnilloGrupalId);

            //if ((result == null || !result.Any()) && model.TipoCodigo == Constants.Core.Catalogo.PreSolicitudCabeceraTipo.Paralelo)
            //    throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
            //        "Monto total como mínimo es de S/ 100 soles.");
            //else if (result.Any() && result.Sum(s => s.Monto) < Constants.Core.Catalogo.MontoMinimoPresolicitud)
            //    throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
            //        "Monto total como mínimo es de S/ 100 soles.");
            if (result == null || !result.Any())
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                    "Debe registrar presolicitudes.");

            if (model.TipoCodigo == Constants.Core.Catalogo.PreSolicitudCabeceraTipo.CuentaExterna)
            {
                var cantSociaPorBancoyAnillo = await FormularioRepository.TotalBancoComunalyAnillo(model.BancoComunalId, model.AnilloGrupalId);
                var cantSociaConPresolicitud = result.Where(p => p.BancoComunalRetiradoId == null
                                                            && p.AnilloGrupalRetiroId == null).Select(s => s.SociaId).Distinct().ToList().Count();

                if (cantSociaConPresolicitud > cantSociaPorBancoyAnillo || cantSociaConPresolicitud < cantSociaPorBancoyAnillo)
                {
                    throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                            "Todas las socias deben de tener una preSolicitud sin importar el estado(registrada, rechazada, retirada, ahorrista).");
                }
            }

            //ValidacionUnaPreSolicitudPorSocia(result);
            var fechaDesem = DateTime.ParseExact(model.FechaDesembolso, "dd/MM/yyyy", null);
            //var estadoPorAprobar =
            //    await CatalogoDetalleRepository.ObtenerPorCodigoConEstadoActivo(Constants.Core.Catalogo
            //        .PreSolicitudCabeceraEstado.PorAprobar);
            var listaCatalogo = await CatalogoDetalleRepository.ObtenerPorListCodigo(new List<string>()
            {
                Constants.Core.Catalogo.PreSolicitudCabeceraEstado.PorAprobar,
                model.TipoCodigo,
                Constants.Core.Catalogo.PreSolicitudEstado.Registrado
            });

            var estadoPresolicitudRegistrado =
                listaCatalogo.First(p => p.Code == Constants.Core.Catalogo.PreSolicitudEstado.Registrado).Id;

            try
            {
                await UnitOfWork.BeginTransaction();

                var entity = new PreSolicitudCabeceraEntity
                {
                    BancoComunalId = model.BancoComunalId,
                    AnilloGrupalId = model.AnilloGrupalId,
                    Monto = result.Where(p => p.EstadoId == estadoPresolicitudRegistrado).Select(se => se.Monto).DefaultIfEmpty(0).Sum(),
                    Plazo = result.Where(p => p.EstadoId == estadoPresolicitudRegistrado).Select(se => se.Plazo).DefaultIfEmpty(0).Max(),
                    EstadoId = listaCatalogo.First(f => f.Code == Constants.Core.Catalogo.PreSolicitudCabeceraEstado.PorAprobar).Id, //estadoPorAprobar.Id,
                    PreSolicitud = result,
                    FechaDesembolso = fechaDesem,
                    TipoId = listaCatalogo.First(f => f.Code == model.TipoCodigo).Id
                };
                await UnitOfWork.Set<PreSolicitudCabeceraEntity>().AddAsync(entity);
                await UnitOfWork.SaveChangeTransaction();
            }
            catch (FunctionalException ex)
            {
                await UnitOfWork.Rollback();
                throw new FunctionalException(ex.Message);
            }
            catch (Exception ex)
            {
                await UnitOfWork.Rollback();
                throw new TechnicalException(ex.Message);
            }

            return new ResponseDto() { Message = CommonResource.register_ok };
        }

        public async Task<ResponseDto> ListaEstadoPreSolicitud(FiltroCrearPreSolicitudCabeceraModel model)
        {
            var result = await PreSolicitudCabeceraRepository.ObtenerPorBancoComunalYEstado(model.BancoComunalId,
                model.AnilloGrupalId);
            var listaTipoReporte = new List<DropdownDto>();
            if (result != null && result.Any())
            {
                foreach (var v in result)
                {
                    v.DetalleTipoCredito = v.DetalleTipoCredito.Where(p => !string.IsNullOrEmpty(p)).Distinct().ToList();

                    if (v.DetalleTipoCredito.Any(a => a == Constants.Core.Catalogo.DetSubTipoCredito.BancoComunal))
                    {
                        listaTipoReporte.Add(new DropdownDto()
                        {
                            Code = Constants.Core.Catalogo.DetTipoReporte.CtaExternaMasPromocional[0],
                            Description = Constants.Core.Catalogo.DetTipoReporte.CtaExternaMasPromocional[1]
                        });
                    }
                    if (v.DetalleTipoCredito.Any(a => a != Constants.Core.Catalogo.DetSubTipoCredito.CreditoAnilloCampaña
                        && a != Constants.Core.Catalogo.DetSubTipoCredito.CreditoAnilloComplementario
                        && a != Constants.Core.Catalogo.DetSubTipoCredito.BancoComunal
                        && a != Constants.Core.Catalogo.DetSubTipoCredito.AnilloGrupal
                        && !string.IsNullOrEmpty(a)
                    ))
                    {
                        if (v.DetalleTipoCredito.Any(a => a == Constants.Core.Catalogo.DetSubTipoCredito.ExternoPromocional))
                        {
                            listaTipoReporte.Add(new DropdownDto()
                            {
                                Code = Constants.Core.Catalogo.DetTipoReporte.CreditoParaleloMasPromocional[0],
                                Description = Constants.Core.Catalogo.DetTipoReporte.CreditoParaleloMasPromocional[1]
                            });
                        }
                        else
                        {
                            listaTipoReporte.Add(new DropdownDto()
                            {
                                Code = Constants.Core.Catalogo.DetTipoReporte.CreditoParalelo[0],
                                Description = Constants.Core.Catalogo.DetTipoReporte.CreditoParalelo[1]
                            });
                        }
                    }
                    if (v.DetalleTipoCredito.Any(a => a == Constants.Core.Catalogo.DetSubTipoCredito.AnilloGrupal))
                    {
                        listaTipoReporte.Add(new DropdownDto()
                        {
                            Code = Constants.Core.Catalogo.DetTipoReporte.AnilloGrupal[0],
                            Description = Constants.Core.Catalogo.DetTipoReporte.AnilloGrupal[1]
                        });
                    }
                    if (v.DetalleTipoCredito.Any(a => a == Constants.Core.Catalogo.DetSubTipoCredito.CreditoAnilloCampaña
                     || a == Constants.Core.Catalogo.DetSubTipoCredito.CreditoAnilloComplementario
                    ))
                    {
                        listaTipoReporte.Add(new DropdownDto()
                        {
                            Code = Constants.Core.Catalogo.DetTipoReporte.CreditoParaleloAnilloGrupal[0],
                            Description = Constants.Core.Catalogo.DetTipoReporte.CreditoParaleloAnilloGrupal[1]
                        });
                    }

                    //var s = listaTipoReporte.Distinct().ToList();
                    //var f = listaTipoReporte
                    //    .Select(o => new DropdownDto { Code = o.Code, Description = o.Description, Id = o.Id, Value = o.Value }).Distinct().ToList();
                    //var z = listaTipoReporte.GroupBy(g => g.Code).Select(z => z.First()).ToList();
                    //var y = listaTipoReporte.GroupBy(g => g.Code)
                    //    .Select(o => o.Key).ToList();
                    v.TipoReporte = listaTipoReporte.GroupBy(g => g.Code).Select(z => z.First()).ToList();
                    v.AbreviaturaTipoCredito = v.AbreviaturaTipoCredito?.Where(p => !string.IsNullOrEmpty(p)).Distinct().ToList();
                    listaTipoReporte.Clear();
                }
            }
            return new ResponseDto()
            {
                Data = result
            };
        }

        public async Task<ResponseDto> ActualizarEstadoPreSolicitudCab(ActualizarEstadoPreSolicitudCabModel model)
        {
            var listaEstado =
                    await CatalogoDetalleRepository.ObtenerPorListaCodigoCatalogo(
                        new List<string>() {
                            Constants.Core.Catalogo.EstadoPreSolicitud,
                            Constants.Core.Catalogo.EstadoPreSolicitudCabecera
                        })
                ;

            var estadoId = listaEstado.First(p => p.Code == Constants.Core.Catalogo.EstadoPreSolicitudCabecera
                                                            && p.DetailCode == model.EstadoCodigo
            ).DetailId;

            var estadoRegistradoId = listaEstado.First(p => p.Code == Constants.Core.Catalogo.EstadoPreSolicitud
                                                            && p.DetailCode == Constants.Core.Catalogo
                                                                .PreSolicitudEstado.Registrado
            ).DetailId;
            var estadoRetiradoId = listaEstado.First(p => p.Code == Constants.Core.Catalogo.EstadoPreSolicitud
                                                            && p.DetailCode == Constants.Core.Catalogo
                                                                .PreSolicitudEstado.Retirada
            ).DetailId;

            var entity = await PreSolicitudCabeceraRepository.ObtenerConDetalle(model.Id, "");
            if (entity == null)
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                    "Pre Solicitud no está registrada.");
            if (model.EstadoCodigo == Constants.Core.Catalogo.PreSolicitudCabeceraEstado.PorAprobar)
            {
                if (entity.EstadoId != listaEstado.First(p =>
                        p.Code == Constants.Core.Catalogo.EstadoPreSolicitudCabecera
                        && p.DetailCode == Constants.Core.Catalogo.PreSolicitudCabeceraEstado.Observada
                    ).DetailId)
                {
                    throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                        "Para asignar el Estado POR APROBAR esta solicitud debe estar observada.");
                }
            }
            else
            {
                if (entity.EstadoId != listaEstado.First(p =>
                        p.Code == Constants.Core.Catalogo.EstadoPreSolicitudCabecera
                        && p.DetailCode == Constants.Core.Catalogo.PreSolicitudCabeceraEstado.PorAprobar
                    ).DetailId)
                {
                    throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                        "Para actualizar el estado, esta solicitud debe estar en estado POR APROBAR.");
                }
            }
            try
            {
                var listaPreSolicitudRetirados = entity.PreSolicitud.Where(p => p.EstadoId == estadoRetiradoId).ToList();

                if (listaPreSolicitudRetirados.Any() && model.EstadoCodigo == Constants.Core.Catalogo.PreSolicitudCabeceraEstado.Aprobada)
                {
                    var listaSociaDarBaja = await DarBajaSocia(listaPreSolicitudRetirados);
                    UnitOfWork.Set<SociaEntity>().UpdateRange(listaSociaDarBaja);
                }

                //entity.PreSolicitud = new List<PreSolicitudEntity>();
                entity.EstadoId = estadoId;
                entity.Monto = entity.PreSolicitud.Where(p=> p.EstadoId == estadoRegistradoId).Sum(s => s.Monto);
                
                entity.Plazo = entity.PreSolicitud.Any(p => p.EstadoId == estadoRegistradoId)
                        ? entity.PreSolicitud.Where(p => p.EstadoId == estadoRegistradoId).Max(m => m.Plazo)
                                : 0
                                ;
                entity.Observacion = model.Observacion;

                UnitOfWork.Set<PreSolicitudCabeceraEntity>().Update(entity);
                await UnitOfWork.SaveChangesAsync();
            }
            catch (FunctionalException ex)
            {
                await UnitOfWork.Rollback();
                throw new FunctionalException(ex.Message);
            }
            catch (Exception ex)
            {
                await UnitOfWork.Rollback();
                throw new TechnicalException(ex.Message);
            }

            return new ResponseDto()
            {
                Message = CommonResource.update_ok
            };
        }

        public async Task<ResponseDto> ActualizarEstadoPorAprobarPreSolicitudCab(ActualizarEstadoPreSolicitudCabModel model)
        {
            var estadoPorAprobar = await CatalogoDetalleRepository.ObtenerPorCodigoConEstadoActivo(Constants.Core.Catalogo.PreSolicitudCabeceraEstado.PorAprobar);
            var preSolicitudCab = await PreSolicitudCabeceraRepository.FirstOrDefault(f => f.EstadoFila && f.Id == model.Id);

            if (preSolicitudCab == null)
            {
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                    "Pre Solicitud no está registrada.");
            }

            preSolicitudCab.EstadoId = estadoPorAprobar.Id;
            UnitOfWork.Set<PreSolicitudCabeceraEntity>().Update(preSolicitudCab);
            await UnitOfWork.SaveChangesAsync();

            return new ResponseDto()
            {
                Message = CommonResource.update_ok
            };
        }

        public async Task<ResponseDto> MaximoCorrelativoAnillo(int bancoComunalId)
        {
            var correlativo = 0;
            var result = await AnilloGrupalRepository.GetWhere(p => p.EstadoFila && p.BancoComunalId == bancoComunalId);
            correlativo = !result.Any() ? 1 : result.Max(m => m.Correlativo) + 1;
            return new ResponseDto()
            {
                Data = correlativo
            };
        }

        #region method private

        private async Task ValidarAccesoBancoComunalPorSucursalAsignado(int? bancoComunalId)
        {
            //if (string.IsNullOrEmpty(bancoComunalCodigo))
            //    throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, "Debe de seleccionar un banco comunal.");
            var data = UserIdentity.FindFirst(Constants.Core.UserAsociadoClaims.Sucursal);
            if (data == null)
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, "Usuario debe estar asignado a una sucursal.");

            var listaSucursal = data.Value.Split(",").ToList();

            var sucursalBancoComunal =
                await BancoComunalRepository.ObtenerSucursalPorCodigo(bancoComunalId);

            if (string.IsNullOrEmpty(sucursalBancoComunal))
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, "Banco Comunal debe estar asignado a una sucursal.");
            if (!listaSucursal.Any(a => a == sucursalBancoComunal))
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, "Usuario no tiene acceso al banco comunal, porque no está asignado a la sucursal del banco.");
        }

        private void ValidacionUnaPreSolicitudPorSocia(List<PreSolicitudEntity> lista)
        {
            var existeDuplicado = lista
                .GroupBy(x => new { x.SociaId })
                .Any(x => x.Skip<PreSolicitudEntity>(1).Any());
            if (existeDuplicado)
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                    "Cada Socia debe tener una PreSolicitud.");
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

        private async Task RechazarPresolicitudCabecera(int? id, List<DropdownDto> listaEstado)
        {
            if (id.HasValue)
            {
                var lista = listaEstado.Where(p => p.Code != Constants.Core.Catalogo.PreSolicitudEstado.Rechazada)
                    .Select(s => s.Id).ToList();
                var estadoRechazadaId = listaEstado.FirstOrDefault(p => p.Code == Constants.Core.Catalogo.PreSolicitudEstado.Rechazada).Id;

                var cantidadPresolicitud = await PreSolicitudRepository.CountWhere(c => c.PreSolicitudCabeceraId == id
                     && c.EstadoFila && lista.Contains(c.EstadoId)
                );
                var cantidadPresolicitudRechazadas = await PreSolicitudRepository.CountWhere(c => c.PreSolicitudCabeceraId == id
                     && c.EstadoFila && c.EstadoId == estadoRechazadaId
                     );

                if (cantidadPresolicitud == cantidadPresolicitudRechazadas)
                {
                    var preSolicitudCabecera = await PreSolicitudCabeceraRepository.FirstOrDefault(f => f.EstadoFila && f.Id == id);

                    if (preSolicitudCabecera == null)
                        throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                            "La solicitud ya fue rechazada ó no existe.");

                    preSolicitudCabecera.EstadoId = estadoRechazadaId;
                    UnitOfWork.Set<PreSolicitudCabeceraEntity>().Update(preSolicitudCabecera);
                    await UnitOfWork.SaveChangesAsync();
                }
            }
        }

        private async Task<bool> RegistrarMsvDependiendoEdad(int sociaId)
        {
            var formulario = await FormularioRepository.ObtenerPorSocia(sociaId);
            if (!formulario.Any())
            {
                return false;
            }
            if (formulario.Any() && formulario.Count > 1)
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, "La socia cuenta con varios formulario, comunicarse con sistemas.");
            if (!formulario.First().FechaNacimiento.HasValue)
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, "La socia debe tener registrado la fecha de nacimiento.");

            var socia = await SociaRepository.ObtenerSistemaExternoIdyEstado(sociaId);
            var edad = Edad(formulario.First().FechaNacimiento);

            switch (edad)
            {
                case < 72 and >= 18 when socia.SociaId_SistemaExterno.HasValue &&
                                        socia.Estado.Codigo == Constants.Core.Catalogo.DetEstadoSocia.Activa:
                case < 70 and >= 18 when !socia.SociaId_SistemaExterno.HasValue &&
                                        socia.Estado.Codigo == Constants.Core.Catalogo.DetEstadoSocia.Activa:
                    return true;

                default:
                    return false;
            }
        }

        private int Edad(DateTime? fechaNacimiento)
        {
            if (!fechaNacimiento.HasValue)
                return 0;

            var end = DateTime.Today;
            if (fechaNacimiento > end)
            {
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                       "Fecha de nacimiento de la socia está incorrecto.");
            }

            //var edad = fechaActual.Year - fechaNacimiento?.Year;
            //if (fechaNacimiento?.Month > fechaActual.Month)
            //{
            //    --edad;
            //}
            //return (int)edad;

            var currentYear = end.Year;
            var currentMonth = end.Month;
            int? currentDay = end.Day;
            var nacYear = fechaNacimiento?.Year;
            var nacMonth = fechaNacimiento?.Month;
            var nacDay = fechaNacimiento?.Day;
            int? monthAge = 0;
            int? dateAge = 0;
            var yearAge = currentYear - nacYear;

            if (currentMonth >= nacMonth)
            {
                monthAge = currentMonth - nacMonth;
                
            }
            else
            {
                yearAge--;
                monthAge = 12 + currentMonth - nacMonth;
                
            }
            if (end.Day >= fechaNacimiento?.Day)
            {
                //get days when the current date is greater
                dateAge = currentDay - nacDay;
                
            }
            else
            {
                monthAge--;
                dateAge = 31 + currentDay - nacDay;
                
                if (monthAge < 0)
                {
                    monthAge = 11;
                    yearAge--;
                    
                }
            }

            return (int)yearAge;
        }

        private bool TieneCargoJefatura()
        {
            var listaRol = new List<string>()
            {
                Constants.Core.Catalogo.Perfil.OficialCredito,
                Constants.Core.Catalogo.Perfil.JefaturaSupervisior,
                Constants.Core.Catalogo.Perfil.PersonalAutorizado
            };
            var data = UserIdentity.FindFirst(Constants.Core.UserAsociadoClaims.RolCodigo);
            foreach (var v in data.Value.Split(','))
            {
                if (listaRol.Any(a => a == v))
                    return true;
            }
            return false;
        }

        private async Task<AnilloGrupalEntity> CrearAnilloGrupal(int bancoComunalId, List<CatalogDto> listaCatalogo)
        {
            var anilloGrupal = await AnilloGrupalRepository.ListarPorBancoComunal(bancoComunalId);
            var correlativo = !anilloGrupal.Any() ? 1 : anilloGrupal.Select(s => s.Correlativo).Max() + 1;
            if (correlativo == 1)
            {
                var bancoComunal = await BancoComunalRepository.ObtenerPorId(bancoComunalId);
                if (bancoComunal == null)
                {
                    throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                                "El banco comunal se encuentra inactivo.");
                }
                var estado = listaCatalogo.Where(p => p.Code == Constants.Core.Catalogo.Estado).ToList();
                var estadoId = estado.First(f => f.DetailCode == Constants.Core.Catalogo.DetEstado.Activo).DetailId;
                return new AnilloGrupalEntity()
                {
                    BancoComunalCodigo = bancoComunal.Codigo,
                    BancoComunalId = bancoComunalId,
                    Descripcion = bancoComunal.Descripcion + " - " + correlativo.ToString("D2"),
                    EstadoId = estadoId,
                    Correlativo = correlativo
                };
            }
            else
            {
                return new AnilloGrupalEntity()
                {
                    BancoComunalCodigo = anilloGrupal.First().BancoComunalCodigo,
                    BancoComunalId = bancoComunalId,
                    Descripcion = anilloGrupal.First().BancoComunal.Descripcion + " - " + correlativo.ToString("D2"),
                    EstadoId = anilloGrupal.First().EstadoId,
                    Correlativo = correlativo
                };
            }
        }

        private async Task<FormularioEntity> ActualizarFormularioAnilloABanco(int SociaId)
        {
            var entity = await FormularioRepository.FirstOrDefault(f => f.EstadoFila && f.SociaId == SociaId);
            entity.AnilloGrupalId = null;
            return entity;
        }

        private async Task<SociaEntity> ActualizarSociaBanco(int SociaId, int? bancoComunalId)
        {
            if (bancoComunalId.HasValue)
            {
                var entity = await SociaRepository.FirstOrDefault(f => f.EstadoFila && f.Id == SociaId);
                entity.BancoComunalId = bancoComunalId;
                return entity;
            }
            else
            {
                return null;
            }
        }

        private async Task<FormularioEntity> ActualizarFormularioBancoAAnillo(int SociaId, int? anilloGrupal)
        {
            var entity = await FormularioRepository.FirstOrDefault(f => f.EstadoFila && f.SociaId == SociaId);
            entity.AnilloGrupalId = anilloGrupal;
            return entity;
        }

        private async Task<List<SociaEntity>> DarBajaSocia(List<PreSolicitudEntity> listaPresolicitud)
        {
            var listaEstadoSocia = await CatalogoDetalleRepository.ListarPorCatalogoCodigoParaDropDown(Constants.Core.Catalogo.EstadoSocia);
            var listaRetiro = await CatalogoDetalleRepository.ListarPorCatalogoCodigoParaDropDown(Constants.Core.Catalogo.MotivoRetiro);
            listaRetiro = listaRetiro.Where(p => p.Code != Constants.Core.Catalogo.DetMotivoRetiro.BancoaBanco &&
            p.Code != Constants.Core.Catalogo.DetMotivoRetiro.AnilloBanco &&
            p.Code != Constants.Core.Catalogo.DetMotivoRetiro.AnilloaAnillo &&
            p.Code != Constants.Core.Catalogo.DetMotivoRetiro.BancoAnillo
            ).ToList();

            var presolicitudRetirado = listaPresolicitud.Where(p => listaRetiro.Select(s => s.Id).ToList().Contains(p.MotivoRetiroId ?? 0)).ToList();
            if (!presolicitudRetirado.Any())
                return new List<SociaEntity>();

            List<int> listaSociaId = new();
            foreach (var v in presolicitudRetirado)
            {
                listaSociaId.Add(v.SociaId);
            }

            var listaSocia = await SociaRepository.ObtenerPorListaId(listaSociaId);
            listaSocia.ForEach(f =>
            {
                f.EstadoId = listaEstadoSocia.First(x => x.Code == Constants.Core.Catalogo.DetEstadoSocia.Debaja).Id;
            });

            return listaSocia;
        }

        private PreSolicitudEntity RegistrarPreSocilicitudAhorrista(List<CatalogDto> listaCatalogo, int sociaId, int correlativoPresolicitud)
        {
            var estadoAhorristaId = listaCatalogo
                          .Where(f => f.Code == Constants.Core.Catalogo.EstadoPreSolicitud).ToList()
                          .First(x => x.DetailCode == Constants.Core.Catalogo.PreSolicitudEstado.Ahorrista).DetailId;

            return new PreSolicitudEntity()
            {
                Nro = correlativoPresolicitud,
                SociaId = sociaId,
                BancoDesembolsoId = null,
                Plazo = 0,
                PlazoGracia = null,
                Monto = 0,
                TipoCreditoId = null,
                SubTipoCreditoId = null,
                NroCuenta = null,
                EstadoId = estadoAhorristaId,
                AsistenciaId = null,
                NivelRiesgoId = null,
                TasaInteresId = null,
                MSV = false,
                AnilloGrupalRetiroId = null,
                BancoComunalRetiradoId = null,
                MotivoRetiroId = null,
                CobraMedianteDj = false
            };
        }

        private PreSolicitudEntity RegistrarPreSocilicitudPorRetiro(List<CatalogDto> listaCatalogo, CrearPreSolicitudModel model
            , int correlativoPresolicitud, int? bancoComunalRetirado, int? anilloGrupalRetirado
            , int? sistemaExternoSociaRetirado)
        {
            var estadoRetiroId = listaCatalogo
                          .Where(f => f.Code == Constants.Core.Catalogo.EstadoPreSolicitud).ToList()
                          .First(x => x.DetailCode == Constants.Core.Catalogo.PreSolicitudEstado.Retirada).DetailId;
            var MotivoRetiro = listaCatalogo.Where(p => p.Code == Constants.Core.Catalogo.MotivoRetiro)
                       .ToList().Where(d => d.DetailCode == model.MotivoCodigo).FirstOrDefault();
            return new PreSolicitudEntity()
            {
                Nro = correlativoPresolicitud,
                SociaId = model.SociaId,
                BancoDesembolsoId = null,
                Plazo = 0,
                PlazoGracia = null,
                Monto = 0,
                TipoCreditoId = null,
                SubTipoCreditoId = null,
                NroCuenta = null,
                EstadoId = estadoRetiroId,
                AsistenciaId = null,
                NivelRiesgoId = null,
                TasaInteresId = null,
                MSV = false,
                AnilloGrupalRetiroId = anilloGrupalRetirado,
                BancoComunalRetiradoId = bancoComunalRetirado,
                SistemaExternoSociaPorRetiro = sistemaExternoSociaRetirado,
                MotivoRetiroId = MotivoRetiro?.DetailId,
                CobraMedianteDj = false
            };
        }

        #endregion method private
    }
}