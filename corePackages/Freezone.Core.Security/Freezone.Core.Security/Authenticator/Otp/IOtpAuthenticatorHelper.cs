namespace Freezone.Core.Security.Authenticator.Otp;

public interface IOtpAuthenticatorHelper
{
    public Task<byte[]> GenerateSecretKeyAsync();
    public Task<string> ConvertSecretKeyToStringAsync(byte[] secretKeyBytes);
    public Task<bool> VerifyCodeAsync(byte[] secretKeyBytes, string code);
}