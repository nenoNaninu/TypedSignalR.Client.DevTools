using System.Collections.Generic;
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
    public IReadOnlyList<string> AuthenticationSchemes { get; }

    public SignalRServiceTypeMetadata(
        ITypeSymbol typeSymbol,
        TypeMetadata hubType,
        TypeMetadata receiverType,
        string path,
        bool isAuthRequired,
        IReadOnlyList<string> authenticationSchemes)
    {
        TypeSymbol = typeSymbol;
        HubType = hubType;
        ReceiverType = receiverType;
        Path = path;
        IsAuthRequired = isAuthRequired;
        AuthenticationSchemes = authenticationSchemes;
        Name = typeSymbol.Name;
    }
}
