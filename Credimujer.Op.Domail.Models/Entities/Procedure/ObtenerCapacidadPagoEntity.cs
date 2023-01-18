using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Domail.Models.Entities.Procedure
{
    [Keyless]
    public class ObtenerCapacidadPagoEntity
    {
        public decimal CapacidadPago { get; set; }
    }
}