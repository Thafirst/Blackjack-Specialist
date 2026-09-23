using Blackjack.Domain.Entities;
using Blackjack.Infrastructure.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Infrastructure.Tests
{
    public class BlackjackDbContextTests
    {
        [Fact]
        public void CanSaveAndRetrieveUserWithCredentials()
        {
            using SqliteConnection connection = new("DataSource=:memory:");
            connection.Open();

            DbContextOptions<BlackjackDbContext> options = new DbContextOptionsBuilder<BlackjackDbContext>()
                .UseSqlite(connection)
                .Options;

            using (BlackjackDbContext context = new(options))
            {
                context.Database.EnsureCreated();

                User user = new User("Nicholai");

                context.Users.Add(user);
                context.SaveChanges();

                UserCredential credentials = new UserCredential(user.Id, "Test-Password-Hash");

                context.UserCredentials.Add(credentials);
                context.SaveChanges();

                User? savedUser = context.Users.SingleOrDefault(x => x.Username == "Nicholai");
                UserCredential? savedCredentials = context.UserCredentials.SingleOrDefault(x => x.Id == user.Id);

                Assert.NotNull(savedUser);
                Assert.Equal("Nicholai", savedUser.Username);

                Assert.NotNull(savedCredentials);
                Assert.Equal(user.Id, savedCredentials.UserId);
                Assert.Equal("Test-Password-Hash", savedCredentials.PasswordHash);
            }
        }
    }
}
