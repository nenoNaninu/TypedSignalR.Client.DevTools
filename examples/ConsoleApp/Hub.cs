using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp;

public class UserDefinedType
{
    public DateTime DateTime { get; set; }
    public Guid Guid { get; set; }
}

public interface IHub
{
    Task<string> Get();
    Task<int> Add(int x, int y);
    Task<string> Cat(string x, string y);
    Task<UserDefinedType> Echo(UserDefinedType instance);
}

public interface IReceiver
{
    Task Receive();
    Task Receive1(int x, int y);
    Task Receive2(string x, string y);
    Task Receive3(UserDefinedType instance);
}

//[Conditional("DEBUG")]
//public static void MapSignalRDevelopmentUI(this IEndpointRouteBuilder endpointRouteBuilder)
//{
//    var paths = SignalRDevelopmentUIContentProvider.GetPaths();

//    foreach (string path in paths)
//    {
//        endpointRouteBuilder.MapGet(path, async (HttpContext context) =>
//        {
//            context.Response.StatusCode = StatusCodes.Status200OK;

//            var source = SignalRDevelopmentUIContentProvider.GetStream(path);
//            await source.CopyToAsync(context.Response.Body);

//        });
//    }
//}

//[Conditional("DEBUG")]
//public static void MapSignalRDevelopmentSpecification(this IEndpointRouteBuilder endpointRouteBuilder)
//{
//    endpointRouteBuilder.MapGet("/signalr-ui/spec.json", (HttpContext context) =>
//    {
//        context.Response.StatusCode = StatusCodes.Status200OK;
//        context.Response.ContentType = "application/json";

//        context.Response.BodyWriter.Write(GetSpecJsonBytes());
//    });
//}

//private static partial ReadOnlySpan<byte> GetSpecJsonBytes();
