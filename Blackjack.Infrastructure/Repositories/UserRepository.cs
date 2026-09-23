using Blackjack.Application.Interfaces;
using Blackjack.Domain.Entities;
using Blackjack.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BlackjackDbContext _context;
        public UserRepository(BlackjackDbContext context)
        {
            _context = context;
        }
        public void Add(User user, UserCredential credentials)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            UserCredential savedCredentials = new UserCredential(user.Id, credentials.PasswordHash);

            _context.UserCredentials.Add(savedCredentials);
            _context.SaveChanges();
        }

        public User? GetByUsername(string username)
        {
            return _context.Users.SingleOrDefault(x => x.Username == username);
        }

        public UserCredential? GetCredentials(int userId)
        {
            return _context.UserCredentials.SingleOrDefault(x => x.UserId == userId);
        }
    }
}
