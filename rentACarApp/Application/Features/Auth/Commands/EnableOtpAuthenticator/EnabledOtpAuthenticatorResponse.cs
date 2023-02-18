namespace Application.Features.Auth.Commands.EnableOtpAuthenticator;

public class EnabledOtpAuthenticatorResponse
{
    public string SecretKey { get; set; }
    public string SecketKeyUrl { get; set; }
}