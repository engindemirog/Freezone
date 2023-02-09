using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.Register;
using Freezone.Core.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : BaseController
{
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
}