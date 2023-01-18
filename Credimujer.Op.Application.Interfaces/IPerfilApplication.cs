using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Credimujer.Op.Common.Base;
using Credimujer.Op.Model.Service.Iam;

namespace Credimujer.Op.Application.Interfaces
{
    public interface IPerfilApplication
    {
        Task<ResponseDto> ObtenerDatoUsuario();
        Task<ResponseDto> ActualizarCelular(ActualizarCelularUsuarioModel model);
        Task<ResponseDto> ActualizarContraseniaUsuario(ActualizarPasswordModel model);

    }
}
