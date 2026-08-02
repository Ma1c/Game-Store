using System;
using GameStore.Data;
using GameStore.Dtos;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Endpoints;

public static class GenresEndpoints
{
    public static void MapGenresEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/genres");

        group.MapGet("/", async (GameStoreContext dbContext) => 
        await dbContext.Genres
                        .Select(genre => new GenreDto(genre.Id, genre.Name))
                        .AsNoTracking()
                        .ToListAsync()
        );
    }
}
