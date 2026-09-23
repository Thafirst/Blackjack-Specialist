using Blackjack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Application.Interfaces
{
    public interface IUserRepository
    {
        User? GetByUsername(string username);

        UserCredential? GetCredentials(int userId);

        void Add(User user, UserCredential credentials);
    }
}
