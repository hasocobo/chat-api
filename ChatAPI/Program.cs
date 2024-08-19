using ChatAPI.Presentation.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();

var app = builder.Build();

app.UseRouting();

app.MapControllers();
app.MapHub<ChatHub>("/chatHub");

app.Run();
