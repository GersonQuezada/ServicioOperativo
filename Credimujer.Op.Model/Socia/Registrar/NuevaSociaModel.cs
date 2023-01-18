namespace Credimujer.Op.Model.Socia.Registrar
{
    public class NuevaSociaModel
    {
        public int? SociaId { get; set; }
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
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

        public int EstadoCivilId { get; set; }
        public int GradoInstruccionId { get; set; }
        public int SituacionDomicilioId { get; set; }
        public int? EntidadBancariaId { get; set; }
        public string SucursalCodigo { get; set; }
        public string FechaNacimiento { get; set; }
        public int CargoBancoComunalId { get; set; }
        public int BancoComunalId { get; set; }
        public int TipoDocumentoId { get; set; }
        public string Telefono { get; set; }
        public bool? ConservarId { get; set; }
    }
}