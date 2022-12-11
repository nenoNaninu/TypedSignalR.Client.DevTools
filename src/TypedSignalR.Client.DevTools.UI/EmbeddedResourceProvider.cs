using System;
using System.IO;
using System.Reflection;

namespace TypedSignalR.Client.DevTools;

public static class EmbeddedResourceProvider
{
    private static readonly Assembly Assembly;
    private static readonly string[]? ResourceNames;

    // check EmbeddedResource in csproj
    static EmbeddedResourceProvider()
    {
        Assembly = typeof(EmbeddedResourceProvider).Assembly;

        ResourceNames = Assembly.GetManifestResourceNames();
    }

    public static ReadOnlyMemory<string> GetResourceNames() => ResourceNames;

    public static Stream GetStream(string resourceName)
    {
        var stream = Assembly.GetManifestResourceStream(resourceName);

        if (stream is null)
        {
            throw new InvalidOperationException("resourceName is invalid");
        }

        return stream;
    }
}
