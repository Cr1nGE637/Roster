using CSharpFunctionalExtensions;
using Roster.Application.Interfaces;
using Roster.Domain.Model;
using Roster.Domain.ValueObjects;

namespace Roster.Application.Services;

public class CreateEmployeeCommandHandler
{
    private readonly IEmployeesRepository _repository;

    public CreateEmployeeCommandHandler(IEmployeesRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(string fullName, string dateOfBirth, string gender)
    {
        var validName = FullName.Create(fullName).Value;
        var validDateOfBirth = DateOfBirth.Create(dateOfBirth).Value;
        var validGender = Employee.ValidateGender(gender).Value;
        var employeeResult = Employee.Create(validName, validDateOfBirth, validGender);
        if (employeeResult.IsFailure)
        {
            return Result.Failure<Employee>("Error");
        }
        await _repository.AddAsync(employeeResult.Value);
        return Result.Success();
    }
}