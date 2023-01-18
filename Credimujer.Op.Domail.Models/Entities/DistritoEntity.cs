namespace Credimujer.Op.Domail.Models.Entities
{
    public class DistritoEntity
    {
        public string Codigo { get; set; }
        public string DepartamentoCodigo { get; set; }
        public string ProvinciaCodigo { get; set; }
        public string Descripcion { get; set; }
        public virtual DepartamentoEntity Departamento { get; set; }
        public virtual ProvinciaEntity Provincia { get; set; }
    }
}