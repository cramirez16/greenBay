using NUnit.Framework;
using Src.Controllers;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using Src.Data;
using Src.Models.Dtos;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Src.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using Src.Services.IServices;
using Microsoft.AspNetCore.Http;


namespace BackendNUnitTest
{
    [TestFixture]
    public class ItemControllerTests
    {
        private FactoryOverride _factory;
        protected LoginRequestDto _adminLogin;

        protected LoginRequestDto _userLogin;

        protected ItemRequestDto? _newItem;
        private List<Item>? _initialData;
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
            _initialData = new List<Item>()
            {
                new Item()
                {
                    Id = 1,
                    Name = "TV Sony",
                    Description = "An amazing TV",
                    PhotoUrl = "https://s13emagst.akamaized.net/products/45635/45634164/images/res_fd42def37fbf80666320c5137faccaf1.jpeg",
                    Price = 30,
                    Bid = 0,
                    IsSellable = true,
                    SellerId = 1,
                    CreationDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow.AddDays(1)

                },
                new Item()
                {
                    Id = 2,
                    Name = "Electrolux Vacum",
                    Description = "A wanderful vacum.",
                    PhotoUrl = "https://www.electrolux.com.my/globalassets/appliances/vacuum-clearner/z931-fr-1500x1500.png",
                    Price = 20,
                    Bid = 0,
                    IsSellable = true,
                    SellerId = 2,
                    CreationDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow.AddDays(1)
                }
            };

            // Create a scope using the factory's services
            using (var scope = _factory.Services.CreateScope())
            {
                // Resolve the JWTService from the scoped provider
                _jwtService = scope.ServiceProvider.GetRequiredService<IJWTService>();
            }
        }

        private async Task<string> LoginAndGetTokenAsync(LoginRequestDto user, HttpClient client)
        {
            var userLoggedIn = await client.PostAsJsonAsync("api/user/login", user);

            // Ensure the login was successful
            Assert.That(userLoggedIn.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Read and return the token from the response
            var loginResponse = await userLoggedIn.Content.ReadFromJsonAsync<LoginResponseDto>();
            Assert.That(loginResponse, Is.Not.Null);
            Assert.That(loginResponse.TokenKey, Is.Not.Null);

            return loginResponse.TokenKey;
        }

        [Test]
        [Order(1)]
        public async Task GetItemsWhitOutLoginIsUnahutorized()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("api/item");
            Assert.That(response, Is.Not.Null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        [Order(2)]
        public async Task GetItemsWithLoginIsOkAndReturnListOfTickets()
        {
            //----------------------------------------------
            //     Init login proces ---> get token
            //----------------------------------------------
            // Arrange
            var client = _factory.CreateClient();
            // Act --> login and get token.
            string tokenKey = await LoginAndGetTokenAsync(_userLogin, client);
            // Arrange ---> Add token to request header.
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenKey);
            // Act ---> make request to get enpoint.
            var responseGetItems = await client.GetAsync("api/item");
            // Assert ---> received ok
            Assert.That(responseGetItems, Is.Not.Null);
            Assert.That(responseGetItems.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            // Arrange ---> convert from json object to List of items.
            var responsedData = await responseGetItems.Content.ReadFromJsonAsync<List<ItemResponseDto>>();
            // Assert ---> List of items ok!
            Assert.That(responsedData, Is.Not.Null);
            Assert.That(responsedData[0].Name, Is.EqualTo("TV Sony"));
        }

        [Test]
        [Order(3)]
        public async Task GetItemtByIdShouldBeOk()
        {
            // Arrange
            var client = _factory.CreateClient();
            // Act --> login and get token.
            string tokenKey = await LoginAndGetTokenAsync(_userLogin, client);
            // Arrange ---> Add token to request header.
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenKey);
            // Act --> Request the item with id=1
            var response = await client.GetAsync("api/item/1");
            // Assert --> StatusCode Ok
            Assert.That(response, Is.Not.Null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            //Arrange ---> Deserialize the recieved data.
            var responsedData = await response.Content.ReadFromJsonAsync<ItemResponseDto>();
            //Assert ---> Received the requested item (id=1).
            Assert.That(responsedData, Is.Not.Null);
            Assert.That(responsedData.Name, Is.EqualTo("TV Sony"));
        }

        [Test]
        [Order(4)]
        public async Task GetItemtByIdWithNotExistingIdShouldBeNotFound()
        {
            // Arrange
            var client = _factory.CreateClient();
            // Act --> login and get token.
            string tokenKey = await LoginAndGetTokenAsync(_userLogin, client);
            // Arrange ---> Add token to request header.
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenKey);
            // Act --> Request ticket with id=0 (dosent exists)
            var response = await client.GetAsync("api/item/0");
            // Assert --> ticket not found!
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        [Order(5)]
        public async Task PostItemtWithoutLoginShouldBeUnauthorized()
        {
            var client = _factory.CreateClient();
            var response = await client.PostAsJsonAsync("api/item", _newItem);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        [Order(7)]
        public async Task PostItemtWithUserLogedShouldBeOK()
        {
            // Arrange
            var client = _factory.CreateClient();
            // Act --> login and get token.
            string tokenKey = await LoginAndGetTokenAsync(_userLogin, client);
            // Arrange ---> Add token to request header.
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenKey);
            // Act --> Post / save a new item in the database.
            var response = await client.PostAsJsonAsync("api/item", _newItem);
            // Assert --> Post was ok!
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }
    }
}