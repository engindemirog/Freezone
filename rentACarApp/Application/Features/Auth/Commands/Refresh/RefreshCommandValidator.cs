using FluentValidation;

namespace Application.Features.Auth.Commands.Refresh;

public class RefreshCommandValidator : AbstractValidator<RefreshCommand>
{
    public RefreshCommandValidator()
    {
        RuleFor(r => r.RefreshToken).NotEmpty();
        RuleFor(r => r.IpAddress).NotEmpty();
    }
}