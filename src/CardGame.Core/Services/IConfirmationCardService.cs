using CardGame.Core.Domain;
using CardGame.Core.RequestResponse.CardGame;

namespace CardGame.Core.Services;

public interface IConfirmationCardService
{
    GuessCardResponse IsConfirmedOrLoosed(GuessCardRequest request, string gameBoardIdCookie, Game game, int? nextGameBoardId);
}