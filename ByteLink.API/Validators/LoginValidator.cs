using ByteLink.Application.Mediator.Commands;
using FluentValidation;
using FluentValidation.Validators;

namespace ByteLink.API.Validators;

public class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.Password)
            .NotNull().WithMessage("Invalid Login")
            .NotEmpty().WithMessage("Invalid Login")
            .MinimumLength(8).WithMessage("Invalid Login.")
            .Matches("[A-Z]").WithMessage("Invalid Login.")
            .Matches("[a-z]").WithMessage("Invalid Login.")
            .Matches("[0-9]").WithMessage("Invalid Login.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Invalid Login");


        RuleFor(x => x.Email)
            .NotNull().WithMessage("Invalid Login.")
            .NotEmpty().WithMessage("Invalid Login.")
            .EmailAddress(EmailValidationMode.AspNetCoreCompatible).WithMessage("Invalid Login.");
    }
}
