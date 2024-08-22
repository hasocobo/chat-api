using Microsoft.EntityFrameworkCore;
using ChatAPI.Core.Entities;

namespace ChatAPI.Infrastructure.Data;

public class AppDbContext : DbContext
{
  public DbSet<User> Users { get; set; }
  public DbSet<Message> Messages { get; set; }

  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}