using Blackjack.Domain.Entities;
using Blackjack.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Domain.Tests
{
    public class BlackjackGameTests
    {
        [Fact]
        public void Start_ShouldDealTwoCardsToPlayerAndDealer()
        {
            Player player = new Player("Nicholai");
            Dealer dealer = new Dealer();
            Deck deck = new Deck(new TestRandomizer());

            BlackjackGame game = new BlackjackGame(player, dealer, deck);

            game.StartGame();

            Assert.Equal(2, game.Player.Hand.Cards.Count);
            Assert.Equal(2, game.Dealer.Hand.Cards.Count);
        }

        [Fact]
        public void Start_ShouldSetStateToPlayerTurn()
        {
            Player player = new Player("Nicholai");
            Dealer dealer = new Dealer();
            Deck deck = new Deck(new TestRandomizer());

            BlackjackGame game = new BlackjackGame(player, dealer, deck);

            game.StartGame();

            Assert.Equal(GameState.PlayerTurn, game.State);
        }

        [Fact]
        public void Start_ShouldRemoveFourCardsFromDeck()
        {
            Player player = new Player("Nicholai");
            Dealer dealer = new Dealer();
            Deck deck = new Deck(new TestRandomizer());

            BlackjackGame game = new BlackjackGame(player, dealer, deck);

            game.StartGame();

            Assert.Equal(48, game.Deck.Cards.Count);
        }

        [Fact]
        public void Start_ShouldPlayerShouldWinOnBlackjack()
        {
            Player player = new Player("Nicholai");
            Dealer dealer = new Dealer();
            List<Card> cards =
            [
                new Card(Suit.Hearts, Rank.Ace),
                new Card(Suit.Diamonds, Rank.Five),
                new Card(Suit.Hearts, Rank.Jack),
                new Card(Suit.Diamonds, Rank.Five),
            ];
            Deck deck = new Deck(new TestRandomizer(), cards);

            BlackjackGame game = new BlackjackGame(player, dealer, deck);

            game.StartGame();

            Assert.Equal(GameState.Finished, game.State);
            Assert.Equal(GameResult.PlayerWins, game.Result);
        }

        [Fact]
        public void Start_ShouldThrowException_WhenCalledTwice()
        {
            Player player = new Player("Nicholai");
            Dealer dealer = new Dealer();
            Deck deck = new Deck(new TestRandomizer());

            BlackjackGame game = new BlackjackGame(player, dealer, deck);

            game.StartGame();

            Assert.Throws<InvalidOperationException>(() => game.StartGame());
        }

        [Fact]
        public void Hit_ShouldGivePlayerAnotherCard()
        {
            Player player = new Player("Nicholai");
            Dealer dealer = new Dealer();
            Deck deck = new Deck(new TestRandomizer());

            BlackjackGame game = new BlackjackGame(player, dealer, deck);

            game.StartGame();

            game.Hit();

            Assert.Equal(3, game.Player.Hand.Cards.Count);
        }

        [Fact]
        public void Hit_BeforeGameStart_ShouldThrowException()
        {
            Player player = new Player("Nicholai");
            Dealer dealer = new Dealer();
            Deck deck = new Deck(new TestRandomizer());

            BlackjackGame game = new BlackjackGame(player, dealer, deck);

            Assert.Throws<InvalidOperationException>(() => game.Hit());
        }

        [Fact]
        public void Hit_ShouldNotBustUnderTwentyOne()
        {
            List<Card> cards =
            [
                new Card(Suit.Hearts, Rank.Seven),
                new Card(Suit.Diamonds, Rank.Five),
                new Card(Suit.Hearts, Rank.Seven),
                new Card(Suit.Diamonds, Rank.Five),
                new Card(Suit.Hearts, Rank.Seven),
                new Card(Suit.Diamonds, Rank.Five),
            ];

            Player player = new Player("Nicholai");
            Dealer dealer = new Dealer();
            Deck deck = new Deck(new TestRandomizer(), cards);

            BlackjackGame game = new BlackjackGame(player, dealer, deck);

            game.StartGame();

            game.Hit();

            Assert.Equal(GameState.PlayerTurn, game.State);
        }

        [Fact]
        public void Hit_ShouldBustOverTwentyOne()
        {
            List<Card> cards =
            [
                new Card(Suit.Hearts, Rank.Seven),
                new Card(Suit.Diamonds, Rank.Five),
                new Card(Suit.Hearts, Rank.Seven),
                new Card(Suit.Diamonds, Rank.Five),
                new Card(Suit.Hearts, Rank.King),
                new Card(Suit.Diamonds, Rank.Five),
            ];

            Player player = new Player("Nicholai");
            Dealer dealer = new Dealer();
            Deck deck = new Deck(new TestRandomizer(), cards);

            BlackjackGame game = new BlackjackGame(player, dealer, deck);

            game.StartGame();

            game.Hit();

            Assert.Equal(GameState.Finished, game.State);
            Assert.Equal(GameResult.DealerWins, game.Result);
        }

        [Fact]
        public void Hit_ShouldDealExpectedCards()
        {
            List<Card> cards =
            [
                new Card(Suit.Hearts, Rank.Seven),
                new Card(Suit.Diamonds, Rank.Five),
                new Card(Suit.Hearts, Rank.Jack),
                new Card(Suit.Diamonds, Rank.Queen),
            ];

            Player player = new Player("Nicholai");
            Dealer dealer = new Dealer();
            Deck deck = new Deck(new TestRandomizer(), cards);

            BlackjackGame game = new BlackjackGame(player, dealer, deck);

            game.StartGame();

            Assert.Equal(Rank.Seven, game.Player.Hand.Cards[0].Rank);
            Assert.Equal(Rank.Five, game.Dealer.Hand.Cards[0].Rank);
            Assert.Equal(Rank.Jack, game.Player.Hand.Cards[1].Rank);
            Assert.Equal(Rank.Queen, game.Dealer.Hand.Cards[1].Rank);
        }

        [Fact]
        public void Hit_AfterFinishedGame_ShouldThrowException()
        {
            Player player = new Player("Nicholai");
            Dealer dealer = new Dealer();
            List<Card> cards =
            [
                new Card(Suit.Hearts, Rank.Ace),
                new Card(Suit.Diamonds, Rank.Five),
                new Card(Suit.Hearts, Rank.Jack),
                new Card(Suit.Diamonds, Rank.Five),
            ];
            Deck deck = new Deck(new TestRandomizer(), cards);

            BlackjackGame game = new BlackjackGame(player, dealer, deck);

            game.StartGame();

            Assert.Throws<InvalidOperationException>(() => game.Hit());
        }

        [Fact]
        public void Stand_WhenNotPlayerTurn_ShouldThrowException()
        {
            Player player = new Player("Nicholai");
            Dealer dealer = new Dealer();
            Deck deck = new Deck(new TestRandomizer());

            BlackjackGame game = new BlackjackGame(player, dealer, deck);

            Assert.Throws<InvalidOperationException>(() => game.Stand());
        }
        

        [Fact]
        public void Stand_WhenOverTwentyOne_ShouldThrowException()
        {
            Player player = new Player("Nicholai");
            Dealer dealer = new Dealer();
            List<Card> cards =
            [
                new Card(Suit.Hearts, Rank.Seven),
                new Card(Suit.Diamonds, Rank.Five),
                new Card(Suit.Hearts, Rank.Seven),
                new Card(Suit.Diamonds, Rank.Five),
                new Card(Suit.Hearts, Rank.Jack),
                new Card(Suit.Diamonds, Rank.Five),
            ];
            Deck deck = new Deck(new TestRandomizer(), cards);

            BlackjackGame game = new BlackjackGame(player, dealer, deck);

            game.StartGame();
            
            game.Hit();

            Assert.Throws<InvalidOperationException>(() => game.Stand());
        }

        [Fact]
        public void Stand_ShouldChangeToDealersTurn()
        {
            List<Card> cards =
            [
                new Card(Suit.Hearts, Rank.Seven),
                new Card(Suit.Diamonds, Rank.Five),
                new Card(Suit.Hearts, Rank.Seven),
                new Card(Suit.Diamonds, Rank.Five),
                new Card(Suit.Hearts, Rank.Ace),
                new Card(Suit.Diamonds, Rank.Five),
            ];

            Player player = new Player("Nicholai");
            Dealer dealer = new Dealer();
            Deck deck = new Deck(new TestRandomizer(), cards);

            BlackjackGame game = new BlackjackGame(player, dealer, deck);

            game.StartGame();

            game.Stand();

            Assert.Equal(GameState.DealerTurn, game.State);
        }

        [Fact]
        public void PlayDealerTurn_ShouldDrawUntilSeventeen()
        {
            List<Card> cards =
            [
                new Card(Suit.Hearts, Rank.Seven),
                new Card(Suit.Diamonds, Rank.Five),
                new Card(Suit.Hearts, Rank.Seven),
                new Card(Suit.Diamonds, Rank.Six),
                new Card(Suit.Hearts, Rank.Ace),
                new Card(Suit.Diamonds, Rank.Four),
                new Card(Suit.Hearts, Rank.Ace),
                new Card(Suit.Diamonds, Rank.Two),
                new Card(Suit.Hearts, Rank.Ace),
                new Card(Suit.Diamonds, Rank.Two),
                new Card(Suit.Hearts, Rank.Ace),
                new Card(Suit.Diamonds, Rank.Two),
            ];

            Player player = new Player("Nicholai");
            Dealer dealer = new Dealer();
            Deck deck = new Deck(new TestRandomizer(), cards);

            BlackjackGame game = new BlackjackGame(player, dealer, deck);

            game.StartGame();

            game.Stand();

            game.PlayDealerTurn();

            Assert.Equal(17, game.Dealer.Hand.Value);
        }

        [Fact]
        public void PlayDealerTurn_ShouldNotDrawIfAlreadySeventeen()
        {
            List<Card> cards =
            [
                new Card(Suit.Hearts, Rank.Seven),
                new Card(Suit.Diamonds, Rank.Jack),
                new Card(Suit.Hearts, Rank.Seven),
                new Card(Suit.Diamonds, Rank.Seven),
                new Card(Suit.Hearts, Rank.Ace),
                new Card(Suit.Diamonds, Rank.Four),
                new Card(Suit.Hearts, Rank.Ace),
                new Card(Suit.Diamonds, Rank.Two),
            ];

            Player player = new Player("Nicholai");
            Dealer dealer = new Dealer();
            Deck deck = new Deck(new TestRandomizer(), cards);

            BlackjackGame game = new BlackjackGame(player, dealer, deck);

            game.StartGame();

            game.Stand();

            game.PlayDealerTurn();

            Assert.Equal(17, game.Dealer.Hand.Value);
        }

        [Fact]
        public void PlayDealerHand_ShouldReturnExceptionIfNotDealerTurn()
        {
            Player player = new Player("Nicholai");
            Dealer dealer = new Dealer();
            Deck deck = new Deck(new TestRandomizer());

            BlackjackGame game = new BlackjackGame(player, dealer, deck);

            Assert.Throws<InvalidOperationException>(() => game.PlayDealerTurn());
        }

        [Fact]
        public void PlayDealerTurn_ShouldChangeToDealerFinished()
        {
            List<Card> cards =
            [
                new Card(Suit.Hearts, Rank.Seven),
                new Card(Suit.Diamonds, Rank.Jack),
                new Card(Suit.Hearts, Rank.Seven),
                new Card(Suit.Diamonds, Rank.Seven),
                new Card(Suit.Hearts, Rank.Ace),
                new Card(Suit.Diamonds, Rank.Four),
                new Card(Suit.Hearts, Rank.Ace),
                new Card(Suit.Diamonds, Rank.Two),
            ];

            Player player = new Player("Nicholai");
            Dealer dealer = new Dealer();
            Deck deck = new Deck(new TestRandomizer(), cards);

            BlackjackGame game = new BlackjackGame(player, dealer, deck);

            game.StartGame();

            game.Stand();

            game.PlayDealerTurn();

            Assert.Equal(GameState.DealerFinished, game.State);
        }

        [Fact]
        public void DetermineResult_ShouldReturnPlayerWins_WhenPlayerHandIsHigher()
        {
            List<Card> cards =
            [
                new Card(Suit.Hearts, Rank.Eight),
                new Card(Suit.Diamonds, Rank.Seven),
                new Card(Suit.Hearts, Rank.King),
                new Card(Suit.Diamonds, Rank.Queen)
            ];

            Player player = new Player("Nicholai");
            Dealer dealer = new Dealer();
            Deck deck = new Deck(new TestRandomizer(), cards);

            BlackjackGame game = new BlackjackGame(player, dealer, deck);

            game.StartGame();

            game.Stand();

            game.PlayDealerTurn();

            game.DetermineResult();

            Assert.Equal(GameState.Finished, game.State);
            Assert.Equal(GameResult.PlayerWins, game.Result);
        }

        [Fact]
        public void DetermineResult_ShouldReturnDealerWins_WhenDealerHandIsHigher()
        {
            List<Card> cards =
            [
                new Card(Suit.Hearts, Rank.Eight),
                new Card(Suit.Diamonds, Rank.Seven),
                new Card(Suit.Hearts, Rank.Two),
                new Card(Suit.Diamonds, Rank.Queen)
            ];

            Player player = new Player("Nicholai");
            Dealer dealer = new Dealer();
            Deck deck = new Deck(new TestRandomizer(), cards);

            BlackjackGame game = new BlackjackGame(player, dealer, deck);

            game.StartGame();

            game.Stand();

            game.PlayDealerTurn();

            game.DetermineResult();

            Assert.Equal(GameState.Finished, game.State);
            Assert.Equal(GameResult.DealerWins, game.Result);
        }

        [Fact]
        public void DetermineResult_ShouldReturnPush_WhenHandsAreEqual()
        {
            List<Card> cards =
            [
                new Card(Suit.Hearts, Rank.Eight),
                new Card(Suit.Diamonds, Rank.Seven),
                new Card(Suit.Hearts, Rank.Nine),
                new Card(Suit.Diamonds, Rank.Queen)
            ];

            Player player = new Player("Nicholai");
            Dealer dealer = new Dealer();
            Deck deck = new Deck(new TestRandomizer(), cards);

            BlackjackGame game = new BlackjackGame(player, dealer, deck);

            game.StartGame();

            game.Stand();

            game.PlayDealerTurn();

            game.DetermineResult();

            Assert.Equal(GameState.Finished, game.State);
            Assert.Equal(GameResult.Push, game.Result);
        }

        [Fact]
        public void DetermineResult_ShouldReturnPlayerWins_WhenDealerBust()
        {
            List<Card> cards =
            [
                new Card(Suit.Hearts, Rank.Six),
                new Card(Suit.Diamonds, Rank.Seven),
                new Card(Suit.Hearts, Rank.Nine),
                new Card(Suit.Diamonds, Rank.Nine),
                new Card(Suit.Hearts, Rank.Nine),
                new Card(Suit.Diamonds, Rank.Nine),
            ];

            Player player = new Player("Nicholai");
            Dealer dealer = new Dealer();
            Deck deck = new Deck(new TestRandomizer(), cards);

            BlackjackGame game = new BlackjackGame(player, dealer, deck);

            game.StartGame();

            game.Stand();

            game.PlayDealerTurn();

            game.DetermineResult();

            Assert.Equal(GameState.Finished, game.State);
            Assert.Equal(GameResult.PlayerWins, game.Result);
        }

        [Fact]
        public void DetermineResult_ShouldReturnExceptionIfPlayerTurn()
        {
            Player player = new Player("Nicholai");
            Dealer dealer = new Dealer();
            Deck deck = new Deck(new TestRandomizer());

            BlackjackGame game = new BlackjackGame(player, dealer, deck);

            game.StartGame();

            Assert.Throws<InvalidOperationException>(() => game.DetermineResult());
        }

        [Fact]
        public void Start_ShouldReturnBlackjackWhenNaturalTwentyOne()
        {
            List<Card> cards =
            [
                new Card(Suit.Hearts, Rank.Ace),
                new Card(Suit.Diamonds, Rank.Seven),
                new Card(Suit.Hearts, Rank.King),
                new Card(Suit.Diamonds, Rank.Nine),
                new Card(Suit.Hearts, Rank.Nine),
                new Card(Suit.Diamonds, Rank.Nine),
            ];

            Player player = new Player("Nicholai");
            Dealer dealer = new Dealer();
            Deck deck = new Deck(new TestRandomizer(), cards);

            BlackjackGame game = new BlackjackGame(player, dealer, deck);

            game.StartGame();

            Assert.True(game.IsBlackjack);
        }

        [Fact]
        public void Start_ShouldReturnNoBlackjackWhenNotTwentyOne()
        {
            List<Card> cards =
            [
                new Card(Suit.Hearts, Rank.Ace),
                new Card(Suit.Diamonds, Rank.Seven),
                new Card(Suit.Hearts, Rank.Nine),
                new Card(Suit.Diamonds, Rank.Nine),
                new Card(Suit.Hearts, Rank.Nine),
                new Card(Suit.Diamonds, Rank.Nine),
            ];

            Player player = new Player("Nicholai");
            Dealer dealer = new Dealer();
            Deck deck = new Deck(new TestRandomizer(), cards);

            BlackjackGame game = new BlackjackGame(player, dealer, deck);

            game.StartGame();

            Assert.False(game.IsBlackjack);
        }

        [Fact]
        public void Start_ShouldReturnNoBlackjackWhenThreeOrMoreCards()
        {
            List<Card> cards =
            [
                new Card(Suit.Hearts, Rank.Ace),
                new Card(Suit.Diamonds, Rank.Seven),
                new Card(Suit.Hearts, Rank.Five),
                new Card(Suit.Diamonds, Rank.Nine),
                new Card(Suit.Hearts, Rank.Five),
                new Card(Suit.Diamonds, Rank.Nine),
            ];

            Player player = new Player("Nicholai");
            Dealer dealer = new Dealer();
            Deck deck = new Deck(new TestRandomizer(), cards);

            BlackjackGame game = new BlackjackGame(player, dealer, deck);

            game.StartGame();

            Assert.False(game.IsBlackjack);
        }
    }
}
