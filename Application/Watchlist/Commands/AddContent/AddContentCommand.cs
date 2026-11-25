using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Watchlist.Commands.AddContent;

/// <summary>
/// Representa la solicitud para añadir un contenido a una lista.
/// </summary>
public sealed record AddContentCommand(
    Guid WatchlistId,
    Guid ContentId
) : IRequest<Unit>;
