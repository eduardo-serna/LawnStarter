public class StarWarsPage<T>
{
    public int Count { get; set; }
    public required string Next { get; set; }
    public required string Previous { get; set; }
    public required List<T> Results { get; set; }
}