namespace Application.Services.AuthService;

public static class AuthServiceBusinessMessages
{
    public const string AuthenticatorCodeSubject = "Girişini onaylamak için kodu giriniz - RentACar";
    public const string InvalidAuthenticatorCode = "İki adımlı doğrulama kodu yanlış.";

    public static string AuthenticatorCodeTextBody(string authenticatorCode)
        => $"İki adımlı doğrulama kodunuz: {authenticatorCode.Substring(startIndex: 0, length: 3)} {authenticatorCode.Substring(3)}";
}