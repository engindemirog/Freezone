using Freezone.Core.Security.Entities;
using Freezone.Core.Security.JWT;

namespace Application.Features.Auth.Commands.Refresh;

public class RefreshedResponse
{
    public AccessToken AccessToken { get; set; }
    public RefreshToken RefreshToken { get; set; }
}