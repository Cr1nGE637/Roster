using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using Roster.Domain.Enums;
using Roster.Domain.ValueObjects;

namespace Roster.Domain.Model;

public class Employee
{
    public FullName FullName { get; set; }
    public DateOfBirth DateOfBirth { get; set; }
    public Gender Gender { get; set; }

    private Employee(FullName name, DateOfBirth dateOfBirth, Gender gender)
    {
        FullName = name;
        DateOfBirth = dateOfBirth;
        Gender = gender;
    }

    public static Result<Employee> Create(FullName fullName, DateOfBirth dateOfBirth, Gender gender)
    {
        return Result.Success(new Employee(fullName, dateOfBirth, gender));
    }

    public static Result<Gender> ValidateGender(string gender)
    {
        if (string.IsNullOrWhiteSpace(gender))
        {
            return Result.Failure<Gender>("Employee gender cannot be empty.");
        }
        return Enum.TryParse<Gender>(gender, true, out var genderParsed) 
            ? Result.Success(genderParsed) 
            : Result.Failure<Gender>("Employee gender has to be male or female.");
    }
    
    public int CalculateAge()
    {
        var age = DateOnly.FromDateTime(DateTime.Now).Year - DateOfBirth.Value.Year;
        return age < 0 ? 0 : age;
    }
}