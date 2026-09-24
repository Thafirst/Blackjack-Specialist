using Blackjack.Application.Interfaces;
using Blackjack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Application.Services
{
    public class UserStatisticsService
    {
        private readonly IUserStatisticsRepository _statisticsRepository;

        public UserStatisticsService(IUserStatisticsRepository statisticsRepository)
        {
            _statisticsRepository = statisticsRepository;
        }

        public UserStatistics GetOrCreate(int userId)
        {
            UserStatistics? userStatistics = _statisticsRepository.GetByUserId(userId);
            
            if(userStatistics != null)
            {
                return userStatistics;
            }

            userStatistics = new UserStatistics(userId);

            _statisticsRepository.Add(userStatistics);

            return userStatistics;
        }

        public void RecordWin(int userId)
        {
            UserStatistics userStatistics = GetOrCreate(userId);

            userStatistics.AddWin();

            _statisticsRepository.Save(userStatistics);
        }

        public void RecordLoss(int userId)
        {
            UserStatistics userStatistics = GetOrCreate(userId);

            userStatistics.AddLoss();

            _statisticsRepository.Save(userStatistics);
        }

        public void RecordDraw(int userId)
        {
            UserStatistics userStatistics = GetOrCreate(userId);

            userStatistics.AddDraw();

            _statisticsRepository.Save(userStatistics);
        }
    }
}
