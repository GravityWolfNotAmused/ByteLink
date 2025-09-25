using AutoMapper;
using ByteLink.Application.Mediator.Commands;
using ByteLink.Application.Models.ViewModels;
using ByteLink.Infrastructure.Persistence.Repositories;
using MediatR;

namespace ByteLink.Application.Mediator.Handlers.MetricsController;

public class GetTotalUrlVisitsHandler(
    IUrlRepository urlRepository,
    IMapper mapper
) : IRequestHandler<GetTotalUrlVisitsCommand, UrlTotalVisitViewModel>
{
    public async Task<UrlTotalVisitViewModel> Handle(GetTotalUrlVisitsCommand request, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(request.ShortCode, nameof(request.ShortCode));

        var url = await urlRepository.GetUrlByShortCodeAsync(request.ShortCode);
        var model = mapper.Map<UrlTotalVisitViewModel>(url);

        return model;
    }
}
