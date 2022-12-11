using Microsoft.AspNetCore.SignalR;
using Shared;

namespace Server.Hubs;

public class UnaryHub : Hub<IUnaryHubReceiver>, IUnaryHub
{
    private readonly ILogger<UnaryHub> _logger;

    public UnaryHub(ILogger<UnaryHub> logger)
    {
        _logger = logger;
    }

    public Task<int> Add(int x, int y)
    {
        _logger.Log(LogLevel.Information, "UnaryHub.Add");

        return Task.FromResult(x + y);
    }

    public Task<string> Cat(string x, string y)
    {
        _logger.Log(LogLevel.Information, "UnaryHub.Cat");

        return Task.FromResult(x + y);
    }

    public Task<UserDefinedType> Echo(UserDefinedType instance)
    {
        _logger.Log(LogLevel.Information, "UnaryHub.Echo");

        return Task.FromResult(instance);
    }

    public Task<byte[]> Echo2(byte[] instance)
    {
        return Task.FromResult(instance);
    }

    public Task<Guid> Echo3(Guid id)
    {
        return Task.FromResult(id);
    }

    public Task<DateTime> Echo4(DateTime dateTime)
    {
        return Task.FromResult(dateTime.ToUniversalTime());
    }

    public Task<string> Get()
    {
        _logger.Log(LogLevel.Information, "UnaryHub.Get");

        return Task.FromResult("TypedSignalR.Client");
    }
}
