using CardGame.Core.Enums;

namespace CardGame.Core.Domain;

public class GivenCard:EntityAudit
{
    public static GivenCard Add(CardType cardType, int value)
    {
        return new GivenCard()
        {
            CardType = cardType,
            Value = value
        };
    }

    public void UpdateGivenCardState()
    {
        HasGone = true;
    }

    public int Id { get; private set; }
    public int GameId { get; private set; }
    public CardType CardType { get; private set; }
    public int Value { get; private set; }
    public bool HasGone { get; set; }
    
}