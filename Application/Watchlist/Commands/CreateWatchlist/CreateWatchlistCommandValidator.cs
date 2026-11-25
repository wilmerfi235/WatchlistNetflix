using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Watchlist.Commands.CreateWatchlist;

/// <summary>
/// Valida los datos necesarios para crear una lista.
/// </summary>
public sealed class CreateWatchlistCommandValidator
    : AbstractValidator<CreateWatchlistCommand>
{
    /// <summary>
    /// Configura las reglas de validación de una nueva lista.
    /// </summary>
    public CreateWatchlistCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("El nombre de la lista es obligatorio.")
            .MaximumLength(100)
            .WithMessage("El nombre no puede superar los 100 caracteres.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("La descripción no puede superar los 500 caracteres.");
    }
}
