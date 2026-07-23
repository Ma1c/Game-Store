using GameStore.Data;
using GameStore.Dtos;
using GameStore.Models;
using Microsoft.EntityFrameworkCore;


namespace GameStore.Endpoints
{

    public static class GamesEndpoints
    {
        const string GetGameEndpointName = "GetGame";

        private static readonly List<GameSummaryDto> games = [
        new (1,
        "Street Fighter II",
        "Fighting",
        19.99M ,
        new DateOnly(1992, 7, 15)),
        new (2,
        "Mario Kart",
        "Racing",
        29.99M ,
        new DateOnly(2015, 5, 20)),
        new (3,
        "Astrobot",
        "Platformer",
        49.99M ,
        new DateOnly(2024, 6, 7)),
        ];

        public static void MapGamesEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/games");

            //GET /games
            group.MapGet("/", async (GameStoreContext dbContext) 
                => await dbContext.Games
                                    .Include(game => game.Genre)
                                    .Select(game => new GameSummaryDto(
                                        game.Id,
                                        game.Name,
                                        game.Genre!.Name,
                                        game.Price,
                                        game.ReleaseDate
                                        ))
                                        .AsNoTracking()
                                        .ToListAsync());


            //GET /games/1
            group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
            {
                var game = await dbContext.Games.FindAsync(id);

                return game is null ? Results.NotFound() : Results.Ok(
                    new GameDetailsDto(
                        game.Id,
                        game.Name,
                        game.GenreId,
                        game.Price,
                        game.ReleaseDate)
                    );
            })
                .WithName(GetGameEndpointName);

            //POST /games
            group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
            {
                Game game = new()
                {
                    Name = newGame.Name,
                    GenreId = newGame.GenreId,
                    Price = newGame.Price,
                    ReleaseDate = newGame.ReleaseDate
                };
                dbContext.Games.Add(game);
                await dbContext.SaveChangesAsync();

                GameDetailsDto gameDto = new(
                    game.Id,
                    game.Name,
                    game.GenreId,
                    game.Price,
                    game.ReleaseDate
                    );

                return Results.CreatedAtRoute(GetGameEndpointName, new { id = gameDto.Id }, gameDto);
            });

            //PUT /games/1
            group.MapPut("/{id}", async (int id, 
                UpdateGameDto updatedGame, 
                GameStoreContext dbContext) =>
            {
                var existingGame = await dbContext.Games.FindAsync(id);

                if (existingGame is null)
                {
                    return Results.NotFound();
                }

                existingGame.Name = updatedGame.Name;
                existingGame.GenreId = updatedGame.Genre;

                return Results.NoContent();
            });

            //DELETE /games/1
            group.MapDelete("/{id}", (int id) =>
            {
                games.RemoveAll(game => game.Id == id);

                return Results.NoContent();
            });

        }
    }
}
