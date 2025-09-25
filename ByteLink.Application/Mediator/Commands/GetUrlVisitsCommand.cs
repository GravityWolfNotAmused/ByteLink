using ByteLink.Application.Models.ViewModels;
using MediatR;

namespace ByteLink.Application.Mediator.Commands;

public class GetUrlVisitsCommand(string shortCode) : IRequest<IEnumerable<UrlVisitViewModel>>
{
    public string ShortCode { get; set; } = shortCode;
}
