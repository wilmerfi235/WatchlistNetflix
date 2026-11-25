using Domain.Identity.ValueObjects;
using Domain.Watchlist.Entities;
using Domain.Watchlist.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Configura el almacenamiento del agregado Watchlist.
/// Establece la relación entre la lista y su propietario.
/// </summary>
public sealed class WatchlistConfiguration
    : IEntityTypeConfiguration<Watchlist>
{
    /// <summary>
    /// Define el mapeo de Watchlist y sus restricciones.
    /// </summary>
    public void Configure(EntityTypeBuilder<Watchlist> builder)
    {
        builder.ToTable("Watchlists");

        builder.HasKey(watchlist => watchlist.Id);

        builder.Property(watchlist => watchlist.Id)
            .HasConversion(
                id => id.Value,
                value => WatchlistId.From(value))
            .HasColumnType("char(36)")
            .ValueGeneratedNever();

        builder.Property(watchlist => watchlist.OwnerId)
            .HasConversion(
                id => id.Value,
                value => UserId.From(value))
            .HasColumnType("char(36)")
            .IsRequired();

        builder.Property(watchlist => watchlist.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(watchlist => watchlist.OwnerId);

        builder.HasMany(watchlist => watchlist.Items)
            .WithOne()
            .HasForeignKey("WatchlistId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
