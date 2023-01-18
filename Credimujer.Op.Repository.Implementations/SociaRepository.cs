using Credimujer.Op.Common;
using Credimujer.Op.Common.Base;
using Credimujer.Op.Domail.Models.Entities;
using Credimujer.Op.Domail.Models.Entities.Procedure;
using Credimujer.Op.Dto.Base;
using Credimujer.Op.Dto.PreSolicitud.GenerarPreSolicitud;
using Credimujer.Op.Dto.Socia.Actualizar;
using Credimujer.Op.Dto.Socia.Busqueda;
using Credimujer.Op.Dto.Socia.Registro;
using Credimujer.Op.Dto.Socia.Reporte;
using Credimujer.Op.Model.Socia.Busqueda;
using Credimujer.Op.Repository.Implementations.Data;
using Credimujer.Op.Repository.Implementations.Data.Base;
using Credimujer.Op.Repository.Implementations.Extensions;
using Credimujer.Op.Repository.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Credimujer.Op.Dto.PreSolicitud.GenerarPreSolicitud.ListaSociaPorBCyAGDto;

namespace Credimujer.Op.Repository.Implementations
{
    public class SociaRepository : BaseRepository<SociaEntity>, ISociaRepository
    {
        private readonly DataContext _context;

        public SociaRepository(DataContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<SociaEntity> ObtenerPorId(int sociaId)
        {
            return await _context.Socia.Where(p => p.EstadoFila && p.Id == sociaId).FirstOrDefaultAsync();
        }

        public async Task<List<SociaEntity>> ObtenerPorListaId(List<int> listaSociaId)
        {
            return await _context.Socia
                .Where(p => p.EstadoFila && listaSociaId.Contains(p.Id)).ToListAsync();
        }

        public async Task<ResponseDto<PaginationResultDTO<ListaSociaParaAprobarDto>>> ListadoSociaParaAprobar(FiltroSociaParaAprobarModel model, List<string> sucursal)
        {
            var responseDto = new ResponseDto<PaginationResultDTO<ListaSociaParaAprobarDto>>();
            var query = _context.Socia.Where(p => p.EstadoFila);
            if (!string.IsNullOrEmpty(model.Dni))
            {
                query = query.Where(p => p.NroDni == model.Dni);
            }

            if (!string.IsNullOrEmpty(model.ApellidoNombre))
            {
                query = query.Where(p =>
                                         (p.ApellidoPaterno + " " + p.ApellidoMaterno + " " + p.Nombre).Contains(model.ApellidoNombre));
            }

            if (sucursal.Any())
            {
                query = query.Where(p => sucursal.Contains(p.Sucursal.Codigo));
            }

            if (!string.IsNullOrEmpty(model.SucursalCodigo))
            {
                query = query.Where(p => p.Sucursal.Codigo == model.SucursalCodigo);
            }

            if (model.BancoComunalId.HasValue)
            {
                query = query.Where(p => p.Formulario.First().BancoComunalId == model.BancoComunalId);
                if (model.AnilloGrupalId.HasValue)
                {
                    query = query.Where(p => p.Formulario.First().AnilloGrupalId == model.AnilloGrupalId);
                }
            }

            if (model.EstadoSociaId.HasValue)
            {
                query = query.Where(p => p.EstadoId == model.EstadoSociaId);
            }

            var result = query.Select(s => new ListaSociaParaAprobarDto()
            {
                Id = s.Id,
                Nombre = s.Nombre,
                ApellidoPaterno = s.ApellidoPaterno,
                ApellidoMaterno = s.ApellidoMaterno,
                Dni = s.NroDni,
                Estado = s.Estado.Descripcion,
                EstadoCodigo = s.Estado.Codigo,
                BancoComunal = s.Formulario.First(f => f.EstadoFila).BancoComunal.Descripcion,
                AnilloGrupal = s.Formulario.First(f => f.EstadoFila).AnilloGrupal.Descripcion,
                BancoComunalCodigo = s.Formulario.First(f => f.EstadoFila).BancoComunal.Codigo,
                CargoBancoComunalCodigo = s.Formulario.First(f => f.EstadoFila).CargoBancoComunal.Codigo,
                BancoComunalId = s.Formulario.FirstOrDefault(f => f.EstadoFila).BancoComunalId
            }).SortBy(model.Order, model.ColumnOrder);

            var response = await result.GetPagedAsync(model.Page, model.PageSize);

            responseDto.Data = response;

            return responseDto;
        }

        public async Task<ActualizarDatoPersonalDto> ObtenerPorIdParaActualizarDatoPersonal(int idSocia)
        {
            return await _context.Socia.Where(p => p.EstadoFila && p.Id == idSocia)
                    .Select(s => new ActualizarDatoPersonalDto()
                    {
                        Id = s.Id,
                        Dni = s.NroDni,
                        Nombre = s.Nombre,
                        ApellidoPaterno = s.ApellidoPaterno,
                        ApellidoMaterno = s.ApellidoMaterno
                    }).FirstOrDefaultAsync()
                ;
        }

        public async Task<SociaNuevaPorDniDto> ObtenerSinFormularioPorDniyListaSucursal(string nroDni, List<string> listaSucursalCodigo)
        {
            return await _context.Socia.Where(p => p.EstadoFila && p.NroDni == nroDni
                                                                && listaSucursalCodigo.Contains(p.Sucursal.Codigo) && !p.Formulario.Any(a => a.EstadoFila)
                    )
                    .Select(s => new SociaNuevaPorDniDto()
                    {
                        Id = s.Id,
                        Dni = s.NroDni,
                        Nombre = s.Nombre,
                        ApellidoPaterno = s.ApellidoPaterno,
                        ApellidoMaterno = s.ApellidoMaterno,
                        SucursalCodigo = s.Sucursal.Codigo,
                        Celular = s.Celular,
                        BancoComunal = s.BancoComunal.Descripcion,
                        BancoComunalId = s.BancoComunalId,
                        CargoBancoComunalId = s.CargoBancoComunalId,
                        Telefono = s.Telefono,
                        TipoDocumentoId = s.TipoDocumentoId,
                    }).FirstOrDefaultAsync()
                ;
        }
        public async Task<SociaNuevaPorDniDto> ObtenerPorDniyListaSucursal(string nroDni, List<string> listaSucursalCodigo)
        {
            return await _context.Socia.Where(p => p.EstadoFila && p.NroDni == nroDni
            && listaSucursalCodigo.Contains(p.Sucursal.Codigo) &&
            (!p.Formulario.Any(a => a.EstadoFila) || (p.Estado.Codigo == Constants.Core.Catalogo.DetEstadoSocia.Debaja && (p.Reingresante == true || p.DatoIncompleto == true)))
            )
                    .Select(s => new SociaNuevaPorDniDto()
                    {
                        Id = s.Id,
                        Dni = s.NroDni,
                        Nombre = s.Nombre,
                        ApellidoPaterno = s.ApellidoPaterno,
                        ApellidoMaterno = s.ApellidoMaterno,
                        SucursalCodigo = s.Sucursal.Codigo,
                        Celular = s.Celular,
                        BancoComunal = s.BancoComunal.Descripcion,
                        BancoComunalId = s.BancoComunalId,
                        CargoBancoComunalId = s.CargoBancoComunalId,
                        Telefono = s.Telefono,
                        TipoDocumentoId = s.TipoDocumentoId,
                        Reingresante = s.Reingresante ?? false,
                        DatoIncompleto = s.DatoIncompleto ?? false,
                        SistemaExternoId = s.SociaId_SistemaExterno,
                        Formulario = s.Formulario.Where(p => p.EstadoFila).Select(se => new FormularioDto()
                        {
                            NroDependiente = se.NroDependiente,
                            Celular = se.Celular,
                            Telefono = se.Telefono,
                            FechaNacimiento = se.FechaNacimiento,
                            ActividadEconomica = se.ActividadEconomica,
                            ActividadEconomica2 = se.ActividadEconomica2,
                            ActividadEconomica3 = se.ActividadEconomica3,
                            Ubicacion = se.Ubicacion,
                            Direccion = se.Direccion,
                            Referencia = se.Referencia,
                            EstadoCivilId = se.EstadoCivilId,
                            GradoInstruccionId = se.GradoInstruccionId,
                            SituacionDomicilioId = se.SituacionDomicilioId,
                            EntidadBancariaId = se.EntidadBancariaId,
                            NroCuenta = se.NroCuenta,
                            Representante = se.Representante,
                            UbicacionNegocio = se.UbicacionNegocio,
                            DireccionNegocio = se.DireccionNegocio,
                            ReferenciaNegocio = se.ReferenciaNegocio
                        }).FirstOrDefault()
                    }).FirstOrDefaultAsync()
                ;
        }

        public async Task<SociaEntity> ObtenerConFormularioPorIdYEstado(int idSocia, string estado)
        {
            var query = _context.Socia.Where(p => p.EstadoFila && p.Id == idSocia

                     );
            if (!string.IsNullOrEmpty(estado))
                query = query.Where(p => p.Estado.Codigo == estado);

            return await query.Select(s => new SociaEntity()
            {
                Id = s.Id,
                NroDni = s.NroDni,
                Nombre = s.Nombre,
                ApellidoPaterno = s.ApellidoPaterno,
                ApellidoMaterno = s.ApellidoMaterno,
                Celular = s.Celular,
                Telefono = s.Telefono,
                EntidadBancario = s.EntidadBancario,
                NroCuenta = s.NroCuenta,
                SucursalId = s.SucursalId,
                EstadoId = s.EstadoId,
                CodigoCliente = s.CodigoCliente,
                BancoComunalId = s.BancoComunalId,
                SociaId_SistemaExterno = s.SociaId_SistemaExterno,
                CargoBancoComunalId = s.CargoBancoComunalId,
                TipoDocumentoId = s.TipoDocumentoId,
                Reingresante = s.Reingresante,
                DatoIncompleto = s.DatoIncompleto,

                EstadoFila = s.EstadoFila,
                FechaCreacion = s.FechaCreacion,
                UsuarioCreacion = s.UsuarioCreacion,
                
                Formulario = s.Formulario.Where(p => p.EstadoFila).ToList(),
                
            }).FirstOrDefaultAsync()
                ;
        }

        public async Task<ResponseDto<PaginationResultDTO<ListaSociaPorBancoComunalDto>>> ObtnerPorBancoComunalYAnilloGrupal(FiltroBusquedaPorBancoComunalModel model)
        {
            var responseDto = new ResponseDto<PaginationResultDTO<ListaSociaPorBancoComunalDto>>();
            var query = _context.Socia.Where(p => p.EstadoFila && p.Estado.Codigo == Constants.Core.Catalogo.DetEstadoSocia.Activa);

            if (model.BancoComunalId.HasValue && model.AnilloGrupalId.HasValue)
            {
                query = query.Where(p => p.Formulario.FirstOrDefault(f => f.EstadoFila).BancoComunalId == model.BancoComunalId
                && p.Formulario.FirstOrDefault(f => f.EstadoFila).AnilloGrupalId == model.AnilloGrupalId
                );
            }
            else if (model.BancoComunalId.HasValue && !model.AnilloGrupalId.HasValue)
            {
                query = query.Where(p => p.Formulario.FirstOrDefault(f => f.EstadoFila).BancoComunalId == model.BancoComunalId
                 && !p.Formulario.FirstOrDefault(f => f.EstadoFila).AnilloGrupalId.HasValue
                ).Union(_context.Socia.Where(s => s.EstadoFila && s.BancoComunalId == model.BancoComunalId
                    && !s.Formulario.Any()
                ));
            }
            //if (!string.IsNullOrEmpty(model.AnilloGrupal))
            //query = query.Where(p => p.Formulario.FirstOrDefault(f => f.EstadoFila).AnilloGrupal.Codigo == model.AnilloGrupal);
            if (model.ListaSucursalCodigo.Any())
                query = query.Where(p => model.ListaSucursalCodigo.Contains(p.Sucursal.Codigo));
            var result = query.Select(s => new ListaSociaPorBancoComunalDto()
            {
                Id = s.Id,
                Codigo = s.CodigoCliente,
                Dni = s.NroDni,
                Nombre = $"{s.ApellidoPaterno} {s.ApellidoMaterno} {s.Nombre}",
                BancoComunal = s.BancoComunal.Descripcion, //s.Formulario.FirstOrDefault(f => f.EstadoFila).BancoComunal.Descripcion,
                AnilloGrupal = s.Formulario.FirstOrDefault(f => f.EstadoFila).AnilloGrupal.Descripcion,
                IdExterno = s.SociaId_SistemaExterno
            }).SortBy(model.Order, model.ColumnOrder);

            var response = await result.GetPagedAsync(model.Page, model.PageSize);

            responseDto.Data = response;

            return responseDto;
        }

        public async Task<InformacionSociaDto> ObtenerInformacionPorId(int idSocia)
        {
            return await _context.Socia.Where(p => p.EstadoFila && p.Id == idSocia)
                    .Select(s => new InformacionSociaDto()
                    {
                        Id = s.Id,
                        NroDni = s.NroDni,
                        Nombre = s.Nombre,
                        ApellidoPaterno = s.ApellidoPaterno,
                        ApellidoMaterno = s.ApellidoMaterno,
                        FechaNacimiento = s.Formulario.FirstOrDefault(s => s.EstadoFila).FechaNacimiento,
                        Celular = s.Celular,
                        Telefono = s.Telefono,
                        EntidadBancaria = s.Formulario.FirstOrDefault(s => s.EstadoFila).EntidadBancariaId,
                        NroCuenta = s.Formulario.FirstOrDefault(f => f.EstadoFila).NroCuenta,
                        Actividad = s.Formulario.FirstOrDefault(f => f.EstadoFila).ActividadEconomica,
                        Actividad2 = s.Formulario.FirstOrDefault(f => f.EstadoFila).ActividadEconomica2,
                        Actividad3 = s.Formulario.FirstOrDefault(f => f.EstadoFila).ActividadEconomica3,
                        CargoBancoComunalId = s.CargoBancoComunalId ?? s.Formulario.FirstOrDefault(f => f.EstadoFila).CargoBancoComunalId,
                        SistemaExternoId = s.SociaId_SistemaExterno,
                        Ubicacion = s.Formulario.FirstOrDefault(f => f.EstadoFila).Ubicacion,
                        Direccion = s.Formulario.FirstOrDefault(f => f.EstadoFila).Direccion,
                        Referencia = s.Formulario.FirstOrDefault(f => f.EstadoFila).Referencia,
                        UbicacionNegocio = s.Formulario.FirstOrDefault(f => f.EstadoFila).UbicacionNegocio,
                        DireccionNegocio = s.Formulario.FirstOrDefault(f => f.EstadoFila).DireccionNegocio,
                        ReferenciaNegocio = s.Formulario.FirstOrDefault(f => f.EstadoFila).ReferenciaNegocio
                    }).FirstOrDefaultAsync()
                ;
        }

        public async Task<List<ListaSociaPorBCyAGDto>> ObtenerPorBancoComunalYAnilloGrupal(int bancoComunalId, int? anilloGrupalId)
        {
            var query = _context.Formulario.Where(p => p.EstadoFila && p.BancoComunalId == bancoComunalId
            && p.AnilloGrupalId == anilloGrupalId
            && p.Socia.Estado.Codigo == Constants.Core.Catalogo.DetEstadoSocia.Activa
            );
            //if (anilloGrupalId.HasValue)
            //{
            //    query = query.Where(p => p.AnilloGrupalId == anilloGrupalId);
            //}

            return await query.Select(s => new ListaSociaPorBCyAGDto()
            {
                Id = s.Socia.Id,

                ApellidoPaterno = s.Socia.ApellidoPaterno,
                ApellidoMaterno = s.Socia.ApellidoMaterno,
                Nombre = s.Socia.Nombre,
                Dni = s.Socia.NroDni,
                SociaId_SistemaExterno = s.Socia.SociaId_SistemaExterno,
                FechaNacimiento = s.Socia.Formulario.FirstOrDefault().FechaNacimiento,
                Reingresante = s.Socia.Reingresante ?? false,
                DatoIncompleto = s.Socia.DatoIncompleto ?? false,
                Formulario = s.Socia.Formulario.Where(f => f.EstadoFila).Select(se => new FormularioSocia()
                {
                    Id = se.Id,
                    BancoComunal = se.BancoComunal.Descripcion,
                    AnilloGrupal = se.AnilloGrupal.Descripcion,
                }).FirstOrDefault()
            }).ToListAsync();
        }

        public async Task<ReportePorSociaDto> ObtenerDatosPorId(int sociaId)
        {
            return await _context.Socia.Where(p => p.EstadoFila && p.Id == sociaId)
                .Select(s => new ReportePorSociaDto()
                {
                    ApePaterno = s.ApellidoPaterno,
                    ApeMaterno = s.ApellidoMaterno,
                    Nombre = s.Nombre,
                    Dni = s.NroDni,
                    FechaNac = s.Formulario.FirstOrDefault(f => f.EstadoFila).FechaNacimiento,
                    ActividadEconomica = s.Formulario.FirstOrDefault(f => f.EstadoFila).ActividadEconomica,
                    Celular = s.Celular,
                    GradoInstruccion = s.Formulario.FirstOrDefault(f => f.EstadoFila).GradoInstruccion.Descripcion,
                    Ubicacion = s.Formulario.FirstOrDefault(f => f.EstadoFila).Ubicacion,
                    Localidad = s.Sucursal.Descripcion,
                    Direccion = s.Formulario.FirstOrDefault(f => f.EstadoFila).Direccion,
                    Referencia = s.Formulario.FirstOrDefault(f => f.EstadoFila).Referencia,
                    BancoComunal = s.Formulario.FirstOrDefault(f => f.EstadoFila).BancoComunal.Descripcion,
                    EntidadFinanciera = s.Formulario.FirstOrDefault(f => f.EstadoFila).EntidadBancaria.Descripcion,
                    IdSocia = s.SociaId_SistemaExterno,
                    NroCtaAhorro = s.Formulario.FirstOrDefault(f => f.EstadoFila).NroCuenta,
                    Cargo = s.Formulario.FirstOrDefault(f => f.EstadoFila).CargoBancoComunal.Descripcion,
                    EstadoCivil = s.Formulario.FirstOrDefault(f => f.EstadoFila).EstadoCivil.Descripcion
                }).FirstOrDefaultAsync();
        }

        public ObtenerTipoDeudaPorDni ObtenerTipoDeuda(string nroDni)
        {
            var query = _context.ObtenerTipoDeudaPorDni
                .FromSqlInterpolated($"USP_OBTNER_TIPO_DEUDA_POR_DNI {nroDni}").AsEnumerable()
                .FirstOrDefault();
            return query;
        }

        public ObtenerTipoRiesgoPorDni ObtenerTipoRiesgo(string nroDni)
        {
            var query = _context.SPObtenerTipoRiesgo
                .FromSqlInterpolated($"USP_OBTNER_TIPO_RIESGO_POR_DNI {nroDni}").AsEnumerable()
                .FirstOrDefault();
            return query;
        }

        public ObtenerCapacidadPagoEntity ObtenerCapacidadPago(string nroDni)
        {
            var query = _context.SPObtenerCapacidadPagos
                .FromSqlInterpolated($"USP_OBTENER_CAPACIDAD_PAGO {nroDni}").AsEnumerable()
                .FirstOrDefault();
            return query;
        }
        
        public async Task<List<ReporteListaSociaDto._Detalle>> ObtenerListaSociaReporte(int bancoComunalId)
        {
            var query = _context.Socia.Where(p => p.EstadoFila && p.BancoComunalId == bancoComunalId
                                                               && p.Estado.Codigo == Constants.Core.Catalogo
                                                                   .DetEstadoSocia.Activa
                )
                .Select(s => new ReporteListaSociaDto._Detalle()
                {
                    Id = s.Id,
                    IdSocia = s.SociaId_SistemaExterno,
                    Nombre = s.ApellidoPaterno + " " + s.ApellidoMaterno + " " + s.Nombre,
                    TipoDocumento = s.TipoDocumento.Descripcion,
                    NroDni = s.NroDni,
                    FechaNacimiento = s.Formulario.FirstOrDefault(p => p.EstadoFila).FechaNacimiento,
                    Producto = s.Formulario.Where(p => p.EstadoFila).Select(s =>
                        s.BancoComunalId != null ? s.AnilloGrupalId != null ? "AG" : "BC" : ""
                    ).FirstOrDefault(),
                    Cargo = s.Formulario.FirstOrDefault(p => p.EstadoFila).CargoBancoComunal.Abreviatura,

                    GradoInstruccion = s.Formulario.FirstOrDefault(p => p.EstadoFila).GradoInstruccion.Abreviatura,
                    NroDependiente = s.Formulario.FirstOrDefault(p => p.EstadoFila).NroDependiente,
                    Celular = s.Formulario.FirstOrDefault(p => p.EstadoFila).Celular,
                    Telefono = s.Formulario.FirstOrDefault(p => p.EstadoFila).Telefono,

                    TipoVivienda = s.Formulario.FirstOrDefault(p => p.EstadoFila).SituacionDomicilio.Descripcion,
                    ActividadEconomica = s.Formulario.FirstOrDefault(p => p.EstadoFila).ActividadEconomica,
                    EntidadFinanciera = s.Formulario.FirstOrDefault(p => p.EstadoFila).EntidadBancaria.Descripcion,
                    NroCuenta = s.Formulario.FirstOrDefault(p => p.EstadoFila).NroCuenta,

                    Ubicacion = s.Formulario.FirstOrDefault(p => p.EstadoFila).Ubicacion,
                    Localidad = s.Sucursal.Descripcion,
                    Direccion = s.Formulario.FirstOrDefault(p => p.EstadoFila).Direccion,
                    Referencia = s.Formulario.FirstOrDefault(p => p.EstadoFila).Referencia,

                    UbicacionNegocio = s.Formulario.FirstOrDefault(p => p.EstadoFila).UbicacionNegocio,
                    DireccionNegocio = s.Formulario.FirstOrDefault(p => p.EstadoFila).DireccionNegocio,
                    ReferenciaNegocio = s.Formulario.FirstOrDefault(p => p.EstadoFila).ReferenciaNegocio,

                    UltimaModificacion = s.Formulario.FirstOrDefault(p => p.EstadoFila).FechaModificacion,
                    Reingreso = s.Reingresante == true ? "Si" : "No",
                    EstadoCivil = s.Formulario.FirstOrDefault(p => p.EstadoFila).EstadoCivil.Descripcion,

                });
                return await query.OrderBy(o => o.IdSocia).ToListAsync();
        }

        public async Task<ResponseDto<PaginationResultDTO<ListaSociaPorSucursalDto>>> ObtenerPorSucursalyDatos
            (FiltroBusquedaPorSucursalDatoModel model)
        {
            var responseDto = new ResponseDto<PaginationResultDTO<ListaSociaPorSucursalDto>>();
            var query = _context.Socia.Where(p => p.EstadoFila && p.Estado.Codigo == Constants.Core.Catalogo.DetEstadoSocia.Activa);

            if (!string.IsNullOrEmpty(model.SucursalCodigo))
            {
                query = query.Where(p => p.Sucursal.Codigo == model.SucursalCodigo);
            }
            if (!string.IsNullOrEmpty(model.Nombre))
            {
                query = query.Where(p => (p.ApellidoPaterno + ' ' + p.ApellidoMaterno + ' ' + p.Nombre).Contains(model.Nombre));
                //query = query.Where(p => p.ApellidoPaterno.Contains(model.Nombre)
                //                         || p.ApellidoMaterno.Contains(model.Nombre)
                //                         || p.Nombre.Contains(model.Nombre)
                //                         );
            }
            if (!string.IsNullOrEmpty(model.Dni))
            {
                query = query.Where(p => p.NroDni.Contains(model.Dni));
            }

            var result = query.Select(s => new ListaSociaPorSucursalDto()
            {
                CodigoSocia = s.CodigoCliente,
                Dni = s.NroDni,
                Nombre = s.ApellidoPaterno + " " + s.ApellidoMaterno + " " + s.Nombre,
                BancoComunal = s.Formulario.FirstOrDefault(f => f.EstadoFila).BancoComunal.Descripcion,
                AnilloGrupal = s.Formulario.FirstOrDefault(f => f.EstadoFila).AnilloGrupal.Descripcion,
            }).SortBy(model.Order, model.ColumnOrder);

            var response = await result.GetPagedAsync(model.Page, model.PageSize);

            responseDto.Data = response;

            return responseDto;
        }

        public List<ListadoProductoSociaEntity> ListadoProducto(string codigoSocia)
        {
            return _context.SPListadoProductoSocia.FromSqlInterpolated($"USP_LISTADO_PRO_SOCIA {codigoSocia}").AsEnumerable().ToList();
        }

        public List<ListadoProductoSociaEntity> ListadoProductoPorNumDocumento(string numeroDocumento)
        {
            return _context.SPListadoProductoSocia.FromSqlInterpolated($"USP_LISTADO_PRO_SOCIA_DNI {numeroDocumento}").AsEnumerable().ToList();
        }

        public List<ListadoCreditoCabeceraEntity> ListadoCreditoCabecera(string numcredito)
        {
            return _context.SPListadoCreditoCabecera.FromSqlInterpolated($"USP_CAB_CREDITO_DETALLADO {numcredito}").ToList();
        }

        public List<ListadoCreditoDetalleEntity> ListadoCreditoDetalle(string numcredito)
        {
            return _context.SPListadoCreditoDetalle.FromSqlInterpolated($"USP_DET_CREDITO_DETALLADO {numcredito}").ToList();
        }

        public List<BusquedaSociaEntity> BusquedaSocia(string dniNombres)
        {
            return _context.SPBusquedaSocia.FromSqlInterpolated($"USP_BUSQUEDA_SOCIA_SFDATA {dniNombres}").ToList();
        }

        public Tuple<List<ListaSociaMotivoBajaDto>, int> ListadoSociaConMotivoBaja(FiltroSociaConMotivoBajaModel model)
        {
            var param = new SqlParameter[] {
                        new SqlParameter() {
                            ParameterName = "@PageNumber",
                            SqlDbType =  System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = model.Page
                        },
                        new SqlParameter() {
                            ParameterName = "@RowsOfPage",
                            SqlDbType =  System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = model.PageSize
                        },
                        new SqlParameter() {
                            ParameterName = "@SortingCol",
                            SqlDbType =  System.Data.SqlDbType.VarChar,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = model.ColumnOrder
                        },
                        new SqlParameter() {
                            ParameterName = "@SortType",
                            SqlDbType =  System.Data.SqlDbType.VarChar,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = model.Order
                        },
                        new SqlParameter() {
                            ParameterName = "@Dni",
                            SqlDbType =  System.Data.SqlDbType.VarChar,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = string.IsNullOrEmpty(model.Dni)?"":model.Dni
                        },
                        new SqlParameter() {
                            ParameterName = "@Nombre",
                            SqlDbType =  System.Data.SqlDbType.VarChar,
                            Direction = System.Data.ParameterDirection.Input,
                            Value =string.IsNullOrEmpty(model.Socia)?"":model.Socia
                        },
                        new SqlParameter() {
                            ParameterName = "@TotalReg",
                            SqlDbType =  System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Output,
                            Value = 0
                        }
            };

            var result = _context.SPSociaMotivoBaja.FromSqlRaw("USP_SOCIA_MOTIVO_BAJAS @PageNumber, @RowsOfPage, @SortingCol, @SortType, @Dni, @Nombre, @TotalReg output", param).ToList()
                .Select(s => new ListaSociaMotivoBajaDto()
                {
                    BancoComunal = s.BancoComunal,
                    Dni = s.Dni,
                    Motivo = s.Motivo,
                    Socia = s.Socia,
                    TipoBaja = s.TipoBaja,
                    FechaBaja = s.FechaBaja
                }).ToList();
            return new Tuple<List<ListaSociaMotivoBajaDto>, int>(result, int.Parse(param[6].Value.ToString()));
        }

        public async Task<SociaEntity> ObtenerSistemaExternoIdyEstado(int idSocia)
        {
            var query = _context.Socia.Where(p => p.EstadoFila && p.Id == idSocia);
            return await query.Select(s => new SociaEntity()
            {
                Estado = s.Estado,
                SociaId_SistemaExterno = s.SociaId_SistemaExterno
            }).FirstOrDefaultAsync()
                ;
        }

        public async Task<bool> ExsiteCelular(string celular, int? sociaId)
        {
            var query = _context.Socia
                .Where(p => p.EstadoFila && p.Estado.Codigo == Constants.Core.Catalogo.DetEstadoSocia.Activa
                && p.Celular == celular
                );
            if (sociaId.HasValue)
                query = query.Where(p => p.Id != sociaId);

            return await query.AnyAsync();
        }

        public async Task<bool> ExsiteTelefono(string telefono, int? sociaId)
        {
            var query = _context.Socia
                .Where(p => p.EstadoFila && p.Estado.Codigo == Constants.Core.Catalogo.DetEstadoSocia.Activa
                && p.Telefono == telefono
                );
            if (sociaId.HasValue)
                query = query.Where(p => p.Id != sociaId);
            return await query.AnyAsync();
        }

        public async Task<SociaEntity> ObtenerSociaConEstado(int sociaId)
        {
            return await _context.Socia.Where(p => p.EstadoFila && p.Id == sociaId)
                .Select(s => new SociaEntity()
                {
                    Id = s.Id,
                    NroDni = s.NroDni,
                    ApellidoPaterno = s.ApellidoPaterno,
                    ApellidoMaterno = s.ApellidoMaterno,
                    Nombre = s.Nombre,
                    Celular = s.Celular,
                    Telefono = s.Telefono,
                    EntidadBancario = s.EntidadBancario,
                    NroCuenta = s.NroCuenta,
                    SucursalId = s.SucursalId,
                    EstadoId = s.EstadoId,
                    CodigoCliente = s.CodigoCliente,
                    BancoComunalId = s.BancoComunalId,
                    SociaId_SistemaExterno = s.SociaId_SistemaExterno,
                    CargoBancoComunalId = s.CargoBancoComunalId,
                    TipoDocumentoId = s.TipoDocumentoId,
                    Reingresante = s.Reingresante,
                    DatoIncompleto = s.DatoIncompleto,
                    Estado = s.Estado,
                    FechaCreacion = s.FechaCreacion,
                    UsuarioCreacion = s.UsuarioCreacion,
                    FechaModificacion = s.FechaModificacion,
                    UsuarioModificacion = s.UsuarioModificacion
                }).FirstOrDefaultAsync();
        }

        public async Task<bool> ExisteCuentaBancaria(int bancoId, string nroCta, int? sociaId)
        {
            var query = _context.Formulario.Where(p => p.EntidadBancariaId == bancoId
            && p.NroCuenta == nroCta
            );
            if (sociaId.HasValue)
            {
                query = query.Where(p => p.SociaId != sociaId);
            }
            return await query.AnyAsync();
        }
    }
}