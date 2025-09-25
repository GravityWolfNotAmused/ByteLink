using AutoMapper;
using ByteLink.Application.Mediator.Commands;
using ByteLink.Application.Models.ViewModels;
using ByteLink.Infrastructure.Persistence.Repositories;
using MediatR;

namespace ByteLink.Application.Mediator.Handlers.UrlController;

public class GetUrlsHandler(
    IUrlRepository urlRepository,
    IMapper mapper
) : IRequestHandler<GetUrlsCommand, IEnumerable<UrlViewModel>>
{
    public async Task<IEnumerable<UrlViewModel>> Handle(GetUrlsCommand request, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(request.UserSquid, nameof(request.UserSquid));

        var urls = await urlRepository.GetUrlsAsync(request.UserSquid);

        return mapper.Map<IEnumerable<UrlViewModel>>(urls);
    }
}
