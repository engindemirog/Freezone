using Freezone.Core.Security.JWT;

namespace Application.Features.Auth.Commands.Login;

public class LoggedResponse
{
    public AccessToken AccessToken { get; set; }
}