using Autofac;
using Credimujer.Op.Application.Interfaces;
using Credimujer.Op.Common;
using Credimujer.Op.Common.Base;
using Credimujer.Op.Dto.Base;
using Credimujer.Op.Dto.PreSolicitud.Reporte;
using Credimujer.Op.Model.PreSolicitud.Reporte;
using Credimujer.Op.Repository.Implementations;
using Credimujer.Op.Repository.Interfaces;
using Credimujer.Op.Repository.Interfaces.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Application.Implementations.PreSolicitud
{
    public class ReporteApplication : IReporteApplication
    {
        private readonly Lazy<IUnitOfWork> _unitOfWork;
        private readonly Lazy<IHttpContextAccessor> _httpContext;
        private readonly AppSetting _setting;

        public ReporteApplication(IOptions<AppSetting> settings, ILifetimeScope lifetimeScope)
        {
            _setting = settings.Value;
            _unitOfWork = new Lazy<IUnitOfWork>(() => lifetimeScope.Resolve<IUnitOfWork>());
            _httpContext = new Lazy<IHttpContextAccessor>(() => lifetimeScope.Resolve<IHttpContextAccessor>());
        }

        private ClaimsPrincipal UserIdentity => _httpContext.Value.HttpContext.User;
        private IUnitOfWork UnitOfWork => _unitOfWork.Value;
        private IPreSolicitudCabeceraRepository PreSolicitudCabeceraRepository => UnitOfWork.Repository<IPreSolicitudCabeceraRepository>();
        private ICatalogoDetalleRepository CatalogoDetalleRepository => UnitOfWork.Repository<ICatalogoDetalleRepository>();
        private IAnilloGrupalRepository AnilloGrupalRepository => UnitOfWork.Repository<IAnilloGrupalRepository>();
        private IPreSolicitudRepository PreSolicitudRepository => UnitOfWork.Repository<IPreSolicitudRepository>();
        private ISociaRepository SociaRepository => UnitOfWork.Repository<ISociaRepository>();

        public async Task<ResponseDto> Catalog(CatalogModel model)
        {
            var data = new ResponseDto();

            if (model.AnilloGrupal)
                data.Data.listaAnilloGrupal = await AnilloGrupalRepository.Lista();
            return data;
        }

        public async Task<ResponseDto<PaginationResultDTO<ListaPreSocilitudCabeceraDto>>> RelacionPresolicitudCabecera(FiltroRelacionPreSolicitudModel model)
        {
            ResponseDto<PaginationResultDTO<ListaPreSocilitudCabeceraDto>> result;
            result = await PreSolicitudCabeceraRepository.ListaRelacion(model);
            var listaTipoReporte = new List<DropdownDto>();
            if (result != null && result.Data.Results.Any())
            {
                foreach (var v in result.Data.Results)
                {
                    v.DetalleTipoCreditoAbreviatura = v.DetalleTipoCreditoAbreviatura?.Where(p => !string.IsNullOrEmpty(p)).Distinct().ToList();
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

                    v.TipoReporte = listaTipoReporte.GroupBy(g => g.Code).Select(z => z.First()).ToList();
                    listaTipoReporte.Clear();
                }
            }
            return result;
        }

        public async Task<ResponseDto> ReporteCtaExterna(int id)
        {
            var usuario = UserIdentity.FindFirst(Constants.Core.UserAsociadoClaims.NombreUsuario);
            var lista = await CatalogoDetalleRepository.ObtenerPorListaCodigoyActivoInactivo(
                new List<string> {
                    Constants.Core.Catalogo.DetSubTipoCredito.BancoComunal,
                    //Constants.Core.Catalogo.DetSubTipoCredito.ExternoPromocional
                });

            List<int?> listaSubTipoCredito = new List<int?>();
            listaSubTipoCredito.Add(null);// para poder incluir las presolicitudes retiradas
            lista.ForEach(action: f => listaSubTipoCredito.Add(f.Id));

            var result = await PreSolicitudCabeceraRepository.ReporteCtaExterna(id, listaSubTipoCredito);
            if (result.Detalle.Any())
                result.Detalle.ForEach(f =>
                {
                    if (string.IsNullOrEmpty(f.SubTipoCredito))
                    {
                        f.SubTipoCredito = f.EstadoPreSolicitud;
                    }
                    //f.TipoDeuda = SociaRepository.ObtenerTipoDeuda(f.Dni)?.TipoDeuda;
                });
            //var listaPresolicitudNoAceptadas = await PreSolicitudRepository.ObtenerPresolicitudNoAceptadas(id);

            //if (listaPresolicitudNoAceptadas != null && listaPresolicitudNoAceptadas.Any())
            //{
            //    listaPresolicitudNoAceptadas.ForEach(f =>
            //    {
            //        result.Detalle.Add(new ReporteCtaExternaDto._Detalle()
            //        {
            //            IdSocia = f.IdSocia,
            //            Dni = f.Dni,
            //            Nombre = f.Nombre,
            //            ActividadEconomica = f.ActividadEconomica,
            //            Observacion = f.Observacion,
            //            SubTipoCredito = f.EstadoPreSolicitud
            //        });
            //    });
            //}
            result.Cabecera.Monto = result.Detalle.Any() ? result.Detalle.Sum(s => s.Monto) : 0;
            result.Cabecera.Plazo = result.Detalle.Any() ? result.Detalle.Max(s => s.Plazo) : 0;
            result.Cabecera.Producto =
                String.Join("/", result.Detalle.Select(s => s.SubTipoCredito).Distinct().ToList());
            result.Cabecera.OficialCredito = result.Cabecera.OficialCredito.Replace("@manuela.org.pe", "", StringComparison.OrdinalIgnoreCase);
            result.Cabecera.Usuario = usuario.Value.Replace("@manuela.org.pe", "", StringComparison.OrdinalIgnoreCase);
            result.Detalle = result.Detalle.OrderBy(o => o.IdSocia).ToList();
            return new ResponseDto()
            {
                Data = result,
            };
        }

        public async Task<ResponseDto> ReporteCtaExternaExcel(int id)
        {
            byte[] bytes;
            var usuario = UserIdentity.FindFirst(Constants.Core.UserAsociadoClaims.NombreUsuario);
            var lista = await CatalogoDetalleRepository.ObtenerPorListaCodigoyActivoInactivo(
                new List<string> {
                    Constants.Core.Catalogo.DetSubTipoCredito.BancoComunal,
                    //Constants.Core.Catalogo.DetSubTipoCredito.ExternoPromocional
                });
            List<int?> listaSubTipoCredito = new List<int?>();
            listaSubTipoCredito.Add(null);// para poder incluir las presolicitudes retiradas
            lista.ForEach(action: f => listaSubTipoCredito.Add(f.Id));
            var result = await PreSolicitudCabeceraRepository.ReporteCtaExterna(id, listaSubTipoCredito);
            if (result.Detalle.Any())
                result.Detalle.ForEach(f =>
                {
                    if (string.IsNullOrEmpty(f.SubTipoCredito))
                    {
                        f.SubTipoCreditoDesc = f.DescripcionEstadoPreSolicitud;
                    }
                    //f.EstadoPreSolicitud = string.Empty;
                    //f.TipoDeuda = SociaRepository.ObtenerTipoDeuda(f.Dni)?.TipoDeuda;
                });
            result.Cabecera.Monto = result.Detalle.Any() ? result.Detalle.Sum(s => s.Monto) : 0;
            result.Cabecera.Plazo = result.Detalle.Any() ? result.Detalle.Max(s => s.Plazo) : 0;
            result.Cabecera.Producto =
                String.Join("/", result.Detalle.Select(s => s.SubTipoCreditoDesc).Distinct().ToList());
            result.Cabecera.OficialCredito = result.Cabecera.OficialCredito.Replace("@manuela.org.pe", "", StringComparison.OrdinalIgnoreCase);
            result.Cabecera.Usuario = usuario.Value.Replace("@manuela.org.pe", "", StringComparison.OrdinalIgnoreCase);
            result.Detalle = result.Detalle.OrderBy(o => o.IdSocia).ToList();
            //var ms = new MemoryStream();
            var fullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "PlantillaExcel/reportectaexterna.xlsx");
            FileInfo fi = new FileInfo(fullPath);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var pck = new ExcelPackage(fi);
            var ws = pck.Workbook.Worksheets["Hoja1"];

            ws.Cells["H1"].Value = $"PRE SOLICITUD N° {result.Cabecera.Id}";

            ws.Cells["E3"].Value = result.Cabecera.BancoComunal;
            ws.Cells["E4"].Value = result.Cabecera.BancoComunalCodigo;
            ws.Cells["E5"].Value = result.Cabecera.Ciclo;
            ws.Cells["E6"].Value = result.Cabecera.Sucursal;

            ws.Cells["J3"].Value = result.Cabecera.Producto;
            ws.Cells["J4"].Value = result.Cabecera.FechaDesembolso==null?"":result.Cabecera.FechaDesembolso.Value.ToString("dd/MM/yyyy");
            ws.Cells["J5"].Value = result.Cabecera.OficialCredito;

            ws.Cells["O3"].Value = $"S/ {string.Format("{0:0.00}", result.Cabecera.Monto)}";
            ws.Cells["O5"].Value = result.Cabecera.Plazo;

            //ws.Cells["A9"].LoadFromCollection(result.Detalle.OrderBy(o => o.IdSocia), false);
            var iniRow = 9;
            if (result.Detalle.Any())
            {
                result.Detalle.ForEach(f =>
                {
                    ws.Cells[$"A{iniRow}"].Value = f.IdSocia;
                    ws.Cells[$"B{iniRow}"].Value = f.Nombre;
                    ws.Cells[$"C{iniRow}"].Value = f.Dni;
                    ws.Cells[$"D{iniRow}"].Value = f.Cargo;
                    ws.Cells[$"E{iniRow}"].Value = f.NivelRiesgo;
                    ws.Cells[$"F{iniRow}"].Value = f.CapacidadPago;
                    ws.Cells[$"G{iniRow}"].Value = f.TipoDeuda;
                    ws.Cells[$"H{iniRow}"].Value = f.ActividadEconomica;
                    ws.Cells[$"I{iniRow}"].Value = f.SubTipoCreditoDesc;
                    ws.Cells[$"J{iniRow}"].Value = f.AfiliadaSmv;
                    ws.Cells[$"K{iniRow}"].Value = f.CobraConDj;
                    ws.Cells[$"L{iniRow}"].Value = f.Monto;
                    ws.Cells[$"M{iniRow}"].Value = f.Plazo;
                    ws.Cells[$"N{iniRow}"].Value = f.TasaInteres;
                    ws.Cells[$"O{iniRow}"].Value = f.PlazoGracia;
                    ws.Cells[$"P{iniRow}"].Value = f.Asistencia;
                    ws.Cells[$"Q{iniRow}"].Value = f.EntidadFinanciera;
                    ws.Cells[$"R{iniRow}"].Value = f.NroCuenta;
                    ws.Cells[$"S{iniRow}"].Value = f.Observacion;
                    ws.Cells[$"T{iniRow}"].Value = f.Dispositivo;
                    iniRow++;
                });
                var celda = $"L{result.Detalle.Count() + 10}";
                ws.Cells[celda].Formula = $"SUM(L9:L{result.Detalle.Count() + 9})";
            }
            using (MemoryStream ms = new MemoryStream(pck.GetAsByteArray()))
            {
                bytes = ms.ToArray();
            }
            return new ResponseDto()
            {
                Data = Convert.ToBase64String(bytes),
            };
        }

        public async Task<ResponseDto> ReporteParalelo(int id)
        {
            var usuario = UserIdentity.FindFirst(Constants.Core.UserAsociadoClaims.NombreUsuario);
            var excluir = new List<string> {
                    Constants.Core.Catalogo.DetSubTipoCredito.CreditoAnilloCampaña,
                    Constants.Core.Catalogo.DetSubTipoCredito.CreditoAnilloComplementario,
                    Constants.Core.Catalogo.DetSubTipoCredito.BancoComunal,
                    Constants.Core.Catalogo.DetSubTipoCredito.AnilloGrupal,
                };
            //var listaTipoCredito = await CatalogoDetalleRepository.ObtenerPorValoryActivoInactivo(Constants.Core.Catalogo.DetTipoCredito.CtaParalela);
            var listaTipoCredito = await CatalogoDetalleRepository.ListarPorCatalogoCodigoParaDropDown(Constants.Core.Catalogo.SubTipoCredito);
            listaTipoCredito = listaTipoCredito.Where(p => !excluir.Contains(p.Code)).ToList();

            List<int?> listaSubTipoCredito = new List<int?>();
            listaSubTipoCredito.Add(null);// para poder incluir las presolicitudes retiradas
            listaTipoCredito.ForEach(action: f => listaSubTipoCredito.Add(f.Id));

            var result = await PreSolicitudCabeceraRepository.ReporteParalelo(id, listaSubTipoCredito);
            if (result.Detalle.Any())
                result.Detalle.ForEach(f =>
                {
                    if (string.IsNullOrEmpty(f.SubTipoCredito))
                    {
                        f.SubTipoCredito = f.EstadoPreSolicitudAbrev;
                    }
                });
            //if (result != null && result.Detalle.Any())
            //{
            //    result.Detalle.ForEach(f =>
            //    {
            //        f.TipoDeuda = SociaRepository.ObtenerTipoDeuda(f.Dni)?.TipoDeuda;
            //    });
            //}
            result.Cabecera.OficialCredito = result.Cabecera.OficialCredito.Replace("@manuela.org.pe", "", StringComparison.OrdinalIgnoreCase);
            result.Cabecera.Usuario = usuario.Value.Replace("@manuela.org.pe", "", StringComparison.OrdinalIgnoreCase);
            result.Cabecera.Monto = result.Detalle.Any() ? result.Detalle.Sum(s => s.Monto) : 0;
            result.Detalle = result.Detalle.OrderBy(o => o.IdSocia).ToList();
            return new ResponseDto()
            {
                Data = result,
            };
        }

        public async Task<ResponseDto> ReporteParaleloExcel(int id)
        {
            byte[] bytes;
            var usuario = UserIdentity.FindFirst(Constants.Core.UserAsociadoClaims.NombreUsuario);
            var excluir = new List<string> {
                    Constants.Core.Catalogo.DetSubTipoCredito.CreditoAnilloCampaña,
                    Constants.Core.Catalogo.DetSubTipoCredito.CreditoAnilloComplementario,
                    Constants.Core.Catalogo.DetSubTipoCredito.BancoComunal,
                    Constants.Core.Catalogo.DetSubTipoCredito.AnilloGrupal,
                };
            //var listaTipoCredito = await CatalogoDetalleRepository.ObtenerPorValoryActivoInactivo(Constants.Core.Catalogo.DetTipoCredito.CtaParalela);
            var listaTipoCredito = await CatalogoDetalleRepository.ListarPorCatalogoCodigoParaDropDown(Constants.Core.Catalogo.SubTipoCredito);
            listaTipoCredito = listaTipoCredito.Where(p => !excluir.Contains(p.Code)).ToList();

            List<int?> listaSubTipoCredito = new List<int?>();
            listaSubTipoCredito.Add(null);// para poder incluir las presolicitudes retiradas
            listaTipoCredito.ForEach(action: f => listaSubTipoCredito.Add(f.Id));

            var result = await PreSolicitudCabeceraRepository.ReporteParalelo(id, listaSubTipoCredito);
            if (result.Detalle.Any())
                result.Detalle.ForEach(f =>
                {
                    if (string.IsNullOrEmpty(f.SubTipoCredito))
                    {
                        f.SubTipoCreditoDes = f.EstadoPreSolicitud;
                    }
                });
            //if (result != null && result.Detalle.Any())
            //{
            //    result.Detalle.ForEach(f =>
            //    {
            //        f.TipoDeuda = SociaRepository.ObtenerTipoDeuda(f.Dni)?.TipoDeuda;
            //    });
            //}
            result.Cabecera.OficialCredito = result.Cabecera.OficialCredito.Replace("@manuela.org.pe", "", StringComparison.OrdinalIgnoreCase);
            result.Cabecera.Usuario = usuario.Value.Replace("@manuela.org.pe", "", StringComparison.OrdinalIgnoreCase);
            result.Detalle = result.Detalle.OrderBy(o => o.IdSocia).ToList();
            var fullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "PlantillaExcel/reportecreditoparalelo.xlsx");
            FileInfo fi = new FileInfo(fullPath);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var pck = new ExcelPackage(fi);
            var ws = pck.Workbook.Worksheets["Hoja1"];
            ws.Cells["E2"].Value = $"PRE SOLICITUD N° {result.Cabecera.Id}";

            ws.Cells["D4"].Value = result.Cabecera.BancoComunal;
            ws.Cells["D5"].Value = result.Cabecera.BancoComunalCodigo;
            ws.Cells["J4"].Value = result.Cabecera.Ciclo;
            ws.Cells["J5"].Value = result.Cabecera.Sucursal;
            ws.Cells["M4"].Value = result.Cabecera.FechaDesembolso.Value.ToString("dd/MM/yyyy");
            ws.Cells["M5"].Value = result.Detalle.Any() ? result.Detalle.Sum(s => s.Monto) : 0;
            ws.Cells["D6"].Value = result.Cabecera.OficialCredito;
            //ws.Cells["A9"].LoadFromCollection(result.Detalle.OrderBy(o => o.IdSocia), false);
            var iniRow = 9;
            if (result.Detalle.Any())
            {
                result.Detalle.ForEach(f =>
                {
                    ws.Cells[$"A{iniRow}"].Value = f.IdSocia;
                    ws.Cells[$"B{iniRow}"].Value = f.Nombre;
                    ws.Cells[$"C{iniRow}"].Value = f.Dni;
                    ws.Cells[$"D{iniRow}"].Value = f.CobraConDj;
                    ws.Cells[$"E{iniRow}"].Value = f.SubTipoCreditoDes;
                    ws.Cells[$"F{iniRow}"].Value = f.NivelRiesgo;
                    ws.Cells[$"G{iniRow}"].Value = f.CapacidadPago;
                    ws.Cells[$"H{iniRow}"].Value = f.TipoDeuda;
                    ws.Cells[$"I{iniRow}"].Value = f.Monto;
                    ws.Cells[$"J{iniRow}"].Value = f.Plazo;
                    ws.Cells[$"K{iniRow}"].Value = f.TasaInteres;
                    ws.Cells[$"L{iniRow}"].Value = f.PlazoGracia;
                    ws.Cells[$"M{iniRow}"].Value = f.EntidadFinanciera;
                    ws.Cells[$"N{iniRow}"].Value = f.NumeroCuenta;
                    ws.Cells[$"O{iniRow}"].Value = f.Dispositivo;
                    iniRow++;
                });
                var celda = $"I{result.Detalle.Count() + 10}";
                ws.Cells[celda].Formula = $"SUM(I9:I{result.Detalle.Count() + 9})";
            }
            using (MemoryStream ms = new MemoryStream(pck.GetAsByteArray()))
            {
                bytes = ms.ToArray();
            }
            return new ResponseDto()
            {
                Data = Convert.ToBase64String(bytes),
            };
        }

        public async Task<ResponseDto> ReporteAnilloGrupal(int id)
        {
            var usuario = UserIdentity.FindFirst(Constants.Core.UserAsociadoClaims.NombreUsuario);
            var lista = await CatalogoDetalleRepository.ObtenerPorListaCodigoyActivoInactivo(
                new List<string> {
                    Constants.Core.Catalogo.DetSubTipoCredito.AnilloGrupal
                });
            List<int?> listaSubTipoCredito = new List<int?>();
            listaSubTipoCredito.Add(null);// para poder incluir las presolicitudes retiradas
            lista.ForEach(action: f => listaSubTipoCredito.Add(f.Id));
            var result = await PreSolicitudCabeceraRepository.ReporteCtaExterna(id, listaSubTipoCredito);
            if (result.Detalle.Any())
                result.Detalle.ForEach(f =>
                {
                    if (string.IsNullOrEmpty(f.SubTipoCredito))
                    {
                        f.SubTipoCredito = f.EstadoPreSolicitud;
                    }
                    //f.TipoDeuda = SociaRepository.ObtenerTipoDeuda(f.Dni)?.TipoDeuda;
                });
            //var listaPresolicitudNoAceptadas = await PreSolicitudRepository.ObtenerPresolicitudNoAceptadas(id);

            //if (listaPresolicitudNoAceptadas != null && listaPresolicitudNoAceptadas.Any())
            //{
            //    listaPresolicitudNoAceptadas.ForEach(f =>
            //    {
            //        result.Detalle.Add(new ReporteCtaExternaDto._Detalle()
            //        {
            //            IdSocia = f.IdSocia,
            //            Dni = f.Dni,
            //            Nombre = f.Nombre,
            //            ActividadEconomica = f.ActividadEconomica,
            //            Observacion = f.Observacion,
            //            SubTipoCredito = f.EstadoPreSolicitud
            //        });
            //    });
            //}
            result.Cabecera.Monto = result.Detalle.Any() ? result.Detalle.Sum(s => s.Monto) : 0;
            result.Cabecera.Plazo = result.Detalle.Any() ? result.Detalle.Max(s => s.Plazo) : 0;
            result.Cabecera.Producto =
                String.Join("/", result.Detalle.Select(s => s.SubTipoCredito).Distinct().ToList());
            result.Cabecera.OficialCredito = result.Cabecera.OficialCredito.Replace("@manuela.org.pe", "");
            result.Cabecera.Usuario = usuario.Value.Replace("@manuela.org.pe", "");
            result.Detalle = result.Detalle.OrderBy(o => o.IdSocia).ToList();
            return new ResponseDto()
            {
                Data = result,
            };
        }

        public async Task<ResponseDto> ReporteAnilloGrupalExcel(int id)
        {
            byte[] bytes;
            var usuario = UserIdentity.FindFirst(Constants.Core.UserAsociadoClaims.NombreUsuario);
            var lista = await CatalogoDetalleRepository.ObtenerPorListaCodigoyActivoInactivo(
                new List<string> {
                    Constants.Core.Catalogo.DetSubTipoCredito.AnilloGrupal
                });
            List<int?> listaSubTipoCredito = new List<int?>();
            listaSubTipoCredito.Add(null);// para poder incluir las presolicitudes retiradas
            lista.ForEach(action: f => listaSubTipoCredito.Add(f.Id));
            var result = await PreSolicitudCabeceraRepository.ReporteCtaExterna(id, listaSubTipoCredito);
            if (result.Detalle.Any())
                result.Detalle.ForEach(f =>
                {
                    if (string.IsNullOrEmpty(f.SubTipoCredito))
                    {
                        f.SubTipoCreditoDesc = f.DescripcionEstadoPreSolicitud;
                    }
                    //f.TipoDeuda = SociaRepository.ObtenerTipoDeuda(f.Dni)?.TipoDeuda;
                });
            //var listaPresolicitudNoAceptadas = await PreSolicitudRepository.ObtenerPresolicitudNoAceptadas(id);

            //if (listaPresolicitudNoAceptadas != null && listaPresolicitudNoAceptadas.Any())
            //{
            //    listaPresolicitudNoAceptadas.ForEach(f =>
            //    {
            //        result.Detalle.Add(new ReporteCtaExternaDto._Detalle()
            //        {
            //            IdSocia = f.IdSocia,
            //            Dni = f.Dni,
            //            Nombre = f.Nombre,
            //            ActividadEconomica = f.ActividadEconomica,
            //            Observacion = f.Observacion,
            //            SubTipoCredito = f.EstadoPreSolicitud
            //        });
            //    });
            //}

            result.Cabecera.Monto = result.Detalle.Any() ? result.Detalle.Sum(s => s.Monto) : 0;
            result.Cabecera.Plazo = result.Detalle.Any() ? result.Detalle.Max(s => s.Plazo) : 0;
            result.Cabecera.Producto =
                String.Join("/", result.Detalle.Select(s => s.SubTipoCreditoDesc).Distinct().ToList());
            result.Cabecera.OficialCredito = result.Cabecera.OficialCredito.Replace("@manuela.org.pe", "", StringComparison.OrdinalIgnoreCase);
            result.Cabecera.Usuario = usuario.Value.Replace("@manuela.org.pe", "", StringComparison.OrdinalIgnoreCase);
            result.Detalle = result.Detalle.OrderBy(o => o.IdSocia).ToList();
            var fullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "PlantillaExcel/reporteanillogrupal.xlsx");
            FileInfo fi = new FileInfo(fullPath);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var pck = new ExcelPackage(fi);
            var ws = pck.Workbook.Worksheets["Hoja1"];

            ws.Cells["G1"].Value = $"PRE SOLICITUD N° {result.Cabecera.Id}";

            ws.Cells["E3"].Value = result.Cabecera.BancoComunal;
            ws.Cells["E4"].Value = result.Cabecera.BancoComunalCodigo;
            ws.Cells["E5"].Value = result.Cabecera.Ciclo;
            ws.Cells["E6"].Value = result.Cabecera.Sucursal;

            ws.Cells["J3"].Value = result.Cabecera.Producto;
            ws.Cells["J4"].Value = result.Cabecera.FechaDesembolso.Value.ToString("dd/MM/yyyy");
            ws.Cells["J5"].Value = result.Cabecera.OficialCredito;

            ws.Cells["O3"].Value = $"S/ {string.Format("{0:0.00}", result.Cabecera.Monto)}";
            ws.Cells["O5"].Value = result.Cabecera.Plazo;

            //ws.Cells["A9"].LoadFromCollection(result.Detalle.OrderBy(o => o.IdSocia), false);
            var iniRow = 9;
            if (result.Detalle.Any())
            {
                result.Detalle.ForEach(f =>
                {
                    ws.Cells[$"A{iniRow}"].Value = f.IdSocia;
                    ws.Cells[$"B{iniRow}"].Value = f.Nombre;
                    ws.Cells[$"C{iniRow}"].Value = f.Dni;
                    ws.Cells[$"D{iniRow}"].Value = f.Cargo;
                    ws.Cells[$"E{iniRow}"].Value = f.NivelRiesgo;
                    ws.Cells[$"F{iniRow}"].Value = f.CapacidadPago;
                    ws.Cells[$"G{iniRow}"].Value = f.TipoDeuda;
                    ws.Cells[$"H{iniRow}"].Value = f.ActividadEconomica;
                    ws.Cells[$"I{iniRow}"].Value = f.SubTipoCreditoDesc;
                    ws.Cells[$"J{iniRow}"].Value = f.AfiliadaSmv;
                    ws.Cells[$"K{iniRow}"].Value = f.CobraConDj;
                    ws.Cells[$"L{iniRow}"].Value = f.Monto;
                    ws.Cells[$"M{iniRow}"].Value = f.Plazo;
                    ws.Cells[$"N{iniRow}"].Value = f.TasaInteres;
                    ws.Cells[$"O{iniRow}"].Value = f.PlazoGracia;
                    ws.Cells[$"P{iniRow}"].Value = f.Asistencia;
                    ws.Cells[$"Q{iniRow}"].Value = f.EntidadFinanciera;
                    ws.Cells[$"R{iniRow}"].Value = f.NroCuenta;
                    ws.Cells[$"S{iniRow}"].Value = f.Observacion;
                    ws.Cells[$"T{iniRow}"].Value = f.Dispositivo;
                    iniRow++;
                });

                var celda = $"L{result.Detalle.Count() + 10}";
                ws.Cells[celda].Formula = $"SUM(L9:L{result.Detalle.Count() + 9})";
            }
            using (MemoryStream ms = new MemoryStream(pck.GetAsByteArray()))
            {
                bytes = ms.ToArray();
            }
            return new ResponseDto()
            {
                Data = Convert.ToBase64String(bytes),
            };
        }

        public async Task<ResponseDto> ReporteParaleloAnilloGrupal(int id)
        {
            var usuario = UserIdentity.FindFirst(Constants.Core.UserAsociadoClaims.NombreUsuario);
            var lista = await CatalogoDetalleRepository.ObtenerPorListaCodigoyActivoInactivo(
               new List<string> {
                     Constants.Core.Catalogo.DetSubTipoCredito.CreditoAnilloCampaña,
                    Constants.Core.Catalogo.DetSubTipoCredito.CreditoAnilloComplementario
               });
            List<int?> listaSubTipoCredito = new List<int?>();
            listaSubTipoCredito.Add(null);// para poder incluir las presolicitudes retiradas
            lista.ForEach(action: f => listaSubTipoCredito.Add(f.Id));

            var result = await PreSolicitudCabeceraRepository.ReporteParalelo(id, listaSubTipoCredito);
            if (result.Detalle.Any())
                result.Detalle.ForEach(f =>
                {
                    if (string.IsNullOrEmpty(f.SubTipoCredito))
                    {
                        f.SubTipoCredito = f.EstadoPreSolicitudAbrev;
                    }
         
                });
            //if (result != null && result.Detalle.Any())
            //{
            //    result.Detalle.ForEach(f =>
            //    {
            //        f.TipoDeuda = SociaRepository.ObtenerTipoDeuda(f.Dni)?.TipoDeuda;
            //    });
            //}
            result.Cabecera.OficialCredito = result.Cabecera.OficialCredito.Replace("@manuela.org.pe", "", StringComparison.OrdinalIgnoreCase);
            result.Cabecera.Usuario = usuario.Value.Replace("@manuela.org.pe", "", StringComparison.OrdinalIgnoreCase);
            result.Cabecera.Monto = result.Detalle.Any() ? result.Detalle.Sum(s => s.Monto) : 0;
            result.Detalle = result.Detalle.OrderBy(o => o.IdSocia).ToList();
            return new ResponseDto()
            {
                Data = result,
            };
        }

        public async Task<ResponseDto> ReporteParaleloAnilloGrupalExcel(int id)
        {
            byte[] bytes;
            var usuario = UserIdentity.FindFirst(Constants.Core.UserAsociadoClaims.NombreUsuario);
            var lista = await CatalogoDetalleRepository.ObtenerPorListaCodigoyActivoInactivo(
               new List<string> {
                     Constants.Core.Catalogo.DetSubTipoCredito.CreditoAnilloCampaña,
                    Constants.Core.Catalogo.DetSubTipoCredito.CreditoAnilloComplementario
               });
            List<int?> listaSubTipoCredito = new List<int?>();
            listaSubTipoCredito.Add(null);// para poder incluir las presolicitudes retiradas
            lista.ForEach(action: f => listaSubTipoCredito.Add(f.Id));

            var result = await PreSolicitudCabeceraRepository.ReporteParalelo(id, listaSubTipoCredito);
            if (result.Detalle.Any())
                result.Detalle.ForEach(f =>
                {
                    if (string.IsNullOrEmpty(f.SubTipoCredito))
                    {
                        f.SubTipoCreditoDes = f.EstadoPreSolicitud;
                    }

                });
            //if (result != null && result.Detalle.Any())
            //{
            //    result.Detalle.ForEach(f =>
            //    {
            //        f.TipoDeuda = SociaRepository.ObtenerTipoDeuda(f.Dni)?.TipoDeuda;
            //    });
            //}
            result.Cabecera.OficialCredito = result.Cabecera.OficialCredito.Replace("@manuela.org.pe", "", StringComparison.OrdinalIgnoreCase);
            result.Cabecera.Usuario = usuario.Value.Replace("@manuela.org.pe", "", StringComparison.OrdinalIgnoreCase);
            result.Detalle = result.Detalle.OrderBy(o => o.IdSocia).ToList();
            var fullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "PlantillaExcel/reporteparaleloanillogrupal.xlsx");
            FileInfo fi = new FileInfo(fullPath);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var pck = new ExcelPackage(fi);
            var ws = pck.Workbook.Worksheets["Hoja1"];
            ws.Cells["E2"].Value = $"PRE SOLICITUD N° {result.Cabecera.Id}";

            ws.Cells["D4"].Value = result.Cabecera.BancoComunal;
            ws.Cells["D5"].Value = result.Cabecera.BancoComunalCodigo;
            ws.Cells["J4"].Value = result.Cabecera.Ciclo;
            ws.Cells["J5"].Value = result.Cabecera.Sucursal;
            ws.Cells["M4"].Value = result.Cabecera.FechaDesembolso.Value.ToString("dd/MM/yyyy");
            ws.Cells["M5"].Value = result.Detalle.Any() ? result.Detalle.Sum(s => s.Monto) : 0;
            ws.Cells["D6"].Value = result.Cabecera.OficialCredito;
            //ws.Cells["A9"].LoadFromCollection(result.Detalle.OrderBy(o => o.IdSocia), false);
            var iniRow = 9;
            if (result.Detalle.Any())
            {
                result.Detalle.ForEach(f =>
                {
                    ws.Cells[$"A{iniRow}"].Value = f.IdSocia;
                    ws.Cells[$"B{iniRow}"].Value = f.Nombre;
                    ws.Cells[$"C{iniRow}"].Value = f.Dni;
                    ws.Cells[$"D{iniRow}"].Value = f.CobraConDj;
                    ws.Cells[$"E{iniRow}"].Value = f.SubTipoCreditoDes;
                    ws.Cells[$"F{iniRow}"].Value = f.NivelRiesgo;
                    ws.Cells[$"G{iniRow}"].Value = f.CapacidadPago;
                    ws.Cells[$"H{iniRow}"].Value = f.TipoDeuda;
                    ws.Cells[$"I{iniRow}"].Value = f.Monto;
                    ws.Cells[$"J{iniRow}"].Value = f.Plazo;
                    ws.Cells[$"K{iniRow}"].Value = f.TasaInteres;
                    ws.Cells[$"L{iniRow}"].Value = f.PlazoGracia;
                    ws.Cells[$"M{iniRow}"].Value = f.EntidadFinanciera;
                    ws.Cells[$"N{iniRow}"].Value = f.NumeroCuenta;
                    ws.Cells[$"O{iniRow}"].Value = f.Dispositivo;
                    iniRow++;
                });
                var celda = $"I{result.Detalle.Count() + 10}";
                ws.Cells[celda].Formula = $"SUM(I9:I{result.Detalle.Count() + 9})";
            }
            using (MemoryStream ms = new MemoryStream(pck.GetAsByteArray()))
            {
                bytes = ms.ToArray();
            }
            return new ResponseDto()
            {
                Data = Convert.ToBase64String(bytes),
            };
        }

        public async Task<ResponseDto> ReporteParaleloPromocional(int id)
        {
            var usuario = UserIdentity.FindFirst(Constants.Core.UserAsociadoClaims.NombreUsuario);
            var result = await PreSolicitudCabeceraRepository.ReporteParaleloPromocional(id);
            if (result.Detalle.Any())
                result.Detalle.ForEach(f =>
                {
                    if (string.IsNullOrEmpty(f.SubTipoCredito))
                    {
                        f.SubTipoCredito = f.EstadoPreSolicitud;
                    }
                    f.EstadoPreSolicitud = string.Empty;
                    //f.TipoDeuda = SociaRepository.ObtenerTipoDeuda(f.Dni)?.TipoDeuda;
                });

            result.Cabecera.Monto = result.Detalle.Any() ? result.Detalle.Sum(s => s.Monto) : 0;
            result.Cabecera.Plazo = result.Detalle.Any() ? result.Detalle.Max(s => s.Plazo) : 0;
            result.Cabecera.Producto =
                String.Join("/", result.Detalle.Select(s => s.SubTipoCredito).Distinct().ToList());
            result.Cabecera.OficialCredito = result.Cabecera.OficialCredito.Replace("@manuela.org.pe", "", StringComparison.OrdinalIgnoreCase);
            result.Cabecera.Usuario = usuario.Value.Replace("@manuela.org.pe", "", StringComparison.OrdinalIgnoreCase);
            return new ResponseDto()
            {
                Data = result
            };
        }

        public async Task<ResponseDto> ReporteParaleloPromocionalExcel(int id)
        {
            byte[] bytes;
            var usuario = UserIdentity.FindFirst(Constants.Core.UserAsociadoClaims.NombreUsuario);
            var result = await PreSolicitudCabeceraRepository.ReporteParaleloPromocional(id);
            if (result.Detalle.Any())
                result.Detalle.ForEach(f =>
                {
                    if (string.IsNullOrEmpty(f.SubTipoCredito))
                    {
                        f.SubTipoCreditoDesc = f.DescripcionEstadoPreSolicitud;
                    }
                    f.EstadoPreSolicitud = string.Empty;
                    //f.TipoDeuda = SociaRepository.ObtenerTipoDeuda(f.Dni)?.TipoDeuda;
                });

            result.Cabecera.Monto = result.Detalle.Any() ? result.Detalle.Sum(s => s.Monto) : 0;
            result.Cabecera.Plazo = result.Detalle.Any() ? result.Detalle.Max(s => s.Plazo) : 0;
            result.Cabecera.Producto =
                String.Join("/", result.Detalle.Select(s => s.SubTipoCreditoDesc).Distinct().ToList());
            result.Cabecera.OficialCredito = result.Cabecera.OficialCredito.Replace("@manuela.org.pe", "", StringComparison.OrdinalIgnoreCase);
            result.Cabecera.Usuario = usuario.Value.Replace("@manuela.org.pe", "", StringComparison.OrdinalIgnoreCase);
            result.Detalle = result.Detalle.OrderBy(o => o.IdSocia).ToList();
            //var ms = new MemoryStream();
            var fullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "PlantillaExcel/reporteparalelo_promocional.xlsx");
            FileInfo fi = new FileInfo(fullPath);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var pck = new ExcelPackage(fi);
            var ws = pck.Workbook.Worksheets["Hoja1"];

            ws.Cells["E2"].Value = $"PRE SOLICITUD N° {result.Cabecera.Id}";

            ws.Cells["E3"].Value = result.Cabecera.BancoComunal;
            ws.Cells["E4"].Value = result.Cabecera.BancoComunalCodigo;
            ws.Cells["E5"].Value = result.Cabecera.OficialCredito;

            ws.Cells["J3"].Value = result.Cabecera.Ciclo;
            ws.Cells["J4"].Value = result.Cabecera.Sucursal;

            ws.Cells["O3"].Value = result.Cabecera.FechaDesembolso.Value.ToString("dd/MM/yyyy");
            ws.Cells["O5"].Value = result.Detalle.Any() ? result.Detalle.Sum(s => s.Monto) : 0;

            //ws.Cells["A9"].LoadFromCollection(result.Detalle.OrderBy(o => o.IdSocia), false);

            if (result.Detalle.Any())
            {
                var iniRow = 9;
                result.Detalle.ForEach(f =>
                {
                    ws.Cells[$"A{iniRow}"].Value = f.IdSocia;
                    ws.Cells[$"B{iniRow}"].Value = f.Nombre;
                    ws.Cells[$"C{iniRow}"].Value = f.Dni;
                    ws.Cells[$"D{iniRow}"].Value = f.Cargo;
                    ws.Cells[$"E{iniRow}"].Value = f.NivelRiesgo;
                    ws.Cells[$"F{iniRow}"].Value = f.CapacidadPago;
                    ws.Cells[$"G{iniRow}"].Value = f.TipoDeuda;
                    ws.Cells[$"H{iniRow}"].Value = f.ActividadEconomica;
                    ws.Cells[$"I{iniRow}"].Value = f.SubTipoCreditoDesc;
                    ws.Cells[$"J{iniRow}"].Value = f.AfiliadaSmv;
                    ws.Cells[$"K{iniRow}"].Value = f.CobraConDj;
                    ws.Cells[$"L{iniRow}"].Value = f.Monto;
                    ws.Cells[$"M{iniRow}"].Value = f.Plazo;
                    ws.Cells[$"N{iniRow}"].Value = f.TasaInteres;
                    ws.Cells[$"O{iniRow}"].Value = f.PlazoGracia;
                    ws.Cells[$"P{iniRow}"].Value = f.Asistencia;
                    ws.Cells[$"Q{iniRow}"].Value = f.EntidadFinanciera;
                    ws.Cells[$"R{iniRow}"].Value = f.NroCuenta;
                    ws.Cells[$"S{iniRow}"].Value = f.Dispositivo;
                    iniRow++;
                });

                var celda = $"L{result.Detalle.Count() + 10}";
                ws.Cells[celda].Formula = $"SUM(L9:L{result.Detalle.Count() + 9})";
            }
            using (MemoryStream ms = new MemoryStream(pck.GetAsByteArray()))
            {
                bytes = ms.ToArray();
            }
            return new ResponseDto()
            {
                Data = Convert.ToBase64String(bytes),
            };
        }
    }
}