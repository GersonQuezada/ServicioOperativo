using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Common
{
    public class AppSetting
    {
        public ConnectionString ConnectionStrings { get; set; }
        public JWTConfiguration JWTConfigurations { get; set; }
        public ApiIam ApiIamSocia { get; set; }
        public ApiIam ApiIamOperativo { get; set; }
        public int MostrarCambioFormularioPorCantidadDias { get; set; }

        public class ConnectionString
        {
            public string DefaultConnection { get; set; }
        }

        public class JWTConfiguration
        {
            public string Secret { get; set; }
            public int ExpirationTimeHours { get; set; }
            public string Iss { get; set; }
            public string Aud { get; set; }
        }

        public class ApiIam
        {
            public string Name { get; set; }
            public string Key { get; set; }
            public string Iam { get; set; }
            public Path Paths { get; set; }
        }

        public class Path
        {
            public string NuevaUsuarioSocia { get; set; }
            public string ActualizarCelularUsuario { get; set; }
            public string ObtenerDatosUsuario { get; set; }
            public string ActualizarContraseniaUsuario { get; set; }
            public string ActualizarCuentaUsuarioConDni { get; set; }
            public string EliminarSocia { get; set; }
            public string ListaOficialPorSucursal { get; set; }
        }
    }
}