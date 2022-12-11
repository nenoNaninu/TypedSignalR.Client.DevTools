using System;
using System.Threading.Tasks;
using TypedSignalR.Client;

namespace Shared;

public class UserDefinedType
{
    public DateTime DateTime { get; set; }
    public Guid Guid { get; set; }
}

[Hub]
public interface IUnaryHub
{
    Task<string> Get();
    Task<int> Add(int x, int y);
    Task<string> Cat(string x, string y);
    Task<UserDefinedType> Echo(UserDefinedType instance);
    Task<byte[]> Echo2(byte[] binary);
    Task<Guid> Echo3(Guid id);
    Task<DateTime> Echo4(DateTime dateTime);
}

[Receiver]
public interface IUnaryHubReceiver
{
}
