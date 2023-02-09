using Freezone.Core.Security.Entities;
using Freezone.Core.Security.JWT;

namespace Application.Features.Auth.Commands.Register;

public class RegisteredResponse
{
    public AccessToken AccessToken { get; set; }
    public RefreshToken RefreshToken { get; set; }
}