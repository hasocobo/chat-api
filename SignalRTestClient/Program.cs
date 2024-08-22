using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5223/chatHub")
            .Build();

        await connection.StartAsync();

        Console.Write("Enter your username: ");
        var username = Console.ReadLine();

        await connection.InvokeAsync("SetUsername", username);

        connection.On<string>("ReceiveMessage", message =>
        {
            Console.WriteLine(message);
        });

        while (true)
        {
            var input = Console.ReadLine();
            if (input.StartsWith("/w "))
            {
                var parts = input.Split(' ', 3);
                if (parts.Length == 3)
                {
                    var targetUsername = parts[1];
                    var message = parts[2];
                    await connection.InvokeAsync("SendMessageToUser", targetUsername, message);
                }
            }
            else
            {
                await connection.InvokeAsync("BroadcastMessage", input);
            }
        }
    }
}
