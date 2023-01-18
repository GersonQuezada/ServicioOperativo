using Credimujer.Op.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Application.Interfaces
{
    public interface ISociaReporteApplication
    {
        Task<ResponseDto> ReportePorSocia(int sociaId);

        Task<ResponseDto> ReporteListaSociaPorBanCoComunal(int bancoComunalId);

        Task<ResponseDto> ReporteExcelListaSociaPorBanCoComunal(int bancoComunalId);
    }
}