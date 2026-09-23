using Blackjack.Application.Interfaces;
using Blackjack.Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blackjack.Infrastructure.Tests
{
    public class BCryptPasswordHasherTests
    {
        [Fact]
        public void Hash_ReturnDifferentValueFromPassword()
        {
            IPasswordHasher hasher = new BCryptPasswordHasher();

            string password = "1234567890";

            string hash = hasher.Hash(password);

            Assert.NotEqual(password, hash);
        }

        [Fact]
        public void Verify_ReturnDifferentValueFromPassword()
        {
            IPasswordHasher hasher = new BCryptPasswordHasher();

            string password = "1234567890";
            string hash = hasher.Hash(password);

            bool result = hasher.Verify(password, hash);

            Assert.True(result);
        }

        [Fact]
        public void Verify_ReturnFalseForWrongPassword()
        {
            IPasswordHasher hasher = new BCryptPasswordHasher();

            string password = "1234567890";
            string hash = hasher.Hash(password);

            bool result = hasher.Verify("WrongPassword", hash);

            Assert.False(result);
        }
    }
}
