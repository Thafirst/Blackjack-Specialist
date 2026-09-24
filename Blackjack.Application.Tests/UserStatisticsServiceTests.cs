using Blackjack.Application.Interfaces;
using Blackjack.Application.Services;
using Blackjack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Application.Tests
{
    public class UserStatisticsServiceTests
    {
        private class TestUserStatisticsRepository : IUserStatisticsRepository
        {
            private readonly List<UserStatistics> _statistics = new();

            public bool SaveCalled { get; private set; }

            public UserStatistics? GetByUserId(int userId)
            {
                return _statistics.SingleOrDefault(statistics => statistics.UserId == userId);
            }

            public void Add(UserStatistics statistics)
            {
                _statistics.Add(statistics);
            }

            public void Save(UserStatistics statistics)
            {
                SaveCalled = true;
            }
        }

        [Fact]
        public void GetOrCreate_CreatesStatistics_WhenNoneExist()
        {
            TestUserStatisticsRepository repository =
                new TestUserStatisticsRepository();

            UserStatisticsService service =
                new UserStatisticsService(repository);

            UserStatistics statistics =
                service.GetOrCreate(1);

            Assert.NotNull(statistics);
            Assert.Equal(1, statistics.UserId);
            Assert.Equal(0, statistics.Wins);
            Assert.Equal(0, statistics.Losses);
            Assert.Equal(0, statistics.Draws);
        }

        [Fact]
        public void GetOrCreate_ReturnsExistingStatistics()
        {
            TestUserStatisticsRepository repository =
                new TestUserStatisticsRepository();

            UserStatistics existingStatistics =
                new UserStatistics(1);

            existingStatistics.AddWin();

            repository.Add(existingStatistics);

            UserStatisticsService service =
                new UserStatisticsService(repository);

            UserStatistics statistics =
                service.GetOrCreate(1);

            Assert.Same(existingStatistics, statistics);
            Assert.Equal(1, statistics.Wins);
        }

        [Fact]
        public void RecordWin_AddsWinAndSaves()
        {
            TestUserStatisticsRepository repository =
                new TestUserStatisticsRepository();

            UserStatisticsService service =
                new UserStatisticsService(repository);

            service.RecordWin(1);

            UserStatistics? statistics =
                repository.GetByUserId(1);

            Assert.NotNull(statistics);
            Assert.Equal(1, statistics.Wins);
            Assert.Equal(0, statistics.Losses);
            Assert.Equal(0, statistics.Draws);
            Assert.True(repository.SaveCalled);
        }

        [Fact]
        public void RecordLoss_AddsLossAndSaves()
        {
            TestUserStatisticsRepository repository =
                new TestUserStatisticsRepository();

            UserStatisticsService service =
                new UserStatisticsService(repository);

            service.RecordLoss(1);

            UserStatistics? statistics =
                repository.GetByUserId(1);

            Assert.NotNull(statistics);
            Assert.Equal(0, statistics.Wins);
            Assert.Equal(1, statistics.Losses);
            Assert.Equal(0, statistics.Draws);
            Assert.True(repository.SaveCalled);
        }

        [Fact]
        public void RecordDraw_AddsDrawAndSaves()
        {
            TestUserStatisticsRepository repository =
                new TestUserStatisticsRepository();

            UserStatisticsService service =
                new UserStatisticsService(repository);

            service.RecordDraw(1);

            UserStatistics? statistics =
                repository.GetByUserId(1);

            Assert.NotNull(statistics);
            Assert.Equal(0, statistics.Wins);
            Assert.Equal(0, statistics.Losses);
            Assert.Equal(1, statistics.Draws);
            Assert.True(repository.SaveCalled);
        }


    }
}
