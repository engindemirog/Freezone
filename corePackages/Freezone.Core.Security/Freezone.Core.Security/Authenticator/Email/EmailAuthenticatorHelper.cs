using System.Security.Cryptography;

namespace Freezone.Core.Security.Authenticator.Email;

public class EmailAuthenticatorHelper : IEmailAuthenticatorHelper
{
    public Task<string> CreateEmailActivationKeyAsync()
    {
        byte[] keyBytes = RandomNumberGenerator.GetBytes(64);
        return Task.FromResult(Convert.ToBase64String(keyBytes));
    } // Email'i doğrulamak için

    public Task<string> CreateEmailAuthenticatorCodeAsync()
    {
        string code = RandomNumberGenerator
                      .GetInt32(Convert.ToInt32(
                                    Math.Pow(x: 10, y: 6))) //1000000'e kadar bir sayı üretecek. Min 1, Max 999999
                      .ToString() // 152
                      .PadLeft(totalWidth: 6, paddingChar: '0'); // 000 152
        return Task.FromResult(code);
    } // Doğrulanmış email'den sonra, login işlemlerine ek olarak isteyeceğimiz kod için
}