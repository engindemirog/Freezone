using Freezone.Core.Security.Authenticator;
using Freezone.Core.Security.Entities;
using Freezone.Core.Security.JWT;

namespace Application.Features.Auth.Commands.Login;

public class LoggedResponse
{
    public AccessToken? AccessToken { get; set; }
    public RefreshToken? RefreshToken { get; set; }
    public AuthenticatorType? RequiredAuthenticatorType { get; set; }
}