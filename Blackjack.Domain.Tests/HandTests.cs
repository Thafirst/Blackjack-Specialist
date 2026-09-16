using Blackjack.Domain.Entities;
using Blackjack.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Domain.Tests
{
    public class HandTests
    {
        [Fact]
        public void NewHand_ShouldHaveValueOfZero()
        {
            Hand hand = new Hand();

            int value = hand.Value;

            Assert.Equal(0, value);
        }

        [Fact]
        public void Hand_ShouldCalculateNormalCardValues()
        {
            Hand hand = new Hand();

            hand.AddCard(new Card(Suit.Hearts, Rank.Seven));
            hand.AddCard(new Card(Suit.Spades, Rank.Eight));

            int value = hand.Value;

            Assert.Equal(15, value);
        }

        [Fact]
        public void Hand_ShouldCalculateFaceCardValues()
        {
            Hand hand = new Hand();

            hand.AddCard(new Card(Suit.Hearts, Rank.King));
            hand.AddCard(new Card(Suit.Spades, Rank.Queen));

            int value = hand.Value;

            Assert.Equal(20, value);
        }

        [Fact]
        public void Hand_ShouldCalculateAceAsElevenWhenPossible()
        {
            Hand hand = new Hand();

            hand.AddCard(new Card(Suit.Hearts, Rank.Ace));
            hand.AddCard(new Card(Suit.Spades, Rank.Seven));

            int value = hand.Value;

            Assert.Equal(18, value);
        }

        [Fact]
        public void Hand_ShouldCalculateAceAsOneWhenBust()
        {
            Hand hand = new Hand();

            hand.AddCard(new Card(Suit.Hearts, Rank.Ace));
            hand.AddCard(new Card(Suit.Spades, Rank.Seven));
            hand.AddCard(new Card(Suit.Clubs, Rank.Nine));

            int value = hand.Value;

            Assert.Equal(17, value);
        }

        [Fact]
        public void Hand_ShouldCalculateMultipleAcesCorrectly()
        {
            Hand hand = new Hand();

            hand.AddCard(new Card(Suit.Hearts, Rank.Ace));
            hand.AddCard(new Card(Suit.Spades, Rank.Ace));
            hand.AddCard(new Card(Suit.Clubs, Rank.Nine));

            int value = hand.Value;

            Assert.Equal(21, value);
        }
    }
}
