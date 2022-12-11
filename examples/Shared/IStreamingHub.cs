using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using TypedSignalR.Client;

namespace Shared;

[Hub]
public interface IStreamingHub
{
    // Server-to-Client streaming
    IAsyncEnumerable<Person> ZeroParameter();
    IAsyncEnumerable<Person> CancellationTokenOnly(CancellationToken cancellationToken);
    IAsyncEnumerable<StreamingHubMessage> Counter(Person publisher, int init, int step, int count);
    IAsyncEnumerable<StreamingHubMessage> CancelableCounter(Person publisher, int init, int step, int count, CancellationToken cancellationToken);
    Task<IAsyncEnumerable<StreamingHubMessage>> TaskCancelableCounter(Person publisher, int init, int step, int count, CancellationToken cancellationToken);

    // Server-to-Client streaming
    Task<ChannelReader<Person>> ZeroParameterChannel();
    Task<ChannelReader<Person>> CancellationTokenOnlyChannel(CancellationToken cancellationToken);
    Task<ChannelReader<StreamingHubMessage>> CounterChannel(Person publisher, int init, int step, int count);
    Task<ChannelReader<StreamingHubMessage>> CancelableCounterChannel(Person publisher, int init, int step, int count, CancellationToken cancellationToken);
    Task<ChannelReader<StreamingHubMessage>> TaskCancelableCounterChannel(Person publisher, int init, int step, int count, CancellationToken cancellationToken);

    // Client-to-Server streaming
    // TODO: HOW TO TEST?
    Task UploadStream(Person publisher, IAsyncEnumerable<Person> stream);
    Task UploadStreamAsChannel(Person publisher, ChannelReader<Person> stream);
}

[Receiver]
public interface IStreamingHubReceiver
{
}

public record Person(Guid Id, string Name, int Number);

public record StreamingHubMessage(Person Publisher, int Value);
