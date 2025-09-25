using ByteLink.Application.Mediator.Commands;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace ByteLink.Application.HostedServices;

public interface IVisitUrlCommandQueue
{
    ValueTask QueueVisitUrlCommandAsync(VisitUrlCommand command);
    ValueTask<VisitUrlCommand> DequeueAsync(CancellationToken cancellationToken);
    Task<bool> HasQueueItem();
}

public class VisitInsertHostedService : IVisitUrlCommandQueue
{
    private readonly Channel<VisitUrlCommand> _queue;

    public VisitInsertHostedService()
    {
        var options = new BoundedChannelOptions(100)
        {
            FullMode = BoundedChannelFullMode.Wait
        };
        _queue = Channel.CreateBounded<VisitUrlCommand>(options);
    }

    public async ValueTask QueueVisitUrlCommandAsync(VisitUrlCommand command)
    {
        if (command == null)
        {
            throw new ArgumentNullException(nameof(command));
        }
        await _queue.Writer.WriteAsync(command);
    }

    public async Task<bool> HasQueueItem()
    {
        return await _queue.Reader.WaitToReadAsync();
    }

    public async ValueTask<VisitUrlCommand> DequeueAsync(CancellationToken cancellationToken)
    {
        var command = await _queue.Reader.ReadAsync(cancellationToken);
        return command;
    }
}

public class VisitUrlCommandHostedService(
    IVisitUrlCommandQueue commandQueue,
    IMediator mediator,
    ILogger<VisitUrlCommandHostedService> logger
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("VisitUrlCommand Hosted Service is running.");

        while (!stoppingToken.IsCancellationRequested)
        {
            while (await commandQueue.HasQueueItem())
            {
                var originalCommand = await commandQueue.DequeueAsync(stoppingToken);
                var newCommand = new VisitUrlQueuedCommand(originalCommand.UserSqid, originalCommand.ShortCode);
                try
                {
                    await mediator.Send(newCommand, stoppingToken);
                    logger.LogInformation("Processed VisitUrlCommand for user {UserSqid} with shortcode {ShortCode}.",
                        newCommand.UserSqid, newCommand.ShortCode);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error occurred processing VisitUrlCommand.");
                }
            }
        }

        logger.LogInformation("VisitUrlCommand Hosted Service is stopping.");
    }
}