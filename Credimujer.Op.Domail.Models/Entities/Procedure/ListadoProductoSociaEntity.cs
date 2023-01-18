using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Credimujer.Op.Domail.Models.Entities.Procedure
{
    [Keyless]
    public class ListadoProductoSociaEntity
    {
        [Column("NUM_PRO_CRE")]
        public int Id { get; set; }

        [Column("PRODUCTO")]
        public string Producto { get; set; }

        [Column("BANCOCOMUNAL")]
        public string BancoComunal { get; set; }

        [Column("CicloBanco")]
        public string Cuota { get; set; }

        [Column("FEC_DESEMB")]
        public DateTime FechaDesembolso { get; set; }

        [Column("MTO_DESEMBOLSADO")]
        public decimal? MontoDesembolso { get; set; }

        [Column("ENTI_DESEMB")]
        public string Banco { get; set; }

        [Column("Estado")]
        public string Estado { get; set; }

        [NotMapped]
        public string Socia { get; set; }
    }
}