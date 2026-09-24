using Blackjack.Application.Interfaces;
using Blackjack.Domain.Entities;
using Blackjack.Infrastructure.Data;
using Blackjack.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Infrastructure.Tests
{
    public class UserStatisticsRepositoryTests
    {
        [Fact]
        public void Add_CanSaveStatistics()
        {
            using SqliteConnection connection = new("DataSource=:memory:");
            connection.Open();

            DbContextOptions<BlackjackDbContext> options = new DbContextOptionsBuilder<BlackjackDbContext>()
                .UseSqlite(connection)
                .Options;

            using (BlackjackDbContext context = new(options))
            {
                context.Database.EnsureCreated();

                User user = new User("TestUser");

                context.Users.Add(user);
                context.SaveChanges();

                UserStatistics statistics = new UserStatistics(user.Id);

                statistics.AddWin();
                statistics.AddLoss();
                statistics.AddDraw();

                UserStatisticsRepository repository = new UserStatisticsRepository(context);

                repository.Add(statistics);
                UserStatistics? savedStatistics = repository.GetByUserId(user.Id);

                Assert.NotNull(savedStatistics);
                Assert.Equal(1, savedStatistics.Wins);
                Assert.Equal(1, savedStatistics.Losses);
                Assert.Equal(1, savedStatistics.Draws);
            }
        }

        [Fact]
        public void GetByUserId_ReturnNullForUnknownUser()
        {
            using SqliteConnection connection = new("DataSource=:memory:");
            connection.Open();

            DbContextOptions<BlackjackDbContext> options = new DbContextOptionsBuilder<BlackjackDbContext>()
                .UseSqlite(connection)
                .Options;

            using (BlackjackDbContext context = new(options))
            {
                context.Database.EnsureCreated();

                UserStatisticsRepository repository = new UserStatisticsRepository(context);

                UserStatistics? savedStatistics = repository.GetByUserId(999);

                Assert.Null(savedStatistics);
            }
        }

        [Fact]
        public void Save_UpdatesStatistics()
        {
            using SqliteConnection connection = new("DataSource=:memory:");
            connection.Open();

            DbContextOptions<BlackjackDbContext> options = new DbContextOptionsBuilder<BlackjackDbContext>()
                .UseSqlite(connection)
                .Options;

            using (BlackjackDbContext context = new(options))
            {
                context.Database.EnsureCreated();

                User user = new User("TestUser");

                context.Users.Add(user);
                context.SaveChanges();

                UserStatistics statistics = new UserStatistics(user.Id);

                UserStatisticsRepository repository = new UserStatisticsRepository(context);

                repository.Add(statistics);

                statistics.AddWin();
                statistics.AddWin();
                statistics.AddLoss();

                repository.Save(statistics);

                UserStatistics? savedStatistics = repository.GetByUserId(user.Id);

                Assert.NotNull(savedStatistics);
                Assert.Equal(2, savedStatistics.Wins);
                Assert.Equal(1, savedStatistics.Losses);
                Assert.Equal(0, savedStatistics.Draws);
            }
        }
    }
}
