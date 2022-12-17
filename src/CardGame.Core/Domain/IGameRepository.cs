namespace CardGame.Core.Domain;

public interface IGameRepository
{
    void Add(Game game);
    void Update(Game game);
    Task<Game> GetById(int id,CancellationToken cancellationToken);
    Task<List<Game>> GetAll(CancellationToken cancellationToken);
}