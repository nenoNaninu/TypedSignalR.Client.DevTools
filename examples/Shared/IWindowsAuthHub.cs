using System.Threading.Tasks;
using TypedSignalR.Client;

namespace Shared;

[Hub]
public interface IWindowsAuthHub
{
    Task Echo(string message);
}

[Receiver]
public interface IWindowsAuthHubReceiver
{
}
