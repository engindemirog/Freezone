using Freezone.Core.Security.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

public class BaseController : ControllerBase
{
    protected IMediator? Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    private IMediator? _mediator;

    protected string getIpAddress()
    {
        if (Request.Headers.ContainsKey("X-Forwarded-For"))
            return Request.Headers["X-Forwarded-For"].ToString();

        return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
    }

    protected void setRefreshTokenToCookie(RefreshToken refreshToken)
    {
        Response.Cookies.Append(key: "refreshToken",
                                refreshToken.Token,
                                options: new CookieOptions
                                {
                                    HttpOnly = true,
                                    Expires = refreshToken.ExpiresDate,
                                    Secure = true,
                                    SameSite = SameSiteMode.Strict
                                });
    }
}