namespace ChatAPI.Application.Services;
using ChatAPI.Application.Interfaces;
using ChatAPI.Core.Entities;
using ChatAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class ChatService : IChatService
{
  private readonly AppDbContext _context;

  public ChatService(AppDbContext context)
  {
    _context = context;
  }

  public async Task AddUserAsync(string username, string connectionId)
  {
    var user = await _context.Users.Include(u => u.ConnectionIds).FirstOrDefaultAsync(u => u.Username == username);

    if (user == null)
    {
      user = new User { Username = username };
      _context.Users.Add(user);
    }

    user.ConnectionIds.Add(new ConnectionId { Connection = connectionId });
    await _context.SaveChangesAsync();
  }

  public async Task RemoveUserAsync(string connectionId)
  {
    var user = await _context.Users.Include(u => u.ConnectionIds)
                                   .FirstOrDefaultAsync(u => u.ConnectionIds.Any(c => c.Connection == connectionId));

    if (user != null)
    {
      user.ConnectionIds.RemoveAll(c => c.Connection == connectionId);
      if (!user.ConnectionIds.Any())
      {
        _context.Users.Remove(user);
      }
      await _context.SaveChangesAsync();
    }
  }

  public async Task<string> GetUsernameByConnectionIdAsync(string connectionId)
  {
    var user = await _context.Users.Include(u => u.ConnectionIds)
                                   .FirstOrDefaultAsync(u => u.ConnectionIds.Any(c => c.Connection == connectionId));
    return user?.Username;
  }

  public async Task<List<string>> GetConnectionIdsByUsernameAsync(string username)
  {
    var user = await _context.Users.Include(u => u.ConnectionIds)
                                   .FirstOrDefaultAsync(u => u.Username == username);

    return user?.ConnectionIds.Select(c => c.Connection).ToList() ?? new List<string>();
  }

  public async Task SaveMessageAsync(Message message)
  {
    _context.Messages.Add(message);
    await _context.SaveChangesAsync();
  }

  public async Task<List<Message>> GetAllMessagesAsync()
  {
    return await _context.Messages.ToListAsync();
  }
}