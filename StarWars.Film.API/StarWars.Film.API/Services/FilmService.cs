using System.Text.Json;
using StackExchange.Redis;
using StarWars.People.API.Models;

public class FilmService
{
    private readonly HttpClient _httpClient;
    private readonly IDatabase _redis;

    public FilmService(HttpClient httpClient, IConnectionMultiplexer muxer)
    {
        _httpClient = httpClient;
        _redis = muxer.GetDatabase();
    }

    public async Task<IResult> GetFilmsAsync()
    {
        var cacheKey = "allFilms";
        var films = new List<Film>();

        var cachedData = await _redis.StringGetAsync(cacheKey);
        if (!string.IsNullOrEmpty(cachedData))
        {
            // Deserialize cached data
            films = JsonSerializer.Deserialize<List<Film>>(cachedData!);
        }
        else
        {
            try
            {
                var response = await _httpClient.GetAsync("https://swapi.dev/api/films/");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var root = JsonDocument.Parse(json);
                var resultsJson = root.RootElement.GetProperty("results").ToString();
                films = JsonSerializer.Deserialize<List<Film>>(resultsJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                 // Serialize data and store it in Redis cache
                var serializedData = JsonSerializer.Serialize(films);
                await _redis.StringSetAsync(cacheKey, serializedData);
            }
            catch (Exception ex)
            {
                return Results.Problem($"An error occurred while fetching data: {ex.Message}");
            }
        }

        return Results.Ok(films);
    }
}
