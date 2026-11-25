using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WatchlistNetflix.Api.Controllers;

public sealed record GenreResponse(Guid Id, string Name);

[ApiController]
[Route("api/[controller]")]
public sealed class GenresController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public GenresController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType<IReadOnlyList<GenreResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<GenreResponse>>> GetAll(
        CancellationToken cancellationToken)
    {
        var genres = await _context.Genres
            .AsNoTracking()
            .OrderBy(genre => genre.Name)
            .Select(genre => new GenreResponse(genre.Id.Value, genre.Name))
            .ToListAsync(cancellationToken);

        return Ok(genres);
    }
}
