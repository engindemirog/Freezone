using FluentValidation;
using Freezone.Core.CrossCuttingConcerns.Exceptions.HttpProblemDetails;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freezone.Core.CrossCuttingConcerns.Exceptions.Handlers;

public class HttpExceptionHandler : ExceptionHandler
{
    private HttpResponse? _response;

    public HttpResponse Response
    {
        get => _response ?? throw new ArgumentNullException(nameof(_response));
        set => _response = value;
    }

    protected override Task HandleException(BusinessException businessException)
    {
        Response.StatusCode = StatusCodes.Status400BadRequest;
        string details = new BusinessProblemDetails(businessException.Message).AsJson();
        return Response.WriteAsync(details);
    }

    protected override Task HandleException(ValidationException validationException)
    {
        Response.StatusCode = StatusCodes.Status400BadRequest;
        var details = new ValidationProblemDetails(validationException.Errors).AsJson();
        return Response.WriteAsync(details.ToString());
    }

    protected override Task HandleException(UnauthorizedAccessException exception)
    {
        Response.StatusCode = StatusCodes.Status401Unauthorized;
        string details = new AuthorizationProblemDetails(exception.Message).AsJson();
        return Response.WriteAsync(details);
    }

    //protected override Task HandleException(AuthorizationException authorizationException)
    //{
    //    Response.StatusCode = StatusCodes.Status401Unauthorized;
    //    string details = new AuthorizationProblemDetails(authorizationException.Message).AsJson();
    //    return Response.WriteAsync(details);
    //}

    //protected override Task HandleException(NotFoundException notFoundException)
    //{
    //    Response.StatusCode = StatusCodes.Status404NotFound;
    //    string details = new NotFoundProblemDetails(notFoundException.Message).AsJson();
    //    return Response.WriteAsync(details);
    //}

    protected override Task HandleException(Exception exception)
    {
        Response.StatusCode = StatusCodes.Status500InternalServerError;
        string details = new InternalServerErrorProblemDetails(exception.Message).AsJson();
        return Response.WriteAsync(details);
    }
}
