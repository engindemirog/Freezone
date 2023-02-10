namespace Freezone.Core.Security.JWT;

public class TokenOptions
{
    public string Audience { get; set; }
    public string Issuer { get; set; }
    public int AccessTokenExpiration { get; set; } // Bir JWT token'ın yaşam süresi // 10m
    public string SecurityKey { get; set; }
    public int RefreshTokenExpiration { get; set; } // Bir Refresh token'ın yaşam süresi // 1 g
    public int RefreshTokenTTL { get; set; } // 3 sa
    /*
     * Login olunduğunda, hali hazırda olan aktif reflesh token yeterince eksiyse (oluşturulma tarihine eklenmiş TTL süresine göre) silinerek oturum sonlandırılır.
     * Bu işlev çoklu oturumları etkin kılmayı sağlar.
     */
}