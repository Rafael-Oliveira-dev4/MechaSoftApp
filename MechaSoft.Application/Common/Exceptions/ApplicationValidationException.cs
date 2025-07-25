using FluentValidation.Results;


namespace MechaSoft.Application.Common.Exceptions;

public class ApplicationValidationException : ApplicationException
{
    public IDictionary<string, string[]> Errors { get; }

    public ApplicationValidationException(string? property = null, string? message = null)
        : base("One or more validation failures have occurred")
    {
        Errors = new Dictionary<string, string[]>();

        if (property != null && message != null)
        {
            Errors.Add(property, new[] { message });
        }
    }

    public ApplicationValidationException(IEnumerable<ValidationFailure> failures) : this()
    {
        Errors = failures.GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());

    }
}
