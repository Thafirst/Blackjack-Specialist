using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Domain.Entities
{
    public class User
    {
        public int Id { get; private set; }
        public string Username { get; private set; }

        public User(string username)
        {
            Username = username;
        }
    }
}
