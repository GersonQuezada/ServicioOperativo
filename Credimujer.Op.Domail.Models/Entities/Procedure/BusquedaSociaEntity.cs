using Microsoft.EntityFrameworkCore;

namespace Credimujer.Op.Domail.Models.Entities.Procedure
{
    [Keyless]
    public class BusquedaSociaEntity
    {
        public string BancoComunal { get; set; }
        public string Nombre { get; set; }
        public string NumeroDocumento { get; set; }
    }
}