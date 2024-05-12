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

var builder = WebApplication.CreateBuilder(args);

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

//Populate the DB
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ServerContext>();

    Seeder auxSeeder = new Seeder();

    //dbContext.People.AddRange(auxSeeder.GetPeople(50));
    dbContext.Items.AddRange(auxSeeder.GetItems(300));
    /*
        Person[] p = dbContext.People.Take(10).ToArray();

        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine(p.ToString());
        }*/
}

using (var scope = app.Services.CreateScope())
{
    var pessoas = scope.ServiceProvider.GetRequiredService<PersonController>();

    foreach (MethodInfo method in pessoas.GetType().GetMethods())
    {

    }
}

app.Run();
