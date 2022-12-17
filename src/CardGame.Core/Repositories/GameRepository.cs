using CardGame.Core.Context;
using CardGame.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace CardGame.Core.Repositories;

public class GameRepository : IGameRepository
{
    private readonly CardGameContext _context;

    public GameRepository(CardGameContext context)
    {
        _context = context;
    }

    public void Add(Game game)
    {
        _context.Games.Add(game);
    }

    public void Update(Game game)
    {
        _context.Games.Update(game);
    }

    public async Task<Game> GetById(int id, CancellationToken cancellationToken)
        => await _context.Games.Include(a => a.GivenCards).Include(a => a.GameBoards)
            .ThenInclude(a => a.BoardDetails)
            .FirstOrDefaultAsync(a => a.Id.Equals(id), cancellationToken);

    public async Task<List<Game>> GetAll(CancellationToken cancellationToken)
        => await _context.Games.Include(a => a.GivenCards).Include(a => a.GameBoards)
            .ThenInclude(a => a.BoardDetails)
            .ToListAsync(cancellationToken: cancellationToken);
}