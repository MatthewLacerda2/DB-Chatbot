using DB_Chatbot.Server.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Server.Data;
using ChatAIze.GenerativeCS.Extensions;
using ChatAIze.GenerativeCS.Options.Gemini;
using ChatAIze.GenerativeCS.Constants;
using ChatAIze.GenerativeCS.Models;
using ChatAIze.GenerativeCS.Clients;
using Server.Controllers;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;
using Microsoft.Data.Sqlite;
using DB_Chatbot.Server.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = new SqliteConnectionStringBuilder
{
    DataSource = ":memory:"
};
var connection = new SqliteConnection(connectionString.ToString());

builder.Services.AddDbContext<ServerContext>(options =>
    options.UseSqlite("Data Source=:memory:")
);

var chatOpts = new ChatCompletionOptions
{
    Model = ChatCompletionModels.Gemini.GeminiPro,
    MaxAttempts = 7,
    CharacterLimit = 20000,
    IsTimeAware = true,
    DefaultFunctionCallback = async (name, arguments, cancellationToken) =>
    {
        await Console.Out.WriteLineAsync($"Function {name} called with arguments {arguments}");
        return new { Success = true, Property1 = "ABC", Property2 = 123 };
    },
    TimeCallback = () => DateTime.Now
};

builder.Services.AddGeminiClient(configure =>
{
    configure.ApiKey = "<GEMINI API KEY>";
    configure.DefaultCompletionOptions = chatOpts;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "DB-Chatbot API", Version = "1.0" });
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
