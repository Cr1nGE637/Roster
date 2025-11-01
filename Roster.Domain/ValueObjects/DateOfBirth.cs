using System.Globalization;
using CSharpFunctionalExtensions;

namespace Roster.Domain.ValueObjects;

public class DateOfBirth : ValueObject
{
    public DateOnly Value { get; }
    
    private static readonly string[] SupportedFormats =
    [
        "yyyy-MM-dd",
        "MM-dd-yyyy",
        "dd-MM-yyyy",
        "yyyy.MM.dd",
        "MM.dd.yyyy",
        "dd.MM.yyyy",
        "yyyy/MM/dd",
        "MM/dd/yyyy",
        "dd/MM/yyyy",
    ];
    
    private DateOfBirth(DateOnly value)
    {
        Value = value;
    }
    
    public static Result<DateOfBirth> Create(string dateOfBirth)
    {
        var dateOfBirthParsed = Parse(dateOfBirth);
        return dateOfBirthParsed.IsSuccess 
            ? Result.Success(new DateOfBirth(dateOfBirthParsed.Value)) 
            : Result.Failure<DateOfBirth>("Invalid date of birth format.");
    }

    public static DateOfBirth Create(DateOnly dateOfBirth)
    {
        return new DateOfBirth(dateOfBirth);
    }

    private static Result<DateOnly> Parse(string dateOfBirth)
    {
        if (string.IsNullOrWhiteSpace(dateOfBirth))
        {
            return Result.Failure<DateOnly>("Employee date of birthday cannot be empty.");
        }
        return DateOnly.TryParseExact(
            dateOfBirth,
            SupportedFormats,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out var dateOfBirthParsed)
            ? Result.Success(dateOfBirthParsed)
            : Result.Failure<DateOnly>("Invalid date of birth format.");
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}