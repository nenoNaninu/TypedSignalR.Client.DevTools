using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace TypedSignalR.Client.CodeAnalysis;

public sealed class SpecialSymbols
{
    public readonly INamedTypeSymbol TaskSymbol;
    public readonly INamedTypeSymbol GenericTaskSymbol;
    public readonly INamedTypeSymbol CancellationTokenSymbol;
    public readonly INamedTypeSymbol AsyncEnumerableSymbol;
    public readonly INamedTypeSymbol ChannelReaderSymbol;
    public readonly ImmutableArray<INamedTypeSymbol> HubAttributeSymbols;
    public readonly ImmutableArray<INamedTypeSymbol> ReceiverAttributeSymbols;
    public readonly ImmutableArray<INamedTypeSymbol> AuthorizeAttributeSymbols;
    public readonly ImmutableArray<IMethodSymbol> MapHubMethodSymbols;

    public SpecialSymbols(
        INamedTypeSymbol taskSymbol,
        INamedTypeSymbol genericTaskSymbol,
        INamedTypeSymbol cancellationTokenSymbol,
        INamedTypeSymbol asyncEnumerableSymbol,
        INamedTypeSymbol channelReaderSymbol,
        ImmutableArray<INamedTypeSymbol> hubAttributeSymbols,
        ImmutableArray<INamedTypeSymbol> receiverAttributeSymbols,
        ImmutableArray<INamedTypeSymbol> authorizeAttributeSymbols,
        ImmutableArray<IMethodSymbol> mapHubMethodSymbols)
    {
        TaskSymbol = taskSymbol;
        GenericTaskSymbol = genericTaskSymbol;
        CancellationTokenSymbol = cancellationTokenSymbol;
        AsyncEnumerableSymbol = asyncEnumerableSymbol;
        ChannelReaderSymbol = channelReaderSymbol;
        HubAttributeSymbols = hubAttributeSymbols;
        ReceiverAttributeSymbols = receiverAttributeSymbols;
        AuthorizeAttributeSymbols = authorizeAttributeSymbols;
        MapHubMethodSymbols = mapHubMethodSymbols;
    }
}
