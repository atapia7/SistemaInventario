using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaInventario.Modelos;
using System;


namespace SistemaInventario.AccesoDatos.Configuracion
{
    public class ProductoConfiguracion : IEntityTypeConfiguration<Producto>
    { 
        public void Configure(EntityTypeBuilder<Producto> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.NumeroSerie).IsRequired().HasMaxLength(60);
            builder.Property(x => x.Descripcion).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Estado).IsRequired();
            builder.Property(x => x.Precio).IsRequired();
            builder.Property(x => x.Costo).IsRequired();
            builder.Property(x => x.CategoriaId).IsRequired();
            builder.Property(x => x.MarcaId).IsRequired();
            builder.Property(x => x.ImagenUrl).IsRequired(false);
            builder.Property(x => x.PadreId).IsRequired(false);

            /*Relaciones*/

            builder.HasOne(x=>x.Categoria).WithMany()
                .HasForeignKey(x => x.CategoriaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m=>m.Marca).WithMany()
                .HasForeignKey(m=> m.MarcaId);

            builder.HasOne(x=>x.Padre).WithMany()
                .HasForeignKey(x=> x.PadreId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
