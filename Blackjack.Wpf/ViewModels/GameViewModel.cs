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
        private readonly UserStatisticsService _userStatisticsService;
        private int? _loggedInUserId;
        

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

        public GameViewModel(GameService gameService, UserStatisticsService userStatisticsService)
        {
            _gameService = gameService;
            _userStatisticsService = userStatisticsService;
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

                RecordWin();
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

                RecordLoss();
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

            switch (results)
            {
                case GameResult.PlayerWins:
                    GameStatus = "You win!";
                    RecordWin();
                    break;
                case GameResult.DealerWins:
                    GameStatus = "Dealer win!";
                    RecordLoss();
                    break;
                case GameResult.Push:
                    GameStatus = "Push!";
                    RecordDraw();
                    break;
                default:
                    GameStatus = "Game finished!";
                    break;
            }
        }

        public void SetLoggedInUserId(int? userId)
        {
            _loggedInUserId = userId;
        }

        private void RecordWin()
        {
            if (_loggedInUserId == null)
                return;

            _userStatisticsService.RecordWin(_loggedInUserId.Value);
            Wins++;
        }

        private void RecordLoss()
        {
            if (_loggedInUserId == null)
                return;

            _userStatisticsService.RecordLoss(_loggedInUserId.Value);
            Losses++;
        }

        private void RecordDraw()
        {
            if (_loggedInUserId == null)
                return;

            _userStatisticsService.RecordDraw(_loggedInUserId.Value);
            Draws++;
        }

        private int _wins;
        public int Wins
        {
            get => _wins;
            private set
            {
                _wins = value;
                OnPropertyChanged(nameof(Wins));
            }
        }

        private int _losses;
        public int Losses
        {
            get => _losses;
            private set
            {
                _losses = value;
                OnPropertyChanged(nameof(Losses));
            }
        }

        private int _draws;
        public int Draws
        {
            get => _draws;
            private set
            {
                _draws = value;
                OnPropertyChanged(nameof(Draws));
            }
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void LoadStatistics(int? userId)
        {
            _loggedInUserId = userId;

            if (userId == null)
            {
                Wins = 0;
                Losses = 0;
                Draws = 0;
                return;
            }

            UserStatistics statistics =
                _userStatisticsService.GetOrCreate(userId.Value);

            Wins = statistics.Wins;
            Losses = statistics.Losses;
            Draws = statistics.Draws;
        }
    }
}
