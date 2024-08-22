using ChatAPI.Core.Entities;

namespace ChatAPI.Application.Interfaces;
public interface IChatService
{
  Task AddUserAsync(string username, string connectionId);
  Task RemoveUserAsync(string connectionId);
  Task<string> GetUsernameByConnectionIdAsync(string connectionId);
  Task<List<string>> GetConnectionIdsByUsernameAsync(string username);
  Task SaveMessageAsync(Message message);
  Task<List<Message>> GetAllMessagesAsync();
}
