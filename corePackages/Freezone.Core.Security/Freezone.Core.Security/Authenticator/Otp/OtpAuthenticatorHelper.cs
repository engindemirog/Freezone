using OtpNet;

namespace Freezone.Core.Security.Authenticator.Otp;

public class OtpAuthenticatorHelper : IOtpAuthenticatorHelper
{
    public Task<byte[]> GenerateSecretKeyAsync()
    {
        byte[] secretKeyBytes = KeyGeneration.GenerateRandomKey(20);
        // Byte uzunluğuna göre üretilen totp kodun uzunluğu da değişir. Şuanki değer 6 haneli kodlar üretimi için uygundur.

        return Task.FromResult(secretKeyBytes);
    }

    public Task<string> ConvertSecretKeyToStringAsync(byte[] secretKeyBytes)
    {
        string base32SecretKey = Base32Encoding.ToString(secretKeyBytes);

        return Task.FromResult(base32SecretKey);
    }

    public Task<bool> VerifyCodeAsync(byte[] secretKeyBytes, string code)
    {
        Totp totp = new(secretKeyBytes);

        string generatedOtpCode = totp.ComputeTotp(); // O anki dakikada üretilen zaman bazlı kodu üretir.
        //string generatedOtpCodeBeforeOneMinute = totp.ComputeTotp(DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(1))); // Bir dakika önce üretilen zaman bazlı kodu üretir.

        bool result = generatedOtpCode == code; //|| generatedOtpCodeBeforeOneMinute == code
        return Task.FromResult(result);
    }
}