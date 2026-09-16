using Blackjack.Domain.Entities;
using Blackjack.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Domain.Tests
{
    public class DeckTests
    {
        [Fact]
        public void NewDeck_ShouldContain52Cards()
        {
            Deck deck = new Deck(new TestRandomizer());

            int cardCount = deck.Cards.Count;

            Assert.Equal(52, cardCount);
        }

        [Fact]
        public void NewDeck_ShouldContainOneCardOfEverySuitAndRank()
        {
            Deck deck = new Deck(new TestRandomizer());

            foreach(Rank rank in Enum.GetValues<Rank>())
            {
                foreach(Suit suit in Enum.GetValues<Suit>())
                {
                    Assert.Contains(
                        deck.Cards,
                        card => card.Suit == suit && card.Rank == rank);
                }
            }
        }

        [Fact]
        public void DrawCard_ShouldRemoveCardFromDeck()
        {
            Deck deck = new Deck(new TestRandomizer());

            Card card = deck.DrawCard();

            Assert.Equal(51, deck.Cards.Count);
            Assert.DoesNotContain(card, deck.Cards);
        }

        [Fact]
        public void DrawCard_WhenDeckIsEmpty_ShouldThrowException()
        {
            Deck deck = new Deck(new TestRandomizer());

            for(int i = 0; i < 52; i++)
            {
                deck.DrawCard();
            }

            Assert.Throws<InvalidOperationException>(() => deck.DrawCard());
        }

        [Fact]
        public void Shuffle_ShouldChangeCardOrder()
        {
            Deck deck = new Deck(new TestRandomizer());

            IReadOnlyList<Card> cards = deck.Cards.ToList();

            deck.Shuffle();

            Assert.Equal(52, deck.Cards.Count);
            Assert.Equal(
                cards.OrderBy(card => card.Suit).ThenBy(card => card.Rank),
                deck.Cards.OrderBy(card => card.Suit).ThenBy(card => card.Rank));
        }
    }
}
