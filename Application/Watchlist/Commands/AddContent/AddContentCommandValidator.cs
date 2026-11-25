using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Watchlist.Commands.AddContent;

/// <summary>
/// Valida los identificadores necesarios para añadir un contenido.
/// </summary>
public sealed class AddContentCommandValidator
    : AbstractValidator<AddContentCommand>
{
    /// <summary>
    /// Configura las reglas de validación de la operación.
    /// </summary>
    public AddContentCommandValidator()
    {
        RuleFor(x => x.WatchlistId)
            .NotEmpty()
            .WithMessage("El identificador de la lista es obligatorio.");

        RuleFor(x => x.ContentId)
            .NotEmpty()
            .WithMessage("El identificador del contenido es obligatorio.");
    }
}
