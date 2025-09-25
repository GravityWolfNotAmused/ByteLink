using ByteLink.Application.Mediator.Commands;
using ByteLink.Application.Models.ResultModels;
using ByteLink.Infrastructure.Persistence.Repositories;
using MediatR;

namespace ByteLink.Application.Mediator.Handlers.AuthController;

public class LoginHandler(
    IUserRepository userRepository
) : IRequestHandler<LoginCommand, LoginCommandResult>
{
    public async Task<LoginCommandResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var token = await userRepository.LoginAsync(request.Email, request.Password);

        return new LoginCommandResult()
        {
            Token = token
        };
    }
}