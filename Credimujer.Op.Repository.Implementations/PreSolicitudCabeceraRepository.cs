using Credimujer.Op.Common;
using Credimujer.Op.Common.Base;
using Credimujer.Op.Domail.Models.Entities;
using Credimujer.Op.Dto.Base;
using Credimujer.Op.Dto.Oficial;
using Credimujer.Op.Dto.PreSolicitud.EstadoPreSolicitud;
using Credimujer.Op.Dto.PreSolicitud.Reporte;
using Credimujer.Op.Model.Oficial;
using Credimujer.Op.Model.PreSolicitud.Reporte;
using Credimujer.Op.Repository.Implementations.Data;
using Credimujer.Op.Repository.Implementations.Data.Base;
using Credimujer.Op.Repository.Implementations.Extensions;
using Credimujer.Op.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Credimujer.Op.Dto.Socia.Reporte;
using Microsoft.EntityFrameworkCore.Internal;

namespace Credimujer.Op.Repository.Implementations
{
    public class PreSolicitudCabeceraRepository : BaseRepository<PreSolicitudCabeceraEntity>, IPreSolicitudCabeceraRepository
    {
        private readonly DataContext _context;

        public PreSolicitudCabeceraRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ListaPreSolicitudCabeceraDto>> ObtenerPorBancoComunalYEstado(int bancoComunalId,
            int? anilloGrupalId, string estadoCodigo = "")
        {
            var query = _context.PreSolicitudCabecera.Where(p => p.EstadoFila
                                                                 && p.BancoComunal.Id == bancoComunalId
                                                                 && p.AnilloGrupalId == anilloGrupalId
            );
            if (!string.IsNullOrEmpty(estadoCodigo))
            {
                query = query.Where(p => p.Estado.Codigo == estadoCodigo);
            }
            //if (!string.IsNullOrEmpty(anilloGrupalCodigo))
            //{
            //query = query.Where(p => p.AnilloGrupalId == anilloGrupalId);
            //}

            var result = query.Select(s => new ListaPreSolicitudCabeceraDto()
            {
                Id = s.Id,
                BancoComunal = s.BancoComunal.Descripcion,
                AnilloGrupal = s.AnilloGrupal.Descripcion,
                Monto = s.Monto,
                Plazo = s.Plazo,
                Estado = s.Estado.Descripcion,
                EstadoCodigo = s.Estado.Codigo,
                Observacion = s.Observacion,
                FechaDesembolso = s.FechaDesembolso,
                DetalleTipoCredito = s.PreSolicitud.Select(ss => ss.SubTipoCredito.Codigo).ToList(),
                AbreviaturaTipoCredito = s.PreSolicitud.Select(ss => ss.SubTipoCredito.Abreviatura).ToList(),
            });
            return await result.ToListAsync();
        }

        public async Task<PreSolicitudCabeceraEntity> ObtenerConDetalle(int id, string estadoCodigo)
        {
            var query = _context.PreSolicitudCabecera.Where(p => p.EstadoFila
                                                                && p.Id == id
            );
            if (!string.IsNullOrEmpty(estadoCodigo))
            {
                query = query.Where(p => p.Estado.Codigo == estadoCodigo);
            }

            return await query.Select(s => new PreSolicitudCabeceraEntity()
            {
                Id = s.Id,
                Monto = s.Monto,
                Plazo = s.Plazo,
                EstadoId = s.EstadoId,
                Observacion = s.Observacion,
                BancoComunalId = s.BancoComunalId,
                AnilloGrupalId = s.AnilloGrupalId,
                PreSolicitud = s.PreSolicitud.Where(x => x.EstadoFila).ToList(),
                EstadoFila = s.EstadoFila,
                FechaDesembolso = s.FechaDesembolso,
                FechaCreacion = s.FechaCreacion,
                UsuarioCreacion = s.UsuarioCreacion,
                TipoId = s.TipoId
            }).FirstOrDefaultAsync();
        }

        public async Task<ReporteCtaExternaDto> ReporteCtaExterna(int id,
            List<int?> listSubTipoCredito
            )
        {
            return await _context.PreSolicitudCabecera.Where(p => p.EstadoFila
                && p.Id == id
                ).Select(s => new ReporteCtaExternaDto()
                {
                    Cabecera = new ReporteCtaExternaDto._Cabecera
                    {
                        Id = s.Id,
                        BancoComunal = s.BancoComunal.Descripcion,
                        BancoComunalCodigo = s.BancoComunal.Codigo,
                        Ciclo = s.BancoComunal.Ciclo,
                        Sucursal = s.BancoComunal.Sucursal.Descripcion,
                        FechaDesembolso = s.FechaDesembolso,
                        //Monto = s.Monto,
                        //Plazo = s.Plazo,
                        Fecha = s.FechaCreacion,
                        OficialCredito = s.UsuarioCreacion
                    },
                    Detalle = s.PreSolicitud.Where(p => p.EstadoFila
                      && listSubTipoCredito.Contains(p.SubTipoCreditoId)
                      && p.Estado.Codigo != Constants.Core.Catalogo.PreSolicitudEstado.Rechazada
                      )
                    .Select(ss => new ReporteCtaExternaDto._Detalle
                    {
                        IdSocia = ss.Socia.SociaId_SistemaExterno,
                        Nombre = ss.Socia.ApellidoPaterno + " " + ss.Socia.ApellidoMaterno + " " + ss.Socia.Nombre,
                        Dni = ss.Socia.NroDni,
                        Cargo = ss.Socia.Formulario.FirstOrDefault(f => f.EstadoFila).CargoBancoComunal.Descripcion,
                        NivelRiesgo = ss.NivelRiesgo.Descripcion,
                        CapacidadPago = ss.CapacidadPago,
                        ActividadEconomica = ss.Socia.Formulario.FirstOrDefault(f => f.EstadoFila).ActividadEconomica,
                        SubTipoCredito = ss.SubTipoCredito.Abreviatura,// ss.SubTipoCredito.Descripcion,
                        SubTipoCreditoDesc = ss.SubTipoCredito.Descripcion,
                        AfiliadaSmv = ss.MSV ? "Si" : "No",
                        CobraConDj = ss.SociaDj.ApellidoPaterno + " " + ss.SociaDj.ApellidoMaterno + " " + ss.SociaDj.Nombre,//ss.CobraMedianteDj ? "Si" : "No",
                        Monto = ss.Monto,
                        Plazo = ss.Plazo,
                        TasaInteres = ss.TasaInteres.Descripcion,
                        PlazoGracia = ss.PlazoGracia,
                        Asistencia = ss.Asistencia.Descripcion,
                        EntidadFinanciera = ss.BancoDesembolso.Descripcion,
                        NroCuenta = ss.NroCuenta,
                        Observacion = string.IsNullOrEmpty(ss.MotivoRetiro.Descripcion) ? ss.Comentario : string.IsNullOrEmpty(ss.Comentario)
                                            ? ss.MotivoRetiro.Descripcion : ss.Comentario + " | " + ss.MotivoRetiro.Descripcion,
                        EstadoPreSolicitud = ss.Estado.Abreviatura,
                        DescripcionEstadoPreSolicitud= ss.Estado.Descripcion,
                        Dispositivo =ss.TipoDispositivo.Descripcion,
                        TipoDeuda = ss.TipoDeuda.Descripcion
                        //CodigoEstadoPreSolicitud=ss.Estado.Codigo
                    }).ToList(),
                })
                .FirstOrDefaultAsync();
        }

        public async Task<ReporteParaleloDto> ReporteParalelo(int id,
            List<int?> listSubTipoCredito
            )
        {
            return await _context.PreSolicitudCabecera.Where(p => p.EstadoFila
                && p.Id == id
                ).Select(s => new ReporteParaleloDto()
                {
                    Cabecera = new ReporteParaleloDto._Cabecera
                    {
                        Id = s.Id,
                        BancoComunal = s.BancoComunal.Descripcion,
                        BancoComunalCodigo = s.BancoComunal.Codigo,
                        Ciclo = s.BancoComunal.Ciclo,
                        Sucursal = s.BancoComunal.Sucursal.Descripcion,
                        FechaDesembolso = s.FechaDesembolso,
                        Fecha = s.FechaCreacion,
                        OficialCredito = s.UsuarioCreacion
                    },
                    Detalle = s.PreSolicitud.Where(p => p.EstadoFila
                      && listSubTipoCredito.Contains(p.SubTipoCreditoId)
                      && p.Estado.Codigo != Constants.Core.Catalogo.PreSolicitudEstado.Rechazada
                      )
                    .Select(ss => new ReporteParaleloDto._Detalle
                    {
                        IdSocia = ss.Socia.SociaId_SistemaExterno,
                        Nombre = ss.Socia.ApellidoPaterno + " " + ss.Socia.ApellidoMaterno + " " + ss.Socia.Nombre,
                        Dni = ss.Socia.NroDni,
                        CobraConDj = ss.SociaDj.ApellidoPaterno + " " + ss.SociaDj.ApellidoMaterno + " " + ss.SociaDj.Nombre, //ss.CobraMedianteDj ? "Si" : "No",
                        SubTipoCredito = ss.SubTipoCredito.Abreviatura,//ss.SubTipoCredito.Descripcion,
                        SubTipoCreditoDes = ss.SubTipoCredito.Descripcion,
                        NivelRiesgo = ss.NivelRiesgo.Descripcion,
                        CapacidadPago = ss.CapacidadPago,
                        Monto = ss.Monto,
                        Plazo = ss.Plazo,
                        TasaInteres = ss.TasaInteres.Descripcion,
                        PlazoGracia = ss.PlazoGracia,
                        EntidadFinanciera = ss.BancoDesembolso.Descripcion,
                        NumeroCuenta = ss.NroCuenta,
                        Dispositivo=ss.TipoDispositivo.Descripcion,
                        TipoDeuda = ss.TipoDeuda.Descripcion,
                        EstadoPreSolicitudAbrev = ss.Estado.Abreviatura,
                        EstadoPreSolicitud = ss.Estado.Descripcion
                    }).ToList(),
                })
                .FirstOrDefaultAsync();
        }

        public async Task<ResponseDto<PaginationResultDTO<ListaPreSocilitudCabeceraDto>>> ListaRelacion
            (FiltroRelacionPreSolicitudModel model)
        {
            var response = new ResponseDto<PaginationResultDTO<ListaPreSocilitudCabeceraDto>>();
            var query = _context.PreSolicitudCabecera.Where(p => p.EstadoFila
            && p.BancoComunalId == model.BancoComunalId
            && p.AnilloGrupalId == model.AnilloGrupalId
            );
            if (model.Nro.HasValue)
                query = query.Where(p => p.Id == model.Nro);

            var result = query.Select(s => new ListaPreSocilitudCabeceraDto()
            {
                Nro = s.Id,
                BancoComunal = s.BancoComunal.Descripcion,
                AnilloGrupal = s.AnilloGrupal.Descripcion,
                Ciclo = s.BancoComunal.Ciclo,
                FechaDesembolso = s.FechaDesembolso,
                Monto = s.Monto,
                Plazo = s.Plazo,
                Estado = s.Estado.Descripcion,
                EstadoCodigo = s.Estado.Codigo,
                DetalleTipoCredito = s.PreSolicitud.Select(ss => ss.SubTipoCredito.Codigo).ToList(),
                DetalleTipoCreditoAbreviatura = s.PreSolicitud.Select(ss => ss.SubTipoCredito.Abreviatura).ToList(),
            }).SortBy(model.Order, model.ColumnOrder);
            response.Data = await result.GetPagedAsync(model.Page, model.PageSize);
            return response;
        }

        public async Task<bool> ExistePreSolicitudAprobadaPorSociaId(int sociaId)
        {
            return await _context.PreSolicitudCabecera.AnyAsync(
                a => a.EstadoFila && a.Estado.Codigo == Constants.Core.Catalogo.PreSolicitudCabeceraEstado.Aprobada
                    && a.PreSolicitud.Any(p => p.EstadoFila && p.SociaId == sociaId)
                );
        }

        public async Task<ResponseDto<PaginationResultDTO<PresolicitudIngresadosDto>>> ObtenerPorUsuario(BuscarPresolicitudPorUsuarioModel model)
        {
            var response = new ResponseDto<PaginationResultDTO<PresolicitudIngresadosDto>>();
            var query = _context.PreSolicitudCabecera.Where(p => p.EstadoFila
                                                                 && p.UsuarioCreacion == model.Usuario
                                                                 && p.BancoComunal.SucursalId == model.SucursalId
            );
            if (model.FechaInicio.HasValue && !model.FechaFin.HasValue)
            {
                query = query.Where(p => p.FechaCreacion.Date >= model.FechaInicio);
            }
            else if (model.FechaInicio.HasValue && model.FechaFin.HasValue)
            {
                query = query.Where(p => p.FechaCreacion.Date >= model.FechaInicio && p.FechaCreacion.Date <= model.FechaFin);
            }

            var result = query.Select(s => new PresolicitudIngresadosDto()
            {
                Id = s.Id,
                BancoComunal = s.BancoComunal.Descripcion,
                AnilloGrupal = s.AnilloGrupal.Descripcion,
                Monto = s.Monto,
                Plazo = s.Plazo,
                Estado = s.Estado.Descripcion,
                EstadoCodigo = s.Estado.Codigo,
                //Observacion = s.Observacion,
                FechaDesembolso = s.FechaDesembolso,
                DetalleTipoCredito = s.PreSolicitud.Select(ss => ss.SubTipoCredito.Abreviatura).ToList(),
            }).SortBy(model.Order, model.ColumnOrder);
            response.Data = await result.GetPaged2Async(model.Page, model.PageSize);
            return response;
        }

        public async Task<ReporteRegistrosIngresadosDto> ReporteRegistrosIngresados(ReportePresolicitudPorUsuarioModel model)
        {
            ReporteRegistrosIngresadosDto data;
            var query = _context.PreSolicitudCabecera
                .Join(_context.BancoComunal, p => p.BancoComunalId, b => b.Id, (p, b) => new { pre = p, ban = b })
                .Where(p => p.pre.EstadoFila && p.pre.BancoComunal.SucursalId == model.SucursalId);
            if (!string.IsNullOrEmpty(model.Oficial))
            {
                query = query.Where(p => p.pre.UsuarioCreacion == model.Oficial);
            }
            if (model.FechaInicio.HasValue && !model.FechaFin.HasValue)
            {
                query = query.Where(p => p.pre.FechaCreacion.Date >= model.FechaInicio);
            }
            else if (model.FechaInicio.HasValue && model.FechaFin.HasValue)
            {
                query = query.Where(p => p.pre.FechaCreacion.Date >= model.FechaInicio && p.pre.FechaCreacion.Date <= model.FechaFin);
            }

            data = new ReporteRegistrosIngresadosDto
            {
                Cabecera = await query.GroupBy(g => new { g.pre.BancoComunal.Sucursal.Descripcion, g.pre.UsuarioCreacion })
                .Select(s => new Cabecera
                {
                    Sucursal = s.Key.Descripcion,
                    Oficial = s.Key.UsuarioCreacion,
                    CantidadRegistros = s.Count()
                }).FirstOrDefaultAsync(),
                Detalle = await query.Select(s => new Detalle()
                {
                    Id = s.pre.Id,
                    BancoComunal = s.ban.Descripcion,
                    AnilloGrupal = s.pre.AnilloGrupal.Descripcion,
                    Monto = s.pre.Monto,
                    Plazo = s.pre.Plazo,
                    Estado = s.pre.Estado.Descripcion,
                    EstadoCodigo = s.pre.Estado.Codigo,
                    FechaDesembolso = s.pre.FechaDesembolso,
                    DetalleTipoCredito = s.pre.PreSolicitud.Where(p => p.SubTipoCredito != null).Select(ss => ss.SubTipoCredito.Codigo).ToList(),
                    AbreviaturaTipoCredito = s.pre.PreSolicitud.Where(p => p.SubTipoCredito != null).Select(ss => ss.SubTipoCredito.Abreviatura).ToList(),
                }
                ).ToListAsync()
            };
            return data;
        }

        public async Task<ReporteParaleloPromocionalDto> ReporteParaleloPromocional(int id,
            List<int?> listSubTipoCredito
            )
        {
            return await _context.PreSolicitudCabecera.Where(p => p.EstadoFila
                && p.Id == id
                ).Select(s => new ReporteParaleloPromocionalDto()
                {
                    Cabecera = new ReporteParaleloPromocionalDto._Cabecera
                    {
                        Id = s.Id,
                        BancoComunal = s.BancoComunal.Descripcion,
                        BancoComunalCodigo = s.BancoComunal.Codigo,
                        Ciclo = s.BancoComunal.Ciclo,
                        Sucursal = s.BancoComunal.Sucursal.Descripcion,
                        FechaDesembolso = s.FechaDesembolso,
                        Fecha = s.FechaCreacion,
                        OficialCredito = s.UsuarioCreacion
                    },
                    Detalle = s.PreSolicitud.Where(p => p.EstadoFila
                      && listSubTipoCredito.Contains(p.SubTipoCreditoId)
                      && s.Estado.Codigo != Constants.Core.Catalogo.PreSolicitudEstado.Rechazada
                      )
                    .Select(ss => new ReporteParaleloPromocionalDto._Detalle
                    {
                        IdSocia = ss.Socia.SociaId_SistemaExterno,
                        Nombre = ss.Socia.ApellidoPaterno + " " + ss.Socia.ApellidoMaterno + " " + ss.Socia.Nombre,
                        Dni = ss.Socia.NroDni,
                        Cargo = ss.Socia.Formulario.FirstOrDefault(f => f.EstadoFila).CargoBancoComunal.Descripcion,
                        NivelRiesgo = ss.NivelRiesgo.Descripcion,
                        CapacidadPago = ss.CapacidadPago,
                        ActividadEconomica = ss.Socia.Formulario.FirstOrDefault(f => f.EstadoFila).ActividadEconomica,
                        SubTipoCredito = ss.SubTipoCredito.Descripcion,
                        AfiliadaSmv = ss.MSV ? "Si" : "No",
                        CobraConDj = ss.SociaDj.ApellidoPaterno + " " + ss.SociaDj.ApellidoMaterno + " " + ss.SociaDj.Nombre,//ss.CobraMedianteDj ? "Si" : "No",
                        Monto = ss.Monto,
                        Plazo = ss.Plazo,
                        TasaInteres = ss.TasaInteres.Descripcion,
                        PlazoGracia = ss.PlazoGracia,
                        Asistencia = ss.Asistencia.Descripcion,
                        EntidadFinanciera = ss.BancoDesembolso.Descripcion,
                        NroCuenta = ss.NroCuenta,
                        EstadoPreSolicitud = ss.Estado.Abreviatura,
                        //CodigoEstadoPreSolicitud=ss.Estado.Codigo
                    }).ToList(),
                })
                .FirstOrDefaultAsync();
        }

        public async Task<ReporteCtaExternaDto> ReporteParaleloPromocional(int id)
        {
            var listaGrupoSubTipoCredito = new List<string>() { Constants.Core.Catalogo.GrupoSubTipoCredito.PROM, Constants.Core.Catalogo.GrupoSubTipoCredito.CCPM };
            var query = _context.PreSolicitudCabecera.Where(p => p.EstadoFila
                && p.Id == id
                ).Select(s => new ReporteCtaExternaDto()
                {
                    Cabecera = new ReporteCtaExternaDto._Cabecera
                    {
                        Id = s.Id,
                        BancoComunal = s.BancoComunal.Descripcion,
                        BancoComunalCodigo = s.BancoComunal.Codigo,
                        Ciclo = s.BancoComunal.Ciclo,
                        Sucursal = s.BancoComunal.Sucursal.Descripcion,
                        FechaDesembolso = s.FechaDesembolso,
                        //Monto = s.Monto,
                        //Plazo = s.Plazo,
                        Fecha = s.FechaCreacion,
                        OficialCredito = s.UsuarioCreacion
                    },
                    Detalle = s.PreSolicitud.Where(p => p.EstadoFila
                      && (listaGrupoSubTipoCredito.Contains(p.SubTipoCredito.Abreviatura)
                      || !p.SubTipoCreditoId.HasValue)
                      && p.Estado.Codigo != Constants.Core.Catalogo.PreSolicitudEstado.Rechazada
                      )
                    .Select(ss =>
                    new ReporteCtaExternaDto._Detalle
                    {
                        IdSocia = ss.Socia.SociaId_SistemaExterno,
                        Nombre = ss.Socia.ApellidoPaterno + " " + ss.Socia.ApellidoMaterno + " " + ss.Socia.Nombre,
                        Dni = ss.Socia.NroDni,
                        Cargo = ss.Socia.Formulario.FirstOrDefault(f => f.EstadoFila).CargoBancoComunal.Descripcion,
                        NivelRiesgo = ss.NivelRiesgo.Descripcion,
                        CapacidadPago = ss.CapacidadPago,
                        ActividadEconomica = ss.Socia.Formulario.FirstOrDefault(f => f.EstadoFila).ActividadEconomica,
                        SubTipoCredito = ss.SubTipoCredito.Abreviatura,// ss.SubTipoCredito.Descripcion,
                        SubTipoCreditoDesc = ss.SubTipoCredito.Descripcion,
                        AfiliadaSmv = ss.MSV ? "Si" : "No",
                        CobraConDj = ss.SociaDj.ApellidoPaterno + " " + ss.SociaDj.ApellidoMaterno + " " + ss.SociaDj.Nombre,//ss.CobraMedianteDj ? "Si" : "No",
                        Monto = ss.Monto,
                        Plazo = ss.Plazo,
                        TasaInteres = ss.TasaInteres.Descripcion,
                        PlazoGracia = ss.PlazoGracia,
                        Asistencia = ss.Asistencia.Descripcion,
                        EntidadFinanciera = ss.BancoDesembolso.Descripcion,
                        NroCuenta = ss.NroCuenta,
                        Observacion = string.IsNullOrEmpty(ss.MotivoRetiro.Descripcion) ? ss.Comentario : string.IsNullOrEmpty(ss.Comentario)
                                            ? ss.MotivoRetiro.Descripcion : ss.Comentario + " | " + ss.MotivoRetiro.Descripcion,
                        EstadoPreSolicitud = ss.Estado.Abreviatura,
                        Dispositivo=ss.TipoDispositivo.Descripcion,
                        TipoDeuda = ss.TipoDeuda.Descripcion
                    }).ToList(),
                });
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<ListaSociaConUltPresolicitudDto>> ObtenerUltPresolicitudPorListaSocia(
            List<int> listaSocia)
        {
            return await _context.Socia.Where(p =>
                p.EstadoFila && p.Estado.Codigo == Constants.Core.Catalogo.DetEstadoSocia.Activa
                             && listaSocia.Contains(p.Id)
                             && p.ListaPreSolicitud.Any(a=>a.EstadoFila && a.PreSolicitudCabeceraId.HasValue
                                                        && a.PreSolicitudCabecera.Estado.Codigo==Constants.Core.Catalogo.PreSolicitudCabeceraEstado.Aprobada
                                                        )
            ).Select(s => new ListaSociaConUltPresolicitudDto()
            {
                SociaId = s.Id,
                Id= s.ListaPreSolicitud.OrderBy(o => o.Id)
                    .LastOrDefault(pr =>
                        pr.PreSolicitudCabecera.Estado.Codigo == Constants.Core.Catalogo.PreSolicitudCabeceraEstado.Aprobada).Id,
                NivelRiesgo = s.ListaPreSolicitud.OrderBy(o=>o.Id)
                    .LastOrDefault(pr => 
                        pr.PreSolicitudCabecera.Estado.Codigo == Constants.Core.Catalogo.PreSolicitudCabeceraEstado.Aprobada).NivelRiesgo.Codigo,

                TipoDeuda = s.ListaPreSolicitud.OrderBy(o => o.Id)
                    .LastOrDefault(pr =>
                        pr.PreSolicitudCabecera.Estado.Codigo == Constants.Core.Catalogo.PreSolicitudCabeceraEstado.Aprobada).TipoDeuda.Descripcion,
                CapacidadPago = s.ListaPreSolicitud.OrderBy(o => o.Id)
                .LastOrDefault(pr =>
                pr.PreSolicitudCabecera.Estado.Codigo == Constants.Core.Catalogo.PreSolicitudCabeceraEstado.Aprobada).CapacidadPago
            }).ToListAsync();
        }

        public async Task<TipoDeudaNivelRiesgoSociaDto> ObtenerTipoDeudaNivelRiesgoPorSocia(int sociaId)
        {
            return await _context.PreSolicitudCabecera.Where(p =>
                    p.EstadoFila && p.Estado.Codigo == Constants.Core.Catalogo.PreSolicitudCabeceraEstado.Aprobada
                                 && p.PreSolicitud.Any(a => a.EstadoFila && a.SociaId == sociaId && a.Estado.Codigo==Constants.Core.Catalogo.PreSolicitudEstado.Registrado))
                .OrderByDescending(o=>o.Id).Select(s => new TipoDeudaNivelRiesgoSociaDto()
                {
                    Id = s.PreSolicitud.OrderBy(o=>o.Id).LastOrDefault().SociaId,
                    NivelRiesgo = s.PreSolicitud.OrderBy(o => o.Id).LastOrDefault().NivelRiesgo.Descripcion,
                    TipoDeuda= s.PreSolicitud.OrderBy(o=>o.Id).LastOrDefault().TipoDeuda.Descripcion,
                }).FirstOrDefaultAsync();

        }
    }
}