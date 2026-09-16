using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Domain.Entities
{
    public class Dealer
    {
        public Hand Hand { get; }

        public Dealer()
        {
            Hand = new Hand();
        }
    }
}
