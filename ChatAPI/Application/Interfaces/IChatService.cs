using ChatAPI.Core.Entities;

namespace ChatAPI.Application.Interfaces;
public interface IChatService
{
  Task AddUser(string username, string connectionId);
  Task RemoveUser(string connectionId);
  Task<string> GetUsernameByConnectionId(string connectionId);
  Task<List<string>> GetConnectionIdsByUsername(string username);
  Task SaveMessage(Message message);
  Task<List<Message>> GetAllMessages();
}
