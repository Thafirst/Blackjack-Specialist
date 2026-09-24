using Blackjack.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Infrastructure.Tests
{
    public class BlackjackDbContextFactoryTests
    {
        [Fact]
        public void Create_ReturnsDbContext()
        {
            using BlackjackDbContext context =
                BlackjackDbContextFactory.Create();

            Assert.NotNull(context);
        }

        [Fact]
        public void Create_ConfiguresSqlite()
        {
            using BlackjackDbContext context =
                BlackjackDbContextFactory.Create();

            Assert.Equal(
                "Microsoft.EntityFrameworkCore.Sqlite",
                context.Database.ProviderName);
        }
    }
}
