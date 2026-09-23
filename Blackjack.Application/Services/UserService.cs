using Blackjack.Application.Interfaces;
using Blackjack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public void Register(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty.");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("password cannot be empty.");

            User? existingUser = _userRepository.GetByUsername(username);

            if (existingUser != null)
                throw new InvalidOperationException("A user with that username already exists.");


            User user = new User(username);

            string passwordHash = _passwordHasher.Hash(password);

            UserCredential credentials = new UserCredential(user.Id, passwordHash);

            _userRepository.Add(user, credentials);
        }

        public bool Login(string username, string password)
        {
            User? user = _userRepository.GetByUsername(username);

            if (user == null)
                return false;

            UserCredential? credentials = _userRepository.GetCredentials(user.Id);

            if(credentials == null)
                return false;

            return _passwordHasher.Verify(password,credentials.PasswordHash);
        }
    }
}
