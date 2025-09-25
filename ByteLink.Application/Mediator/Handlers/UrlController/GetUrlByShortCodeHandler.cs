using AutoMapper;
using ByteLink.Application.Mediator.Commands;
using ByteLink.Application.Models.ViewModels;
using ByteLink.Infrastructure.Persistence.Repositories;
using MediatR;

namespace ByteLink.Application.Mediator.Handlers.UrlController;

public class GetUrlByShortCodeHandler(
    IUrlRepository urlRepository,
    IMapper mapper
) : IRequestHandler<GetUrlByShortCodeCommand, UrlViewModel>
{
    public async Task<UrlViewModel> Handle(GetUrlByShortCodeCommand request, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(request.ShortCode, nameof(request.ShortCode));
        var url = await urlRepository.GetUrlByShortCodeAsync(request.ShortCode);
        var model = mapper.Map<UrlViewModel>(url);
        return model;
    }
}
