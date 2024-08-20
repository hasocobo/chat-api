using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;

namespace ChatAPI.Presentation.Hubs;

public class ChatHub : Hub
{
  private static readonly ConcurrentDictionary<string, string> Users = new();
  public async Task SendMessage(string message)
  {
    Console.WriteLine(Context.UserIdentifier);
    Console.WriteLine("message sent");

    await Clients.All.SendAsync("ReceiveMessage", Users[Context.ConnectionId], message);
  }

  public bool SetUserId(string userId)
  {
    try
    {
      Users[Context.ConnectionId] = userId;
      return true;
    }
    catch (System.Exception)
    {
      return false;
    }
  }

  public async Task SendPrivateMessage(string receiverUserId, string message)
  {
    var receiverConnectionId =
      Users.FirstOrDefault(u => u.Value == receiverUserId).Key;

    if (receiverConnectionId != null)
    {
      await Clients.Client(receiverConnectionId)
        .SendAsync("ReceiveMessage", Users[Context.ConnectionId], message);
    }
  }

  public override Task OnConnectedAsync()
  {
    return base.OnConnectedAsync();
  }
}