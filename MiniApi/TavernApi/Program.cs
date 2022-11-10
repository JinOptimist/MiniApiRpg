using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using RpgCore;

//ConfigureServices
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Fun smile",
        Name = "Auth or something",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();
builder.Services.AddAuthorization();

var apiUrls = builder.Configuration.GetSection("apiUrls").Get<ApiUrls>();


var baseEnemyUrl = new Uri(apiUrls.Enemy);
var getEnemyUrl = new Uri(baseEnemyUrl, "/name");
Console.WriteLine(getEnemyUrl);

var app = builder.Build();
//Configure

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Tavern v3.  /swagger/index.html");

app.MapGet("/quest", async (HttpResponse response) =>
{
    var client = new HttpClient();
    var enemyNameReposne = await client.GetAsync(getEnemyUrl);
    var enemyName = await enemyNameReposne.Content.ReadAsStringAsync();
    await response.WriteAsync($"You are got a Quest. Kill 10 {enemyName}");
});


app.MapGet("/get/{:id}", (int? id) => apiUrls)
    .Produces<ApiUrls>();

app.MapGet("/nude", (string name) => new { name = "sister", url = "google" })
    .RequireAuthorization();

app.Run();
