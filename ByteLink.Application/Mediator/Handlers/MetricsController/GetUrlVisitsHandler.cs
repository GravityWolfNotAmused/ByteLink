using AutoMapper;
using ByteLink.Application.Mediator.Commands;
using ByteLink.Application.Models.ViewModels;
using ByteLink.Domain.Entities;
using ByteLink.Domain.Enums;
using ByteLink.Domain.Generators;
using ByteLink.Infrastructure.Persistence.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ByteLink.Application.Mediator.Handlers.MetricsController;

public class GetUrlVisitsHandler(
    IUrlRepository urlRepository,
    IUrlVisitRepository urlVisitRepository,
    [FromKeyedServices(GeneratorKeyedServices.ShortCodeUrlGenerator)] IGenerator<Url, string> shortCodeUrlGenerator,
    IMapper mapper
) : IRequestHandler<GetUrlVisitsCommand, IEnumerable<UrlVisitViewModel>>
{
    public async Task<IEnumerable<UrlVisitViewModel>> Handle(GetUrlVisitsCommand request, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(request.ShortCode, nameof(request.ShortCode));

        var url = await urlRepository.GetUrlByShortCodeAsync(request.ShortCode);

        var visits = await urlVisitRepository.GetVisitsAsync(url);

        var viewModels = mapper.Map<IEnumerable<UrlVisitViewModel>>(visits, opts =>
        {
            opts.Items["ShortCode"] = request.ShortCode;
            opts.Items["ShortUrl"] = shortCodeUrlGenerator.Generate(url);
        });

        return viewModels;
    }
}