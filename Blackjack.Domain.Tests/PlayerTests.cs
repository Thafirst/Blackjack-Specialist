using Blackjack.Domain.Entities;
using Blackjack.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Domain.Tests
{
    public class PlayerTests
    {
        [Fact]
        public void Player_ShouldHaveCorrectName()
        {
            Player player = new Player("Nicholai");

            string name = player.Name;

            Assert.Equal("Nicholai", name);
        }

        [Fact]
        public void Player_ShouldHaveEmptyHand()
        {
            Player player = new Player("Nicholai");

            int cardCount = player.Hand.Cards.Count;

            Assert.Equal(0, cardCount);
        }

        [Fact]
        public void Player_ShouldBeAbleToAddCardsToHand()
        {
            Player player = new Player("Nicholai");

            player.Hand.AddCard(new Card(Suit.Hearts, Rank.Ace));

            Assert.Single(player.Hand.Cards);
        }
    }
}
