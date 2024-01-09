using System.Threading.Tasks;
using TypedSignalR.Client;

namespace Shared;

public interface IHubBaseBase
{
    Task<string> Get();
}

public interface IHubBase1 : IHubBaseBase
{
    Task<int> Add(int x, int y);
}

public interface IHubBase2 : IHubBaseBase
{
    Task<string> Cat(string x, string y);
}

[Hub]
public interface IInheritHub : IHubBase1, IHubBase2
{
    Task<UserDefinedType> Echo(UserDefinedType instance);
}

[Receiver]
public interface IInheritHubReceiver
{
}
