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
            var apiKey = _configuration["Spoonacular:ApiKey"];
            var url = $"https://api.spoonacular.com/food/trivia/random?apiKey={apiKey}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var trivia = JsonSerializer.Deserialize<TriviaDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return trivia!;
        }
    }

}