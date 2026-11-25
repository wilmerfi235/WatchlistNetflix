using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;


namespace Application.Common.Behaviors;

/// <summary>
/// Ejecuta las validaciones registradas antes de procesar una solicitud.
/// </summary>
/// <typeparam name="TRequest">Tipo de solicitud.</typeparam>
/// <typeparam name="TResponse">Tipo de respuesta.</typeparam>
public sealed class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    /// <summary>
    /// Inicializa el comportamiento con los validadores disponibles.
    /// </summary>
    public ValidationBehavior(
        IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    /// <summary>
    /// Ejecuta las validaciones y continúa con el siguiente comportamiento
    /// únicamente cuando no existen errores.
    /// </summary>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var results = await Task.WhenAll(
            _validators.Select(validator =>
                validator.ValidateAsync(context, cancellationToken)));

        var errors = results
            .SelectMany(result => result.Errors)
            .Where(error => error is not null)
            .Select(error => error.ErrorMessage)
            .Distinct()
            .ToArray();

        if (errors.Length > 0)
        {
            throw new Application.Common.Exceptions.ValidationException(errors);
        }

        return await next();
    }
}
