using ByteLink.Application.Models.ResultModels;
using MediatR;

namespace ByteLink.Application.Mediator.Commands;

public class LoginCommand(
    string email,
    string password
) : IRequest<LoginCommandResult>
{
    public required string Email { get; init; } = email;
    public required string Password { get; init; } = password;
}