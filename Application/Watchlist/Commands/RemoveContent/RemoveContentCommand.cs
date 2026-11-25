using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Watchlist.Commands.RemoveContent;

/// <summary>
/// Representa la solicitud para eliminar un contenido de una lista.
/// </summary>
public sealed record RemoveContentCommand(
    Guid WatchlistId,
    Guid ContentId
) : IRequest<Unit>;
