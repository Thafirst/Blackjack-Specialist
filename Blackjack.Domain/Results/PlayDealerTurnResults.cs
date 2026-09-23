using Blackjack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Domain.Results
{
    public class PlayDealerTurnResults
    {
        public IReadOnlyList<Card> CardsDrawn { get; }
        public int NewHandValue { get; }

        public PlayDealerTurnResults(IReadOnlyList<Card> cardsDrawn, int newHandValue)
        {
            CardsDrawn = cardsDrawn;
            NewHandValue = newHandValue;
        }

    }
}
