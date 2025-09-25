using MediatR;

namespace ByteLink.Application.Mediator.Commands;

public class RegisterUserCommand(
    string email,
    string password
) : IRequest
{
    public required string Email { get; init; } = email;
    public required string Password { get; init; } = password;
}
