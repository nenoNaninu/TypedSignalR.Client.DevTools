using System;
using System.Threading;
using System.Threading.Tasks;
using ConsoleApp;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Shared;
using TypedSignalR.Client;


var cts = new CancellationTokenSource();

var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5186/hubs/UnaryHub")
    .ConfigureLogging(options =>
    {
        options.AddConsole();
        options.SetMinimumLevel(LogLevel.Information);
    })
    .WithAutomaticReconnect()
    .Build();


var hubProxy = connection.CreateHubProxy<IChatHub>(cts.Token);
var subscribe = connection.Register<IChatHubReceiver>(new ChatHubReceiver());

await connection.StartAsync(cts.Token);

await hubProxy.EnterRoom(Guid.Parse("df72d192-bce5-4701-9b28-8acd1671c9ac"), "ConsoleApp");

for (int i = 0; i < 10; i++)
{
    await hubProxy.PostMessage($"PostMessage : {i}");
}

await Exit.WaitAsync();

await connection.StopAsync();

subscribe.Dispose();
cts.Cancel();


class ChatHubReceiver : IChatHubReceiver
{
    public Task OnEnter(string userName)
    {
        Console.WriteLine("ChatHubReceiver::OnEnter");
        Console.WriteLine(userName);
        return Task.CompletedTask;
    }

    public Task OnMessage(Message message)
    {
        Console.WriteLine("ChatHubReceiver::OnMessage");
        Console.WriteLine(message);
        return Task.CompletedTask;
    }
}
