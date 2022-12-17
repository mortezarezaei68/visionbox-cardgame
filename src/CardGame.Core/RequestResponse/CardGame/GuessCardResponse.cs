using CardGame.Core.Enums;

namespace CardGame.Core.RequestResponse.CardGame;

public class GuessCardResponse
{
    public bool IsConfirmed { get; set; }
    public bool IsFinished { get; set; }
    public bool RoundFinished { get; set; }
    public int Value { get; set; }
    public CardType CardType { get; set; }
    public int? UserId { get; set; }
    public List<ScoreResponse> ScoreResponses { get; set; }
}