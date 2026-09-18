using Blackjack.Domain.Enums;
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
        public bool IsBlackjack => Player.Hand.Cards.Count == 2 && Player.Hand.Value == 21;

        public BlackjackGame(Player player, Dealer dealer, Deck deck)
        {
            Player = player;
            Dealer = dealer;
            Deck = deck;

            State = GameState.NotStarted;
        }

        public void StartGame()
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
                return;
            }

            State = GameState.PlayerTurn;
        }

        public void Hit()
        {
            if (State != GameState.PlayerTurn) 
                throw new InvalidOperationException("It is not the player's turn");

            Player.Hand.AddCard(Deck.DrawCard());

            if(Player.Hand.Value > 21) //TODO: Fix so if dealer also busts, then set to Push.
            {
                State = GameState.Finished;
                Result = GameResult.DealerWins;
            }
        }

        public void Stand()
        {
            if (State != GameState.PlayerTurn)
                throw new InvalidOperationException("It is not the player's turn");

            State = GameState.DealerTurn;
        }

        public void PlayDealerTurn()
        {
            if (State != GameState.DealerTurn)
                throw new InvalidOperationException("It is not the dealer's turn");

            while(Dealer.Hand.Value < 17)
            {
                Dealer.Hand.AddCard(Deck.DrawCard());
            }

             State = GameState.DealerFinished;
        }

        public void DetermineResult()
        {
            if (State != GameState.DealerFinished)
                throw new InvalidOperationException("The dealer has not played their turn yet.");

            if (Player.Hand.Value > 21)
                Result = GameResult.DealerWins;
            else if (Dealer.Hand.Value > 21)
                Result = GameResult.PlayerWins;
            else if (Player.Hand.Value > Dealer.Hand.Value)
                Result = GameResult.PlayerWins;
            else if (Dealer.Hand.Value > Player.Hand.Value)
                Result = GameResult.DealerWins;
            else
                Result = GameResult.Push;

            State = GameState.Finished;
        }
    }
}
