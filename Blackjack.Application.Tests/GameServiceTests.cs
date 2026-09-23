using Blackjack.Domain.Enums;
using Blackjack.Domain.Interfaces;
using Blackjack.Testing.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Application.Tests
{
    public class GameServiceTests
    {
        [Fact]
        public void StartGame_ShouldCreateAndStartTheGame()
        {
            IRandomizer randomizer = new TestRandomizer();
            GameService gameService = new GameService(randomizer);

            gameService.StartGame("Nicholai");

            Assert.NotNull(gameService.CurrentGame);
            Assert.Equal(GameState.PlayerTurn, gameService.CurrentGame.State);
        }

        [Fact]
        public void StartGame_ShouldCreatePlayerWithCorrectName()
        {
            IRandomizer randomizer = new TestRandomizer();
            GameService gameService = new GameService(randomizer);

            gameService.StartGame("Nicholai");

            Assert.NotNull(gameService.CurrentGame);
            Assert.Equal("Nicholai", gameService.CurrentGame.Player.Name);
        }

        [Fact]
        public void Hit_ShouldGivePlayerAnotherCard()
        {
            IRandomizer randomizer = new TestRandomizer();
            GameService gameService = new GameService(randomizer);

            gameService.StartGame("Nicholai");

            gameService.Hit();

            Assert.NotNull(gameService.CurrentGame);
            Assert.Equal(3, gameService.CurrentGame.Player.Hand.Cards.Count);
        }

        [Fact]
        public void STand_ShouldChangeStateToDealerTurn()
        {
            IRandomizer randomizer = new TestRandomizer();
            GameService gameService = new GameService(randomizer);

            gameService.StartGame("Nicholai");

            gameService.Hit();

            gameService.Stand();

            Assert.NotNull(gameService.CurrentGame);
            Assert.Equal(GameState.DealerTurn, gameService.CurrentGame.State);
        }

        [Fact]
        public void Hit_WithoutActiveGame_ShouldThrowException()
        {
            IRandomizer randomizer = new TestRandomizer();
            GameService gameService = new GameService(randomizer);

            Assert.Throws<InvalidOperationException>(() => gameService.Hit());
        }

        [Fact]
        public void Stand_WithoutActiveGame_ShouldThrowException()
        {
            IRandomizer randomizer = new TestRandomizer();
            GameService gameService = new GameService(randomizer);

            Assert.Throws<InvalidOperationException>(() => gameService.Stand());
        }

        [Fact]
        public void PlayDealerTurn_WithoutActiveGame_ShouldThrowException()
        {
            IRandomizer randomizer = new TestRandomizer();
            GameService gameService = new GameService(randomizer);

            Assert.Throws<InvalidOperationException>(() => gameService.PlayDealerTurn());
        }

        [Fact]
        public void PlayDealerTurn_ShouldPlayDealersTurn()
        {
            IRandomizer randomizer = new TestRandomizer();
            GameService gameService = new GameService(randomizer);

            gameService.StartGame("Nicholai");

            gameService.Stand();

            gameService.PlayDealerTurn();

            Assert.NotNull(gameService.CurrentGame);
            Assert.True(gameService.CurrentGame.Dealer.Hand.Value >= 17);
        }

        [Fact]
        public void DetermineResult_WithoutActiveGame_ShouldThrowException()
        {
            IRandomizer randomizer = new TestRandomizer();
            GameService gameService = new GameService(randomizer);

            Assert.Throws<InvalidOperationException>(() => gameService.DetermineResult());
        }

        [Fact]
        public void DetermineResult_ShouldFinishTheGame()
        {
            IRandomizer randomizer = new TestRandomizer();
            GameService gameService = new GameService(randomizer);

            gameService.StartGame("Nicholai");

            gameService.Stand();

            gameService.PlayDealerTurn();

            gameService.DetermineResult();

            Assert.NotNull(gameService.CurrentGame);
            Assert.Equal(GameState.Finished, gameService.CurrentGame.State);
        }
    }
}
