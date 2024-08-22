using Microsoft.EntityFrameworkCore;
using ChatAPI.Application.Services;
using ChatAPI.Infrastructure.Data;
using ChatAPI.Presentation.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("ChatDb"));

builder.Services.AddScoped<ChatService>();

var app = builder.Build();

app.UseRouting();

app.MapControllers();
app.MapHub<ChatHub>("/chatHub");

app.Run();
