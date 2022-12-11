using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace TypedSignalR.Client.DevTools;

public static partial class EndpointRouteBuilderExtensions
{
    public static void UseSignalRDevelopmentUI(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        // Actually, it does not use middleware.

        var paths = SignalRDevelopmentUIContentProvider.GetPaths();

        foreach (string path in paths)
        {
            Debug.WriteLine(path);

            endpointRouteBuilder.MapGet(path, async (HttpContext context) =>
            {
                context.Response.StatusCode = StatusCodes.Status200OK;

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
}
