using MediatR;

namespace ByteLink.Application.Mediator.Commands;

public class VisitUrlCommand(string userSqid, string shortCode) : IRequest<string>
{
    public string UserSqid { get; private set; } = userSqid;
    public string ShortCode { get; private set; } = shortCode;
}
