using ByteLink.Application.Mediator.Commands;
using ByteLink.Infrastructure.Persistence.Repositories;
using MediatR;

namespace ByteLink.Application.Mediator.Handlers.UrlController;

public class DeleteUrlByShortCodeHandler(
    IUrlRepository urlRepository
) : IRequestHandler<DeleteUrlByShortCodeCommand>
{
    public async Task Handle(DeleteUrlByShortCodeCommand request, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(request.ShortCode, nameof(request.ShortCode));
        await urlRepository.DeleteUrlByShortCodeAsync(request.ShortCode);
    }
}