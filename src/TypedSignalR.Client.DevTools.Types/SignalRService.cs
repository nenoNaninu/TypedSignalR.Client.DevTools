using System;
using System.Collections.Generic;
using Tapper;

namespace TypedSignalR.Client.DevTools;

[TranspilationSource]
public sealed class SignalRService
{
    public string Name { get; set; } = default!;
    public string Path { get; set; } = default!;
    public bool IsAuthRequired { get; set; }

    public IReadOnlyList<string> AuthenticationSchemes { get; } = default!;
    public TypeMetadata HubType { get; set; } = default!;
    public TypeMetadata ReceiverType { get; set; } = default!;
}

[TranspilationSource]
public sealed class TypeMetadata
{
    public string InterfaceName { get; set; } = default!;
    public string InterfaceFullName { get; set; } = default!;
    public string CollisionFreeName { get; set; } = default!;
    public IReadOnlyList<MethodMetadata> Methods { get; set; } = Array.Empty<MethodMetadata>();
}

[TranspilationSource]
public sealed class MethodMetadata
{
    public string MethodName { get; set; } = default!;
    public string ReturnType { get; set; } = default!;
    public bool IsGenericReturnType { get; set; } = default!;
    public string? GenericReturnTypeArgument { get; set; }
    public IReadOnlyList<ParameterMetadata> Parameters { get; set; } = Array.Empty<ParameterMetadata>();
}

[TranspilationSource]
public sealed class ParameterMetadata
{
    public string Name { get; set; } = default!;
    public string TypeName { get; set; } = default!;
}
