using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Credimujer.Op.Domail.Models.Entities.Procedure
{
    [Keyless]
    public class ListadoCreditoCabeceraEntity
    {
        [Column("FECH_IN")]
        public DateTime FechaInicio { get; set; }

        [Column("FECH_FIN")]
        public DateTime FechaFin { get; set; }

        [Column("MTO_DESEMBOLSADO")]
        public decimal MontoDesemBolsado { get; set; }

        [Column("NOM_PROMOTOR_ABC")]
        public string Promotor { get; set; }

        [NotMapped]
        public string Id { get; set; }
    }
}