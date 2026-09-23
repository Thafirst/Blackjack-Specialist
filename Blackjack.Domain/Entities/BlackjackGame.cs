using Blackjack.Domain.Enums;
using Blackjack.Domain.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Domain.Entities
{
    public class BlackjackGame
    {
        public Player Player { get; }
        public Dealer Dealer { get; }
        public Deck Deck { get; }
        
        public GameState State { get; private set; }
        public GameResult? Result { get; private set; }
        public bool IsBlackjack => Player.Hand.Cards.Count == 2 && Player.Hand.Value == 21 && !(Dealer.Hand.Cards.Count == 2 && Dealer.Hand.Value == 21);
        public bool IsBust = false;

        public BlackjackGame(Player player, Dealer dealer, Deck deck)
        {
            Player = player;
            Dealer = dealer;
            Deck = deck;

            State = GameState.NotStarted;
        }

        public StartGameResult StartGame()
        {
            if (State != GameState.NotStarted)
                throw new InvalidOperationException("The game has already started.");

            Deck.Shuffle();

            Player.Hand.AddCard(Deck.DrawCard());
            Dealer.Hand.AddCard(Deck.DrawCard());

            Player.Hand.AddCard(Deck.DrawCard());
            Dealer.Hand.AddCard(Deck.DrawCard());

            if (IsBlackjack)
            {
                State = GameState.Finished;
                Result = GameResult.PlayerWins;
            } else
                State = GameState.PlayerTurn;

            return new StartGameResult(Player.Hand.Cards.ToList(), Dealer.Hand.Cards.ToList(), IsBlackjack, Player.Hand.Value, Dealer.Hand.Value);

        }

        public HitResults Hit()
        {
            if (State != GameState.PlayerTurn) 
                throw new InvalidOperationException("It is not the player's turn");
            Card card = Deck.DrawCard();
            Player.Hand.AddCard(card);

            if(Player.Hand.Value > 21) //TODO: Fix so if dealer also busts, then set to Push.
            {
                IsBust = true;
                State = GameState.Finished;
                Result = GameResult.DealerWins;
            }

            return new HitResults(card, Player.Hand.Value, IsBust);
        }

        public void Stand()
        {
            if (State != GameState.PlayerTurn)
                throw new InvalidOperationException("It is not the player's turn");

            State = GameState.DealerTurn;
        }

        public PlayDealerTurnResults PlayDealerTurn()
        {
            if (State != GameState.DealerTurn)
                throw new InvalidOperationException("It is not the dealer's turn");
            List<Card> newCards = new List<Card>();

            while(Dealer.Hand.Value < 17)
            {
                Card card = Deck.DrawCard();
                Dealer.Hand.AddCard(card);
                newCards.Add(card);
            }

            PlayDealerTurnResults results = new PlayDealerTurnResults(newCards, Dealer.Hand.Value);

            State = GameState.DealerFinished;

            return results;
        }

        public GameResult DetermineResult()
        {
            if (State != GameState.DealerFinished)
                throw new InvalidOperationException("The dealer has not played their turn yet.");

            if (Dealer.Hand.Value > 21)
                Result = GameResult.PlayerWins;
            else if (Player.Hand.Value > Dealer.Hand.Value)
                Result = GameResult.PlayerWins;
            else if (Dealer.Hand.Value > Player.Hand.Value)
                Result = GameResult.DealerWins;
            else
                Result = GameResult.Push;

            State = GameState.Finished;

            return Result.Value;
        }
    }
}
