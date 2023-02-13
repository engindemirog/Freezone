using FluentValidation;

namespace Application.Features.Auth.Commands.Revoke;

public class RevokeCommandValidator : AbstractValidator<RevokeCommand>
{
    public RevokeCommandValidator()
    {
        RuleFor(x => x.Token).NotEmpty();
        RuleFor(x => x.IPAddress).NotEmpty();
    }
}