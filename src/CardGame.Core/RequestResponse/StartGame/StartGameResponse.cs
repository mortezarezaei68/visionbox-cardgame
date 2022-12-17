using CardGame.Core.Enums;

namespace CardGame.Core.RequestResponse.StartGame;

public class StartGameResponse
{
    public int Value { get; set; }
    public CardType CardType { get; set; }
    public int GameBoardId { get; set; }
}