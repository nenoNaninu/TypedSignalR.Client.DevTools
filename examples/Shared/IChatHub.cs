using System.Threading.Tasks;
using System;
using TypedSignalR.Client;

namespace Shared;

public record Message(Guid UserId, string UserName, string Text, DateTime DateTime);

[Receiver]
public interface IChatHubReceiver
{
    Task OnEnter(string userName);
    Task OnMessage(Message message);
}

[Hub]
public interface IChatHub
{
    Task EnterRoom(Guid roomId, string userName);
    Task PostMessage(string text);
    Task<Message[]> GetMessages();
}
