using System.Threading.Tasks;
using TypedSignalR.Client;

namespace Shared;

[Receiver]
public interface IReceiver
{
    Task ReceiveMessage(string message, int value);
    Task Notify();
    Task ReceiveCustomMessage(UserDefinedType userDefined);
}

[Hub]
public interface IReceiverTestHub
{
    Task Start();
}
