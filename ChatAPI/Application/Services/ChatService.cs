using System.Collections.Concurrent;
using System.Threading.Tasks;
using ChatAPI.Application.Interfaces;
using ChatAPI.Core.Entities;

namespace ChatAPI.Application.Services;

public class ChatService : IChatService
{
    private static readonly ConcurrentDictionary<string, string> Users = new();
    private static readonly ConcurrentDictionary<Guid, Message> Messages = new();

    public Task AddUser(string username, string connectionId)
    {
        Users[connectionId] = username;
        return Task.CompletedTask;
    }

    public Task<List<Message>> GetAllMessages()
    {
        return Task.FromResult(Messages.Values.ToList());
    }

    public Task<List<string>> GetConnectionIdsByUsername(string username)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetUsernameByConnectionId(string connectionId)
    {
        Users.TryGetValue(connectionId, out var username);
        return Task.FromResult(username);
    }

    public Task RemoveUser(string connectionId)
    {
        Users.TryRemove(connectionId, out _);
        return Task.CompletedTask;
    }

    public Task SaveMessage(Message message)
    {
        Messages[message.Id] = message;
        return Task.CompletedTask;
    }
}

