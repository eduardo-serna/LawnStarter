using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost:5001"));
builder.Services.AddHttpClient<CharacterService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/get", async (CharacterService characterService) =>
{
        try
        {
            var character = await characterService.GetCharactersAsync();
            return character;
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