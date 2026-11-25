using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Persistence;

public sealed class ApplicationDbContextFactory
    : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var connectionString = GetConnectionString();
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString))
            .Options;

        return new ApplicationDbContext(options);
    }

    private static string GetConnectionString()
    {
        var connectionString = Environment.GetEnvironmentVariable(
            "ConnectionStrings__DefaultConnection");

        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            return connectionString;
        }

        var currentDirectory = Directory.GetCurrentDirectory();
        var candidates = new[]
        {
            Path.Combine(currentDirectory, "appsettings.json"),
            Path.Combine(currentDirectory, "WatchlistNetflix.Api", "appsettings.json"),
            Path.Combine(currentDirectory, "..", "WatchlistNetflix.Api", "appsettings.json")
        };

        var settingsPath = candidates.FirstOrDefault(File.Exists);
        if (settingsPath is null)
        {
            throw new InvalidOperationException(
                "No se encontró appsettings.json para obtener DefaultConnection. " +
                "Define ConnectionStrings__DefaultConnection o ejecuta la migración " +
                "desde el proyecto API o la raíz de la solución.");
        }

        using var document = JsonDocument.Parse(File.ReadAllText(settingsPath));
        if (document.RootElement.TryGetProperty("ConnectionStrings", out var connectionStrings)
            && connectionStrings.TryGetProperty("DefaultConnection", out var value)
            && !string.IsNullOrWhiteSpace(value.GetString()))
        {
            return value.GetString()!;
        }

        throw new InvalidOperationException(
            $"No se encontró ConnectionStrings:DefaultConnection en '{settingsPath}'.");
    }
}
