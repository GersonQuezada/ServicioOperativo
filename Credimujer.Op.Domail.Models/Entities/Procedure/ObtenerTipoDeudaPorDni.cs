using Microsoft.EntityFrameworkCore;

namespace Credimujer.Op.Domail.Models.Entities.Procedure
{
    [Keyless]
    public class ObtenerTipoDeudaPorDni
    {
        public string TipoDeuda { get; set; }
    }
}