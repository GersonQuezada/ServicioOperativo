using Autofac;
using Credimujer.Op.Application.Interfaces;
using Credimujer.Op.Common;
using Credimujer.Op.Common.Base;
using Credimujer.Op.Common.Exceptions;
using Credimujer.Op.Dto.Socia.Reporte;
using Credimujer.Op.Repository.Interfaces;
using Credimujer.Op.Repository.Interfaces.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Credimujer.Op.Application.Implementations.Socia
{
    public class SociaReporteApplication : ISociaReporteApplication
    {
        private readonly Lazy<IUnitOfWork> _unitOfWork;
        private readonly Lazy<IHttpContextAccessor> _httpContext;
        private readonly AppSetting _setting;

        public SociaReporteApplication(IOptions<AppSetting> settings, ILifetimeScope lifetimeScope)
        {
            _setting = settings.Value;
            _unitOfWork = new Lazy<IUnitOfWork>(() => lifetimeScope.Resolve<IUnitOfWork>());
            _httpContext = new Lazy<IHttpContextAccessor>(() => lifetimeScope.Resolve<IHttpContextAccessor>());
        }

        private ClaimsPrincipal UserIdentity => _httpContext.Value.HttpContext.User;
        private IUnitOfWork UnitOfWork => _unitOfWork.Value;
        private ISociaRepository SociaRepository => UnitOfWork.Repository<ISociaRepository>();
        private IDistritoRepository DistritoRepository => UnitOfWork.Repository<IDistritoRepository>();
        private IBancoComunalRepository BancoComunalRepository => UnitOfWork.Repository<IBancoComunalRepository>();
        private ICatalogoDetalleRepository CatalogoDetalleRepository => UnitOfWork.Repository<ICatalogoDetalleRepository>();
        private IPreSolicitudCabeceraRepository PreSolicitudCabeceraRepository => UnitOfWork.Repository<IPreSolicitudCabeceraRepository>();

        public async Task<ResponseDto> ReportePorSocia(int sociaId)
        {
            var usuario = UserIdentity.FindFirst(Constants.Core.UserAsociadoClaims.NombreUsuario);
            var result = await SociaRepository.ObtenerDatosPorId(sociaId);
            var listaNivelRiesgo = await CatalogoDetalleRepository.ListarPorCatalogoCodigoParaDropDown(Constants.Core.Catalogo.NivelRiesgo);
            if (result.Ubicacion != null)
            {
                var departamentoCodigo = result.Ubicacion.Substring(0, 2);
                var provinciaCodigo = result.Ubicacion.Substring(2, 2);
                var distritoCodigo = result.Ubicacion.Substring(4, 2);

                //var departamentoNegocioCodigo = result.UbicacionNegocio.Substring(0, 2);
                //var provinciaNegocioCodigo = result.UbicacionNegocio.Substring(2, 2);
                //var distritoNegocioCodigo = result.UbicacionNegocio.Substring(4, 2);

                var distrito = await DistritoRepository.ObtenerPorCodigo(departamentoCodigo, provinciaCodigo, distritoCodigo);
                //var distritoNegocio = await DistritoRepository.ObtenerPorCodigo(departamentoNegocioCodigo, provinciaNegocioCodigo, distritoNegocioCodigo);

                result.Departamento = distrito.Departamento.Descripcion;
                result.Provincia = distrito.Provincia.Descripcion;
                result.Distrito = distrito.Descripcion;
            }
            else
            {
                result.Departamento = "-";
                result.Provincia = "-";
                result.Distrito = "-";
            }

            result.Edad = CalcularEdad(result.FechaNac??DateTime.Today);
            var nivelRiesgo = SociaRepository.ObtenerTipoRiesgo(result.Dni)?.RiesgoMalla;
            result.TipoDeuda = SociaRepository.ObtenerTipoDeuda(result.Dni)?.TipoDeuda;
            result.NivelRiesgo = listaNivelRiesgo.FirstOrDefault(f => f.Code == nivelRiesgo)?.Code;
            result.Usuario = usuario.Value.Replace("@manuela.org.pe", "", StringComparison.OrdinalIgnoreCase);

            if (result.TipoDeuda != null && result.NivelRiesgo != null)
                return new ResponseDto
                {
                    Data = result,
                };
            if (result.IdSocia.HasValue)
                return new ResponseDto
                {
                    Data = result,
                };
            var dataTipoDeudaNivelRiego =
                await PreSolicitudCabeceraRepository.ObtenerTipoDeudaNivelRiesgoPorSocia(sociaId);

            result.TipoDeuda ??= dataTipoDeudaNivelRiego?.TipoDeuda;
            result.NivelRiesgo ??= dataTipoDeudaNivelRiego?.NivelRiesgo;


            return new ResponseDto
            {
                Data = result,
            };
        }

        public async Task<ResponseDto> ReporteListaSociaPorBanCoComunal(int bancoComunalId)
        {
            var result = await GetReporteListaSocia(bancoComunalId);
            byte[] bytes;
            var fullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "PlantillaExcel/reporteinformacionsocias.xlsx");
            FileInfo fi = new FileInfo(fullPath);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var pck = new ExcelPackage(fi);
            var ws = pck.Workbook.Worksheets["Hoja1"];

            ws.Cells["I4"].Value = result.Cabecera.Sucursal;
            ws.Cells["M4"].Value = result.Cabecera.BancoComunal;
            ws.Cells["R4"].Value = result.Cabecera.Ciclo;

            //ws.Cells["A9"].LoadFromCollection(result.Detalle, false);
            int row = 9;
            foreach (var x in result.Detalle)
            {
                if (x.UltimaModificacion != null && x.UltimaModificacion >= DateTime.Now)
                {
                    ws.Rows[row].Style.Font.Bold = true;
                    ws.Rows[row].Style.Font.UnderLine = true;
                }
                ws.Cells[row, 1].Value = x.IdSocia;
                ws.Cells[row, 2].Value = x.Nombre;
                ws.Cells[row, 3].Value = x.TipoDocumento;
                ws.Cells[row, 4].Value = x.NroDni;
                ws.Cells[row, 5].Value = x.EstadoCivil;
                ws.Cells[row, 6].Value = x.FechaNacimiento;

                ws.Cells[row, 7].Value = x.Producto;
                ws.Cells[row, 8].Value = x.Cargo;
                ws.Cells[row, 9].Value = x.Edad;
                ws.Cells[row, 10].Value = x.GradoInstruccion;
                ws.Cells[row, 11].Value = x.Riesgo;
                ws.Cells[row, 12].Value = x.TipoDeuda;
                ws.Cells[row, 13].Value = x.NroDependiente;
                ws.Cells[row, 14].Value = x.Celular;
                ws.Cells[row, 15].Value = x.Telefono;
                ws.Cells[row, 16].Value = x.TipoVivienda;
                ws.Cells[row, 17].Value = x.ActividadEconomica;
                ws.Cells[row, 18].Value = x.EntidadFinanciera;
                ws.Cells[row, 19].Value = x.NroCuenta;

                ws.Cells[row, 20].Value = x.Departamento;
                ws.Cells[row, 21].Value = x.Provincia;
                ws.Cells[row, 22].Value = x.Distrito;
                ws.Cells[row, 23].Value = x.Localidad;
                ws.Cells[row, 24].Value = x.Direccion;
                ws.Cells[row, 25].Value = x.Referencia;

                ws.Cells[row, 26].Value = x.DepartamentoNegocio;
                ws.Cells[row, 27].Value = x.ProvinciaNegocio;
                ws.Cells[row, 28].Value = x.DistritoNegocio;
                //ws.Cells[row, 28].Value = x.LocalidadNegocio;
                ws.Cells[row, 29].Value = x.DireccionNegocio;
                ws.Cells[row, 30].Value = x.ReferenciaNegocio;
                ws.Cells[row, 31].Value = x.Reingreso;

                row++;
            }

            using (MemoryStream ms = new MemoryStream(pck.GetAsByteArray()))
            {
                bytes = ms.ToArray();
            }

            return new ResponseDto()
            {
                Data ={
                    pdf= result,//await GetReporteListaSocia(bancoComunalCodigo) ,
                    excel=Convert.ToBase64String(bytes),
                }
            };
        }

        public async Task<ResponseDto> ReporteExcelListaSociaPorBanCoComunal(int bancoComunalId)
        {
            byte[] bytes;
            var result = await GetReporteListaSocia(bancoComunalId);
            var fullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "PlantillaExcel/reporteinformacionsocias.xlsx");
            FileInfo fi = new FileInfo(fullPath);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var pck = new ExcelPackage(fi);
            var ws = pck.Workbook.Worksheets["Hoja1"];

            ws.Cells["E4"].Value = result.Cabecera.Sucursal;
            ws.Cells["I4"].Value = result.Cabecera.BancoComunal;
            ws.Cells["M4"].Value = result.Cabecera.Ciclo;

            //ws.Cells["A9"].LoadFromCollection(result.Detalle, false);

            using (MemoryStream ms = new MemoryStream(pck.GetAsByteArray()))
            {
                bytes = ms.ToArray();
            }
            return new ResponseDto()
            {
                Data = Convert.ToBase64String(bytes),
            };
        }

        #region method private

        private static int CalcularEdad(DateTime fechaNacimiento)
        {
            // Obtiene la fecha actual:
            DateTime fechaActual = DateTime.Today;

            // Comprueba que la se haya introducido una fecha válida; si
            // la fecha de nacimiento es mayor a la fecha actual se muestra mensaje
            // de advertencia:
            if (fechaNacimiento > fechaActual)
            {
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError,
                    "La fecha de nacimiento es mayor que la actual.");
            }
            else
            {
                int edad = fechaActual.Year - fechaNacimiento.Year;

                // Comprueba que el mes de la fecha de nacimiento es mayor
                // que el mes de la fecha actual:
                if (fechaNacimiento.Month > fechaActual.Month)
                {
                    --edad;
                }

                return edad;
            }
        }

        private async Task<ReporteListaSociaDto> GetReporteListaSocia(int bancoComunalId)
        {
            var reporte = new ReporteListaSociaDto();

            var usuario = UserIdentity.FindFirst(Constants.Core.UserAsociadoClaims.NombreUsuario);
            var result = await SociaRepository.ObtenerListaSociaReporte(bancoComunalId);
            var listaNivelRiesgo = await CatalogoDetalleRepository.ListarPorCatalogoCodigoParaDropDown(Constants.Core.Catalogo.NivelRiesgo);
            var listaSociaSinIdSistemaExterno = result.Where(p => !p.IdSocia.HasValue).Select(s => s.Id).ToList();
            var listaSociaConDatosExtras =
                await PreSolicitudCabeceraRepository.ObtenerUltPresolicitudPorListaSocia(listaSociaSinIdSistemaExterno);
            if (result.Any())
            {
                var listaDistrito = await DistritoRepository.Lista();
                foreach (var item in result)
                {
                    var nivelRiesgo = SociaRepository.ObtenerTipoRiesgo(item.NroDni)?.RiesgoMalla;

                    item.UltimaModificacion = item.UltimaModificacion.HasValue ? item.UltimaModificacion.Value.AddDays(_setting.MostrarCambioFormularioPorCantidadDias)
                        : item.UltimaModificacion;
                    item.Edad = item.FechaNacimiento != null ? CalcularEdad(item.FechaNacimiento ?? DateTime.MinValue) : 0;
                    
                    item.TipoDeuda =  SociaRepository.ObtenerTipoDeuda(item.NroDni)?.TipoDeuda ;
                    item.Riesgo = listaNivelRiesgo.FirstOrDefault(f => f.Code == nivelRiesgo)?.Code;

                    if (!item.IdSocia.HasValue &&(item.TipoDeuda == null || item.Riesgo == null))
                    {
                        item.TipoDeuda ??= listaSociaConDatosExtras.FirstOrDefault(f => f.SociaId == item.Id)?.TipoDeuda;
                        item.Riesgo ??= listaSociaConDatosExtras.FirstOrDefault(f => f.SociaId == item.Id)?.NivelRiesgo;
                    }

                    if (string.IsNullOrEmpty(item.Ubicacion))
                    {
                        item.Departamento = "-";
                        item.Provincia = "-";
                        item.Distrito = "-";
                    }
                    else
                    {
                        var departamentoCodigo = item.Ubicacion.Substring(0, 2);
                        var provinciaCodigo = item.Ubicacion.Substring(2, 2);
                        var distritoCodigo = item.Ubicacion.Substring(4, 2);
                        var distrito = listaDistrito.FirstOrDefault(p => p.Codigo == distritoCodigo
                                                                         && p.ProvinciaCodigo == provinciaCodigo && p.DepartamentoCodigo == departamentoCodigo);

                        item.Departamento = distrito.Departamento.Descripcion;
                        item.Provincia = distrito.Provincia.Descripcion;
                        item.Distrito = distrito.Descripcion;

                        var departamentoCodigoNegocio = item.Ubicacion.Substring(0, 2);
                        var provinciaCodigoNegocio = item.Ubicacion.Substring(2, 2);
                        var distritoCodigoNegocio = item.Ubicacion.Substring(4, 2);
                        var distritoNegocio = listaDistrito.FirstOrDefault(p => p.Codigo == distritoCodigoNegocio
                                                                         && p.ProvinciaCodigo == provinciaCodigoNegocio && p.DepartamentoCodigo == departamentoCodigoNegocio);
                        item.DepartamentoNegocio = distritoNegocio.Departamento.Descripcion;
                        item.ProvinciaNegocio = distritoNegocio.Provincia.Descripcion;
                        item.DistritoNegocio = distritoNegocio.Descripcion;
                    }
                }
            }
            else
            {
                throw new FunctionalException(Constants.SystemStatusCode.FunctionalError, "No se encontró registros.");
            }
            reporte.Detalle = result;

            var bancoComunal = await BancoComunalRepository.ObtenerPorCodigo(bancoComunalId);

            reporte.Cabecera.Sucursal = bancoComunal.Sucursal;
            reporte.Cabecera.BancoComunal = bancoComunal.BancoComunal;
            reporte.Cabecera.Ciclo = bancoComunal.Ciclo;
            reporte.Cabecera.Usuario = usuario.Value.Replace("@manuela.org.pe", "", StringComparison.OrdinalIgnoreCase);
            return reporte;
        }

        #endregion method private
    }
}