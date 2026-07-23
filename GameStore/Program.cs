using GameStore.Data;
using GameStore.Dtos;
using GameStore.Endpoints;
using GameStore.Models;



var builder = WebApplication.CreateBuilder(args);


builder.Services.AddValidation();

builder.AddGameStoreDb();

var app = builder.Build();

app.MapGamesEndpoints();

app.MigrateDb();

app.Run();
