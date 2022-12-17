using CardGame.Core.RequestResponse.GetGames;

namespace CardGame.Core.Services;

public interface IGetGameListService
{
    Task<List<GetGamesResponse>> GetAllAsync(CancellationToken cancellationToken);
}