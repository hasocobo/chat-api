using ChatAPI.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;
using ChatAPI.Core.Entities;

namespace ChatAPI.Presentation.Hubs;

public class ChatHub : Hub
{
    private readonly IChatService _chatService;
    private static int _userCounter = 10000;

    public ChatHub(IChatService chatService)
    {
        _chatService = chatService;
    }

    public override async Task OnConnectedAsync()
    {
        var anonymousName = $"Anonymous#{Interlocked.Increment(ref _userCounter)}";
        await _chatService.AddUser(anonymousName, Context.ConnectionId);
        await Clients.Caller.SendAsync("ReceiveMessage", $"You are connected as {anonymousName}.");

        await base.OnConnectedAsync();
    }

    public async Task JoinGroup(string groupCode)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupCode);
        await Clients.Group(groupCode).SendAsync("ReceiveMessage", $"{Context.ConnectionId} has joined the group.");
    }

    public async Task LeaveGroup(string groupCode)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupCode);
        await Clients.Group(groupCode).SendAsync("ReceiveMessage", $"{Context.ConnectionId} has left the group.");
    }

    public async Task SendMessageToGroup(string groupCode, string messageContent)
    {
        var username = await _chatService.GetUsernameByConnectionId(Context.ConnectionId);

        if (username != null)
        {
            var message = new Message
            {
                SenderUsername = username,
                Content = messageContent,
                Id = Guid.NewGuid()
            };

            await _chatService.SaveMessage(message);

            await Clients.Group(groupCode).SendAsync("ReceiveMessage", $"{username}: {message.Content} (ID: {message.Id})");
        }
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var username = await _chatService.GetUsernameByConnectionId(Context.ConnectionId);
        await _chatService.RemoveUser(Context.ConnectionId);

        if (username != null)
        {
            await Clients.All.SendAsync("ReceiveMessage", $"{username} has disconnected.");
        }

        await base.OnDisconnectedAsync(exception);
    }
}

