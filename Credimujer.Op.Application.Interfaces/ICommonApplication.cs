using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Credimujer.Op.Common.Base;

namespace Credimujer.Op.Application.Interfaces
{
    public interface ICommonApplication
    {
        Task<ResponseDto> ObtenerProvincia(string codigoDepartamento);
        Task<ResponseDto> ObtenerDistrito(string codigoDepartamento, string codigoProv);
    }
}
