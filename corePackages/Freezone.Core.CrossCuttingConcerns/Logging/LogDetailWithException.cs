namespace Freezone.Core.CrossCuttingConcerns.Logging;

public class LogDetailWithException : LogDetail 
{
    public string ExceptionMessage { get; set; }
}
