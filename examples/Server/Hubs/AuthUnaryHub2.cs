using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Shared;

namespace Server.Hubs;

/// <summary>
/// AuthorizeAttribute is applied to some methods.
/// </summary>
public class AuthUnaryHub2 : Hub<IAuthUnaryHubReceiver>, IAuthUnaryHub
{
    private readonly ILogger<AuthUnaryHub2> _logger;

    public AuthUnaryHub2(ILogger<AuthUnaryHub2> logger)
    {
        _logger = logger;
    }

    [Authorize]
    public Task<int> Add(int x, int y)
    {
        _logger.Log(LogLevel.Information, "UnaryHub.Add");

        return Task.FromResult(x + y);
    }

    [Authorize]
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
