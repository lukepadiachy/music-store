using Microsoft.EntityFrameworkCore;
using MusicStore.Data;
using MusicStore.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(); // Add Swagger services
builder.Services.AddDbContext<AlbumContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MusicStoreContext")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();   // Generate Swagger JSON
    app.UseSwaggerUI(); // Serve Swagger UI for interactive API documentation
}

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AlbumContext>();
    context.Database.Migrate();

    if (!context.Albums.Any()) 
    {
        context.Albums.AddRange(
            new Album { Artist = "Chessy HD", Genre = "Afrikaans Rap", ReleaseYear = 2024, Title = "Whatever You Want" },
            new Album { Artist = "The Beatles", Genre = "Rock", ReleaseYear = 1969, Title = "Abbey Road" },
            new Album { Artist = "Michael Jackson", Genre = "Pop", ReleaseYear = 1982, Title = "Thriller" },
            new Album { Artist = "Nirvana", Genre = "Grunge", ReleaseYear = 1991, Title = "Nevermind" },
            new Album { Artist = "Adele", Genre = "Pop", ReleaseYear = 2015, Title = "25" },
            new Album { Artist = "Taylor Swift", Genre = "Country", ReleaseYear = 2008, Title = "Fearless" },
            new Album { Artist = "Drake", Genre = "Hip Hop", ReleaseYear = 2016, Title = "Views" },
            new Album { Artist = "Beyoncé", Genre = "R&B", ReleaseYear = 2016, Title = "Lemonade" },
            new Album { Artist = "Kendrick Lamar", Genre = "Hip Hop", ReleaseYear = 2015, Title = "To Pimp a Butterfly" },
            new Album { Artist = "Radiohead", Genre = "Alternative", ReleaseYear = 1997, Title = "OK Computer" }
        );
        context.SaveChanges();
    }
}

app.MapGet("/albums", async (AlbumContext db) =>
{
    return await db.Albums.ToListAsync();
});

app.MapPost("/albums", async (Album album, AlbumContext db) =>
{
    db.Albums.Add(album);
    await db.SaveChangesAsync();
    return Results.Created($"/albums/{album.Id}", album);
});

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
