using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Shared;

namespace Server.Hubs;

[Authorize(AuthenticationSchemes = NegotiateDefaults.AuthenticationScheme)]
public class WindowsAuthHub : Hub<IWindowsAuthHubReceiver>, IWindowsAuthHub
{
    private readonly ILogger<WindowsAuthHub> _logger;

    public WindowsAuthHub(ILogger<WindowsAuthHub> logger)
    {
        _logger = logger;
    }

    public async Task Echo(string message)
    {
        _logger.Log(LogLevel.Information, "WindowsAuthHub.Echo");
    }
}
