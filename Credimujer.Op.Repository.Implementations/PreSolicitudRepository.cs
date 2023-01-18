using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Credimujer.Op.Common;
using Credimujer.Op.Common.Base;
using Credimujer.Op.Domail.Models.Entities;
using Credimujer.Op.Dto.Base;
using Credimujer.Op.Dto.Oficial;
using Credimujer.Op.Dto.PreSolicitud.GenerarPreSolicitud;
using Credimujer.Op.Dto.PreSolicitud.Reporte;
using Credimujer.Op.Model.Oficial;
using Credimujer.Op.Model.PreSolicitud.GenerarPresolicitud;
using Credimujer.Op.Repository.Implementations.Data;
using Credimujer.Op.Repository.Implementations.Data.Base;
using Credimujer.Op.Repository.Implementations.Extensions;
using Credimujer.Op.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Credimujer.Op.Repository.Implementations
{
    public class PreSolicitudRepository : BaseRepository<PreSolicitudEntity>, IPreSolicitudRepository
    {
        private readonly DataContext _context;
        private IQueryable<ResultadoResumenRegistroIngresadoDto> result;

        public PreSolicitudRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ListaPreSolicitudPorBancoDto>> ObtenerPorPreSolicitudCabecera
        (int id, List<string> listaEstadoCodigo, List<string> listaEstadoCodigoPreSolicitudCabecera)
        {
            var query = _context.PreSolicitud.Where(p => p.EstadoFila
            && p.PreSolicitudCabeceraId == id
            );

            if (listaEstadoCodigo.Any())
                query = query.Where(p => listaEstadoCodigo.Contains(p.Estado.Codigo));

            if (listaEstadoCodigoPreSolicitudCabecera.Any())
                query = query.Where(p =>
                    listaEstadoCodigoPreSolicitudCabecera.Contains(p.PreSolicitudCabecera.Estado.Codigo));

            return await query.Select(s => new ListaPreSolicitudPorBancoDto()
            {
                Id = s.Id,
                Nro = s.Nro,
                BancoComunal = s.Socia.Formulario.FirstOrDefault(f => f.EstadoFila).BancoComunal.Descripcion,
                AnilloGrupal = s.Socia.Formulario.FirstOrDefault(f => f.EstadoFila).AnilloGrupal.Descripcion,
                TienePeriodoGracia = s.Socia.Formulario.FirstOrDefault(f => f.EstadoFila).BancoComunal.PeriodoGracia,
                Nombre = s.Socia.ApellidoPaterno + " " + s.Socia.ApellidoMaterno + " " + s.Socia.Nombre,
                Dni = s.Socia.NroDni,
                Monto = s.Monto,
                Plazo = s.Plazo,
                EntidadBancaria = s.BancoDesembolso.Descripcion,
                Estado = s.Estado.Descripcion,
                EstadoCodigo = s.Estado.Codigo,
                SociaId = s.SociaId,
                SociaDj = s.SociaDjId,
                SociaIdSistemaExterno = s.Socia.SociaId_SistemaExterno,
                FechaNacimiento = s.Socia.Formulario.FirstOrDefault(f => f.EstadoFila).FechaNacimiento,
            }).OrderBy(o => o.Nombre).ToListAsync();
        }

        public async Task<List<ListaPreSolicitudPorBancoDto>> ObtenerSinCabeceraPreSolicitud
            (FiltroPorBancoComunalModel model)
        {
            //var listaEstado = new List<string>()
            //{
            //    Constants.Core.Catalogo.PreSolicitudCabeceraEstado.Rechazada,
            //    Constants.Core.Catalogo.PreSolicitudCabeceraEstado.Aprobada
            //};
            var query = _context.PreSolicitud.Where(p => p.EstadoFila
                                                         && p.PreSolicitudCabecera == null
                                                         && p.BancoComunalRetiradoId == null
                                                         && p.AnilloGrupalRetiroId == null
                                                         && p.Estado.Codigo != Constants.Core.Catalogo.PreSolicitudEstado.Rechazada
                                                         && p.Socia.Estado.Codigo == Constants.Core.Catalogo.DetEstadoSocia.Activa
                );

            query = query.Where(p => p.Socia.Formulario.FirstOrDefault(f => f.EstadoFila).BancoComunalId == model.BancoComunalId);

            query = query.Where(p => p.Socia.Formulario.FirstOrDefault(f => f.EstadoFila).AnilloGrupalId == model.AnilloGrupalId);

            return await query.Select(s => new ListaPreSolicitudPorBancoDto()
            {
                Id = s.Id,
                Nro = s.Nro,
                BancoComunal = s.Socia.Formulario.FirstOrDefault(f => f.EstadoFila).BancoComunal.Descripcion,
                AnilloGrupal = s.Socia.Formulario.FirstOrDefault(f => f.EstadoFila).AnilloGrupal.Descripcion,
                Nombre = s.Socia.ApellidoPaterno + " " + s.Socia.ApellidoMaterno + " " + s.Socia.Nombre,
                FechaNacimiento = s.Socia.Formulario.FirstOrDefault(f => f.EstadoFila).FechaNacimiento,
                Monto = s.Monto,
                Plazo = s.Plazo,
                EntidadBancaria = s.BancoDesembolso.Descripcion, //s.Socia.Formulario.FirstOrDefault(f => f.EstadoFila).EntidadBancaria.Descripcion,
                Estado = s.Estado.Descripcion,
                EstadoCodigo = s.Estado.Codigo,
                SociaId = s.SociaId,
                SociaIdSistemaExterno = s.Socia.SociaId_SistemaExterno,
                TipoCredito = s.SubTipoCredito.Abreviatura,
                EsOrigenSistemaSocia = s.SistemaOrigen.Codigo == Constants.Core.Catalogo.SistemaOrigenSocia,
                TasaInteres = s.TasaInteres.Descripcion,
                Reingresante = s.Socia.Reingresante??false,
                DatoIncompleto = s.Socia.DatoIncompleto??false
            }).OrderByDescending(o => o.Estado).ToListAsync();
        }

        public async Task<List<PreSolicitudEntity>> ObtenerPorBancoComunal
            (int bancoComunalId, int? anilloGrupalId)
        {
            var query = _context.PreSolicitud.Where(p => p.EstadoFila
                                                         && p.PreSolicitudCabeceraId == null
                                                         && p.Socia.Estado.Codigo == Constants.Core.Catalogo.DetEstadoSocia.Activa
            && p.Socia.Formulario.FirstOrDefault(f => f.EstadoFila).BancoComunalId == bancoComunalId
            && p.Socia.Formulario.FirstOrDefault(f => f.EstadoFila).AnilloGrupalId == anilloGrupalId
            ).Union(
                _context.PreSolicitud.Where(p => p.EstadoFila && p.PreSolicitudCabeceraId == null
                && p.BancoComunalRetiradoId == bancoComunalId
                && p.AnilloGrupalRetiroId == anilloGrupalId
                )
            );

            //if (!string.IsNullOrEmpty(bancoComunalCodigo))
            //    query = query.Where(p => p.Socia.Formulario.FirstOrDefault(f => f.EstadoFila).BancoComunalCodigo == bancoComunalCodigo);

            //if (!string.IsNullOrEmpty(anilloGrupalCodigo))
            //    query = query.Where(p => p.Socia.Formulario.FirstOrDefault(f => f.EstadoFila).AnilloGrupalCodigo == anilloGrupalCodigo);

            //if (string.IsNullOrEmpty(anilloGrupalCodigo))
            //    query = query.Where(p => p.Socia.Formulario.FirstOrDefault(f => f.EstadoFila).AnilloGrupalCodigo == null);
            return await query.ToListAsync();
        }

        public async Task<PreSolicitudPorIdDto> ObtenerPorIdYEstado(int id, string estadoCodigo)
        {
            return await _context.PreSolicitud.Where(p => p.EstadoFila
                && p.Estado.Codigo == estadoCodigo
                && p.Id == id
                )
                .Select(s => new PreSolicitudPorIdDto()
                {
                    TipoCreditoId = s.TipoCreditoId,
                    SubTipoCreditoId = s.SubTipoCreditoId,
                    Monto = s.Monto,
                    Plazo = s.Plazo,
                    TasaInteresId = s.TasaInteresId,
                    PlazoGracia = s.PlazoGracia,
                    //EntidadBancariaId = s.EntidadBancariaId, ya no va este campo
                    EntidadBancariaId = s.BancoDesembolsoId,
                    NroCuenta = s.NroCuenta,
                    AsistenciaId = s.AsistenciaId,
                    NivelRiesgoId = s.NivelRiesgoId,
                    Msv = s.MSV,
                    SociaId = s.SociaId,
                    CobraMedianteDj = s.CobraMedianteDj,
                    SociaDjId = s.SociaDjId,
                    CapacidadPago = s.CapacidadPago,
                    TipoDeudaId = s.TipoDeudaId
                }).FirstOrDefaultAsync();
        }

        public async Task<List<ListaPreSolicitudParaValidacionDto>> ListaPreSolicitudPorSocia(
           int sociaId
           )
        {
            var mesActual = DateTime.Now.Month;

            return await _context.PreSolicitud.Where(p => p.EstadoFila && p.SociaId == sociaId
             && p.Socia.Formulario.Any(a => a.EstadoFila)// && p.Estado.Codigo == Constants.Core.Catalogo.PreSolicitudEstado.Registrado
             && p.PreSolicitudCabeceraId.HasValue
             && p.PreSolicitudCabecera.EstadoFila
             && p.PreSolicitudCabecera.Estado.Codigo != Constants.Core.Catalogo.PreSolicitudCabeceraEstado.Rechazada
             && p.FechaCreacion.Month == mesActual
            ).Union(_context.PreSolicitud.Where(p => p.EstadoFila && p.SociaId == sociaId
                && p.Socia.Formulario.Any(a => a.EstadoFila)
                && !p.PreSolicitudCabeceraId.HasValue

            )).
            Select(s => new ListaPreSolicitudParaValidacionDto()
            {
                Id = s.Id,
                SociaId = s.SociaId,
                BancoComunal = s.Socia.Formulario.FirstOrDefault(f => f.EstadoFila).BancoComunal.Descripcion,
                BancoComunalCodigo = s.Socia.Formulario.FirstOrDefault(f => f.EstadoFila).BancoComunal.Codigo,
                AnilloGrupal = s.Socia.Formulario.FirstOrDefault(f => f.EstadoFila).AnilloGrupal.Descripcion,
                AnilloGrupalCodigo = s.Socia.Formulario.FirstOrDefault(f => f.EstadoFila).AnilloGrupal.Codigo,
                TipoCredito = s.TipoCredito.Descripcion,
                TipoCreditoCodigo = s.TipoCredito.Codigo,

                SubTipoCredito = s.SubTipoCredito.Descripcion,
                SubTipoCreditoCodigo = s.SubTipoCredito.Codigo,
                EstadoCodigo = s.Estado.Codigo,
                FechaCreacion = s.FechaCreacion,
                PreSolicitudCabecera = s.PreSolicitudCabeceraId
            }).ToListAsync();
        }

        public async Task<List<PreSolicitudEntity>> ObtenerSinPresolicitudCabeceraPorSocia(int sociaId)
        {
            return await _context.PreSolicitud.Where(p => p.EstadoFila && p.SociaId == sociaId
            && p.PreSolicitudCabeceraId == null).ToListAsync();
        }

        public async Task<List<ListaPreSolicitudDto>> ObtenerPresolicitudNoAceptadas(int preSolicitudId)
        {
            var listaPresolicitudNoIncluidas = new List<string>
            {
                Constants.Core.Catalogo.PreSolicitudEstado.Ahorrista,
                Constants.Core.Catalogo.PreSolicitudEstado.Retirada,
                //Constants.Core.Catalogo.PreSolicitudEstado.Rechazada
            };
            return await _context.PreSolicitud.Where(p => p.EstadoFila
                            && p.PreSolicitudCabeceraId == preSolicitudId
                            && p.Socia.Estado.Codigo == Constants.Core.Catalogo.DetEstadoSocia.Activa
                            && listaPresolicitudNoIncluidas.Contains(p.Estado.Codigo)
                            )
                .Select(s => new ListaPreSolicitudDto()
                {
                    IdSocia = s.Socia.SociaId_SistemaExterno,
                    Dni = s.Socia.NroDni,
                    Nombre = s.Socia.ApellidoPaterno + " " + s.Socia.ApellidoMaterno + " " + s.Socia.Nombre,
                    ActividadEconomica = s.Socia.Formulario.FirstOrDefault(f => f.EstadoFila).ActividadEconomica,
                    Observacion = s.MotivoRetiro.Descripcion + " " + s.Comentario,
                    CodigoEstadoPreSolicitud = s.Estado.Codigo,
                    EstadoPreSolicitud = s.Estado.Abreviatura
                }).ToListAsync();
        }

        public async Task<List<ListaPreSolicitudPorBancoDto>> ObtenerPresolicitudMotivoRetiro
            (int bancoComunalRetiradoId, int? anilloGrupalRetirado)
        {
            var query = _context.PreSolicitud.Where(p => p.EstadoFila
                    && p.PreSolicitudCabecera == null && p.AnilloGrupalRetiroId == anilloGrupalRetirado
                    && p.BancoComunalRetiradoId == bancoComunalRetiradoId
                    && p.Estado.Codigo != Constants.Core.Catalogo.PreSolicitudEstado.Rechazada
                    && p.Socia.Estado.Codigo == Constants.Core.Catalogo.DetEstadoSocia.Activa
                );
            return await query.Select(s => new ListaPreSolicitudPorBancoDto()
            {
                Id = s.Id,
                Nro = s.Nro,
                BancoComunal = s.Socia.Formulario.FirstOrDefault(f => f.EstadoFila).BancoComunal.Descripcion,
                AnilloGrupal = s.Socia.Formulario.FirstOrDefault(f => f.EstadoFila).AnilloGrupal.Descripcion,
                Nombre = s.Socia.ApellidoPaterno + " " + s.Socia.ApellidoMaterno + " " + s.Socia.Nombre,
                FechaNacimiento = s.Socia.Formulario.FirstOrDefault(f => f.EstadoFila).FechaNacimiento,
                Monto = s.Monto,
                Plazo = s.Plazo,
                EntidadBancaria = s.BancoDesembolso.Descripcion, //s.Socia.Formulario.FirstOrDefault(f => f.EstadoFila).EntidadBancaria.Descripcion,
                Estado = s.Estado.Descripcion,
                EstadoCodigo = s.Estado.Codigo,
                SociaId = s.SociaId,
                SociaIdSistemaExterno = s.Socia.SociaId_SistemaExterno,
                TipoCredito = s.SubTipoCredito.Abreviatura,
                Reingresante = s.Socia.Reingresante??false,
                DatoIncompleto = s.Socia.DatoIncompleto??false
            }).OrderByDescending(o => o.Estado).ToListAsync();
        }

        public async Task<ResponseDto<PaginationResultDTO<ResultadoResumenRegistroIngresadoDto>>>
            ObtenerPorUsuarioySucursal(BuscarResumenIngresoModel model)
        {
            var responseDto = new ResponseDto<PaginationResultDTO<ResultadoResumenRegistroIngresadoDto>>();

            var query = _context.PreSolicitudCabecera.Where(p => p.EstadoFila);
            if (!string.IsNullOrEmpty(model.usuario))
                query = query.Where(p => p.UsuarioCreacion == model.usuario);
            else
            {
                query = query.Where(p => p.UsuarioCreacion.Contains("@"));
            }
            if (!string.IsNullOrEmpty(model.SucursalCodigo))
                query = query.Where(p => p.BancoComunal.Sucursal.Codigo == model.SucursalCodigo);
            if (model.FechaInicio.HasValue)
                query = query.Where(p => p.FechaCreacion.Date >= model.FechaInicio.Value.Date);
            if (model.FechaFin.HasValue)
                query = query.Where(p => p.FechaCreacion.Date <= model.FechaFin.Value.Date);

            //var query = _context.PreSolicitud
            //    .Join(_context.Socia, pre => pre.SociaId, so => so.Id, (pre, so) => new { pre, so })
            //    .Where(p => p.pre.EstadoFila && p.so.EstadoFila);
            //if (!string.IsNullOrEmpty(model.usuario))
            //    query = query.Where(p => p.pre.UsuarioCreacion == model.usuario);
            //else
            //{
            //    query = query.Where(p => p.pre.UsuarioCreacion.Contains("@"));
            //}
            //if (!string.IsNullOrEmpty(model.SucursalCodigo))
            //    query = query.Where(p => p.so.Sucursal.Codigo == model.SucursalCodigo);
            //if (model.FechaInicio.HasValue)
            //    query = query.Where(p => p.pre.FechaCreacion >= model.FechaInicio);
            //if (model.FechaFin.HasValue)
            //    query = query.Where(p => p.pre.FechaCreacion <= model.FechaFin);

            var result = query
                .Select(s => new ResultadoResumenRegistroIngresadoDto()
                {
                    SucursalId = s.BancoComunal.SucursalId,
                    Sucursal = s.BancoComunal.Sucursal.Descripcion,
                    Oficial = s.UsuarioCreacion,
                    CantidadRegistros = query.Where(x => x.UsuarioCreacion == s.UsuarioCreacion).Count()
                }).Distinct().SortBy(model.Order, model.ColumnOrder);

            var response = await result.GetPagedAsync(model.Page, model.PageSize);

            responseDto.Data = response;

            return responseDto;
        }
    }
}