using Microsoft.EntityFrameworkCore;
using GameStore.Models;

namespace GameStore.Data
{
    public class GameStoreContext(DbContextOptions<GameStoreContext> options) 
        : DbContext(options)
    {
        public DbSet<Game> Games => Set<Game>();

        public DbSet<Genre> Genres => Set<Genre>();

    }
}
