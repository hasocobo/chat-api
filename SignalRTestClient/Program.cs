using Microsoft.AspNetCore.SignalR.Client;

var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5223/chatHub", options =>
    {
        options.HttpMessageHandlerFactory = (message) =>
        {
            if (message is HttpClientHandler clientHandler)
                clientHandler.ServerCertificateCustomValidationCallback +=
                    (sender, certificate, chain, sslPolicyErrors) => { return true; };
            return message;
        };
    })
    .WithAutomaticReconnect()
    .Build();

connection.On<string, string>("ReceiveMessage", (user, message) => 
{
    Console.WriteLine($"{user}: {message}");
});

try
{   
    Console.WriteLine("Please enter your name: ");
    var userName = Console.ReadLine();
    
    await connection.StartAsync();
    Console.WriteLine("Connection started");

    while (true)
    {
        var userInput = Console.ReadLine();
        await connection.SendAsync("SendMessage", userName, userInput);
    }
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}
