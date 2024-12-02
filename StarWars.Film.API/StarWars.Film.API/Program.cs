using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost:5001"));
builder.Services.AddHttpClient<FilmService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/get", async (FilmService filmService) =>
{
        try
        {
            var films = await filmService.GetFilmsAsync();
            return films;
        } 
        catch (HttpRequestException ex) 
        {
            return Results.NotFound($"Character with ID not found. Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred: {ex.Message}");
        }
})
.WithName("Get")
.WithOpenApi();

app.Run();
