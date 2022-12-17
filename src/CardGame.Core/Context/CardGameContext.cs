using CardGame.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace CardGame.Core.Context;

public class CardGameContext : CoreDbContext
{
    public CardGameContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Game> Games { get; set; }
}