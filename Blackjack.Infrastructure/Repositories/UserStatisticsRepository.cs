using Blackjack.Application.Interfaces;
using Blackjack.Domain.Entities;
using Blackjack.Infrastructure.Data;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Infrastructure.Repositories
{
    public class UserStatisticsRepository : IUserStatisticsRepository
    {
        private readonly BlackjackDbContext _context;

        public UserStatisticsRepository(BlackjackDbContext context)
        {
            _context = context;
        }

        public void Add(UserStatistics statistics)
        {
            _context.UserStatistics.Add(statistics);
            _context.SaveChanges();
        }

        public UserStatistics? GetByUserId(int userId)
        {
            return _context.UserStatistics.SingleOrDefault(x => x.UserId == userId);
        }

        public void Save(UserStatistics statistics)
        {
            _context.Update(statistics);
            _context.SaveChanges();
        }
    }
}
