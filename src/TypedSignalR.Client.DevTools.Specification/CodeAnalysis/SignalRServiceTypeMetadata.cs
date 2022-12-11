using Microsoft.CodeAnalysis;

namespace TypedSignalR.Client.CodeAnalysis;

public sealed class SignalRServiceTypeMetadata : ITypeSymbolHolder
{
    public ITypeSymbol TypeSymbol { get; }

    public string Name { get; }
    public string Path { get; }
    public bool IsAuthRequired { get; }

    public TypeMetadata HubType { get; }
    public TypeMetadata ReceiverType { get; }

    public SignalRServiceTypeMetadata(ITypeSymbol typeSymbol, TypeMetadata hubType, TypeMetadata receiverType, string path, bool isAuthRequired)
    {
        TypeSymbol = typeSymbol;
        Name = typeSymbol.Name;
        Path = path;
        HubType = hubType;
        ReceiverType = receiverType;
        IsAuthRequired = isAuthRequired;
    }
}
