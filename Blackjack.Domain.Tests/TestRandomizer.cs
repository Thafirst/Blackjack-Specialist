using Blackjack.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Domain.Tests
{
    public class TestRandomizer : IRandomizer
    {
        public int Next(int maxValue)
        {
            return maxValue-1;
        }
    }
}
