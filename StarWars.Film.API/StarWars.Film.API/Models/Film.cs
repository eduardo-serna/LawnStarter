using System.Text.Json.Serialization;

namespace StarWars.People.API.Models {
    public class Film
    {
        public string? Title { get; set; }

        [JsonPropertyName("opening_crawl")]
        public string? OpeningCrawl { get; set; }

        public List<string>? Characters { get; set; }
    }
}
