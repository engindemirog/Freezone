namespace Application.Features.Auth.Constants;

public static class AuthBusinessMessages
{
    public const string UserEmailAlreadyExists = "Bu e-posta adresi ile daha önce kayıt olunmuş.";

    public static string UserEmailAlreadyExitsByEmail(string email)
    {
        return $"{email} e-posta adresiyle ile daha önce kayıt olunmuş.";
    }
}