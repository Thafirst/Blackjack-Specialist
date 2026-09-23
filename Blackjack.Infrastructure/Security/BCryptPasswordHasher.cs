using BCrypt.Net;
using Blackjack.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Infrastructure.Security
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool Verify(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
