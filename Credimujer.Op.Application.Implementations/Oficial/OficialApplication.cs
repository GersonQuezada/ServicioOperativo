using Autofac;
using Credimujer.Op.Application.Interfaces;
using Credimujer.Op.Common;
using Credimujer.Op.Common.Base;
using Credimujer.Op.Dto.Base;
using Credimujer.Op.Dto.Oficial;
using Credimujer.Op.Model.Oficial;
using Credimujer.Op.Repository.Interfaces;
using Credimujer.Op.Repository.Interfaces.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Credimujer.Op.Repository.Implementations;
using Credimujer.Op.Service.Interfaces;
using Credimujer.Op.Dto.PreSolicitud.EstadoPreSolicitud;
using System.Linq;
using OfficeOpenXml;
using System.IO;
using System.Reflection;

namespace Credimujer.Op.Application.Implementations.Oficial
{
    public class OficialApplication : IOficialApplication
    {
        private readonly Lazy<IUnitOfWork> _unitOfWork;
        private readonly AppSetting _setting;
        private readonly Lazy<IHttpContextAccessor> _httpContext;
        private readonly Lazy<IIamService> _iamService;

        public OficialApplication(IOptions<AppSetting> settings, ILifetimeScope lifetimeScope)
        {
            _setting = settings.Value;
            _unitOfWork = new Lazy<IUnitOfWork>(() => lifetimeScope.Resolve<IUnitOfWork>());
            _httpContext = new Lazy<IHttpContextAccessor>(() => lifetimeScope.Resolve<IHttpContextAccessor>());
            _iamService = new Lazy<IIamService>(() => lifetimeScope.Resolve<IIamService>());
        }

        private ClaimsPrincipal UserIdentity => _httpContext.Value.HttpContext.User;
        private IUnitOfWork UnitOfWork => _unitOfWork.Value;
        private IPreSolicitudRepository PreSolicitudRepository => UnitOfWork.Repository<IPreSolicitudRepository>();
        private IIamService IamService => _iamService.Value;

        private ICatalogoDetalleRepository CatalogoDetalleRepository =>
            UnitOfWork.Repository<ICatalogoDetalleRepository>();

        private IPreSolicitudCabeceraRepository PreSolicitudCabeceraRepository =>
            UnitOfWork.Repository<IPreSolicitudCabeceraRepository>();

        public async Task<ResponseDto> Catalogo(CatalogoModel model)
        {
            var comunDto = new CatalogoDto();
            if (model.ObtenerSucursal)
                comunDto.ListaSucursal =
                    await CatalogoDetalleRepository.ListarPorCatalogoCodigoParaDropDown(Constants.Core.Catalogo
                        .Sucursal);
            return new ResponseDto
            {
                Data = comunDto
            };
        }

        public async Task<ResponseDto<PaginationResultDTO<ResultadoResumenRegistroIngresadoDto>>> BuscarResumenDeRegistrosIngresedos(BuscarResumenIngresoModel model)
        {
            var result = await PreSolicitudRepository.ObtenerPorUsuarioySucursal(model);
            if (result.Data.Results.Any())
            {
                foreach (var v in result.Data.Results)
                {
                    v.FechaIni = model.FechaInicio.HasValue ? model.FechaInicio.Value.Date : null;
                    v.FechaFin = model.FechaFin.HasValue ? model.FechaFin.Value.Date : null;
                }
            }
            return result;
        }

        public async Task<ResponseDto> ListaOficialPorSucursal(string sucursalCodigo)
        {
            var result = await IamService.ListaOficialPorSucursal(sucursalCodigo);
            return result;
        }

        public async Task<ResponseDto<PaginationResultDTO<PresolicitudIngresadosDto>>> ObtenerPresolicitudesRegistradas(BuscarPresolicitudPorUsuarioModel model)
        {
            model.FechaInicio = model.FechaInicio.HasValue ? model.FechaInicio.Value.Date : model.FechaInicio;
            model.FechaFin = model.FechaFin.HasValue ? model.FechaFin.Value.Date : model.FechaFin;
            var result = await PreSolicitudCabeceraRepository.ObtenerPorUsuario(model);
            foreach (var v in result.Data.Results)
            {
                v.DetalleTipoCredito = v.DetalleTipoCredito.Where(p => !string.IsNullOrEmpty(p)).Distinct().ToList();
            }
            return result;
        }

        public async Task<ResponseDto> ReporteResumenDeRegistrosIngresados(ReportePresolicitudPorUsuarioModel model)
        {
            model.FechaInicio = model.FechaInicio.HasValue ? model.FechaInicio.Value.Date : model.FechaInicio;
            model.FechaFin = model.FechaFin.HasValue ? model.FechaFin.Value.Date : model.FechaFin;
            var result = await PreSolicitudCabeceraRepository.ReporteRegistrosIngresados(model);
            if (result != null && result.Cabecera != null)
                result.Cabecera.Oficial = result.Cabecera.Oficial.Replace("@manuela.org.pe", "", StringComparison.OrdinalIgnoreCase);
            if (result != null && result.Detalle.Any())
                foreach (var v in result.Detalle)
                {
                    v.AbreviaturaTipoCredito = v.AbreviaturaTipoCredito.Where(p => !string.IsNullOrEmpty(p)).Distinct().ToList();
                }
            return new ResponseDto
            {
                Data = result
            };
        }

        public async Task<ResponseDto> ReporteResumenDeRegistrosIngresadosExcel
            (ReportePresolicitudPorUsuarioModel model)
        {
            model.FechaInicio = model.FechaInicio.HasValue ? model.FechaInicio.Value.Date : model.FechaInicio;
            model.FechaFin = model.FechaFin.HasValue ? model.FechaFin.Value.Date : model.FechaFin;
            var result = await PreSolicitudCabeceraRepository.ReporteRegistrosIngresados(model);
            if (result != null && result.Cabecera != null)
                result.Cabecera.Oficial = result.Cabecera.Oficial.Replace("@manuela.org.pe", "", StringComparison.OrdinalIgnoreCase);
            if (result != null && result.Detalle.Any())
                foreach (var v in result.Detalle)
                {
                    v.AbreviaturaTipoCredito = v.AbreviaturaTipoCredito.Where(p => !string.IsNullOrEmpty(p)).Distinct().ToList();
                }
            var fullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "PlantillaExcel/reporteResumenDeRegistrosIngresados.xlsx");
            FileInfo fi = new FileInfo(fullPath);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var pck = new ExcelPackage(fi);
            var ws = pck.Workbook.Worksheets["Hoja1"];
            byte[] bytes;
            ws.Cells["C4"].Value = result.Cabecera.Sucursal;
            ws.Cells["E4"].Value = result.Cabecera.Oficial;
            ws.Cells["H4"].Value = result.Cabecera.CantidadRegistros;

            //ws.Cells["A8"].LoadFromCollection(result.Detalle.OrderBy(o => o.Id), false);
            var iniRow = 8;
            if (result.Detalle.Any())
            {
                result.Detalle.ForEach(f =>
                {
                    ws.Cells[$"A{iniRow}"].Value = f.Id;
                    ws.Cells[$"B{iniRow}"].Value = string.IsNullOrEmpty(f.AnilloGrupal) ? f.BancoComunal : f.AnilloGrupal;
                    ws.Cells[$"C{iniRow}"].Value = f.FechaDesembolso;
                    ws.Cells[$"D{iniRow}"].Value = string.Join(',', f.AbreviaturaTipoCredito);
                    ws.Cells[$"E{iniRow}"].Value = f.Monto;
                    ws.Cells[$"F{iniRow}"].Value = f.Plazo;
                    ws.Cells[$"G{iniRow}"].Value = f.Estado;
                    iniRow++;
                });

                var celda = $"E{result.Detalle.Count() + 10}";
                ws.Cells[celda].Formula = $"SUM(E8:E{result.Detalle.Count() + 9})";
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