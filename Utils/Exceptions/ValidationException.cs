namespace KeycloackTest.Domain.Utils.Exceptions;

public class ValidationException: Exception
{
    public List<string> Errors { get; } = new List<string>();
    public Dictionary<string, List<string>> DetailedErrors { get; } = new Dictionary<string, List<string>>();

    public ValidationException(string message) : base(message)
    {
    }

    public ValidationException(string message, Dictionary<string, List<string>> detailedErrors) : base(message)
    {
        DetailedErrors = detailedErrors;
    }

    public ValidationException(string message, List<string> erros) : base(message)
    {
        Errors = erros;
    }
}
