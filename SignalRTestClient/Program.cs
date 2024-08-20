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
    await connection.StartAsync();
    Console.WriteLine("Connection started");

    Console.WriteLine("Please enter your name: ");
    var userName = Console.ReadLine();
    await connection.InvokeAsync("SetUserId", userName);


    while (true)
    {
        Console.WriteLine("Who do you want to send a message? Press enter if you want to send to everyone, otherwise enter the user name you want to send a message to.");
        var receiverUserId = Console.ReadLine();

        if (!string.IsNullOrEmpty(receiverUserId))
        {
            Console.WriteLine($"Sending message to {receiverUserId}: ");
            var message = Console.ReadLine();
            await connection.InvokeAsync("SendPrivateMessage", receiverUserId, message);
        }
        else
        {
            Console.WriteLine("Sending message to everyone: ");
            var message = Console.ReadLine();
            await connection.InvokeAsync("SendMessage", message);
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}
