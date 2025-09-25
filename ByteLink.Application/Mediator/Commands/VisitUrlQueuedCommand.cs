using MediatR;

namespace ByteLink.Application.Mediator.Commands;

public class VisitUrlQueuedCommand(string userSqid, string shortCode) : IRequest
{
    public string UserSqid { get; set; } = userSqid;
    public string ShortCode { get; set; } = shortCode;
}