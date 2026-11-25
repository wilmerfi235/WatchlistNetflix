using Domain.Catalog.Enums;
using Domain.Catalog.ValueObjects;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Catalog.Entities;

/// <summary>
/// Representa una película o serie del catálogo.
///
/// Un contenido puede estar asociado a varios géneros.
/// No depende de ninguna lista personal.
/// </summary>
public sealed class Content : Entity<ContentId>
{
    //private readonly List<GenreId> _genreIds = [];
    private readonly List<Genre> _genres = [];

    public string Title { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public ContentType Type { get; private set; }

    public int ReleaseYear { get; private set; }

    public string? PosterUrl { get; private set; }

    /*public IReadOnlyCollection<GenreId> GenreIds
        => _genreIds.AsReadOnly();*/
    public IReadOnlyCollection<Genre> Genres
    => _genres.AsReadOnly();


    private Content()
    {
        // Constructor requerido por EF Core.
    }

    private Content(
        ContentId id,
        string title,
        string? description,
        ContentType type,
        int releaseYear,
        string? posterUrl)
        : base(id)
    {
        Title = title.Trim();
        Description = NormalizeDescription(description);
        Type = type;
        ReleaseYear = releaseYear;
        PosterUrl = NormalizeUrl(posterUrl);
    }

    /// <summary>
    /// Crea un nuevo contenido en el catálogo.
    /// </summary>
    public static Content Create(
        string title,
        string? description,
        ContentType type,
        int releaseYear,
        string? posterUrl = null)
    {
        ValidateTitle(title);
        ValidateType(type);
        ValidateReleaseYear(releaseYear);

        return new Content(
            ContentId.New(),
            title,
            description,
            type,
            releaseYear,
            posterUrl);
    }

    /// <summary>
    /// Modifica la información descriptiva del contenido.
    /// </summary>
    public void UpdateInformation(
        string title,
        string? description,
        int releaseYear,
        string? posterUrl)
    {
        ValidateTitle(title);
        ValidateReleaseYear(releaseYear);

        Title = title.Trim();
        Description = NormalizeDescription(description);
        ReleaseYear = releaseYear;
        PosterUrl = NormalizeUrl(posterUrl);
    }

    /// <summary>
    /// Asocia un género al contenido.
    /// Evita asociar el mismo género dos veces.
    /// </summary>
    /*public void AddGenre(GenreId genreId)
    {
        ArgumentNullException.ThrowIfNull(genreId);

        if (_genreIds.Contains(genreId))
            return;

        _genreIds.Add(genreId);
    }*/

    /// <summary>
    /// Asocia un género al contenido.
    /// Evita asociaciones duplicadas.
    /// </summary>
    public void AddGenre(Genre genre)
    {
        ArgumentNullException.ThrowIfNull(genre);

        if (_genres.Any(g => g.Id.Equals(genre.Id)))
            return;

        _genres.Add(genre);
    }


    /// <summary>
    /// Elimina la asociación entre un contenido y un género.
    /// No elimina el género del catálogo.
    /// </summary>
    /*public void RemoveGenre(GenreId genreId)
    {
        ArgumentNullException.ThrowIfNull(genreId);

        _genreIds.Remove(genreId);
    }*/

    /// <summary>
    /// Elimina la asociación con un género.
    /// No elimina el género del catálogo.
    /// </summary>
    public void RemoveGenre(GenreId genreId)
    {
        ArgumentNullException.ThrowIfNull(genreId);

        var genre = _genres.FirstOrDefault(
            g => g.Id.Equals(genreId));

        if (genre is not null)
            _genres.Remove(genre);
    }


    private static void ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException(
                "El título es obligatorio.");

        if (title.Trim().Length > 200)
            throw new ArgumentException(
                "El título no puede superar 200 caracteres.");
    }

    private static void ValidateType(ContentType type)
    {
        if (!Enum.IsDefined(type))
            throw new ArgumentException(
                "El tipo de contenido no es válido.");
    }

    private static void ValidateReleaseYear(int year)
    {
        if (year < 1888 || year > DateTime.UtcNow.Year + 1)
            throw new ArgumentException(
                "El año de lanzamiento no es válido.");
    }

    private static string? NormalizeDescription(string? description)
    {
        return string.IsNullOrWhiteSpace(description)
            ? null
            : description.Trim();
    }

    private static string? NormalizeUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return null;

        if (!Uri.TryCreate(url.Trim(), UriKind.Absolute, out var uri)
            || (uri.Scheme != Uri.UriSchemeHttp
                && uri.Scheme != Uri.UriSchemeHttps))
        {
            throw new ArgumentException(
                "La URL del póster no es válida.");
        }

        return uri.ToString();
    }
}
