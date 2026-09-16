using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Domain.Interfaces
{
    public interface IRandomizer
    {
        int Next(int maxValue);
    }
}
