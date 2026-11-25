using Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace WatchlistNetflix.Api.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Error no controlado al procesar {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            if (context.Response.HasStarted)
            {
                throw;
            }

            var (status, title, detail, errors) = exception switch
            {
                ValidationException validation => (
                    StatusCodes.Status400BadRequest,
                    "Error de validación",
                    validation.Message,
                    validation.Errors),
                UnauthorizedException => (
                    StatusCodes.Status401Unauthorized,
                    "No autorizado",
                    exception.Message,
                    (IReadOnlyCollection<string>?)null),
                NotFoundException => (
                    StatusCodes.Status404NotFound,
                    "No encontrado",
                    exception.Message,
                    (IReadOnlyCollection<string>?)null),
                ConflictException => (
                    StatusCodes.Status409Conflict,
                    "Conflicto",
                    exception.Message,
                    (IReadOnlyCollection<string>?)null),
                _ => (
                    StatusCodes.Status500InternalServerError,
                    "Error interno",
                    "Ocurrió un error inesperado al procesar la solicitud.",
                    (IReadOnlyCollection<string>?)null)
            };

            context.Response.Clear();
            context.Response.StatusCode = status;
            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = detail,
                Extensions = { ["errors"] = errors }
            });
        }
    }
}
