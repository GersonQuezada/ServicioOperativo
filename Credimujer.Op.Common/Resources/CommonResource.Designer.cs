﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Credimujer.Op.Common.Resources {
    using System;
    
    
    /// <summary>
    ///   Clase de recurso fuertemente tipado, para buscar cadenas traducidas, etc.
    /// </summary>
    // StronglyTypedResourceBuilder generó automáticamente esta clase
    // a través de una herramienta como ResGen o Visual Studio.
    // Para agregar o quitar un miembro, edite el archivo .ResX y, a continuación, vuelva a ejecutar ResGen
    // con la opción /str o recompile su proyecto de VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class CommonResource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CommonResource() {
        }
        
        /// <summary>
        ///   Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Credimujer.Op.Common.Resources.CommonResource", typeof(CommonResource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Reemplaza la propiedad CurrentUICulture del subproceso actual para todas las
        ///   búsquedas de recursos mediante esta clase de recurso fuertemente tipado.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Se eliminó correctamente.
        /// </summary>
        public static string delete_ok {
            get {
                return ResourceManager.GetString("delete_ok", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a El servicio retornó un error.
        /// </summary>
        public static string httpresponse_500 {
            get {
                return ResourceManager.GetString("httpresponse_500", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Se rechazó correctamente.
        /// </summary>
        public static string rechazo_ok {
            get {
                return ResourceManager.GetString("rechazo_ok", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Se registró correctamente.
        /// </summary>
        public static string register_ok {
            get {
                return ResourceManager.GetString("register_ok", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Se actualizó correctamente.
        /// </summary>
        public static string update_ok {
            get {
                return ResourceManager.GetString("update_ok", resourceCulture);
            }
        }
    }
}
