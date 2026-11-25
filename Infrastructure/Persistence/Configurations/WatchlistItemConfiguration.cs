using Domain.Catalog.ValueObjects;
using Domain.Watchlist.Entities;
using Domain.Watchlist.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Configura la entidad WatchlistItem, que representa
/// la asociación entre una lista y un contenido audiovisual.
/// </summary>
public sealed class WatchlistItemConfiguration
    : IEntityTypeConfiguration<WatchlistItem>
{
    /// <summary>
    /// Define las columnas, identificadores y restricciones
    /// de los elementos de las listas.
    /// </summary>
    public void Configure(EntityTypeBuilder<WatchlistItem> builder)
    {
        builder.ToTable("WatchlistItems");

        builder.HasKey(item => item.Id);

        builder.Property(item => item.Id)
            .HasConversion(
                id => id.Value,
                value => WatchlistItemId.From(value))
            .HasColumnType("char(36)")
            .ValueGeneratedNever();

        builder.Property(item => item.ContentId)
            .HasConversion(
                id => id.Value,
                value => ContentId.From(value))
            .HasColumnType("char(36)")
            .IsRequired();

        builder.Property<WatchlistId>("WatchlistId")
            .HasConversion(
                id => id.Value,
                value => WatchlistId.From(value))
            .HasColumnType("char(36)");

        builder.HasOne<Domain.Catalog.Entities.Content>()
            .WithMany()
            .HasForeignKey(item => item.ContentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex("WatchlistId", nameof(WatchlistItem.ContentId))
            .IsUnique();
    }
}
