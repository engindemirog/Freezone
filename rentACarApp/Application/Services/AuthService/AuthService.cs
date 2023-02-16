using Application.Services.Repositories;
using Freezone.Core.CrossCuttingConcerns.Exceptions;
using Freezone.Core.Mailing;
using Freezone.Core.Security.Authenticator;
using Freezone.Core.Security.Authenticator.Email;
using Freezone.Core.Security.Authenticator.Otp;
using Freezone.Core.Security.Entities;
using Freezone.Core.Security.JWT;

namespace Application.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly IUserOperationClaimRepository _userOperationClaimRepository;
    private readonly ITokenHelper _tokenHelper;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IEmailAuthenticatorHelper _emailAuthenticatorHelper;
    private readonly IUserEmailAuthenticatorRepository _userEmailAuthenticatorRepository;
    private readonly IMailService _mailService;
    private readonly IOtpAuthenticatorHelper _otpAuthenticatorHelper;
    private readonly IUserOtpAuthenticatorRepository _userOtpAuthenticatorRepository;

    public AuthService(IUserOperationClaimRepository userOperationClaimRepository, ITokenHelper tokenHelper,
                       IRefreshTokenRepository refreshTokenRepository,
                       IEmailAuthenticatorHelper emailAuthenticatorHelper,
                       IUserEmailAuthenticatorRepository userEmailAuthenticatorRepository,
                       IMailService mailService, 
                       IOtpAuthenticatorHelper otpAuthenticatorHelper,
                       IUserOtpAuthenticatorRepository userOtpAuthenticatorRepository)
    {
        _userOperationClaimRepository = userOperationClaimRepository;
        _tokenHelper = tokenHelper;
        _refreshTokenRepository = refreshTokenRepository;
        _emailAuthenticatorHelper = emailAuthenticatorHelper;
        _userEmailAuthenticatorRepository = userEmailAuthenticatorRepository;
        _mailService = mailService;
        _otpAuthenticatorHelper = otpAuthenticatorHelper;
        _userOtpAuthenticatorRepository = userOtpAuthenticatorRepository;
    }

    public async Task<AccessToken> CreateAccessToken(User user)
    {
        ICollection<OperationClaim> operationClaims =
            await _userOperationClaimRepository.GetOperationClaimsByUserIdAsync(user.Id);
        AccessToken accessToken = _tokenHelper.CreateToken(user, operationClaims);
        return accessToken;
    }

    public Task<RefreshToken> CreateRefreshToken(User user, string ipAddress)
    {
        RefreshToken refreshToken = _tokenHelper.CreateRefreshToken(user, ipAddress);
        return Task.FromResult(refreshToken);
    }

    public async Task<RefreshToken> AddRefreshToken(RefreshToken refreshToken)
    {
        RefreshToken addedRefreshToken = await _refreshTokenRepository.AddAsync(refreshToken);
        return addedRefreshToken;
    }

    public async Task DeleteOldActiveRefreshTokens(User user)
    {
        // Yeni login işleminde, yeni bir refresh token zinciri oluşacaktır. Hali hazırda bulunan diğer zincirler (en son aktif olan token'a göre) yeterince eskiyse (RefreshTokenTTL opsiyonundaki süre kadar) silinir.
        ICollection<RefreshToken> oldActiveRefreshTokens = await _refreshTokenRepository
                                                               .GetAllOldActiveRefreshTokensAsync(
                                                                   user, _tokenHelper.RefreshTokenTTLOption);
        await _refreshTokenRepository.DeleteRangeAsync(oldActiveRefreshTokens.ToList());
    }

    public async Task RevokeRefreshToken(RefreshToken token, string ipAddress, string reason,
                                         string? replacedByToken = null)
    {
        // Bir refresh token'ı geçersiz kılmak için ilgili bilgileri doldurur ve veri tabanında günceller.
        token.RevokedDate = DateTime.UtcNow;
        token.RevokedByIp = ipAddress;
        token.RevokedReason = reason;
        token.ReplacedByToken = replacedByToken;
        await _refreshTokenRepository.UpdateAsync(token);
    }

    public async Task RevokeDescendantRefreshTokens(RefreshToken token, string ipAddress, string reason)
    {
        // Eğerki hali hazırda geçersiz refresh token kullanılmaya çalışılırsa, o refresh token'ın tüm child refresh token'ları gezilerek en son zincirdeki aktif refresh token geçersiz kılınır.
        RefreshToken childRefreshToken =
            (await _refreshTokenRepository.GetAsync(rt => token.ReplacedByToken == rt.Token))!;
        if (childRefreshToken == null)
            throw new BusinessException("Kullanılmaya çalışılan Refresh Token'ın child bulunamadı");

        if (childRefreshToken.RevokedDate == null) await RevokeRefreshToken(childRefreshToken, ipAddress, reason);
        else await RevokeDescendantRefreshTokens(childRefreshToken, ipAddress, reason);
    }

    public async Task<RefreshToken> RotateRefreshToken(User user, RefreshToken token, string ipAddress)
    {
        // Kullanılan refresh token'ı geçersiz kılıcak (revoke) aktifliği yeni refresh token'a vericek. Refresh token'larla oluşan oturum temsil eden zincire yeni aktif bir refresh token eklenmiş olacak.
        RefreshToken newRefreshToken = _tokenHelper.CreateRefreshToken(user, ipAddress);

        await RevokeRefreshToken(token, ipAddress, reason: "Yeni Refresh Token oluşturuldu", newRefreshToken.Token);
        await AddRefreshToken(newRefreshToken);

        return newRefreshToken;
    }

    public async Task<UserEmailAuthenticator> CreateEmailAuthenticator(User user) // Entity üretmek için
        => new()
        {
            UserId = user.Id,
            Key = await _emailAuthenticatorHelper.CreateEmailActivationKeyAsync(),
            IsVerified = false
        };

    public async Task<UserOtpAuthenticator> CreateOtpAuthenticator(User user) // Entity üretmek için
        => new()
        {
            UserId = user.Id,
            SecretKey = await _otpAuthenticatorHelper.GenerateSecretKeyAsync(),
            IsVerified = false,
        };

    public Task<string> ConvertOtpSecretKeyToString(byte[] secretKeyBytes)
        => _otpAuthenticatorHelper.ConvertSecretKeyToStringAsync(secretKeyBytes); // Base32 ile secret key'i string'e dönüştürecek. Kullanıcıya dönmek için kullanacağız.

    public async Task SendAuthenticatorCode(User user)
    {
        switch (user.AuthenticatorType)
        {
            case AuthenticatorType.Email:
                await sendAuthenticatorCodeWithEmail(user);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public async Task VerifyAuthenticatorCode(User user, string code)
    {
        switch (user.AuthenticatorType)
        {
            case AuthenticatorType.Email:
                await verifyEmailAuthenticatorCode(user, code);
                break;
            case AuthenticatorType.Otp:
                await verifyOtpAuthenticatorCode(user, code);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    private async Task sendAuthenticatorCodeWithEmail(User user)
    {
        UserEmailAuthenticator userEmailAuthenticator =
            await _userEmailAuthenticatorRepository.GetAsync(uea => uea.UserId == user.Id);

        string authenticatorCode = await _emailAuthenticatorHelper.CreateEmailAuthenticatorCodeAsync();
        userEmailAuthenticator.Key = authenticatorCode;
        await _userEmailAuthenticatorRepository.UpdateAsync(userEmailAuthenticator);

        Mail mailData = new()
        {
            ToFullName = $"{user.FirstName} {user.LastName}",
            ToEmail = user.Email,
            Subject = AuthServiceBusinessMessages.AuthenticatorCodeSubject,
            TextBody = AuthServiceBusinessMessages.AuthenticatorCodeTextBody(authenticatorCode)
        };
        await _mailService.SendAsync(mailData);
    }

    private async Task verifyEmailAuthenticatorCode(User user, string code)
    {
        UserEmailAuthenticator userEmailAuthenticator =
            await _userEmailAuthenticatorRepository.GetAsync(uea => uea.UserId == user.Id);

        if (userEmailAuthenticator.Key != code)
            throw new BusinessException(AuthServiceBusinessMessages.InvalidAuthenticatorCode);

        userEmailAuthenticator.Key = null;
        await _userEmailAuthenticatorRepository.UpdateAsync(userEmailAuthenticator);
    }
    
    private async Task verifyOtpAuthenticatorCode(User user, string codeToVerify)
    {
        UserOtpAuthenticator userOtpAuthenticator =
            await _userOtpAuthenticatorRepository.GetAsync(uoa => uoa.UserId == user.Id);

        bool result = await _otpAuthenticatorHelper.VerifyCodeAsync(userOtpAuthenticator.SecretKey, codeToVerify);

        if (!result)
            throw new BusinessException(AuthServiceBusinessMessages.InvalidAuthenticatorCode);
    }
}