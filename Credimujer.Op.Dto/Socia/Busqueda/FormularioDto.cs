using System;

namespace Credimujer.Op.Dto.Socia.Busqueda
{
    public class FormularioDto
    {

        public string NroDependiente { get; set; }
        public string ActividadEconomica { get; set; }
        public string ActividadEconomica2 { get; set; }
        public string ActividadEconomica3 { get; set; }
        public string Celular { get; set; }
        public string Ubicacion { get; set; }
        public string Direccion { get; set; }
        public string Referencia { get; set; }

        public string NroCuenta { get; set; }
        public string Representante { get; set; }
        public string UbicacionNegocio { get; set; }
        public string DireccionNegocio { get; set; }
        public string ReferenciaNegocio { get; set; }

        public int? EstadoCivilId { get; set; }
        public int? GradoInstruccionId { get; set; }
        public int? SituacionDomicilioId { get; set; }
        public int? EntidadBancariaId { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string Telefono { get; set; }
    }
}
