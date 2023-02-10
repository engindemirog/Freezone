namespace Application.Features.Auth.Constants;

public static class AuthBusinessMessages
{
    public const string UserEmailAlreadyExists = "Bu e-posta adresi ile daha önce kayıt olunmuş.";
    public const string UserNotFound = "Kullanıcı bulunamadı.";
    public const string UserPasswordNotMatch = "Kullanıcı şifresi eşleşmiyor.";
    public const string RefreshTokenNotFound = "Refresh token bulunamadı.";

    public static string UserEmailAlreadyExitsByEmail(string email)
    {
        return $"{email} e-posta adresiyle ile daha önce kayıt olunmuş.";
    }
}