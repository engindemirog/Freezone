using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Freezone.Core.CrossCuttingConcerns.Exceptions.HttpProblemDetails;

public class ValidationProblemDetails : ProblemDetails
{
    public object Failures { get; init; }

    public ValidationProblemDetails(object failures)
    {
        Title = "Validation error(s)";
        Failures = failures;
        Status = StatusCodes.Status400BadRequest;
        Type = "https://example.com/probs/validation";
        Detail = "";
        Instance = "";
    }
}
