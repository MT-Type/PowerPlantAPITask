using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace PowerPlantAPITask.Tests
{
    public class PowerPlantPostTests
    {
        private readonly HttpClient _client;

        public PowerPlantPostTests()
        {
            var factory = new CustWebAppFactory();
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Post_ReturnsBadRequest_WhenPowerIsInvalid()
        {
            var payload = new
            {
                owner = "test user",
                power = 500, // bad value
                validFrom = "2025-01-01",
                validTo = "2025-01-01"
            };

            var response = await _client.PostAsJsonAsync("api/powerplants", payload);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Post_ReturnsBadRequest_WhenOwnerFormatIsInvalid()
        {
            var payload = new
            {
                owner = "badname123", //bad value
                power = 120,
                validFrom = "2025-01-01",
                validTo = "2025-01-01"
            };

            var response = await _client.PostAsJsonAsync("api/powerplants", payload);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Post_ReturnsCreated_WhenDataIsValid()
        {
            var payload = new
            {
                owner = "good data",
                power = 12,
                validFrom = "2025-01-01",
                // validTo can be null
            };

            var response = await _client.PostAsJsonAsync("/api/powerplants", payload);

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);   // debug

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        }
    }
}
