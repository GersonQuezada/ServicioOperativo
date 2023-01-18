using Credimujer.Op.Common.Base;
using Credimujer.Op.Model.Service.Iam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Service.Interfaces
{
    public interface IIamService
    {
        Task<ResponseDto> RegistrarUsuarioTipoSocia(RegistrarUsuarioModel usuario);

        Task<ResponseDto> ObtenerDatosUsuario(string usuario);

        Task<ResponseDto> ActualizarCelularUsuario(ActualizarCelularUsuarioModel model);

        Task<ResponseDto> ActualizarContraseniaUsuario(ActualizarPasswordModel model);

        Task<ResponseDto> ActualizarCuentaUsuarioConDni(ActualizarUsuarioConDniModel model);

        Task<ResponseDto> EliminarSocia(UsuarioModel model);
        Task<ResponseDto> ListaOficialPorSucursal(string sucursalCodigo);
    }
}