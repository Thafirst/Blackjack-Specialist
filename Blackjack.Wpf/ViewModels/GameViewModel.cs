using Blackjack.Application.Services;
using Blackjack.Domain.Entities;
using Blackjack.Domain.Enums;
using Blackjack.Domain.Results;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace Blackjack.Wpf.ViewModels
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private readonly GameService _gameService;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<Card> PlayerCards { get; }
        public ObservableCollection<Card> DealerCards { get; }
        public ObservableCollection<Card> AdditionalDealerCards { get; }
        private string _gameStatus = "Game Not Started";
        public string GameStatus
        {
            get => _gameStatus;
            private set
            {
                _gameStatus = value;
                OnPropertyChanged(nameof(GameStatus));
            }
        }
        private bool _isPlayerTurn = false;
        public bool IsPlayerTurn
        {
            get => _isPlayerTurn;
            private set
            {
                _isPlayerTurn = value;
                OnPropertyChanged(nameof(IsPlayerTurn));
            }
        }
        private bool _CanStartGame = true;
        public bool CanStartGame
        {
            get => _CanStartGame;
            private set
            {
                _CanStartGame = value;
                OnPropertyChanged(nameof(CanStartGame));
            }
        }

        private int _playerHandValue;
        public int PlayerHandValue
        {
            get => _playerHandValue;
            set
            {
                _playerHandValue = value;
                OnPropertyChanged(nameof(PlayerHandValue));
            }
        }

        private int _dealerHandValue;
        public int DealerHandValue
        {
            get => _dealerHandValue;
            set
            {
                _dealerHandValue = value;
                OnPropertyChanged(nameof(DealerHandValue));
            }
        }

        private bool _hideDealerSecondCard;
        public bool HideDealerSecondCard
        {
            get => _hideDealerSecondCard;
            set
            {
                _hideDealerSecondCard = value;
                OnPropertyChanged(nameof(HideDealerSecondCard));
            }
        }

        public GameViewModel(GameService gameService)
        {
            _gameService = gameService;
            PlayerCards = new ObservableCollection<Card>();
            DealerCards = new ObservableCollection<Card>();
            AdditionalDealerCards = new ObservableCollection<Card>();
        }

        public void StartGame(string playerName)
        {
            StartGameResult result = _gameService.StartGame(playerName);

            PlayerCards.Clear();
            DealerCards.Clear();
            AdditionalDealerCards.Clear();
            HideDealerSecondCard = true;

            GameStatus = "Your Turn";
            CanStartGame = false;
            IsPlayerTurn = true;

            foreach (Card card in result.PlayerHand)
                PlayerCards.Add(card);
            PlayerHandValue = result.PlayerHandValue;

            foreach (Card card in result.DealerHand)
                DealerCards.Add(card);
            DealerHandValue = result.DealerHandValue;

            if(result.IsBlackjack)
            {
                HideDealerSecondCard = false;
                GameStatus = "You got blackjack!";
                CanStartGame = true;
                IsPlayerTurn = false;
            }
        }

        public void Hit()
        {
            HitResults result = _gameService.Hit();

            PlayerCards.Add(result.Card);
            PlayerHandValue = result.HandValue;

            if (result.IsBust)
            {
                HideDealerSecondCard = false;
                GameStatus = "You Busted!";
                CanStartGame = true;
                IsPlayerTurn = false;
            }
        }

        public void Stand()
        {
            _gameService.Stand();

            HideDealerSecondCard = false;
            CanStartGame = true;
            IsPlayerTurn = false;

            PlayDealerTurnResults dealerResults = _gameService.PlayDealerTurn();

            foreach(Card card in dealerResults.CardsDrawn)
            {
                AdditionalDealerCards.Add(card);
            }
            DealerHandValue = dealerResults.NewHandValue;

            GameResult results = _gameService.DetermineResult();

            GameStatus = results switch
            {
                GameResult.PlayerWins => "You win!",
                GameResult.DealerWins => "Dealer win!",
                GameResult.Push => "Push!",
                _ => "Game Finished"
            };
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
