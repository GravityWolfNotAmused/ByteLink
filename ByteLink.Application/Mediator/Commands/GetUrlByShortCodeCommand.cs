using ByteLink.Application.Models.ViewModels;
using MediatR;

namespace ByteLink.Application.Mediator.Commands;

public class GetUrlByShortCodeCommand(string shortCode) : IRequest<UrlViewModel>
{
    public string ShortCode { get; private set; } = shortCode;
}
