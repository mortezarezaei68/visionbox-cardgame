using CardGame.Core.RequestResponse.CardGame;
using CardGame.Core.RequestResponse.StartGame;

namespace CardGame.Core.Services;

public interface IStartGameService
{
    Task<StartGameResponse> StartGameAsync(CancellationToken cancellationToken);
}