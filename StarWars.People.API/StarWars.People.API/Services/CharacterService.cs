using System.Text.Json;
using StackExchange.Redis;
using StarWars.People.API.Models;

public class CharacterService
{
    private readonly HttpClient _httpClient;
    private readonly IDatabase _redis;

    public CharacterService(HttpClient httpClient, IConnectionMultiplexer muxer)
    {
        _httpClient = httpClient;
        _redis = muxer.GetDatabase();
    }

    public async Task<IResult> GetCharactersAsync()
    {
        var cacheKey = "allCharacters";
        var characters = new List<Character>();

        var cachedData = await _redis.StringGetAsync(cacheKey);
        if (!string.IsNullOrEmpty(cachedData))
        {
            // Deserialize cached data
            characters = JsonSerializer.Deserialize<List<Character>>(cachedData!);
        }
        else
        {
            try
            {
                string nextUrl = "https://swapi.dev/api/people/";

                while (!string.IsNullOrEmpty(nextUrl))
                {
                    var response = await _httpClient.GetAsync(nextUrl);
                    response.EnsureSuccessStatusCode();

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var page = JsonSerializer.Deserialize<StarWarsPage<Character>>(jsonResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (page != null && page.Results != null)
                    {
                        characters.AddRange(page.Results);
                        nextUrl = page.Next!;
                    }
                    else
                    {
                        break;
                    }
                }

                 // Serialize data and store it in Redis cache
                var serializedData = JsonSerializer.Serialize(characters);
                await _redis.StringSetAsync(cacheKey, serializedData);
            }
            catch (Exception ex)
            {
                return Results.Problem($"An error occurred while fetching data: {ex.Message}");
            }
        }

        return Results.Ok(characters);
    }
}
