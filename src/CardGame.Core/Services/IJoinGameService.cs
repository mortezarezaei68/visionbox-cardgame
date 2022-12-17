using CardGame.Core.RequestResponse.JoinGame;

namespace CardGame.Core.Services;

public interface IJoinGameService
{
    Task JoinGameAsync(JoinGameRequest request, CancellationToken cancellationToken);
}