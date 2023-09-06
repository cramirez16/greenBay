using NUnit.Framework;
using src.Controllers;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using BackendNUnitTest;
using src.Data;
using src.Models.Dtos;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using src.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using src.Services.IServices;

namespace BackendNUnitTest
{
    [TestFixture]
    public class UserControllerTests
    {
        private FactoryOverride _factory;
        protected LoginRequestDto _adminLogin;
        protected ItemRequestDto? _newItem;
        private List<UserResponseDto>? _initialData;
        private IJWTService _jwtService;

        [SetUp]
        public void Setup()
        {
            _factory = new FactoryOverride();
            _adminLogin = new LoginRequestDto()
            {
                Email = "admin@fox.hu",
                Password = "Admin123",
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
                UpdateDate = DateTime.UtcNow
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
        public async Task RegistrationShouldBeCreatedWhenUserDataIsvalid()
        {
            // Arrange
            var client = _factory.CreateClient();
            var newUser = new
            {
                // valid name
                Name = "John",
                // valid password format, min 6 characters + Uppercase
                Password = "123456Aa",
                // valid email format
                Email = "John@mail.com"
            };
            // Act
            var response = await client.PostAsJsonAsync("/api/user/register", newUser);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }


        [Test]
        [Order(2)]
        public async Task RegistrationShouldThrowErrorWhenEmailIsInvalid()
        {
            // Arrange
            var client = _factory.CreateClient();
            // Act
            var response = await client.PostAsJsonAsync("/api/user/register", new object { });
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        [Order(3)]
        public async Task RegistrationShouldThrowErrorWhenOnlyEmailFormatIsInvalid()
        {
            // Arrange
            var client = _factory.CreateClient();
            var newUser = new
            {
                // valid name
                Name = "John",
                // valid password format, min 6 characters + Uppercase
                Password = "123456Aa",
                // invalid email format
                Email = "invalidEmailYolo42069"
            };
            // Act
            var response = await client.PostAsJsonAsync("/api/user/register", newUser);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        [Order(4)]
        public async Task RegistrationShouldThrowErrorWhenOnlyPasswordFormatIsInvalid()
        {
            var client = _factory.CreateClient();
            var newUser = new
            {
                // valid name
                Name = "John",
                // Invalid password format, min 6 characters + Uppercase
                Password = "1",
                Email = "john@email.com"
            };

            var response = await client.PostAsJsonAsync("/api/user/register", newUser);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        [Order(5)]
        public async Task LoginShouldThrowErrorWhenOnlyEmailIsInvalid()
        {
            // inserting a valid user in the database
            var client = _factory.CreateClient();
            var newUser = new
            {
                Name = "John",
                Password = "123456Aa",
                Email = "john@email.com"
            };

            await client.PostAsJsonAsync("/api/user/register", newUser);

            // trying to log with a wrong email.
            var userLogin = new { Email = "Peter", Password = "123456Aa" };
            var response = await client.PostAsJsonAsync("api/user/login", userLogin);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        [Order(6)]
        public async Task LoginShouldThrowErrorWhenOnlyPasswordIsInvalid()
        {
            // inserting a valid user in the database
            var client = _factory.CreateClient();
            var newUser = new { Name = "John", Password = "123456Aa", Email = "john@email.com" };
            await client.PostAsJsonAsync("/api/user/register", newUser);

            // trying to log with and invalid email.
            var userLogin = new { Email = "Peter", Password = "000000Aa" };
            var response = await client.PostAsJsonAsync("api/user/login", userLogin);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        [Order(7)]
        public async Task LoginShouldBeOKWhenEmailAndPasswordAreValid()
        {
            // Arrange
            var client = _factory.CreateClient();
            var newUser = new { Name = "John", Password = "123456Aa", Email = "john@email.com" };
            await client.PostAsJsonAsync("/api/user/register", newUser);
            // Act
            var response = await client.PostAsJsonAsync("api/user/login", newUser);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        [Order(8)]
        public async Task GetAllUserDataByAdminShouldBeOk()
        {
            // Arrange
            var client = _factory.CreateClient();
            List<UserResponseDto> expectedUsersResult = _initialData!;
            // Act - Admin loggin
            var userLoggedIn = await client.PostAsJsonAsync("api/user/login", _adminLogin);
            // Assert - Admin logged
            Assert.That(userLoggedIn.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Act
            var responsedData = await userLoggedIn.Content.ReadFromJsonAsync<LoginResponseDto>();
            // Assert
            Assert.That(responsedData, Is.Not.Null);
            Assert.That(responsedData.TokenKey, Is.Not.Null);

            // Arrange
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responsedData.TokenKey);
            // Act - Admin retrieves user data
            HttpResponseMessage? response = await client.GetAsync("api/user/");
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Arrange - extract user data from response
            string responseBody = await response.Content.ReadAsStringAsync();
            List<UserResponseDto> users = JsonConvert.DeserializeObject<List<UserResponseDto>>(responseBody)!;
            // Calculate hash of orginal list of user and user list recived from data base.
            var expectedUsersSet = new HashSet<UserResponseDto>(expectedUsersResult, new UserComparer());
            var recievedUsersSet = new HashSet<UserResponseDto>(users, new UserComparer());
            // Assert
            Assert.IsTrue(expectedUsersSet.SetEquals(recievedUsersSet));

        }

        [Test]
        [Order(9)]
        public async Task GetAllUserDataByNoAdminShouldBeForbidden()
        {
            // Arrange
            var client = _factory.CreateClient();
            var userLogin = new LoginRequestDto { Email = "testuser@abc.de", Password = "User123" };
            var userLoggedIn = await client.PostAsJsonAsync("api/user/login", userLogin);
            // Assert
            Assert.That(userLoggedIn.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Act
            var responsedData = await userLoggedIn.Content.ReadFromJsonAsync<LoginResponseDto>();
            // Assert
            Assert.That(responsedData, Is.Not.Null);
            Assert.That(responsedData.TokenKey, Is.Not.Null);

            // Arrange
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responsedData.TokenKey);
            // Act
            HttpResponseMessage? response = await client.GetAsync("api/user/");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }

        [Test]
        [Order(10)]
        public async Task GetAnyUserDataByAdminShouldBeOk()
        {
            // Arrange
            var client = _factory.CreateClient();
            UserResponseDto expectedUsersResult = _initialData![1];
            // Act
            var userLoggedIn = await client.PostAsJsonAsync("api/user/login", _adminLogin);
            // Assert
            Assert.That(userLoggedIn.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Act
            var responsedData = await userLoggedIn.Content.ReadFromJsonAsync<LoginResponseDto>();
            // Assert
            Assert.That(responsedData, Is.Not.Null);
            Assert.That(responsedData.TokenKey, Is.Not.Null);

            // Arrange
            var userComparer = new UserComparer();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responsedData.TokenKey);
            // Act
            HttpResponseMessage? response = await client.GetAsync("api/user/2");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            string responseBody = await response.Content.ReadAsStringAsync();
            UserResponseDto user = JsonConvert.DeserializeObject<UserResponseDto>(responseBody)!;

            // Assert
            Assert.IsTrue(userComparer.Equals(expectedUsersResult, user));
        }

        [Test]
        [Order(11)]
        public async Task GetUserDataOfOtherUserByNoAdminShouldBeForbidden()
        {
            // Arrange
            var client = _factory.CreateClient();
            var userLogin = new LoginRequestDto { Email = "testuser@abc.de", Password = "User123" };

            // Act
            var userLoggedIn = await client.PostAsJsonAsync("api/user/login", userLogin);
            // Assert
            Assert.That(userLoggedIn.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Act
            var responsedData = await userLoggedIn.Content.ReadFromJsonAsync<LoginResponseDto>();
            // Assert
            Assert.That(responsedData, Is.Not.Null);
            Assert.That(responsedData.TokenKey, Is.Not.Null);

            // Arrange
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responsedData.TokenKey);
            // Act
            HttpResponseMessage? response = await client.GetAsync("api/user/1");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }

        [Test]
        [Order(12)]
        public async Task UpdateUsersDataShouldBeOk()
        {
            // Arrange
            var client = _factory.CreateClient();
            var userLogin = new LoginRequestDto { Email = "testuser@abc.de", Password = "User123" };

            // Act
            var userLoggedIn = await client.PostAsJsonAsync("api/user/login", userLogin);
            // Assert
            Assert.That(userLoggedIn.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Act
            var responseData = await userLoggedIn.Content.ReadFromJsonAsync<LoginResponseDto>();
            // Assert
            Assert.That(responseData, Is.Not.Null);
            Assert.That(responseData.TokenKey, Is.Not.Null);

            // Arrange
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseData.TokenKey);
            var claims = _jwtService.GetClaimsFromToken(responseData.TokenKey);

            // Act            
            var response = await client.GetAsync("api/user/" + claims?.Id);
            var responseData2 = await response.Content.ReadFromJsonAsync<UserResponseDto>();
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Arrange
            var requestData = new User()
            {
                Id = responseData2!.Id,
                Name = "Peter",
                Password = userLogin.Password,
                Email = responseData2.Email,
                Role = responseData2.Role,
                Money = responseData2.Money,
                CreationDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            // Act
            var response2 = await client.PutAsync("api/user/" + claims?.Id, jsonContent);

            Assert.That(response2.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        [Order(13)]
        public async Task UpdateOtherUsersDataShouldBeForbidden()
        {
            // Arrange
            var client = _factory.CreateClient();
            var userLogin = new LoginRequestDto { Email = "testuser@abc.de", Password = "User123" };

            // Act
            var userLoggedIn = await client.PostAsJsonAsync("api/user/login", userLogin);
            // Assert
            Assert.That(userLoggedIn.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Act
            var responseData = await userLoggedIn.Content.ReadFromJsonAsync<LoginResponseDto>();
            // Assert
            Assert.That(responseData, Is.Not.Null);
            Assert.That(responseData.TokenKey, Is.Not.Null);

            // Arrange
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseData.TokenKey);

            //Arrange
            var requestData = new User()
            {
                Id = 1,
                Name = "Peter",
                Password = userLogin.Password,
                Email = _adminLogin.Email,
                Role = "Admin",
                Money = 100m,
                CreationDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            // Act
            var response2 = await client.PutAsync("api/user/1", jsonContent);

            Assert.That(response2.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }

        [Test]
        [Order(14)]
        public async Task UpdateUsersDataAsAdminShouldBeOk()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var userLoggedIn = await client.PostAsJsonAsync("api/user/login", _adminLogin);
            // Assert
            Assert.That(userLoggedIn.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Act
            var responseData = await userLoggedIn.Content.ReadFromJsonAsync<LoginResponseDto>();
            // Assert
            Assert.That(responseData, Is.Not.Null);
            Assert.That(responseData.TokenKey, Is.Not.Null);

            // Arrange
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseData.TokenKey);
            // Act
            var response = await client.GetAsync("api/user/2");
            var responseData2 = await response.Content.ReadFromJsonAsync<UserResponseDto>();
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Arrange
            var requestData = new User()
            {
                Id = responseData2!.Id,
                Name = "Peter",
                Password = "123456Aa",
                Email = responseData2.Email,
                Role = responseData2.Role,
                Money = responseData2.Money,
                CreationDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            // Act
            var response2 = await client.PutAsync("api/user/" + responseData2.Id, jsonContent);

            Assert.That(response2.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        [Order(15)]
        public async Task DeleteUsersDataShouldBeNoContent()
        {
            // Arrange
            var client = _factory.CreateClient();
            var userLogin = new LoginRequestDto { Email = "testuser@abc.de", Password = "User123" };

            // Act
            var userLoggedIn = await client.PostAsJsonAsync("api/user/login", userLogin);
            // Assert
            Assert.That(userLoggedIn.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            // Act
            var responseData = await userLoggedIn.Content.ReadFromJsonAsync<LoginResponseDto>();
            // Assert
            Assert.That(responseData, Is.Not.Null);
            Assert.That(responseData.TokenKey, Is.Not.Null);

            // Arrange
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseData.TokenKey);
            var claims = _jwtService.GetClaimsFromToken(responseData.TokenKey);
            // Act
            var response2 = await client.DeleteAsync("api/user/" + claims?.Id);
            // Assert
            Assert.That(response2.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        [Order(16)]
        public async Task DeleteUsersDataAsAdminShouldBeNoContent()
        {
            // Arrange
            var client = _factory.CreateClient();
            // Act
            var userLoggedIn = await client.PostAsJsonAsync("api/user/login", _adminLogin);
            // Assert
            Assert.That(userLoggedIn.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            // Act
            var responseData = await userLoggedIn.Content.ReadFromJsonAsync<LoginResponseDto>();
            // Assert
            Assert.That(responseData, Is.Not.Null);
            Assert.That(responseData.TokenKey, Is.Not.Null);

            // Arrange
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseData.TokenKey);
            // Act
            var response2 = await client.DeleteAsync("api/user/2");
            // Assert
            Assert.That(response2.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        [Order(17)]
        public async Task DeleteOtherUsersDataShouldBeNoForbidden()
        {
            // Arrange
            var client = _factory.CreateClient();
            var userLogin = new LoginRequestDto { Email = "testuser@abc.de", Password = "User123" };

            // Act
            var userLoggedIn = await client.PostAsJsonAsync("api/user/login", userLogin);
            // Assert
            Assert.That(userLoggedIn.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            // Act
            var responseData = await userLoggedIn.Content.ReadFromJsonAsync<LoginResponseDto>();
            // Assert
            Assert.That(responseData, Is.Not.Null);
            Assert.That(responseData.TokenKey, Is.Not.Null);

            // Arrange
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseData.TokenKey);
            // Act
            var response2 = await client.DeleteAsync("api/user/1");
            // Assert
            Assert.That(response2.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }
    }
}

public class UserComparer : IEqualityComparer<UserResponseDto>
{
    public bool Equals(UserResponseDto? x, UserResponseDto? y)
    {
        return x!.Id == y!.Id &&
                       x.Name == y.Name &&
                       x.Email == y.Email &&
                       x.Role == y.Role;
    }

    public int GetHashCode(UserResponseDto obj)
    {
        // Calculate a hash code based on the properties of the object
        return HashCode.Combine(obj.Id, obj.Name, obj.Email, obj.Role);
    }
}