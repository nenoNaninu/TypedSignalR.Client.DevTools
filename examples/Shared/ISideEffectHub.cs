using System.Threading.Tasks;
using TypedSignalR.Client;

namespace Shared;


[Hub]
public interface ISideEffectHub
{
    Task Init();
    Task Increment();
    Task<int> Result();

    Task Post(UserDefinedType instance);
    Task<UserDefinedType[]> Fetch();
}

[Receiver]
public interface ISideEffectHubReceiver
{

}
