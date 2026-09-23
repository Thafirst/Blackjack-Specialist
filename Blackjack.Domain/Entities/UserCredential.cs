using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Domain.Entities
{
    public class UserCredential
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public string PasswordHash { get; private set; }

        public UserCredential(int userId, string passwordHash)
        {
            UserId = userId;
            PasswordHash = passwordHash;
        }
    }
}
