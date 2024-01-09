using Microsoft.CodeAnalysis;

namespace TypedSignalR.Client.CodeAnalysis;

public static class DiagnosticDescriptorItems
{
    // ==== TypedSignalR.Client ====

    public static readonly DiagnosticDescriptor UnexpectedException = new(
        id: "TSRD000",
        title: "Unexpected exception",
        messageFormat: "[Unexpected exception] {0}",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Unexpected exception.");

    public static readonly DiagnosticDescriptor TypeArgumentRule = new(
        id: "TSRD001",
        title: "The type argument must be an interface",
        messageFormat: "{0} is not an interface. The type argument must be an interface.",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "The type argument must be an interface.");

    public static readonly DiagnosticDescriptor InterfaceDefineRule = new(
        id: "TSRD002",
        title: "Only method definitions are allowed in the interface",
        messageFormat: "{0} is not a method. Define only methods in the interface.",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Only method definitions are allowed in the interface.");

    public static readonly DiagnosticDescriptor HubMethodReturnTypeRule = new(
        id: "TSRD003",
        title: "The return type of methods in the interface must be Task, Task<T>, IAsyncEnumerable<T>, Task<IAsyncEnumerable<T>> or Task<ChannelReader<T>>",
        messageFormat: "The return type of {0} is not suitable. Instead, use Task, Task<T>, IAsyncEnumerable<T>, Task<IAsyncEnumerable<T>> or Task<ChannelReader<T>>.",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "The return type of methods in the interface used for hub proxy must be Task, Task<T>, IAsyncEnumerable<T>, Task<IAsyncEnumerable<T>> or Task<ChannelReader<T>>.");

    public static readonly DiagnosticDescriptor ReceiverMethodReturnTypeRule = new(
        id: "TSRD004",
        title: "The return type of methods in the interface must be Task or Task<T>",
        messageFormat: "The return type of {0} is not suitable. Instead, use Task or Task<T>.",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "The return type of methods in the interface used for receiver must be Task or Task<T>.");

    public static readonly DiagnosticDescriptor HubMethodCancellationTokenParameterRule = new(
        id: "TSRD005",
        title: "CancellationToken can be used as a parameter only in the server-to-client streaming method",
        messageFormat: "CancellationToken cannot be used as a parameter in the {0}",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "CancellationToken can be used as a parameter only in the server-to-client streaming method.");

    public static readonly DiagnosticDescriptor HubMethodMultipleCancellationTokenParameterRule = new(
        id: "TSRD006",
        title: "Using multiple CancellationToken in method parameters is prohibited",
        messageFormat: "Multiple CancellationToken cannot be used as a parameter in the {0}",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Using multiple CancellationToken in method parameters is prohibited.");

    public static readonly DiagnosticDescriptor ServerStreamingMethodParameterRule = new(
        id: "TSRD007",
        title: "Using IAsyncEnumerable<T> or ChannelReader<T> as a parameter in a server-to-client streaming method is prohibited",
        messageFormat: "Do not use IAsyncEnumerable<T> or ChannelReader<T> as a parameter because the {0} was analyzed as a server-to-client streaming method",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Using IAsyncEnumerable<T> or ChannelReader<T> as a parameter in a server-to-client streaming method is prohibited.");

    public static readonly DiagnosticDescriptor ClientStreamingMethodReturnTypeRule = new(
        id: "TSRD008",
        title: "The return type of client-to-server streaming method must be Task",
        messageFormat: "The return type of a client-to-server streaming method must be Task. The return type of {0} is not Task.",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "The return type of client-to-server streaming method must be Task.");

    // ==== TypedSignalR.Client.DevTools ====

    public static readonly DiagnosticDescriptor BaseHubTypeRule = new(
        id: "TSRD009",
        title: "Hub<T> must be used for the base class, not Hub",
        messageFormat: "The base class for {0} is Hub. Must use Hub<T>.",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Hub<T> must be used for the base class, not Hub.");

    public static readonly DiagnosticDescriptor HubAttributeAnnotationRule = new(
        id: "TSRD010",
        title: "The interface representing the hub must be annotated with HubAttribute",
        messageFormat: "{0} does not implement an interface annotated with HubAttribute",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "The interface representing the hub must be annotated with HubAttribute.");

    public static readonly DiagnosticDescriptor ReceiverAttributeAnnotationRule = new(
        id: "TSRD011",
        title: "The interface representing the receiver must be annotated with ReceiverAttribute",
        messageFormat: "{0} is not annotated with ReceiverAttribute",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "The interface representing the receiver must be annotated with ReceiverAttribute.");
}
