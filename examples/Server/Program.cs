using AspNetCore.SignalR.OpenTelemetry;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Server.Hubs;
using TypedSignalR.Client.DevTools;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSignalR()
    .AddHubInstrumentation();

builder.Services.AddSingleton<IDataStore, DataStore>();
builder.Services.AddSingleton<IMessageRepository, MessageRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://{configuration["Auth0:Domain"]}/";
        options.Audience = configuration["Auth0:Audience"];

        // For SignalR auth
        //     check JwtBearerHandler.HandleAuthenticateAsync
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = static context =>
            {
                var accessToken = context.Request.Query["access_token"];

                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs", System.StringComparison.OrdinalIgnoreCase))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
    .AddNegotiate();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseSignalRHubSpecification();
    app.UseSignalRHubDevelopmentUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/hubs/ChatHub");
app.MapHub<UnaryHub>("/hubs/UnaryHub");
app.MapHub<SideEffectHub>("/hubs/SideEffectHub");
app.MapHub<ReceiverTestHub>("/hubs/ReceiverTestHub");
app.MapHub<StreamingHub>("/hubs/StreamingHub");

app.MapHub<AuthUnaryHub>("/hubs/AuthUnaryHub");
app.MapHub<AuthUnaryHub2>("/hubs/AuthUnaryHub2");

app.MapHub<InheritHub>("/hubs/InheritHub");
app.MapHub<WindowsAuthHub>("/hubs/WindowsAuthHub");

app.Run();
