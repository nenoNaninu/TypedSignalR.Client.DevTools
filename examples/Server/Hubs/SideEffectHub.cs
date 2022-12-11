using Microsoft.AspNetCore.SignalR;
using Shared;

namespace Server.Hubs;

public class SideEffectHub : Hub<ISideEffectHubReceiver>, ISideEffectHub
{
    private readonly IDataStore _dataStore;
    private readonly ILogger<SideEffectHub> _logger;

    public SideEffectHub(IDataStore dataStore, ILogger<SideEffectHub> logger)
    {
        _dataStore = dataStore;
        _logger = logger;
    }

    public Task Init()
    {
        _logger.Log(LogLevel.Information, "SideEffectHub.Init");

        var data = _dataStore.Get(this.Context.ConnectionId);
        data.Value = 0;
        return Task.CompletedTask;
    }

    public Task Increment()
    {
        _logger.Log(LogLevel.Information, "SideEffectHub.Increment");

        var data = _dataStore.Get(this.Context.ConnectionId);
        data.Value++;
        return Task.CompletedTask;
    }

    public Task<int> Result()
    {
        _logger.Log(LogLevel.Information, "SideEffectHub.Result");

        var data = _dataStore.Get(this.Context.ConnectionId);
        return Task.FromResult(data.Value);
    }

    public Task Post(UserDefinedType instance)
    {
        _logger.Log(LogLevel.Information, "SideEffectHub.Post");

        var data = _dataStore.Get(this.Context.ConnectionId);
        data.Data.Add(instance);
        return Task.CompletedTask;
    }

    public Task<UserDefinedType[]> Fetch()
    {
        _logger.Log(LogLevel.Information, "SideEffectHub.Fetch");

        var data = _dataStore.Get(this.Context.ConnectionId);
        return Task.FromResult(data.Data.ToArray());
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _dataStore.Remove(this.Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }
}

public interface IDataStore
{
    IUserData Get(string connectionId);
    void Remove(string connectionId);
}

/// <summary>
/// Singleton
/// </summary>
public class DataStore : IDataStore
{
    private readonly Dictionary<string, IUserData> _dictionary = new();

    public IUserData Get(string connectionId)
    {
        if (_dictionary.TryGetValue(connectionId, out var data))
        {
            return data;
        }
        else
        {
            var newData = new UserData();

            _dictionary.Add(connectionId, newData);

            return newData;
        }
    }

    public void Remove(string connectionId)
    {
        _dictionary.Remove(connectionId);
    }
}

public interface IUserData
{
    int Value { get; set; }
    List<UserDefinedType> Data { get; }
}

class UserData : IUserData
{
    public int Value { get; set; }
    public List<UserDefinedType> Data { get; set; } = new();
}
