using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Common
{
    public class Constants
    {
        public struct SystemStatusCode
        {
            public const int Ok = 0;
            public const int Required = 1;
            public const int TechnicalError = -1;
            public const int FunctionalError = -2;
        }

        public struct DateTimeFormats
        {
            public const string DD_MM_YYYY = "dd/MM/yyyy";
            public const string DD_MM_YYYY_HH_MM_SS = "dd/MM/yyyy HH:mm:ss";
            public const string DD_MM_YYYY_HH_MM_TT_12 = "dd/MM/yyyy hh:mm tt";
            public const string DD_MM_YYYY_HH_MM_SS_TT_12 = "dd/MM/yyyy hh:mm:ss tt";
            public const string DD_MM_YYYY_HH_MM_24 = "dd/MM/yyyy HH:mm";
            public const string DD_MM_YYYY_HH_MM_SS_FFF = "yyyyMMddHHmmssFFF";
            public const string DD_MM_YYY_HH_MM_SS = "ddMMyyyHHmmss";
            public const string YYYY_MM_DD = "yyyyMMdd";
        }

        public struct Core
        {
            public struct Audit
            {
                public const string CreationUser = "UsuarioCreacion";
                public const string CreationDate = "FechaCreacion";
                public const string ModificationUser = "UsuarioModificacion";
                public const string ModificationDate = "FechaModificacion";
                public const string RowStatu = "EstadoFila";
                public const string System = "CrediMujerSystem";
            }

            public struct UserAsociadoClaims
            {
                public const string NombreUsuario = "NombreUsuario";
                public const string NombreCompleto = "NombreCompleto";
                public const string GuiId = "GuiId";//en caso el usuario se logea en 2 dispositivos.
                public const string SociaId = "SociaId";
                public const string Sucursal = "Sucursal";
                public const string RolCodigo = "RolCodigo";
            }

            public struct Catalogo
            {
                public const string EstadoCivil = "ESTCIVIL";
                public const string GradoInstruccion = "GRADOINS";
                public const string SituacionDomicilio = "SITDOMIC";
                public const string Afirmacion = "SINO";
                public const string EntidadFinanciera = "ENTIFINAN";
                public const string EstadoSocia = "ESTADOSOC";
                public const string TipoCredito = "TIPCRED";
                public const string SubTipoCredito = "SUBTIPCRE";
                public const string Sucursal = "SUC";
                public const string TasaInteres = "TASAINT";
                public const string TasaInteresRebatir = "TAINTREB";
                public const string AsistenciaPreSolicitud = "ASISPRESOL";
                public const string NivelRiesgo = "NIVRIESG";
                public const string EstadoPreSolicitud = "ESTPRESOL";
                public const string EstadoPreSolicitudCabecera = "ESTCABPRES";
                public const string CargoBancoComunal = "CABACO";
                public const string Estado = "ESTADO";
                public const string MotivoRetiro = "MOTRETIRO";
                public const string TipoPresolicitudGenerar = "TIPPRESOL";

                public const int MontoMinimoPresolicitud = 100;
                public const string TipoDocumentoIdentidad = "TIPDOC";
                public const decimal MontoMaximoCapacidadPago = 20;
                public const string SistemaOrigenOficiales = "SISOFICIAL";
                public const string SistemaOrigenSocia = "SISSOCIA";

                public const string TipoDispositivo = "DISPTVO";
                public const string TipoDeuda = "TDEUD";
                public struct DetEstadoSocia
                {
                    public const string Activa = "ACTIVO";
                    public const string PendienteConfirmacion = "PENDIEN";
                    public const string Rechazada = "RECHAZ";
                    public const string Debaja = "DEBAJA";
                }

                public struct PreSolicitudEstado
                {
                    public const string Registrado = "REG";
                    public const string Rechazada = "ANU";
                    public const string Ahorrista = "AHORR";
                    public const string Retirada = "ARETI";
                }

                public struct PreSolicitudCabeceraEstado
                {
                    public const string PorAprobar = "PORAP";
                    public const string Aprobada = "APROB";
                    public const string Rechazada = "RECHA";
                    public const string Observada = "OBSBD";
                }

                public struct DetTipoCredito
                {
                    public const string CtaExterna = "CTAEX";
                    public const string CtaParalela = "CPARA";
                }

                public struct DetSubTipoCredito
                {
                    public const string CreditoComplementario = "CRECOM";
                    public const string CreditoCampaña = "CRECAM";
                    public const string CreditoMicelular = "CRECEL";
                    public const string BancoComunal = "BANCOMUN"; //CTAEX
                    public const string AnilloGrupal = "ANIGRU";//CTAEX
                    public const string ExternoPromocional = "EXTPROM";//CTAEX

                    public const string CreditoAnilloCampaña = "PACRCA";
                    public const string CreditoAnilloComplementario = "PACRCO";
                }

                public struct GrupoSubTipoCredito
                {
                    public const string CCPM = "CCPM";
                    public const string CIAS = "CIAS";
                    public const string BBCC = "BBCC";
                    public const string AAGG = "AAGG";
                    public const string PROM = "PROM";
                }

                public struct DetEstado
                {
                    public const string Activo = "EACT";
                    public const string InActivo = "EINACT";
                }

                public struct PreSolicitudCabeceraTipo
                {
                    public const string CuentaExterna = "TIPCTAEX";
                    public const string Paralelo = "TIPPARAL";
                }

                public struct Perfil
                {
                    public const string AsistenteAdministrativo = "001";
                    public const string OficialCredito = "002";
                    public const string JefaturaSupervisior = "003";
                    public const string PersonalAutorizado = "004";
                }

                public struct DetMotivoRetiro
                {
                    public const string AnilloBanco = "CBMRAGBC";
                    public const string BancoAnillo = "CBMRBCAN";
                    public const string BancoaBanco = "CBMASOCCOM";
                    public const string AnilloaAnillo = "CBMRAA";
                }

                public struct DetTipoReporte
                {
                    public static List<string> CtaExternaMasPromocional = new() { "EXTPRO", "Cta Externa" };
                    public static List<string> CreditoParalelo = new() { "CREPAR", "crédito paralelo" };
                    public static List<string> AnilloGrupal = new() { "ANIGRU", "anillo grupal" };
                    public static List<string> CreditoParaleloAnilloGrupal = new() { "PARAANI", "crédito paralelo anillo grupal" };
                    public static List<string> CreditoParaleloMasPromocional = new() { "CREPRO", "crédito paralelo + promocional" };
                }

                public struct DetCargoBancoComunal
                {
                    public const string SinCargo = "BCSINCA";
                }
            }
        }
    }
}