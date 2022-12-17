# TypedSignalR.Client.DevTools

SignalR development tools inspired by SwaggerUI.

## What is TypedSignalR.Client.DevTools?
ASP.NET Core can easily use OpenAPI/SwaggerUI (Built in the default Web API template).
It allows for quick execution and debugging of REST APIs.

On the other hand, SignalR does not have an ecosystem like SwaggerUI.
So, to execute and debug, we had to connect to the SignalR Hub from the client application or web front end under development for your project or write a simple console app each time. These are tedious.

`TypedSignalR.Client.DevTools` is intended to play a similar role to the SwaggerUI in SignalR.
In other words, it allows easy and quick execution and debugging from the GUI.

## Table of Contents
- [What is TypedSignalR.Client.DevTools?](#what-is-typedsignalrclientdevtools)
- [Install](#install)
- [Usage](#usage)
- [Input Rules](#input-rules)
  - [Built-in Types](#built-in-types)
  - [User Defined Types](#user-defined-types)
- [Streaming Support](#streaming-support)
  - [Server-to-Client Streaming](#server-to-client-streaming)
  - [Client-to-Server Streaming](#client-to-server-streaming)
- [JWT Authentification Support](#jwt-authentification-support)
- [Recommendation](#recommendation)
- [Example](#example)
  - [CLI](#cli)
  - [Visual Studio](#visual-studio)
  - [JWT Authentification Example](#jwt-authentification-example)
- [Related Work](#related-work)

## Install
NuGet: [TypedSignalR.Client.DevTools](https://www.nuget.org/packages/TypedSignalR.Client.DevTools/)

```
dotnet add package TypedSignalR.Client.DevTools
```

## Usage

Two steps are required to use this library.

1. Add two middleware to the http pipeline.
1. Add the attribute to hub and receiver interfaces.

Fist, after installing the package, add the `app.UseSignalRHubSpecification()` and `app.UseSignalRDevelopmentUI()` like the following code to the HTTP pipeline in `Program.cs`. 
It is recommended to activate it only in the development environment.

```cs
using TypedSignalR.Client.DevTools; // <- Add!

var builder = WebApplication.CreateBuilder(args);

// Impl...

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseSignalRHubSpecification(); // <- Add!
    app.UseSignalRHubDevelopmentUI(); // <- Add!
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
```

Next, apply the attributes to your application's hub and receiver interfaces.

```cs
using TypedSignalR.Client; // <- Add!

[Hub] // <- Add!
public interface IChatHub
{
    Task EnterRoom(Guid roomId, string userName);
    Task PostMessage(string text);
    Task<Message[]> GetMessages();
}

[Receiver] // <- Add!
public interface IChatHubReceiver
{
    Task OnEnter(string userName);
    Task OnMessage(Message message);
}

public record Message(Guid UserId, string UserName, string Text, DateTime DateTime);
```

Then implement the Hub as usual. Nothing special is required.

```cs
public class ChatHub : Hub<IChatHubReceiver>, IChatHub
{
    // Implementation as usual.
}
```

Finally, add `MapHub` as usual in `Program.cs`.
Note that the first argument of MapHub must be a string literal because this library determines the hub URL at compile time.

```cs
// Impl...

app.MapControllers();

app.MapHub<ChatHub>("/hubs/ChatHub"); // <- Add MapHub as usual.

app.Run();
```

Launch the application server and access `/signalr-dev` from browser! Now you can easily execute and debug your SignalR Application!

![demo](https://user-images.githubusercontent.com/27144255/206090956-1d43856b-9088-4af0-a79b-e6ec465156fa.gif)

Messages received from the Hub are displayed in the right panel, as shown in the figure below.
Received messages are displayed in JSON format. If the indent option is turned on, indented JSON will be displayed.

![receiver](https://user-images.githubusercontent.com/27144255/206198494-dcc467dd-4f40-4c2b-9849-3f435fdd0a01.gif)

## Input Rules

Input rules depend on the argument type. 

### Built-in Types

If an argument is a built-in type, enter the string as normal, as shown in the following image.

![built-in-types](https://user-images.githubusercontent.com/27144255/206098651-9b0a84f0-a815-444e-8d69-30b0711f4092.png)

Build-in types: `bool` `char` `byte` `sbyte` `decimal` `double` `float` `int` `uint` `long` `ulong` `short` `ushort` `byte[]` `string` `Uri` `Guid` `DateTime`

Note: `byte[]` require base64 string as input.

### User Defined Types

If an argument is a user defined type, enter the json string, as shown in the following image.

![image](https://user-images.githubusercontent.com/27144255/206103388-07a23fb3-c397-4750-b323-d551f45fc7ca.png)

## Streaming Support

This library supports both server-to-client streaming and client-to-server streaming. 

### Server-to-Client Streaming

In server-to-client streaming, messages sent by the server are displayed in the right panel. Unlike ordinary server-to-client invocation, the keywords `OnNext`, `OnCompleted`, and `OnError` are added next to the method name.

When defining a server-to-client streaming method in a C# interface, it is possible to take a `CancellationToken` as an argument.
However, it is impossible to set the `CancellationToken` from the GUI provided by this library. It is normal behavior for the input area to be disabled.

![server_to_client](https://user-images.githubusercontent.com/27144255/206890469-38a41dc5-daf6-48f0-81a9-0e1007e5d7b7.gif)

### Client-to-Server Streaming

When using client-to-server streaming, method definitions on C# interfaces include an `IAsyncEnumerable<T>` or `ChannelReader<T>` argument. The library automatically set up a stream for that argument internally. Therefore, the input area is disabled on the GUI, which is the correct behavior.

Once you click the invoke button, the stream will start. Set a value in the input area below the invoke button, and click the `next` button to stream the set value. Also, click buttons like `complete` and `cancel` to control the stream.

![client_to_server](https://user-images.githubusercontent.com/27144255/206889812-9496580c-8887-4fad-bff8-8e61c28ba120.gif)

## JWT Authentification Support

This library supports authentication using JWT.
When `[Authorize]` is applied to a hub or a hub method, a field to enter JWT will appear in the GUI automatically.

![auth](https://user-images.githubusercontent.com/27144255/206092695-59dde18c-fa1d-46e9-96c7-495153736b74.gif)

## Recommendation

I recommend setting up `Properties/launchSettings.json` as follows.
In other words, have two patterns of launchUrl, `signalr-dev` and `swagger`.
This way, if you are using Visual Studio or Rider, you can easily switch between SignalR development and REST API development with one click.

```json
{
    "profiles": {
        "ProjectName-SignalR": {
            "commandName": "Project",
            "dotnetRunMessages": true,
            "launchBrowser": true,
            "launchUrl": "signalr-dev",
            "applicationUrl": "https://localhost:7186;http://localhost:5186",
            "environmentVariables": {
                "ASPNETCORE_ENVIRONMENT": "Development"
            }
        },
        "ProjectName-Swagger": {
            "commandName": "Project",
            "dotnetRunMessages": true,
            "launchBrowser": true,
            "launchUrl": "swagger",
            "applicationUrl": "https://localhost:7186;http://localhost:5186",
            "environmentVariables": {
                "ASPNETCORE_ENVIRONMENT": "Development"
            }
        }
    }
    ...other settings...
}
```

## Example

### CLI

Enter the following command.
After entering the command and starting the server, try to access `/signalr-dev` from your browser.

```
$ git clone git@github.com:nenoNaninu/TypedSignalR.Client.DevTools.git
$ cd TypedSignalR.Client.DevTools
$ dotnet run --project ./examples/Server/Server.csproj
```

### Visual Studio

If you use visual studio, set the startup project to server and select Server-SignalR to run the application. A browser will automatically launch and access `/signalr-dev`.

![example-visual-studio](https://user-images.githubusercontent.com/27144255/205682179-42458d14-1ba3-4a5b-90e9-77a1e5c2b256.png)

### JWT Authentification Example

This example uses auth0 for authentication.
For the auth0 key, set the following to the user secret, etc.
If keys are empty, this example works well, but JWT authentication does not work correctly.
Please refer to [this blog](https://auth0.com/blog/aspnet-web-api-authorization/) if you need to learn how to use auth0. 

```
{
    "Auth0:Domain": "xxxx.auth0.com",
    "Auth0:Audience": "yyyy"
}
```


## Related Work
- [nenoNaninu/TypedSignalR.Client](https://github.com/nenoNaninu/TypedSignalR.Client)
  - C# Source Generator to create strongly typed SignalR clients.
- [nenoNaninu/TypedSignalR.Client.TypeScript](https://github.com/nenoNaninu/TypedSignalR.Client.TypeScript)
  - TypeScript source generator to provide strongly typed SignalR clients by analyzing C# type definitions.
