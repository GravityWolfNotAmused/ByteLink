using ByteLink.Application.Mediator.Commands;
using ByteLink.Infrastructure.Persistence.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLink.Application.Mediator.Handlers.UrlController;

public class VisitUrlQueuedHandler(
    IServiceScopeFactory serviceScopeFactory
) : IRequestHandler<VisitUrlQueuedCommand>
{
    public async Task Handle(VisitUrlQueuedCommand request, CancellationToken cancellationToken)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var urlRepository = scope.ServiceProvider.GetRequiredService<IUrlRepository>();
        var urlVisitRepository = scope.ServiceProvider.GetRequiredService<IUrlVisitRepository>();

        var url = await urlRepository.GetUrlByShortCodeAndUserIdAsync(request.UserSqid, request.ShortCode);
        var successfullyInserted = await urlVisitRepository.AddVisitAsync(url);

        if (!successfullyInserted)
            throw new InvalidOperationException($"Unable to add visit for URL: {url}");
    }
}
