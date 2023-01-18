using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Credimujer.Op.Application.Interfaces;
using Credimujer.Op.Common;
using Credimujer.Op.Common.Base;
using Credimujer.Op.Dto.Base;
using Credimujer.Op.Repository.Interfaces;
using Credimujer.Op.Repository.Interfaces.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Credimujer.Op.Application.Implementations
{
    public class CommonApplication: ICommonApplication
    {
        private readonly Lazy<IUnitOfWork> _unitOfWork;
        private readonly AppSetting _setting;
        private readonly Lazy<IHttpContextAccessor> _httpContext;
        public CommonApplication(IOptions<AppSetting> settings,
            ILifetimeScope lifetimeScope

        )
        {
            _setting = settings.Value;
            _unitOfWork = new Lazy<IUnitOfWork>(() => lifetimeScope.Resolve<IUnitOfWork>());
            _httpContext = new Lazy<IHttpContextAccessor>(() => lifetimeScope.Resolve<IHttpContextAccessor>());

        }
        #region Properties

        public IUnitOfWork UnitOfWork => _unitOfWork.Value;
        private ClaimsPrincipal UserIdentity => _httpContext.Value.HttpContext.User;
        private IDepartamentoRepository DepartamentoRepository => UnitOfWork.Repository<IDepartamentoRepository>();
        private IProvinciaRepository ProvinciaRepository => UnitOfWork.Repository<IProvinciaRepository>();
        private IDistritoRepository DistritoRepository => UnitOfWork.Repository<IDistritoRepository>();
        #endregion
        public async Task<ResponseDto> ObtenerProvincia(string codigoDepartamento)
        {
            var result = await ProvinciaRepository.GetWhere(p => p.DepartamentoCodigo == codigoDepartamento);
            var data = result.Select(s => new DropdownDto()
            {
                Code = s.Codigo,
                Description = s.Descripcion
            }).ToList();
            return new ResponseDto()
            {
                Data = data
            };
        }
        public async Task<ResponseDto> ObtenerDistrito(string codigoDepartamento, string codigoProv)
        {
            var result = await DistritoRepository
                .GetWhere(p => p.DepartamentoCodigo == codigoDepartamento && p.ProvinciaCodigo == codigoProv);
            var data = result.Select(s => new DropdownDto()
            {
                Code = s.Codigo,
                Description = s.Descripcion
            }).ToList();
            return new ResponseDto()
            {
                Data = data
            };
        }
    }
}
