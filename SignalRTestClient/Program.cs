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


        Console.Write("Enter group code to join: ");
        var groupCode = Console.ReadLine();

        await connection.InvokeAsync("JoinGroup", groupCode);

        connection.On<string>("ReceiveMessage", message =>
        {
            Console.WriteLine(message);
        });

        while (true)
        {
            var input = Console.ReadLine();
            await connection.InvokeAsync("SendMessageToGroup", groupCode, input);
        }
    }
}
