using Blackjack.Domain.Enums;
using Blackjack.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Domain.Entities
{
    public class Deck
    {
        private readonly List<Card> _cards = new List<Card>();
        private readonly IRandomizer _randomizer;
        public IReadOnlyList<Card> Cards => _cards;

        public Deck(IRandomizer randomizer){
            _randomizer = randomizer;
            foreach(Rank rank in Enum.GetValues<Rank>()){
                foreach(Suit suit in Enum.GetValues<Suit>())
                {
                    _cards.Add(new Card(suit, rank));
                }
            }
        }

        public Card DrawCard()
        {
            if (_cards.Count == 0)
                throw new InvalidOperationException("Cannot draw cards from an empty deck.");

            Card card = _cards[0];
            _cards.RemoveAt(0);

            return card;
        }

        public void Shuffle()
        {
            for (int i = _cards.Count - 1; i > 0; i--)
            {
                int randomIndex = _randomizer.Next(i+1);

                Card temp = _cards[i];
                _cards[i] = _cards[randomIndex];
                _cards[randomIndex] = temp;
            }
        }

    }
}
