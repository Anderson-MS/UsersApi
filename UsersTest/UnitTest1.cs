using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Users.Api;
using Users.Api.Models;

namespace Users.Tests
{
    public class UserApiTests
    {
        private HttpClient _httpClient;

        [OneTimeSetUp]
        public void Setup()
        {
            var factory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureAppConfiguration((context, config) =>
                    {
                        config.Sources.Clear();
                        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                        config.AddJsonFile("appsettings.Test.json", optional: true, reloadOnChange: true);
                        config.AddEnvironmentVariables();
                    });
                   
                    builder.ConfigureServices(services => {});
                });

            _httpClient = factory.CreateClient();
        }

        [Test, Order(1)]
        public async Task GetHelloWorld()
        {
            // Arrange
            var requestPath = "/api/Users/hello";

            // Act
            var response = await _httpClient.GetAsync(requestPath);
            var stringResponse = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            stringResponse.Should().Be("Hello World!");
        }

        [Test, Order(2)]
        public async Task GetUsers()
        {
            // Arrange
            var requestPath = "/api/Users";

            // Act
            var response = await _httpClient.GetAsync(requestPath);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var users = await response.Content.ReadFromJsonAsync<User[]>();

            // Assert
            Assert.NotNull(users);

            if (users != null)
                Assert.GreaterOrEqual(users.Length, 1);
        }

        [Test, Order(3)]
        public async Task InsertUser()
        {
            // Arrange
            var requestPath = "/api/Users";
            User userToCreate = new User { Email = "test@test.com.br", Name = "User999 User999" };

            // Act
            var response = await _httpClient.PostAsJsonAsync(requestPath, userToCreate);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            // Check if the response content is a valid JSON
            var usersResult = await response.Content.ReadFromJsonAsync<User>();

            // Output the user result for debugging
            Console.WriteLine($"User Result: {usersResult}");

            userToCreate.Should().BeEquivalentTo(usersResult, options => options.Excluding(u => u.Id));
        }

        [Test, Order(4)]
        public async Task UpdateUser()
        {
            // Arrange
            var getRequestPath = "/api/Users";

            // Act
            var getResponse = await _httpClient.GetAsync(getRequestPath);
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getUsers = await getResponse.Content.ReadFromJsonAsync<User[]>();
            var getUser = getUsers.FirstOrDefault();

            if (getUser == null) Assert.Fail("No users found to update");

            var userToUpdate = new User { Id = getUser.Id, Email = getUser.Email, Name = getUser.Name + " UPDATE" };
            var putRequestPath = $"/api/Users/{getUser.Id}";
            var putResponse = await _httpClient.PutAsJsonAsync(putRequestPath, userToUpdate);

            // Assert
            putResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test, Order(5)]
        public async Task DeleteUser()
        {
            // Arrange
            var getRequestPath = "/api/Users";

            // Act
            var getResponse = await _httpClient.GetAsync(getRequestPath);
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var getUsers = await getResponse.Content.ReadFromJsonAsync<User[]>();
            var getUser = getUsers.FirstOrDefault();

            if (getUser == null) Assert.Fail("No users found to delete");

            var deleteRequestPath = $"/api/Users/{getUser.Id}";
            var deleteResponse = await _httpClient.DeleteAsync(deleteRequestPath);

            // Assert
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _httpClient.Dispose();
        }
    }
}
