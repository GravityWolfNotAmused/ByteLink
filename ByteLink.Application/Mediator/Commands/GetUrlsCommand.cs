using ByteLink.Application.Models.ViewModels;
using MediatR;

namespace ByteLink.Application.Mediator.Commands;

public class GetUrlsCommand(string userSquid) : IRequest<IEnumerable<UrlViewModel>>
{
    public string UserSquid { get; set; } = userSquid;
}