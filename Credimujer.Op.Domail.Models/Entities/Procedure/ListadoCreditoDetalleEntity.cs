using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Credimujer.Op.Domail.Models.Entities.Procedure
{
    [Keyless]
    public class ListadoCreditoDetalleEntity
    {
        [Column("NRO_CUOTA_PK")]
        public int Cuota { get; set; }

        [Column("FEC_VENCIM")]
        public DateTime FechaVencimiento { get; set; }

        [Column("TOTAL_PAGAR")]
        public decimal MontoPagar { get; set; }

        [Column("Estado")]
        public string Estado { get; set; }

        [Column("DiasAtrazo")]
        public int? DiasRetraso { get; set; }
    }
}