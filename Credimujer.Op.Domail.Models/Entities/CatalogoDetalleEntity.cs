using Credimujer.Op.Domail.Models.Base;
using System.Collections.Generic;

namespace Credimujer.Op.Domail.Models.Entities
{
    public class CatalogoDetalleEntity : BaseEntity
    {
        public CatalogoDetalleEntity()
        {
            SociaEstado = new List<SociaEntity>();
            SociaEstadoCivil = new List<FormularioEntity>();
            SociaGradoInstruccion = new List<FormularioEntity>();
            SociaSituacionDomicilio = new List<FormularioEntity>();
            SociaEntidadBancaria = new List<FormularioEntity>();
            PreSolicitudEstado = new List<PreSolicitudEntity>();
            //PreSolicitudEntidadBancaria = new List<PreSolicitudEntity>();
            PreSolicitudTipoCredito = new List<PreSolicitudEntity>();
            SociaSucursal = new List<SociaEntity>();
            BancoComunalSucursal = new List<BancoComunalEntity>();
            PreSolicitudCabeceraEstado = new List<PreSolicitudCabeceraEntity>();
            PreSolicitudTasaInteres = new List<PreSolicitudEntity>();
            PreSolicitudAsistencia = new List<PreSolicitudEntity>();
            PreSolicitudNivelRiesgo = new List<PreSolicitudEntity>();

            PreSolicitudBancoDesembolso = new List<PreSolicitudEntity>();
            PreSolicitudSubTipoCredito = new List<PreSolicitudEntity>();
            BancoComunalEstado = new List<BancoComunalEntity>();
            AnilloGrupalEstado = new List<AnilloGrupalEntity>();
            PreSolicitudMotivoRetiro = new List<PreSolicitudEntity>();
            PreSolicitudCabeceraTipo = new List<PreSolicitudCabeceraEntity>();
            SociaTipoDocumento = new List<SociaEntity>();
            FormularioTipoDocumento = new List<FormularioEntity>();
            PreSolicitudSistemaOrigen = new List<PreSolicitudEntity>();
            PreSolicitudTipoDispositivo = new List<PreSolicitudEntity>();
            PreSolicitudTipoDeuda= new List<PreSolicitudEntity>();
        }

        public int Id { get; set; }
        public int CatalogoId { get; set; }
        public string Codigo { get; set; }
        public string Valor { get; set; }
        public string Descripcion { get; set; }
        public int Orden { get; set; }
        public string Abreviatura { get; set; }
        public virtual CatalogoEntity Catalogo { get; set; }
        public virtual ICollection<SociaEntity> SociaEstado { get; set; }
        public virtual ICollection<SociaEntity> SociaSucursal { get; set; }
        public virtual ICollection<SociaEntity> SociaTipoDocumento { get; set; }
        public virtual ICollection<FormularioEntity> SociaEstadoCivil { get; set; }
        public virtual ICollection<FormularioEntity> SociaGradoInstruccion { get; set; }
        public virtual ICollection<FormularioEntity> SociaSituacionDomicilio { get; set; }
        public virtual ICollection<FormularioEntity> SociaEntidadBancaria { get; set; }
        public virtual ICollection<FormularioEntity> SociaCargoBancoComunal { get; set; }
        public virtual ICollection<FormularioEntity> FormularioTipoDocumento { get; set; }
        public virtual ICollection<PreSolicitudEntity> PreSolicitudEstado { get; set; }

        //public virtual ICollection<PreSolicitudEntity> PreSolicitudEntidadBancaria { get; set; }
        public virtual ICollection<PreSolicitudEntity> PreSolicitudTipoCredito { get; set; }

        public virtual ICollection<PreSolicitudEntity> PreSolicitudSubTipoCredito { get; set; }
        public virtual ICollection<BancoComunalEntity> BancoComunalSucursal { get; set; }
        public virtual ICollection<PreSolicitudCabeceraEntity> PreSolicitudCabeceraEstado { get; set; }
        public virtual ICollection<PreSolicitudCabeceraEntity> PreSolicitudCabeceraTipo { get; set; }

        public virtual ICollection<PreSolicitudEntity> PreSolicitudTasaInteres { get; set; }
        public virtual ICollection<PreSolicitudEntity> PreSolicitudAsistencia { get; set; }
        public virtual ICollection<PreSolicitudEntity> PreSolicitudNivelRiesgo { get; set; }
        public virtual ICollection<PreSolicitudEntity> PreSolicitudBancoDesembolso { get; set; }
        public virtual ICollection<PreSolicitudEntity> PreSolicitudMotivoRetiro { get; set; }
        public virtual ICollection<PreSolicitudEntity> PreSolicitudSistemaOrigen { get; set; }
        public virtual ICollection<PreSolicitudEntity> PreSolicitudTipoDispositivo { get; set; }
        public virtual ICollection<PreSolicitudEntity> PreSolicitudTipoDeuda { get; set; }
        public virtual ICollection<BancoComunalEntity> BancoComunalEstado { get; set; }
        public virtual ICollection<AnilloGrupalEntity> AnilloGrupalEstado { get; set; }
    }
}