using Blackjack.Application.Interfaces;
using Blackjack.Application.Services;
using Blackjack.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Application.Tests
{
    public class UserServiceTest
    {
        private class TestUserRepository : IUserRepository
        {
            public User? User { get; set; }
            public UserCredential? UserCredential { get; set; }

            public void Add(User user, UserCredential credentials)
            {
                User = user;
                UserCredential = credentials;
            }

            public User? GetByUsername(string username)
            {
                if (User?.Username == username)
                    return User;
                return null;
            }

            public UserCredential? GetCredentials(int userId)
            {
                if(UserCredential?.UserId == userId)
                    return UserCredential;
                return null;
            }
        }

        private class TestPasswordHasher : IPasswordHasher
        {
            public string HashResult { get; set; } = "Hashed-Password";
            public bool VerifyResult { get; set; }
            public string Hash(string password)
            {
                return HashResult;
            }

            public bool Verify(string password, string passwordHash)
            {
                return VerifyResult;
            }
        }

        [Fact]
        public void Register_CreatesUser()
        {
            TestUserRepository userRepository = new TestUserRepository();
            TestPasswordHasher passwordHasher = new TestPasswordHasher();

            UserService service = new UserService(userRepository, passwordHasher);

            service.Register("Nicholai", "MyPassword");

            Assert.NotNull(userRepository.User);
            Assert.Equal("Nicholai", userRepository.User.Username);
        }

        [Fact]
        public void Register_HashesPassword()
        {
            TestUserRepository userRepository = new TestUserRepository();
            TestPasswordHasher passwordHasher = new TestPasswordHasher();

            passwordHasher.HashResult = "My-Hash";

            UserService service = new UserService(userRepository, passwordHasher);

            service.Register("Nicholai", "MyPassword");

            Assert.NotNull(userRepository.UserCredential);
            Assert.Equal("My-Hash", userRepository.UserCredential.PasswordHash);
        }

        [Fact]
        public void Register_ThrowsIfUsernameAlreadyExists()
        {
            TestUserRepository userRepository = new TestUserRepository()
            {
                User = new User("Nicholai")
            };
            TestPasswordHasher passwordHasher = new TestPasswordHasher();

            UserService service = new UserService(userRepository, passwordHasher);

            Assert.Throws<InvalidOperationException>(() => service.Register("Nicholai", "MyPassword"));
        }

        [Fact]
        public void Login_ReturnsFalseForUnknownUser()
        {
            TestUserRepository userRepository = new TestUserRepository();
            TestPasswordHasher passwordHasher = new TestPasswordHasher();

            UserService service = new UserService(userRepository, passwordHasher);

            bool result = service.Login("Nicholai", "MyPassword");

            Assert.False(result);
        }

        [Fact]
        public void Login_ReturnsFalseIfCredentialsAreMissing()
        {
            TestUserRepository userRepository = new TestUserRepository()
            {
                User = new User("Nicholai")
            };
            TestPasswordHasher passwordHasher = new TestPasswordHasher();

            UserService service = new UserService(userRepository, passwordHasher);

            bool result = service.Login("Nicholai", "MyPassword");

            Assert.False(result);
        }

        [Fact]
        public void Login_ReturnsTrueForCorrectPassword()
        {
            TestUserRepository userRepository = new TestUserRepository();
            TestPasswordHasher passwordHasher = new TestPasswordHasher()
            {
                VerifyResult = true
            };

            User User = new User("Nicholai");
            userRepository.User = User;
            userRepository.UserCredential = new UserCredential(User.Id, "Hashed-Password");

            UserService service = new UserService(userRepository, passwordHasher);

            bool result = service.Login("Nicholai", "MyPassword");

            Assert.True(result);
        }

        [Fact]
        public void Login_ReturnsFalseForIncorrectPassword()
        {
            TestUserRepository userRepository = new TestUserRepository();
            TestPasswordHasher passwordHasher = new TestPasswordHasher()
            {
                VerifyResult = false
            };

            User User = new User("Nicholai");
            userRepository.User = User;
            userRepository.UserCredential = new UserCredential(User.Id, "Hashed-Password");

            UserService service = new UserService(userRepository, passwordHasher);

            bool result = service.Login("Nicholai", "WrongPassword");

            Assert.False(result);
        }

        [Fact]
        public void Register_EmptyUsername_ThrowsException()
        {
            TestUserRepository userRepository = new TestUserRepository();
            TestPasswordHasher passwordHasher = new TestPasswordHasher();

            UserService service = new UserService(userRepository, passwordHasher);

            Assert.Throws<ArgumentException>(() => service.Register("", "A-Password"));
        }

        [Fact]
        public void Register_WhitespaceUsername_ThrowsException()
        {
            TestUserRepository userRepository = new TestUserRepository();
            TestPasswordHasher passwordHasher = new TestPasswordHasher();

            UserService service = new UserService(userRepository, passwordHasher);

            Assert.Throws<ArgumentException>(() => service.Register("     ", "A-Password"));
        }

        [Fact]
        public void Register_EmptyPassword_ThrowsException()
        {
            TestUserRepository userRepository = new TestUserRepository();
            TestPasswordHasher passwordHasher = new TestPasswordHasher();

            UserService service = new UserService(userRepository, passwordHasher);

            Assert.Throws<ArgumentException>(() => service.Register("Nicholai", ""));
        }

        [Fact]
        public void Register_WhitespacePassword_ThrowsException()
        {
            TestUserRepository userRepository = new TestUserRepository();
            TestPasswordHasher passwordHasher = new TestPasswordHasher();

            UserService service = new UserService(userRepository, passwordHasher);

            Assert.Throws<ArgumentException>(() => service.Register("Nicholai", "    "));
        }

        [Fact]
        public void GetUserId_ReturnsUserId_WhenUserExists()
        {
            TestUserRepository repository =
                new TestUserRepository();

            User user =
                new User("TestUser");

            repository.Add(user, new UserCredential(0, "password"));

            UserService service =
                new UserService(
                    repository,
                    new TestPasswordHasher());

            int? userId =
                service.GetUserId("TestUser");

            Assert.Equal(user.Id, userId);
        }

        [Fact]
        public void GetUserId_ReturnsNull_WhenUserDoesNotExist()
        {
            TestUserRepository repository =
                new TestUserRepository();

            UserService service =
                new UserService(
                    repository,
                    new TestPasswordHasher());

            int? userId =
                service.GetUserId("UnknownUser");

            Assert.Null(userId);
        }
    }
}
