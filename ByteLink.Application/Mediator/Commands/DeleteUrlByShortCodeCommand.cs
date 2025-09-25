using MediatR;

namespace ByteLink.Application.Mediator.Commands;

public class DeleteUrlByShortCodeCommand(string shortCode) : IRequest
{
    public string ShortCode { get; private set; } = shortCode;
}
