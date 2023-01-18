using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Credimujer.Op.Domail.Models.Entities.Procedure
{
    [Keyless]
    public class SociaMotivoBajasEntity
    {
        [Column("DES_ASOC_COMUNAL")]
        public string BancoComunal { get; set; }

        [Column("NUM_DOC")]
        public string Dni { get; set; }

        [Column("NOM_CLI_LARGO")]
        public string Socia { get; set; }

        [Column("TIPOBAJA")]
        public string TipoBaja { get; set; }

        [Column("MOTIVOBAJA")]
        public string Motivo { get; set; }

        [Column("FECHA_BAJA")]
        public DateTime? FechaBaja { get; set; }
    }
}