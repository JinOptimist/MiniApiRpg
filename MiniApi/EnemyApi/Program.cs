var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Your enemy is a goblin");

app.MapGet("/name", () => "goblin");

app.Run();
