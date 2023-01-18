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
using Credimujer.Op.Model.Service.Iam;
using Credimujer.Op.Repository.Interfaces.Data;
using Credimujer.Op.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Credimujer.Op.Application.Implementations.Perfil
{
    public class PerfilApplication : IPerfilApplication
    {
        private readonly Lazy<IUnitOfWork> _unitOfWork;
        private readonly AppSetting _setting;
        private readonly Lazy<IHttpContextAccessor> _httpContext;
        private readonly Lazy<IIamService> _iamService;

        public PerfilApplication(IOptions<AppSetting> settings, ILifetimeScope lifetimeScope)
        {
            _setting = settings.Value;
            _unitOfWork = new Lazy<IUnitOfWork>(() => lifetimeScope.Resolve<IUnitOfWork>());
            _httpContext = new Lazy<IHttpContextAccessor>(() => lifetimeScope.Resolve<IHttpContextAccessor>());
            _iamService = new Lazy<IIamService>(() => lifetimeScope.Resolve<IIamService>());
        }

        private ClaimsPrincipal UserIdentity => _httpContext.Value.HttpContext.User;
        private IUnitOfWork UnitOfWork => _unitOfWork.Value;

        private IIamService IamService => UnitOfWork.Repository<IIamService>();

        public async Task<ResponseDto> ObtenerDatoUsuario()
        {
            var usuario = UserIdentity.FindFirst(Constants.Core.UserAsociadoClaims.NombreUsuario).Value;
            var response = await IamService.ObtenerDatosUsuario(usuario);
            return response;
        }

        public async Task<ResponseDto> ActualizarCelular(ActualizarCelularUsuarioModel model)
        {
            var usuario = UserIdentity.FindFirst(Constants.Core.UserAsociadoClaims.NombreUsuario).Value;
            model.Usuario = usuario;
            var response = await IamService.ActualizarCelularUsuario(model);
            return response;
        }

        public async Task<ResponseDto> ActualizarContraseniaUsuario(ActualizarPasswordModel model)
        {
            var usuario = UserIdentity.FindFirst(Constants.Core.UserAsociadoClaims.NombreUsuario).Value;
            model.Usuario = usuario;
            var response = await IamService.ActualizarContraseniaUsuario(model);
            return response;
        }
    }
}