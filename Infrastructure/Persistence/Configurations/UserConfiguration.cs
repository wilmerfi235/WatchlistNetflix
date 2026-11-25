using Domain.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Define el mapeo de la entidad User a la tabla Users.
/// Establece restricciones, tipos de datos e índices.
/// </summary>
public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <summary>
    /// Configura la estructura de persistencia de los usuarios.
    /// </summary>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(user => user.Id);

        builder.Property(user => user.Id)
            .HasConversion(
                id => id.Value,
                value => Domain.Identity.ValueObjects.UserId.From(value))
            .HasColumnName("Id")
            .HasColumnType("char(36)")
            .ValueGeneratedNever();

        builder.HasIndex(user => user.Email)
            .IsUnique();

        builder.Property(user => user.Email)
            .HasConversion(
                email => email.Value,
                value => Domain.Identity.ValueObjects.Email.Create(value))
            .HasColumnName("Email")
            .HasMaxLength(254)
            .IsRequired();
    }
}
