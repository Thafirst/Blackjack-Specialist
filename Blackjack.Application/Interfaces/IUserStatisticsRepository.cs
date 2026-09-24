using Blackjack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Application.Interfaces
{
    public interface IUserStatisticsRepository
    {
        UserStatistics? GetByUserId(int userId);

        void Add(UserStatistics statistics);

        void Save(UserStatistics statistics);
    }
}
