namespace StarWars.People.API.Models {
    public class Character
    {
        public string? Name { get; set; }
        public string? Height { get; set; }
        public string? Mass { get; set; }
        public string? HairColor { get; set; }
        public string? SkinColor { get; set; }
        public string? EyeColor { get; set; }
        public string? BirthYear { get; set; }
        public string? Gender { get; set; }
        public string? Homeworld { get; set; }
        public List<string>? Films { get; set; }
        public DateTime Created { get; set; }
        public DateTime Edited { get; set; }
        public required string Url { get; set; }
    }
}