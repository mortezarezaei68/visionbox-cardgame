using CardGame.Core.Domain;
using CardGame.Core.Enums;

namespace CardGame.Core.Services;

public static class GenerateCardRandomly
{
    public static Func<(CardType cardType, int value)> GetNextUniqueIntegerFunc(List<GivenCard> givenCards)
    {
        var rndValue = new Random();
        var rnd = new Random();


        (CardType cardType, int value) GetNextValue()
        {
            while (true)
            {
                var value = rndValue.Next(1, 14);
                var cardType = rnd.Next(4);


                if (givenCards.FirstOrDefault(a => a.CardType == (CardType) cardType && a.Value == value) is not null)
                    continue;

                return ((CardType)cardType, value);
            }
        }

        return GetNextValue;
    }
}