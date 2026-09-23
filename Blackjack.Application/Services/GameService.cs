using Blackjack.Domain.Entities;
using Blackjack.Domain.Enums;
using Blackjack.Domain.Interfaces;
using Blackjack.Domain.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Application.Services
{
    public class GameService
    {
        public BlackjackGame? CurrentGame { get; private set; }

        private readonly IRandomizer _randomizer;

        public GameService(IRandomizer randomizer)
        {
            _randomizer = randomizer;
        }

        public StartGameResult StartGame(string playerName)
        {
            Player player = new Player(playerName);
            Dealer dealer = new Dealer();
            Deck deck = new Deck(_randomizer);

            CurrentGame = new BlackjackGame(player, dealer, deck);
            return CurrentGame.StartGame();
        }

        public HitResults Hit()
        {
            if (CurrentGame == null)
                throw new InvalidOperationException("There is no active game");

            return CurrentGame.Hit();
        }

        public void Stand()
        {
            if (CurrentGame == null)
                throw new InvalidOperationException("There is no active game");

            CurrentGame.Stand();
        }

        public PlayDealerTurnResults PlayDealerTurn()
        {
            if (CurrentGame == null)
                throw new InvalidOperationException("There is no active game");

            return CurrentGame.PlayDealerTurn();
        }

        public GameResult DetermineResult()
        {
            if (CurrentGame == null)
                throw new InvalidOperationException("There is no active game");

            return CurrentGame.DetermineResult();
        }


    }
}
