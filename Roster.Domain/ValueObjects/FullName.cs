using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace Roster.Domain.ValueObjects;

public class FullName : ValueObject
{
    private string Value { get; }

    private FullName(string value)
    {
        Value = value;
    }

    public static Result<FullName> Create(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            return Result.Failure<FullName>("Employee name cannot be empty.");

        var pattern = @"^[A-Z][a-z]+ [A-Z][a-z]+ [A-Z][a-z]+$";
        var isValid = Regex.IsMatch(fullName, pattern);
        return isValid 
            ? Result.Success(new FullName(fullName)) 
            : Result.Failure<FullName>("Employee name does not match required format.");
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public override string ToString() => Value;
}