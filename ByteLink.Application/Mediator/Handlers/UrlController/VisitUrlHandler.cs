using ByteLink.Application.HostedServices;
using ByteLink.Application.Mediator.Commands;
using ByteLink.Infrastructure.Persistence.Repositories;
using MediatR;

namespace ByteLink.Application.Mediator.Handlers.UrlController;

public class VisitUrlHandler(
    IUrlRepository urlRepository,
    IVisitUrlCommandQueue visitUrlCommandQueue
) : IRequestHandler<VisitUrlCommand, string>
{
    public async Task<string> Handle(VisitUrlCommand request, CancellationToken cancellationToken)
    {
        var url = await urlRepository.GetUrlByShortCodeAndUserIdAsync(request.UserSqid, request.ShortCode);
        await visitUrlCommandQueue.QueueVisitUrlCommandAsync(request);
        return url.SourceUrl;
    }
}
