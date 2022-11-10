using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RpgCore;
using System.Text;

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
    //x.AddSecurityRequirement(
    //    new OpenApiSecurityRequirement
    //    {
    //        {
    //            new OpenApiSecurityScheme
    //            {
    //                Reference = new OpenApiReference
    //                {
    //                    Id = "Bearer",
    //                    Type = ReferenceType.SecurityScheme
    //                }
    //            }, new List<string>()
    //        }
    //    }
    //    );
});

var configuration = builder.Configuration;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        //options.TokenValidationParameters = new TokenValidationParameters
        //{
        //    ValidateIssuer = true,
        //    ValidateAudience = true,
        //    ValidateLifetime = true,
        //    ValidateIssuerSigningKey = true,
        //    ValidIssuer = configuration["Jwf:Issuer"],
        //    ValidAudience = configuration["Jwf:Audience"],
        //    IssuerSigningKey =
        //        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwf:Key"])
        //};
    });
builder.Services.AddAuthorization();

var apiUrls = configuration.GetSection("apiUrls").Get<ApiUrls>();


var baseEnemyUrl = new Uri(apiUrls.Enemy);
var getEnemyUrl = new Uri(baseEnemyUrl, "/name");

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
