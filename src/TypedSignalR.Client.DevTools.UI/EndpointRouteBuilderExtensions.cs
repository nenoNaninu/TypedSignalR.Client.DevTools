using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace TypedSignalR.Client.DevTools;

public static partial class EndpointRouteBuilderExtensions
{
    public static void UseSignalRHubDevelopmentUI(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        // Actually, it does not use middleware.

        var paths = SignalRDevelopmentUIContentProvider.GetPaths();

        foreach (string path in paths)
        {
            Debug.WriteLine(path);

            endpointRouteBuilder.MapGet(path, async (HttpContext context) =>
            {
                context.Response.StatusCode = StatusCodes.Status200OK;

                var contentType = SelectContentType(path);

                if (contentType is not null)
                {
                    context.Response.ContentType = contentType;
                }

                var source = SignalRDevelopmentUIContentProvider.GetStream(path);

                await source.CopyToAsync(context.Response.Body);
            });
        }

        endpointRouteBuilder.MapGet("/signalr-dev", async (HttpContext context) =>
        {
            context.Response.Redirect("/signalr-dev/index.html");
            await context.Response.CompleteAsync();
        });
    }

    private static string? SelectContentType(string path)
    {
        var ex = Path.GetExtension(path);

        return ex switch
        {
            ".js" => "text/javascript",
            ".css" => "text/css",
            ".html" => "text/html",
            ".json" => "application/json",
            _ => null
        };
    }
}
