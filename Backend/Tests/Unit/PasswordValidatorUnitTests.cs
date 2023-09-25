using NUnit.Framework;
using AutoMapper;
using Moq;
using Src.Services;
using Src.Models;
using Src.Models.Dtos;
using System.Security.Claims;


namespace UnitTests
{
    public class PasswordValidatorUnitTests
    {
        [TestFixture]
        public class PasswordValidationAttributeTests
        {
            private PasswordValidationAttribute _passwordValidation;

            [SetUp]
            public void Setup()
            {
                _passwordValidation = new PasswordValidationAttribute();
            }

            [Test]
            public void PasswordWithoutCappitalLeterIsInvalid()
            {
                string password = "sñldkfjñaslkdjfñslakdf";
                Assert.That(_passwordValidation.IsValid(password), Is.False);
            }

            [Test]
            public void PasswordWithoutNumbersIsInvalid()
            {
                string password = "AAAlkdjfñslakdf";
                Assert.That(_passwordValidation.IsValid(password), Is.False);
            }

            [Test]
            public void PasswordWithoutLettersIsInvalid()
            {
                string password = "11111111111";
                Assert.That(_passwordValidation.IsValid(password), Is.False);
            }

            [Test]
            public void PasswordWithValidFormatIsValid()
            {
                string password = "Admin123";
                Assert.That(_passwordValidation.IsValid(password), Is.True);
            }


        }
    }
}