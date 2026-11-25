using Domain.Catalog.ValueObjects;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Catalog.Entities;

/// <summary>
/// Representa un género cinematográfico o televisivo.
///
/// Un género puede estar asociado a múltiples contenidos.
/// </summary>
public sealed class Genre : Entity<GenreId>
{
    public string Name { get; private set; } = string.Empty;

    private Genre()
    {
        // Constructor requerido por EF Core.
    }

    private Genre(GenreId id, string name)
        : base(id)
    {
        Name = name;
    }

    public static Genre Create(string name)
    {
        ValidateName(name);

        return new Genre(
            GenreId.New(),
            name.Trim());
    }

    /// <summary>
    /// Actualiza el nombre del género.
    /// La unicidad se comprueba en Application y en la BD.
    /// </summary>
    public void Rename(string newName)
    {
        ValidateName(newName);

        Name = newName.Trim();
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "El nombre del género es obligatorio.");

        if (name.Trim().Length > 100)
            throw new ArgumentException(
                "El nombre del género no puede superar 100 caracteres.");
    }
}
