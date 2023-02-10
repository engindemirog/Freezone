using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Freezone.Core.CrossCuttingConcerns.Exceptions.HttpProblemDetails;

public class AuthorizationProblemDetails : ProblemDetails
{
    public AuthorizationProblemDetails(string detail)
    {
        Title = "Security violation";
        Detail = detail;
        Status = StatusCodes.Status401Unauthorized;
        Type = "https://example.com/probs/security";
        Instance = "";
    }
}