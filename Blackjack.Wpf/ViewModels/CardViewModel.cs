using Blackjack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Wpf.ViewModels
{
    public class CardViewModel
    {
        public Card Card { get; }
        public bool IsHidden { get; }
        public string DisplayRank => IsHidden ? "" : Card.Rank.ToString();
        public string DisplaySuit => IsHidden ? "" : Card.Suit.ToString();

        public CardViewModel(Card card, bool isHidden = false)
        {
            Card = card;
            IsHidden = isHidden;
        }
    }
}
