using Application.Watchlist.Commands.AddContent;
using Application.Watchlist.Commands.CreateWatchlist;
using Application.Watchlist.Commands.RemoveContent;
using Application.Watchlist.DTOs;
using Application.Watchlist.Queries.GetWatchlistById;
using Application.Watchlist.Queries.GetWatchlists;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WatchlistNetflix.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public sealed class WatchlistsController : ControllerBase
{
    private readonly ISender _sender;

    public WatchlistsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [ProducesResponseType<IReadOnlyCollection<WatchlistDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<WatchlistDto>>> GetAll(
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetWatchlistsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<WatchlistDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WatchlistDto>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetWatchlistByIdQuery(id),
            cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType<Guid>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<Guid>> Create(
        [FromBody] CreateWatchlistRequest request,
        CancellationToken cancellationToken)
    {
        var id = await _sender.Send(
            new CreateWatchlistCommand(request.Name, null),
            cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPost("{watchlistId:guid}/contents/{contentId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddContent(
        Guid watchlistId,
        Guid contentId,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new AddContentCommand(watchlistId, contentId),
            cancellationToken);
        return NoContent();
    }

    [HttpDelete("{watchlistId:guid}/contents/{contentId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveContent(
        Guid watchlistId,
        Guid contentId,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new RemoveContentCommand(watchlistId, contentId),
            cancellationToken);
        return NoContent();
    }
}

public sealed record CreateWatchlistRequest(string Name);
