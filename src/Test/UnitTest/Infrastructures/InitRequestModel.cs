using CardGame.Core.Domain;
using CardGame.Core.Enums;
using CardGame.Core.RequestResponse.CardGame;
using CardGame.Core.RequestResponse.CreateGame;
using CardGame.Core.RequestResponse.JoinGame;
using CardGame.Core.Services;

namespace TestProject.Infrastructures;

public static class InitRequestModel
{
    public static JoinGameRequest JoinGameRequest()=>  new()
    {
        FullName = "Test",
        GameId = 1
    };
    public static CreateGameRequest CreateGameRequest()=>  new()
    {
        FullName = "Test"
    };

    public static GuessCardRequest CreateGuessCardRequest() => new()
    {
        GuessType = GuessType.Lower
    };
    public static Game InitiateGameForValidateUserTurn()
    {
        var game = new Game(true, "Admin");
        var adminUser = game.GameBoards.First();
        adminUser.Id = 1;
        game.UpdateUserList("SubUser");
        var card = GenerateCardRandomly.GetNextUniqueIntegerFunc(game.GivenCards).Invoke();
        game.StartGame(card.cardType, card.value);
        adminUser.TurnOver();
        return game;
    }
    
    public static Game InitiateGameForConfirmCard()
    {
        var game = new Game(true, "Admin");
        var adminUser = game.GameBoards.First();
        adminUser.Id = 1;
        game.UpdateUserList("SubUser");
        game.GenerateNewCard(CardType.Club, 2);
        game.GenerateNewCard(CardType.Diamond, 1);
        var givenCard = game.GivenCards.FirstOrDefault();
        givenCard.CreatedAt = DateTime.Now;
        return game;
    }
}