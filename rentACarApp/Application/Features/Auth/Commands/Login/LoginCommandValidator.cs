using FluentValidation;

namespace Application.Features.Auth.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.UserForLoginDto.Email).EmailAddress();
        RuleFor(x => x.UserForLoginDto.Password).NotEmpty().MinimumLength(6);
    }
}