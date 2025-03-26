using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

using basketballUI.models;

namespace xUnitTesting
{
    public interface IHttpClientService
    {
        Task<HttpResponseMessage> GetAsync(string url);
    }

    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _client;
        public HttpClientService(HttpClient client)
        {
            _client = client;
        }
        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await _client.GetAsync(url);
        }
    }

    public class PlayerService
    {
        private readonly IHttpClientService _httpClientService;
        private const string API_URL = "http://localhost:5121/api/Players";
        //copy the url given when running the api

        public PlayerService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }

        public async Task<Player> GetFirstPlayer()
        {
            try
            {
                HttpResponseMessage response = await _httpClientService.GetAsync(API_URL);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    List<Player> players = JsonConvert.DeserializeObject<List<Player>>(json);
                    return players[0];
                }
                else
                { 
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Player> GetLastPlayer()
        {
            try
            {
                HttpResponseMessage response = await _httpClientService.GetAsync(API_URL);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    List<Player> players = JsonConvert.DeserializeObject<List<Player>>(json);
                    return players[players.Count - 1];
                }
                else
                { 
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    // Mock HTTP client for testing
    public class MockHttpClientService : IHttpClientService
    {
        private readonly HttpResponseMessage _response;
        public MockHttpClientService(HttpResponseMessage response)
        {
            _response = response;
        }
        public Task<HttpResponseMessage> GetAsync(string url)
        {
            return Task.FromResult(_response);
        }
    }

    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(4, 4);
        }

        [Fact]
        public async Task GetFirstPlayer_SuccessfulResponse_ReturnsFirstPlayer()
        {
            // Arrange
            var mockPlayers = new List<Player>
            {
                new Player { PlayerNo = 1, FName = "John", LName = "Doe" },
                new Player { PlayerNo = 2, FName = "Jane", LName = "Smith" }
            };
            var response = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(mockPlayers))
            };
            var mockHttpClient = new MockHttpClientService(response);
            var playerService = new PlayerService(mockHttpClient);

            // Act
            var result = await playerService.GetFirstPlayer();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.PlayerNo);
            Assert.Equal("John", result.FName);
            Assert.Equal("Doe", result.LName);
        }

        [Fact]
        public async Task GetLastPlayer_SuccessfulResponse_ReturnsLastPlayer()
        {
            // Arrange
            var mockPlayers = new List<Player>
            {
                new Player { PlayerNo = 1, FName = "John", LName = "Doe" },
                new Player { PlayerNo = 2, FName = "Jane", LName = "Smith" }
            };
            var response = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(mockPlayers))
            };
            var mockHttpClient = new MockHttpClientService(response);
            var playerService = new PlayerService(mockHttpClient);

            // Act
            var result = await playerService.GetLastPlayer();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.PlayerNo);
            Assert.Equal("Jane", result.FName);
            Assert.Equal("Smith", result.LName);
        }

        [Fact]
        public async Task GetFirstPlayer_FailedResponse_ReturnsNull()
        {
            // Arrange
            var response = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest
            };
            var mockHttpClient = new MockHttpClientService(response);
            var playerService = new PlayerService(mockHttpClient);

            // Act
            var result = await playerService.GetFirstPlayer();

            // Assert
            Assert.Null(result);
        }
    }
}