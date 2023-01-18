using Credimujer.Op.Model.Socia.Registrar;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Api.Filter.Socia
{
    public class RegistrarSociaValidation : AbstractValidator<NuevaSociaModel>
    {
        public RegistrarSociaValidation()
        {
            RuleSet("NuevaSocia", () =>
            {
                RuleFor(x => x.TipoDocumentoId).NotEmpty().NotNull().NotEqual(0).GreaterThanOrEqualTo(0).WithMessage("Debe ingresar tipo de documento");
                RuleFor(x => x.Celular).Length(9).When(p => !string.IsNullOrEmpty(p.Celular)).WithMessage("El celular debe tener 9 dígitos");
                RuleFor(x => x.BancoComunalId).NotEmpty().NotNull().GreaterThanOrEqualTo(0).WithMessage("Debe ingresar banco comunal");
            });
        }
    }
}