using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;
using Src.Models.Dtos;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Src.Services.IServices;
using Microsoft.AspNetCore.Http;
using Integration;


namespace BackendNUnitTest
{
    [TestFixture]
    public class BidControllerTests
    {
        private FactoryOverride _factory;
        protected LoginRequestDto _adminLogin;

        protected LoginRequestDto _userLogin;

        protected ItemRequestDto? _newItem;
        private List<UserResponseDto>? _initialData;
        private IJWTService? _jwtService;

        [SetUp]
        public void Setup()
        {
            _factory = new FactoryOverride();
            _adminLogin = new LoginRequestDto()
            {
                Email = "admin@fox.hu",
                Password = "Admin123",
            };
            _userLogin = new LoginRequestDto()
            {
                Email = "testuser@abc.de",
                Password = "User123",
            };
            _newItem = new ItemRequestDto()
            {
                Name = "Pencil",
                Description = "School Material",
                PhotoUrl = "http://www.images.com/image.jpg",
                Price = 50,
                Bid = 0m,
                IsSellable = true,
                CreationDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                SellerId = 1
            };
            _initialData = new List<UserResponseDto>()
            {
                new UserResponseDto(){
                    Id = 1,
                    Email = "admin@fox.hu",
                    Name = "admin",
                    Role = "Admin",
                    Money = 100m,
                },
                new UserResponseDto()
                {
                    Id = 2,
                    Email = "testuser@abc.de",
                    Name = "testuser",
                    Role = "User",
                    Money = 100m,
                }
            };

            // Create a scope using the factory's services
            using (var scope = _factory.Services.CreateScope())
            {
                // Resolve the JWTService from the scoped provider
                _jwtService = scope.ServiceProvider.GetRequiredService<IJWTService>();
            }
        }

        [Test]
        [Order(1)]
        public async Task BidShouldBeUnauthorizedWhenUserIsNotLoged()
        {
            // Arrange
            var client = _factory.CreateClient();
            var newBid = new BidRequestDto
            {
                BiderId = 1,
                BidAmount = 10m,
                ItemId = 1
            };
            // Act
            var response = await client.PostAsJsonAsync("/api/bid", newBid);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        [Order(2)]
        public async Task BidShouldBeBadRequestWhenBidAmountIsNegativeOrZero()
        {
            // Arrange
            var client = _factory.CreateClient();
            // Act - Admin loggin
            var userLoggedIn = await client.PostAsJsonAsync("api/user/login", _userLogin);
            // Assert - User logged
            Assert.That(userLoggedIn.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Act - Read Token
            var responsedData = await userLoggedIn.Content.ReadFromJsonAsync<LoginResponseDto>();
            // Assert
            Assert.That(responsedData, Is.Not.Null);
            Assert.That(responsedData.TokenKey, Is.Not.Null);

            // Arrange - Add token to Header Request.
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responsedData.TokenKey);
            var newBid = new BidRequestDto
            {
                BiderId = 1,
                BidAmount = -10m,
                ItemId = 1
            };
            // Act - User try an invalid bid.
            var responseBid = await client.PostAsJsonAsync("/api/bid", newBid);
            // Assert -> Bid not acepted.
            Assert.That(responseBid.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        [Order(3)]
        public async Task BidShouldBeBadRequestWhenBiderIdIsInvalid()
        {
            // Arrange
            var client = _factory.CreateClient();
            // Act - Admin loggin
            var userLoggedIn = await client.PostAsJsonAsync("api/user/login", _userLogin);
            // Assert - User logged
            Assert.That(userLoggedIn.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Act - Read Token
            var responsedData = await userLoggedIn.Content.ReadFromJsonAsync<LoginResponseDto>();
            // Assert
            Assert.That(responsedData, Is.Not.Null);
            Assert.That(responsedData.TokenKey, Is.Not.Null);

            // Arrange - Add token to Header Request.
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responsedData.TokenKey);
            var newBid = new BidRequestDto
            {
                BiderId = 0,
                BidAmount = 10m,
                ItemId = 1
            };
            // Act - User try an invalid bid.
            var responseBid = await client.PostAsJsonAsync("/api/bid", newBid);
            // Assert -> Bid not acepted.
            Assert.That(responseBid.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        [Order(4)]
        public async Task BidShouldBeBadRequestWhenItemIdIsInvalid()
        {
            // Arrange
            var client = _factory.CreateClient();
            // Act - Admin loggin
            var userLoggedIn = await client.PostAsJsonAsync("api/user/login", _userLogin);
            // Assert - User logged
            Assert.That(userLoggedIn.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Act - Read Token
            var responsedData = await userLoggedIn.Content.ReadFromJsonAsync<LoginResponseDto>();
            // Assert
            Assert.That(responsedData, Is.Not.Null);
            Assert.That(responsedData.TokenKey, Is.Not.Null);

            // Arrange - Add token to Header Request.
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responsedData.TokenKey);
            var newBid = new BidRequestDto
            {
                BiderId = 1,
                BidAmount = 10m,
                ItemId = 0
            };
            // Act - User try an invalid bid.
            var responseBid = await client.PostAsJsonAsync("/api/bid", newBid);
            // Assert -> Bid not acepted.
            Assert.That(responseBid.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        [Order(5)]
        public async Task BidShouldBeBadRequestWhenItemIsNotSallabel()
        {
            // Arrange
            var client = _factory.CreateClient();
            // Act - Admin loggin
            var userLoggedIn = await client.PostAsJsonAsync("api/user/login", _userLogin);
            // Assert - User logged
            Assert.That(userLoggedIn.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Act - Read Token
            var responsedData = await userLoggedIn.Content.ReadFromJsonAsync<LoginResponseDto>();
            // Assert
            Assert.That(responsedData, Is.Not.Null);
            Assert.That(responsedData.TokenKey, Is.Not.Null);

            // Arrange - Add token to Header Request.
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responsedData.TokenKey);

            _newItem!.IsSellable = false;
            // Act - User add a not sallabel item.
            var responseNewItem = await client.PostAsJsonAsync("/api/item", _newItem);
            // Assert
            Assert.That(responseNewItem, Is.Not.Null);
            Assert.That(responseNewItem.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var newBid = new BidRequestDto
            {
                BiderId = 1,
                BidAmount = 10m,
                ItemId = 3
            };
            // Act - User try a bid of a not sallabel item.
            var responseBid = await client.PostAsJsonAsync("/api/bid", newBid);
            var responseBidData = await responseBid.Content.ReadFromJsonAsync<BidTestResponse>();
            // Assert -> Bid not acepted.
            Assert.That(responseBid.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(responseBidData!.notSallabel, Is.True);
        }

        [Test]
        [Order(6)]
        public async Task BidShouldBeStatusCode402WhenBiderHasLessMoneyThanTheBid()
        {
            // Arrange
            var client = _factory.CreateClient();
            // Act - Admin loggin
            var userLoggedIn = await client.PostAsJsonAsync("api/user/login", _userLogin);
            // Assert - User logged
            Assert.That(userLoggedIn.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Act - Read Token
            var responsedData = await userLoggedIn.Content.ReadFromJsonAsync<LoginResponseDto>();
            // Assert
            Assert.That(responsedData, Is.Not.Null);
            Assert.That(responsedData.TokenKey, Is.Not.Null);

            var tokenClaims = _jwtService?.GetClaimsFromToken(responsedData.TokenKey);

            // Arrange - Add token to Header Request.
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responsedData.TokenKey);

            // Act - User add a not sallabel item.
            var responseNewItem = await client.PostAsJsonAsync("/api/item", _newItem);
            // Assert
            Assert.That(responseNewItem, Is.Not.Null);
            Assert.That(responseNewItem.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var newBid = new BidRequestDto
            {
                BiderId = tokenClaims!.Id,
                BidAmount = tokenClaims!.Money + 1000m, // Bid is bigger than actual bider money
                ItemId = 3
            };
            // Act - User try a bid of a not sallabel item.
            var responseBid = await client.PostAsJsonAsync("/api/bid", newBid);
            var responseBidData = await responseBid.Content.ReadFromJsonAsync<BidTestResponse>();
            // Assert -> Bid not acepted.
            Assert.That((int)responseBid.StatusCode, Is.EqualTo(402));
            Assert.That(responseBidData!.notEnoughtMoneyToBid, Is.True);
        }
    }
}