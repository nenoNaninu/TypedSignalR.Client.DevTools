using Microsoft.AspNetCore.SignalR;
using Shared;

namespace Server.Hubs;

public class InheritHub : Hub<IUnaryHubReceiver>, IInheritHub
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

    public Task<string> Cat(string x, string y)
    {
        return Task.FromResult(x + y);
    }

    public Task<UserDefinedType> Echo(UserDefinedType instance)
    {
        return Task.FromResult(instance);
    }

    public Task<string> Get()
    {
        return Task.FromResult("TypedSignalR.Client.DevTools");
    }
}
