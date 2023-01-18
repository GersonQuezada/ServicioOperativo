using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Credimujer.Op.Dto.Base;

namespace Credimujer.Op.Dto.Socia
{
    public class CatalogoDto
    {
        public List<DropdownDto> EstadoCivil { get; set; }
        public List<DropdownDto> GradoInstruccion { get; set; }
        public List<DropdownDto> SituacionDomicilio { get; set; }
        public List<DropdownDto> Afirmacion { get; set; }
        public List<DropdownDto> EntidadFinanciera { get; set; }
        public List<DropdownDto> Departamento { get; set; }
        public List<DropdownDto> Sucursal { get; set; }
        public List<DropdownDto> AnilloGrupal { get; set; }
        public List<DropdownDto> CargoBancoComunal { get; set; }
        public List<DropdownDto> TipoPresolicitudAGenerar { get; set; }
        public List<DropdownDto> TipoDocumento { get; set; }
        public List<DropdownDto> EstadoSocia { get; set; }
    }
}