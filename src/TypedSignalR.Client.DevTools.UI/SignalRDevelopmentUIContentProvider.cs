using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace TypedSignalR.Client.DevTools;

internal static class SignalRDevelopmentUIContentProvider
{
    private const string Prefix = "TypedSignalR.Client.DevTools";

    private static readonly Dictionary<string, string> PathToResourceNameDictionary = new();

    static SignalRDevelopmentUIContentProvider()
    {
        var paths = EmbeddedResourcePathProvider.GetPaths();

        foreach (var path in paths)
        {
            PathToResourceNameDictionary[path] = PathToResourceName(path);
        }
    }

    public static IReadOnlyCollection<string> GetPaths() => PathToResourceNameDictionary.Keys;

    public static Stream GetStream(string path)
    {
        var resourceName = PathToResourceNameDictionary[path];

        if (resourceName is null)
        {
            throw new InvalidOperationException("path is invalid");
        }

        return EmbeddedResourceProvider.GetStream(resourceName);
    }

    public static string PathToResourceName(string path)
    {
        var filename = Path.GetFileName(path);
        var dir = Path.GetDirectoryName(path)!.Replace('\\', '/');

        var dir2 = Regex.Replace(dir, @"(?<=/)(\d)", "_$1");

        var dir3 = dir2.Replace('/', '.');
        var dir4 = dir3.Replace('-', '_');

        return $"{Prefix}.{dir4}.{filename}";
    }
}
