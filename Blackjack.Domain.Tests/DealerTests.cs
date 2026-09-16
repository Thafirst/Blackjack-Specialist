using Blackjack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Domain.Tests
{
    public class DealerTests
    {
        [Fact]
        public void Dealer_ShouldHandEmptyHand()
        {
            Dealer dealer = new Dealer();

            int cardCount = dealer.Hand.Cards.Count;

            Assert.Equal(0, cardCount);
        }
    }
}
