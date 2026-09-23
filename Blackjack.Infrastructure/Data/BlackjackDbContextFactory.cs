using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Infrastructure.Data
{
    public class BlackjackDbContextFactory : IDesignTimeDbContextFactory<BlackjackDbContext>
    {
        public static BlackjackDbContext Create()
        {
            DbContextOptions<BlackjackDbContext> options = new DbContextOptionsBuilder<BlackjackDbContext>()
                .UseSqlite("Data Source=blackjack.db")
                .Options;

            return new BlackjackDbContext(options);
        }

        public BlackjackDbContext CreateDbContext(string[] args)
        {
            return Create();
        }
    }
}
