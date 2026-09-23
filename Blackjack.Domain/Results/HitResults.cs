using Blackjack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Domain.Results
{
    public class HitResults
    {
        public Card Card { get; }
        public int HandValue { get; }
        public bool IsBust { get; }

        public HitResults(Card card, int handValue, bool isBust)
        {
            Card = card;
            HandValue = handValue;
            IsBust = isBust;
        }
    }
}
