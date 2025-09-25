using ByteLink.Application.Models.ResultModels;
using MediatR;

namespace ByteLink.Application.Mediator.Commands;

public class ShortenUrlCommand(string originalUrl) : IRequest<CreateShortUrlCommandResult>
{
    public string OriginalUrl { get; private set; } = originalUrl;
}
