using System.Text.Json;
using StarWars.People.API.Models;

public class CharacterService
{
    private readonly HttpClient _httpClient;

    public CharacterService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Character>> GetCharacterAsync(string name)
    {
        var characters = new List<Character>();
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
                nextUrl = page.Next;
            }
            else
            {
                break;
            }

            // If a name is provided, filter the results
            characters = FilterResults(name, characters);
        }

        return characters;
    }

    public List<Character> FilterResults (string name, List<Character> characters) 
    {
         if (!string.IsNullOrWhiteSpace(name))
            {
                characters = characters
                    .Where(c => c.Name!.Contains(name, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        
        return characters ?? new List<Character>();
    }
}
