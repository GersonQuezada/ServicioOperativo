using Credimujer.Op.Repository.Interfaces.Configuration.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Credimujer.Op.Repository.Implementations.Configurations.Base
{
    public abstract class EntityConfiguration<T> : IEntityConfiguration<T> where T : class
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property("UsuarioCreacion").HasColumnName("VC_USUARIO_CREACION");
            builder.Property("FechaCreacion").HasColumnName("DT_FECHA_CREACION");
            builder.Property("UsuarioModificacion").HasColumnName("VC_USUARIO_MODIFICACION");
            builder.Property("FechaModificacion").HasColumnName("DT_FECHA_MODIFICACION");
            builder.Property("EstadoFila").HasColumnName("BT_ESTADO_FILA");
        }
    }
}
