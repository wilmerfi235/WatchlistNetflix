using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Catalog.Queries.GetContents;

/// <summary>
/// Valida los parámetros de consulta del catálogo.
/// </summary>
public sealed class GetContentsQueryValidator
    : AbstractValidator<GetContentsQuery>
{
    /// <summary>
    /// Configura las reglas de paginación y búsqueda.
    /// </summary>
    public GetContentsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("El número de página debe ser mayor que cero.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("El tamaño de página debe estar entre 1 y 100.");

        RuleFor(x => x.SearchTerm)
            .MaximumLength(100)
            .WithMessage("El término de búsqueda no puede superar los 100 caracteres.");

        RuleFor(x => x.ContentType)
            .MaximumLength(30)
            .WithMessage("El tipo de contenido no puede superar los 30 caracteres.");
    }
}
