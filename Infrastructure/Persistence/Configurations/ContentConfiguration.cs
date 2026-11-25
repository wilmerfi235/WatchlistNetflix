using Domain.Catalog.Entities;
using Domain.Catalog.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations;


/// <summary>
/// Configura el almacenamiento de los contenidos audiovisuales.
/// Define sus propiedades y relaciones con los géneros.
/// </summary>
public sealed class ContentConfiguration
    : IEntityTypeConfiguration<Content>
{
    /// <summary>
    /// Establece el mapeo de Content a la tabla Contents.
    /// </summary>
    public void Configure(EntityTypeBuilder<Content> builder)
    {
        builder.ToTable("Contents");

        builder.HasKey(content => content.Id);

        builder.Property(content => content.Id)
            .HasConversion(
                id => id.Value,
                value => ContentId.From(value))
            .HasColumnType("char(36)")
            .ValueGeneratedNever();

        builder.HasMany(content => content.Genres)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "ContentGenres",
                right => right
                    .HasOne<Genre>()
                    .WithMany()
                    .HasForeignKey("GenreId"),
                left => left
                    .HasOne<Content>()
                    .WithMany()
                    .HasForeignKey("ContentId"),
                join =>
                {
                    join.ToTable("ContentGenres");
                    join.Property<ContentId>("ContentId")
                        .HasConversion(
                            id => id.Value,
                            value => ContentId.From(value))
                        .HasColumnType("char(36)");
                    join.Property<Domain.Catalog.ValueObjects.GenreId>("GenreId")
                        .HasConversion(
                            id => id.Value,
                            value => Domain.Catalog.ValueObjects.GenreId.From(value))
                        .HasColumnType("char(36)");
                    join.HasKey("ContentId", "GenreId");
                });
    }
}
