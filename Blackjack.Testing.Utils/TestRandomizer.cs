using Blackjack.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Testing.Utils
{
    public class TestRandomizer : IRandomizer
    {
        public int Next(int maxValue)
        {
            return maxValue-1;
        }
    }
}
