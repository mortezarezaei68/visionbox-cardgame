using CardGame.Core.Domain;
using CardGame.Core.RequestResponse.GetGames;

namespace CardGame.Core.Services.Implementations;

public class GetGameListService:IGetGameListService
{
    private readonly IGameRepository _gameRepository;

    public GetGameListService(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    public async Task<List<GetGamesResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        var games=await _gameRepository.GetAll(cancellationToken);
        return games.Select(a => new GetGamesResponse
        {
            Id = a.Id,
            FullName = a.GameBoards.FirstOrDefault(a => a.IsMain).FullName
        }).ToList();
    }
}