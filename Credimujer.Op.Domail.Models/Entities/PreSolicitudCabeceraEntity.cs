using Credimujer.Op.Domail.Models.Base;
using System;
using System.Collections.Generic;

namespace Credimujer.Op.Domail.Models.Entities
{
    public class PreSolicitudCabeceraEntity : BaseEntity
    {
        public PreSolicitudCabeceraEntity()
        {
            PreSolicitud = new List<PreSolicitudEntity>();
        }

        public int Id { get; set; }
        public decimal Monto { get; set; }
        public int Plazo { get; set; }
        public int EstadoId { get; set; }
        public string Observacion { get; set; }
        public int BancoComunalId { get; set; }
        public int? AnilloGrupalId { get; set; }
        public DateTime? FechaDesembolso { get; set; }
        public int TipoId { get; set; }
        public virtual CatalogoDetalleEntity Estado { get; set; }
        public virtual CatalogoDetalleEntity Tipo { get; set; }
        public virtual ICollection<PreSolicitudEntity> PreSolicitud { get; set; }
        public virtual BancoComunalEntity? BancoComunal { get; set; }
        public virtual AnilloGrupalEntity AnilloGrupal { get; set; }
    }
}