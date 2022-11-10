using RpgCore;

//ConfigureServices
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var apiUrls = builder.Configuration.GetSection("apiUrls").Get<ApiUrls>();
var baseEnemyUrl = new Uri(apiUrls.Enemy);
var getEnemyUrl = new Uri(baseEnemyUrl, "/name");

var app = builder.Build();
//Configure

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Tavern v3.  Call /quest");

app.MapGet("/quest", async (HttpResponse response) =>
{
    var client = new HttpClient();
    var enemyNameReposne = await client.GetAsync(getEnemyUrl);
    var enemyName = await enemyNameReposne.Content.ReadAsStringAsync();
    await response.WriteAsync($"You are got a Quest. Kill 10 {enemyName}");
});

app.Run();
