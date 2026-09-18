using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Domain.Enums
{
    public enum GameState
    {
        NotStarted,
        PlayerTurn,
        DealerTurn,
        DealerFinished,
        Finished
    }
}
