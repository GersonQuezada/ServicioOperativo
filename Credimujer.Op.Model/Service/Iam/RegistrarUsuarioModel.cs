using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Credimujer.Op.Model.Service.Iam
{
    public class RegistrarUsuarioModel
    {
        public int Id { get; set; }
        public string Estado { get; set; }
        public int BancoComunalId { get; set; }
        public int? CargoBancoComunalId { get; set; }

        //[IgnoreDataMember]
        public string Nombre { get; set; }

        //[IgnoreDataMember]
        public string ApellidoPaterno { get; set; }

        //[IgnoreDataMember]
        public string ApellidoMaterno { get; set; }

        //[JsonIgnore]
        public string Dni { get; set; }

        //[JsonIgnore]
        public string Celular { get; set; }
    }
}