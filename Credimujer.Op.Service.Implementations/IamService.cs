using Autofac;
using Credimujer.Op.Common;
using Credimujer.Op.Common.Base;
using Credimujer.Op.Model.Service.Iam;
using Credimujer.Op.Service.Implementations.Base;
using Credimujer.Op.Service.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Credimujer.Op.Service.Implementations
{
    public class IamService : IIamService
    {
        private readonly AppSetting _settings;
        private ILifetimeScope _lifetimeScope;

        public IamService(IOptions<AppSetting> settings, ILifetimeScope lifetimeScope)
        {
            this._settings = settings.Value;
            _lifetimeScope = lifetimeScope;
        }

        public async Task<ResponseDto> RegistrarUsuarioTipoSocia(RegistrarUsuarioModel usuario)
        {
            ResponseDto response;
            var url = _settings.ApiIamSocia.Iam + _settings.ApiIamSocia.Paths.NuevaUsuarioSocia;
            var client = new HttpClientService(_lifetimeScope);
            var result = await client.InvokeWithApiKeyAsync<ResponseDto>(HttpMethod.Post, url, _settings.ApiIamSocia.Name, _settings.ApiIamSocia.Key, usuario);
            response = result;
            return response;
        }

        public async Task<ResponseDto> ObtenerDatosUsuario(string usuario)
        {
            ResponseDto response;
            var url = _settings.ApiIamOperativo.Iam + _settings.ApiIamOperativo.Paths.ObtenerDatosUsuario + $"/{usuario}"; ;
            var client = new HttpClientService(_lifetimeScope);
            var result = await client.InvokeWithApiKeyAsync<ResponseDto>(HttpMethod.Get, url, _settings.ApiIamOperativo.Name, _settings.ApiIamOperativo.Key, usuario);
            response = result;
            return response;
        }

        public async Task<ResponseDto> ActualizarCelularUsuario(ActualizarCelularUsuarioModel model)
        {
            ResponseDto response;
            var url = _settings.ApiIamOperativo.Iam + _settings.ApiIamOperativo.Paths.ActualizarCelularUsuario;
            var client = new HttpClientService(_lifetimeScope);
            var result = await client.InvokeWithApiKeyAsync<ResponseDto>(HttpMethod.Post, url, _settings.ApiIamOperativo.Name, _settings.ApiIamOperativo.Key, model);
            response = result;
            return response;
        }

        public async Task<ResponseDto> ActualizarContraseniaUsuario(ActualizarPasswordModel model)
        {
            ResponseDto response;
            var url = _settings.ApiIamOperativo.Iam + _settings.ApiIamOperativo.Paths.ActualizarContraseniaUsuario;
            var client = new HttpClientService(_lifetimeScope);
            var result = await client.InvokeWithApiKeyAsync<ResponseDto>(HttpMethod.Post, url, _settings.ApiIamOperativo.Name, _settings.ApiIamOperativo.Key, model);
            response = result;
            return response;
        }

        public async Task<ResponseDto> ActualizarCuentaUsuarioConDni(ActualizarUsuarioConDniModel model)
        {
            ResponseDto response;
            var url = _settings.ApiIamOperativo.Iam + _settings.ApiIamOperativo.Paths.ActualizarCuentaUsuarioConDni;
            var client = new HttpClientService(_lifetimeScope);
            var result = await client.InvokeWithApiKeyAsync<ResponseDto>(HttpMethod.Post, url, _settings.ApiIamOperativo.Name, _settings.ApiIamOperativo.Key, model);
            response = result;
            return response;
        }

        public async Task<ResponseDto> EliminarSocia(UsuarioModel model)
        {
            ResponseDto response;
            var url = _settings.ApiIamOperativo.Iam + _settings.ApiIamOperativo.Paths.EliminarSocia;
            var client = new HttpClientService(_lifetimeScope);
            var result = await client.InvokeWithApiKeyAsync<ResponseDto>(HttpMethod.Post, url, _settings.ApiIamOperativo.Name, _settings.ApiIamOperativo.Key, model);
            response = result;
            return response;
        }
        public async Task<ResponseDto> ListaOficialPorSucursal(string sucursalCodigo)
        {
            ResponseDto response;
            var url = _settings.ApiIamOperativo.Iam + _settings.ApiIamOperativo.Paths.ListaOficialPorSucursal + $"/{sucursalCodigo}"; ;
            var client = new HttpClientService(_lifetimeScope);
            var result = await client.InvokeWithApiKeyAsync<ResponseDto>(HttpMethod.Get, url, _settings.ApiIamOperativo.Name, _settings.ApiIamOperativo.Key, sucursalCodigo);
            response = result;
            return response;
        }
    }
}