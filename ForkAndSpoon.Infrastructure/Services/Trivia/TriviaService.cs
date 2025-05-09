using Microsoft.Extensions.Configuration;
using System.Text.Json;
using ForkAndSpoon.Application.ExternalApi;

namespace ForkAndSpoon.Infrastructure.Services.Trivia
{
    public class TriviaService : ITriviaService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public TriviaService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<TriviaDto> GetRandomTriviaAsync()
        {
            // Get the Spoonacular API key from configuration
            var apiKey = _configuration["Spoonacular:ApiKey"];

            // Build the request URL
            var url = $"https://api.spoonacular.com/food/trivia/random?apiKey={apiKey}";

            // Send GET request to Spoonacular API
            var response = await _httpClient.GetAsync(url);

            // Throw exception if response is not successful
            response.EnsureSuccessStatusCode();

            // Read response content as JSON string
            var json = await response.Content.ReadAsStringAsync();

            // Deserialize JSON string into TriviaDto
            var trivia = JsonSerializer.Deserialize<TriviaDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Return the trivia object (non-nullable assertion assumed safe)
            return trivia!;
        }
    }
}