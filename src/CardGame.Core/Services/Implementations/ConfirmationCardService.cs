using CardGame.Core.Domain;
using CardGame.Core.Enums;
using CardGame.Core.RequestResponse.CardGame;

namespace CardGame.Core.Services.Implementations;

public class ConfirmationCardService : IConfirmationCardService
{
    public GuessCardResponse IsConfirmedOrLoosed(GuessCardRequest request, string gameBoardIdCookie, Game game,
        int? nextGameBoardId)
    {
        if (IsConfirmedOrLoose(request, game))
            return ConfirmedCard(game, gameBoardIdCookie, nextGameBoardId);
        
        return LooseCard(game, nextGameBoardId);
    }

    private static bool IsConfirmedOrLoose(GuessCardRequest request, Game game)
    {
        return game.GivenCards.LastOrDefault(a => a.CreatedAt == new DateTime()).Value >
               game.GivenCards.LastOrDefault(a => a.CreatedAt != new DateTime()).Value &&
               request.GuessType == GuessType.Higher ||
               game.GivenCards.LastOrDefault(a => a.CreatedAt == new DateTime()).Value <
               game.GivenCards.LastOrDefault(a => a.CreatedAt != new DateTime()).Value &&
               request.GuessType == GuessType.Lower;
    }

    private GuessCardResponse ConfirmedCard(Game game, string gameBoardIdCookie, int? nextGameBoardId)
    {
        game.UpdateCurrentlyRoundScore(int.Parse(gameBoardIdCookie));

        var lastCardGenerated = game.GivenCards.OrderBy(a => a.CreatedAt).LastOrDefault(a => !a.HasGone);

        return new GuessCardResponse
        {
            IsFinished = false,
            CardType = lastCardGenerated.CardType,
            IsConfirmed = true,
            Value = lastCardGenerated.Value,
            UserId = nextGameBoardId
        };
    }

    private GuessCardResponse LooseCard(Game game, int? validGameBoardId)
    {
        var lastCardGenerated = game.GivenCards.OrderBy(a => a.CreatedAt).LastOrDefault(a => !a.HasGone);

        return new GuessCardResponse
        {
            IsFinished = false,
            CardType = lastCardGenerated.CardType,
            IsConfirmed = false,
            Value = lastCardGenerated.Value,
            UserId = validGameBoardId,
        };
    }
}