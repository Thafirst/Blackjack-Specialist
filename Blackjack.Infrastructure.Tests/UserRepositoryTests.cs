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
    public class UserRepositoryTests
    {
        [Fact]
        public void Add_CanSaveUserAndCredentials()
        {
            using SqliteConnection connection = new("DataSource=:memory:");
            connection.Open();

            DbContextOptions<BlackjackDbContext> options = new DbContextOptionsBuilder<BlackjackDbContext>()
                .UseSqlite(connection)
                .Options;

            using (BlackjackDbContext context = new(options))
            {
                context.Database.EnsureCreated();

                IUserRepository repository = new UserRepository(context);

                User user = new User("Nicholai");
                UserCredential credentials = new UserCredential(user.Id, "Test-Password-Hash");

                repository.Add(user, credentials);

                User? savedUser = context.Users.SingleOrDefault(x => x.Username == "Nicholai");
                UserCredential? savedCredentials = context.UserCredentials.SingleOrDefault(x => x.Id == user.Id);

                Assert.NotNull(savedUser);
                Assert.Equal("Nicholai", savedUser.Username);

                Assert.NotNull(savedCredentials);
                Assert.Equal(user.Id, savedCredentials.UserId);
                Assert.Equal("Test-Password-Hash", savedCredentials.PasswordHash);
            }
        }

        [Fact]
        public void GetByUsername_ReturnCorrectUser()
        {
            using SqliteConnection connection = new("DataSource=:memory:");
            connection.Open();

            DbContextOptions<BlackjackDbContext> options = new DbContextOptionsBuilder<BlackjackDbContext>()
                .UseSqlite(connection)
                .Options;

            using (BlackjackDbContext context = new(options))
            {
                context.Database.EnsureCreated();

                IUserRepository repository = new UserRepository(context);

                User user = new User("Nicholai");
                UserCredential credentials = new UserCredential(user.Id, "Test-Password-Hash");

                repository.Add(user, credentials);

                User? result = repository.GetByUsername("Nicholai");

                Assert.NotNull(result);
                Assert.Equal("Nicholai", result.Username);
            }
        }

        [Fact]
        public void GetByUsername_ReturnNullForUnknownUser()
        {
            using SqliteConnection connection = new("DataSource=:memory:");
            connection.Open();

            DbContextOptions<BlackjackDbContext> options = new DbContextOptionsBuilder<BlackjackDbContext>()
                .UseSqlite(connection)
                .Options;

            using (BlackjackDbContext context = new(options))
            {
                context.Database.EnsureCreated();

                IUserRepository repository = new UserRepository(context);

                User? result = repository.GetByUsername("DoesNotExist");

                Assert.Null(result);
            }
        }
        [Fact]
        public void GetCredentials_ReturnsCorrectCredentials()
        {
            using SqliteConnection connection = new("DataSource=:memory:");
            connection.Open();

            DbContextOptions<BlackjackDbContext> options = new DbContextOptionsBuilder<BlackjackDbContext>()
                .UseSqlite(connection)
                .Options;

            using (BlackjackDbContext context = new(options))
            {
                context.Database.EnsureCreated();

                IUserRepository repository = new UserRepository(context);

                User user = new User("Nicholai");
                UserCredential credentials = new UserCredential(user.Id, "Test-Password-Hash");

                repository.Add(user, credentials);

                UserCredential? result = repository.GetCredentials(user.Id);

                Assert.NotNull(result);
                Assert.Equal(user.Id, result.Id);
                Assert.Equal("Test-Password-Hash", result.PasswordHash);
            }
        }
        [Fact]
        public void GetCredentials_ReturnsNullForUnknownUser()
        {
            using SqliteConnection connection = new("DataSource=:memory:");
            connection.Open();

            DbContextOptions<BlackjackDbContext> options = new DbContextOptionsBuilder<BlackjackDbContext>()
                .UseSqlite(connection)
                .Options;

            using (BlackjackDbContext context = new(options))
            {
                context.Database.EnsureCreated();

                IUserRepository repository = new UserRepository(context);

                User user = new User("Nicholai");
                UserCredential credentials = new UserCredential(user.Id, "Test-Password-Hash");

                repository.Add(user, credentials);

                UserCredential? result = repository.GetCredentials(42069);

                Assert.Null(result);
            }
        }
    }
}
