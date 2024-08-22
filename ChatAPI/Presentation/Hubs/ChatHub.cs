using System.Collections.Concurrent;
using ChatAPI.Application.Services;
using ChatAPI.Core.Entities;
using Microsoft.AspNetCore.SignalR;

namespace ChatAPI.Presentation.Hubs;

public class ChatHub : Hub
{
  private readonly ChatService _chatService;

  public ChatHub(ChatService chatService)
  {
    _chatService = chatService;
  }
  public async Task SetUsername(string username)
  {
    await _chatService.AddUserAsync(username, Context.ConnectionId);
    await Clients.All.SendAsync("ReceiveMessage", $"{username} has joined the chat.");
  }

  public override async Task OnDisconnectedAsync(Exception exception)
  {
    var username = await _chatService.GetUsernameByConnectionIdAsync(Context.ConnectionId);
    await _chatService.RemoveUserAsync(Context.ConnectionId);

    if (username != null)
    {
      await Clients.All.SendAsync("ReceiveMessage", $"{username} has left the chat.");
    }

    await base.OnDisconnectedAsync(exception);
  }

  public async Task SendMessageToUser(string targetUsername, string messageContent)
  {
    var senderUsername = await _chatService.GetUsernameByConnectionIdAsync(Context.ConnectionId);
    var targetConnectionIds = await _chatService.GetConnectionIdsByUsernameAsync(targetUsername);

    if (targetConnectionIds != null)
    {
      var message = new Message
      {
        SenderUsername = senderUsername,
        ReceiverUsername = targetUsername,
        Content = messageContent
      };

      await _chatService.SaveMessageAsync(message);

      foreach (var connectionId in targetConnectionIds)
      {
        await Clients.Client(connectionId).SendAsync("ReceiveMessage", $"{senderUsername}: {messageContent}");
      }
    }
  }

  public async Task BroadcastMessage(string messageContent)
  {
    var username = await _chatService.GetUsernameByConnectionIdAsync(Context.ConnectionId);

    var message = new Message
    {
      SenderUsername = username,
      Content = messageContent
    };

    await _chatService.SaveMessageAsync(message);
    await Clients.All.SendAsync("ReceiveMessage", $"{username}: {messageContent}");
  }
}