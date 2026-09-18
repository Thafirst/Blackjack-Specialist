using Blackjack.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Domain.Entities
{
    public class Hand
    {
        private readonly List<Card> _cards = new();
        public IReadOnlyList<Card> Cards => _cards;

        public void AddCard(Card card)
        {
            _cards.Add(card);
        }

        public int Value { get
            {
                int value = 0;
                int aceCount = 0;

                foreach(Card card in _cards)
                {
                    switch (card.Rank)
                    {
                        case Rank.Ace:
                            value += 11;
                            aceCount++;
                            break;

                        case Rank.Jack:
                        case Rank.Queen:
                        case Rank.King:
                            value += 10;
                            break;

                        default:
                            value += (int)card.Rank;
                            break;
                    }
                }
                
                while(value > 21 && aceCount > 0)
                {
                    value -= 10;
                    aceCount--;
                }

                return value;
            } }

    }
}
