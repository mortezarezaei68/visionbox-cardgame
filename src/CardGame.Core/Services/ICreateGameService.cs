using CardGame.Core.RequestResponse.CreateGame;

namespace CardGame.Core.Services;

public interface ICreateGameService
{
    Task CreateAsync(CreateGameRequest request,CancellationToken cancellationToke);
}