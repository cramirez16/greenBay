using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Moq;
using Src.Services;
using Src.Models;
using Src.Models.Dtos;
using System.Security.Claims;


namespace UnitTests
{
    public class JwtServiceUnitTests
    {
        [TestFixture]
        public class JWTServiceTests
        {
            private JWTService _jwtService;
            private IConfiguration _configuration;
            private IHttpContextAccessor _httpContextAccessor;
            private IMapper _mapper;

            [SetUp]
            public void Setup()
            {
                // Mock IConfiguration
                var configurationMock = new Mock<IConfiguration>();
                configurationMock.Setup(x => x["JwtSettings:Key"]).Returns("DfRVw5b0ahmA2fPYRScOj2uvPDEiokpu3MxBLRnw");

                // Mock IHttpContextAccessor
                var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

                // Mock IMapper
                var mapperMock = new Mock<IMapper>();

                _configuration = configurationMock.Object;
                _httpContextAccessor = httpContextAccessorMock.Object;
                _mapper = mapperMock.Object;

                _jwtService = new JWTService(_configuration, _httpContextAccessor, _mapper);
            }

            [Test]
            public void TestCreateToken()
            {
                // Arrange
                var jwtPayLoad = new JwtPayLoad
                {
                    UserId = "4",
                    Name = "zamorano",
                    Email = "zamorano@gmail.com",
                    Role = "User",
                    Money = "12"
                };

                // Act
                var token = _jwtService.CreateToken(jwtPayLoad);
                Assert.IsNotNull(token);

            }

            [Test]
            public void TestGetClaimsFromToken()
            {
                // Arrange
                var jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiI5ZGRhZWNiOC0wZGJmLTQwNGQtYTBhOS01MjMzNWNiODczMjIiLCJ1c2VySWQiOiI0IiwibmFtZSI6InphbW9yYW5vIiwiZW1haWwiOiJ6YW1vcmFub0BnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJVc2VyIiwibW9uZXkiOiIxMiwwMCIsImV4cCI6MTY5NDYzMjYyMX0.RuHKxtXf1rhn-T5zrVhr8BRN1NlyjcOsJrwD4hweDgI";

                // Mock the IMapper for the JWTService
                var mapperMock = new Mock<IMapper>();

                // Define the expected UserResponseDto
                var expectedDto = new UserResponseDto
                {
                    Id = 4,
                    Name = "zamorano",
                    Email = "zamorano@gmail.com",
                    Role = "User",
                    Money = 12
                };

                // Set up the IMapper mock to return the expectedDto when mapping
                mapperMock.Setup(mapper => mapper.Map<UserResponseDto>(It.IsAny<List<Claim>>()))
                          .Returns(expectedDto);

                // Create the JWTService with the IMapper mock
                var jwtService = new JWTService(_configuration, _httpContextAccessor, mapperMock.Object);

                // Act
                var userResponseDto = jwtService.GetClaimsFromToken(jwtToken);

                // Assert
                Assert.IsNotNull(userResponseDto);
                Assert.That(userResponseDto.Id, Is.EqualTo(expectedDto.Id));
                Assert.That(userResponseDto.Name, Is.EqualTo(expectedDto.Name));
                Assert.That(userResponseDto.Email, Is.EqualTo(expectedDto.Email));
                Assert.That(userResponseDto.Role, Is.EqualTo(expectedDto.Role));
                Assert.That(userResponseDto.Money, Is.EqualTo(expectedDto.Money));
            }
        }
    }
}