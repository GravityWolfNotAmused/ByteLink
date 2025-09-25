using ByteLink.Application.Mediator.Commands;
using FluentValidation;
using FluentValidation.Validators;

namespace ByteLink.API.Validators;

public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.")
            .Matches(@"^(?!.*[<>;]).*$").WithMessage("Email cannot contain the characters '<', '>', or ';'.");
        ;

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be empty.")
            .NotNull().WithMessage("Email cannot be null")
            .EmailAddress(EmailValidationMode.AspNetCoreCompatible).WithMessage("Must be an email address.");
    }
}
