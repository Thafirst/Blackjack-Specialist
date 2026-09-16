using Blackjack.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Infrastructure
{
    public class Randomizer : IRandomizer
    {
        private readonly Random _random = new();
        public int Next(int maxValue)
        {
            return _random.Next(maxValue);
        }
    }
}
