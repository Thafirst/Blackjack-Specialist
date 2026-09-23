using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Domain.Entities
{
    public class UserStatistics
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public int Wins { get; private set; }
        public int Losses { get; private set; }
        public int Draws { get; private set; }

        public UserStatistics(int userId)
        {
            UserId = userId;
        }

        public void AddWin()
        {
            Wins++;
        }

        public void AddLoss()
        {
            Losses++;
        }

        public void AddDraw()
        {
            Draws++;
        }
    }
}
