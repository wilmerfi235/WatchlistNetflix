using Domain.Catalog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Define el mapeo de Genre a la tabla Genres.
/// Configura su clave primaria y restricciones de nombre.
/// </summary>
public sealed class GenreConfiguration
    : IEntityTypeConfiguration<Genre>
{
    /// <summary>
    /// Configura las propiedades persistentes de los géneros.
    /// </summary>
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.ToTable("Genres");

        builder.HasKey(genre => genre.Id);

        builder.Property(genre => genre.Id)
            .HasConversion(
                id => id.Value,
                value => Domain.Catalog.ValueObjects.GenreId.From(value))
            .HasColumnType("char(36)")
            .ValueGeneratedNever();

        builder.Property(genre => genre.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(genre => genre.Name)
            .IsUnique();
    }
}
