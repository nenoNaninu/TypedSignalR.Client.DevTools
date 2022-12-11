using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace TypedSignalR.Client.CodeAnalysis;

public sealed class SpecialSymbols
{
    public readonly INamedTypeSymbol TaskSymbol;
    public readonly INamedTypeSymbol GenericTaskSymbol;
    public readonly INamedTypeSymbol CancellationTokenSymbol;
    public readonly INamedTypeSymbol AsyncEnumerableSymbol;
    public readonly INamedTypeSymbol ChannelReaderSymbol;
    public readonly INamedTypeSymbol HubAttributeSymbol;
    public readonly INamedTypeSymbol ReceiverAttributeSymbol;
    public readonly INamedTypeSymbol AuthorizeAttributeSymbol;
    public readonly IReadOnlyList<IMethodSymbol> MapHubMethodSymbols;

    public SpecialSymbols(
        INamedTypeSymbol taskSymbol,
        INamedTypeSymbol genericTaskSymbol,
        INamedTypeSymbol cancellationTokenSymbol,
        INamedTypeSymbol asyncEnumerableSymbol,
        INamedTypeSymbol channelReaderSymbol,
        INamedTypeSymbol hubAttributeSymbol,
        INamedTypeSymbol receiverAttributeSymbol,
        INamedTypeSymbol authorizeAttributeSymbol,
        IReadOnlyList<IMethodSymbol> mapHubMethodSymbols)
    {
        TaskSymbol = taskSymbol;
        GenericTaskSymbol = genericTaskSymbol;
        CancellationTokenSymbol = cancellationTokenSymbol;
        AsyncEnumerableSymbol = asyncEnumerableSymbol;
        ChannelReaderSymbol = channelReaderSymbol;
        HubAttributeSymbol = hubAttributeSymbol;
        ReceiverAttributeSymbol = receiverAttributeSymbol;
        AuthorizeAttributeSymbol = authorizeAttributeSymbol;
        MapHubMethodSymbols = mapHubMethodSymbols;
    }
}
