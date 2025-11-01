using CSharpFunctionalExtensions;
using Roster.Application.Interfaces;
using Roster.Domain.Enums;
using Roster.Domain.Model;
using Roster.Domain.ValueObjects;

namespace Roster.Application.Services;

public class CreateListOfEmployeesCommandHandler
{
    private readonly IEmployeesRepository _repository;

    public CreateListOfEmployeesCommandHandler(IEmployeesRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle()
    {
        List<Employee> list = [];
        for (var i = 0; i < 1000000; i++)
        {
            var fullName = FullName.Create(EmployeeGenerator.RandomFullName());
            var dateOfBirth = DateOfBirth.Create(EmployeeGenerator.RandomDateOfBirth());
            var gender = Employee.ValidateGender(EmployeeGenerator.RandomGender().ToString()).Value;
            
            var employees = Employee.Create(fullName.Value, dateOfBirth.Value, gender);
            list.Add(employees.Value);
        }

        for (var i = 0; i < 100; i++)
        {
            var fullName = FullName.Create(EmployeeGenerator.RandomFullNameWithF());
            var dateOfBirth = DateOfBirth.Create(EmployeeGenerator.RandomDateOfBirth());
            
            var employees = Employee.Create(fullName.Value, dateOfBirth.Value, Gender.Male);
            list.Add(employees.Value);
        }
        
        return await _repository.AddRangeAsync(list);
    }
}