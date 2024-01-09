using Microsoft.AspNetCore.SignalR;
using Shared;

namespace Server.Hubs;

public class InheritHub : Hub<IInheritHubReceiver>, IInheritHub
{
    private readonly ILogger<InheritHub> _logger;

    public InheritHub(ILogger<InheritHub> logger)
    {
        _logger = logger;
    }

    public Task<int> Add(int x, int y)
    {
        return Task.FromResult(x + y);
    }

    public async Task<string> Cat(string x, string y)
    {
        var str = x + y;
        await this.Clients.All.ReceiveMessage(str, str.Length);
        return str;
    }

    public async Task<UserDefinedType> Echo(UserDefinedType instance)
    {
        await this.Clients.All.ReceiveCustomMessage(instance);
        return instance;
    }

    public Task<string> Get()
    {
        return Task.FromResult("TypedSignalR.Client.DevTools");
    }
}
