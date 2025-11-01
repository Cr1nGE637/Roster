using System.Globalization;
using AutoMapper;
using Roster.Domain.Enums;

namespace Roster.Application.Services;

public static class EmployeeGenerator
{
    private static readonly Random Random = new();

    private static string RandomString(int minLength = 5, int maxLength = 10)
    {
        var length = Random.Next(minLength, maxLength + 1);
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Next(s.Length)]).ToArray());
    }


    public static string RandomDateOfBirth()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var minDate = today.AddYears(-80);
        var maxDate = today.AddYears(-18);
        var range = maxDate.DayNumber - minDate.DayNumber;
        var randomDayNumber = minDate.DayNumber + Random.Next(range + 1);
        var randomDate = DateOnly.FromDayNumber(randomDayNumber);

        var formats = new[]
        {
            "yyyy-MM-dd",
            "MM-dd-yyyy",
            "dd-MM-yyyy",
            "yyyy.MM.dd",
            "MM.dd.yyyy",
            "dd.MM.yyyy",
            "yyyy/MM/dd",
            "MM/dd/yyyy",
            "dd/MM/yyyy",
        };

        var randomFormat = formats[Random.Next(formats.Length)];
        return randomDate.ToString(randomFormat, CultureInfo.InvariantCulture);
    }

    public static Gender RandomGender()
    {
        return Random.Next(2) == 0 ? Gender.Male : Gender.Female;
    }

    public static string RandomFullName()
    {
        var firstName = (char)('A' + Random.Next(26)) + RandomString(3, 7).ToLower();
        var lastName = (char)('A' + Random.Next(26)) + RandomString(4, 8).ToLower();
        var middleName = (char)('A' + Random.Next(26)) + RandomString(5, 9).ToLower();
        return $"{Capitalize(lastName)} {Capitalize(firstName)} {Capitalize(middleName)}";
    }

    public static string RandomFullNameWithF()
    {
        var lastName = "F" + RandomString(4, 8).ToLower();
        var firstName = (char)('A' + Random.Next(26)) + RandomString(3, 7).ToLower();
        var middleName = (char)('A' + Random.Next(26)) + RandomString(5, 9).ToLower();
        return $"{Capitalize(lastName)} {Capitalize(firstName)} {Capitalize(middleName)}";
    }

    private static string Capitalize(string input)
    {
        return input.Length == 0 ? input : char.ToUpper(input[0]) + input[1..].ToLower();
    }
}