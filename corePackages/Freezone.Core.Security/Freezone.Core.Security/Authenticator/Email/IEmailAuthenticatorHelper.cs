namespace Freezone.Core.Security.Authenticator.Email;

public interface IEmailAuthenticatorHelper
{
    public Task<string> CreateEmailActivationKeyAsync();
    public Task<string> CreateEmailAuthenticatorCodeAsync();
}