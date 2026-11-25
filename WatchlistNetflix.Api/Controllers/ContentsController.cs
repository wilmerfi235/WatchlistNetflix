using Application.Catalog.Queries.GetContentById;
using Application.Catalog.Queries.GetContents;
using Application.Catalog.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WatchlistNetflix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ContentsController : ControllerBase
{
    private readonly ISender _sender;

    public ContentsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [ProducesResponseType<PagedResultDto<ContentDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResultDto<ContentDto>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] string? contentType = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(
            new GetContentsQuery(pageNumber, pageSize, searchTerm, contentType),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<ContentDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContentDto>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetContentByIdQuery(id),
            cancellationToken);

        return Ok(result);
    }
}
