using Application.Features.Auth.Commands.EnableEmailAuthenticator;
using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.Refresh;
using Application.Features.Auth.Commands.Register;
using Application.Features.Auth.Commands.Revoke;
using Application.Features.Auth.Commands.VerifyEmailAuthenticator;
using Freezone.Core.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebAPI.ValueObjects;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : BaseController
{
    private WebApiConfigurations _webApiConfigurations;

    public AuthController(IConfiguration configuration)
    {
        _webApiConfigurations = configuration.GetSection("WebApiConfigurations").Get<WebApiConfigurations>()
                             ?? throw new ArgumentNullException(nameof(WebApiConfigurations));
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] UserForLoginDto userForLoginDto)
    {
        LoggedResponse response = await Mediator.Send(new LoginCommand
        {
            UserForLoginDto = userForLoginDto,
            IpAddress = getIpAddress()
        });
        setRefreshTokenToCookie(response.RefreshToken);
        return Ok(response.AccessToken);
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] UserForRegisterDto userForRegisterDto)
    {
        RegisteredResponse response =
            await Mediator.Send(new RegisterCommand
            {
                UserForRegisterDto = userForRegisterDto,
                IpAddress = getIpAddress()
            });
        setRefreshTokenToCookie(response.RefreshToken);
        return Ok(response.AccessToken);
    }

    [HttpGet("RefreshToken")]
    public async Task<IActionResult> RefreshToken()
    {
        RefreshedResponse response =
            await Mediator.Send(new RefreshCommand
            {
                RefreshToken = getRefreshTokenFromCookie(),
                IpAddress = getIpAddress()
            });
        setRefreshTokenToCookie(response.RefreshToken);
        return Ok(response.AccessToken);
    }

    [HttpPut("RevokeToken")]
    public async Task<IActionResult> RevokeToken(
        [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)]
        string? refreshToken) // Uzaktan revoke işlemi için refresh token gönderilebilir.
    {
        RevokedResponse response =
            await Mediator.Send(new RevokeCommand
            {
                Token = refreshToken ?? getRefreshTokenFromCookie(),
                IPAddress = getIpAddress()
            });
        return Ok(response);
    }

    [HttpPost("EnableEmailAuthenticator")]
    public async Task<IActionResult> EnableEmailAuthenticator()
    {
        EnableEmailAuthenticatorCommand command = new()
        {
            UserId = getUserIdFromToken(),
            VerifyEmailUrl = _webApiConfigurations.AuthVerifyEmailUrl
        };
        await Mediator.Send(command);
        return Ok();
    }

    [HttpGet("VerifyEmailAuthenticator")] // Verify Email URL api'a yönlendirdiği için GET kullandık. Bir frontend yardımıyla yapılırsa PUT olabilir.
    public async Task<IActionResult> VerifyEmailAuthenticator([FromQuery] string activationKey)
    {
        VerifyEmailAuthenticatorCommand command = new()
        {
            //UserId = getUserIdFromToken(),
            ActivationKey = activationKey
        };
        await Mediator.Send(command);
        return Ok();
    }
}