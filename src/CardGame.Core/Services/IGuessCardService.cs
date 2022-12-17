using CardGame.Core.RequestResponse.CardGame;

namespace CardGame.Core.Services;

public interface IGuessCardService
{
    Task<GuessCardResponse> GuessCardAsync(GuessCardRequest request,CancellationToken cancellationToken);
}