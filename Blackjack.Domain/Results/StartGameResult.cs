using Blackjack.Domain.Entities;
using Blackjack.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Domain.Results
{
    public class StartGameResult
    {
        public IReadOnlyList<Card> PlayerHand {  get; }
        public int PlayerHandValue {  get; }
        public IReadOnlyList<Card> DealerHand { get; }
        public int DealerHandValue { get; }
        public bool IsBlackjack { get; }

        public StartGameResult(IReadOnlyList<Card> playerHand, IReadOnlyList<Card> dealerHand, bool  isBlackjack,int playerHandValue, int dealerHandValue)
        {
            PlayerHand = playerHand;
            DealerHand = dealerHand;
            IsBlackjack = isBlackjack;
            PlayerHandValue = playerHandValue;
            DealerHandValue = dealerHandValue;
        }
    }
}
