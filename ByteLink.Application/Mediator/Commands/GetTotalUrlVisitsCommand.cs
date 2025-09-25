using ByteLink.Application.Models.ViewModels;
using MediatR;

namespace ByteLink.Application.Mediator.Commands;

public class GetTotalUrlVisitsCommand(string shortCode) : IRequest<UrlTotalVisitViewModel>
{
    public string ShortCode { get; private set; } = shortCode;
}