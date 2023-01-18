namespace Credimujer.Op.Dto.Socia.Busqueda
{
    public class SociaNuevaPorDniDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Dni { get; set; }
        public string Celular { get; set; }
        public string SucursalCodigo { get; set; }
        public string BancoComunal { get; set; }
        public int? BancoComunalId { get; set; }
        public int? CargoBancoComunalId { get; set; }
        public string Telefono { get; set; }
        public int TipoDocumentoId { get; set; }
        public FormularioDto Formulario { get; set; }
        public bool Reingresante { get; set; }
        public bool DatoIncompleto { get; set; }
        public int? SistemaExternoId { get; set; }
    }
}