using CSharpFunctionalExtensions;
using Roster.Application.Interfaces;
using Roster.Domain.Model;

namespace Roster.Application.Services;

public class GetAllEmployeesQueryHandler
{
    private readonly IEmployeesRepository _repository;

    public GetAllEmployeesQueryHandler(IEmployeesRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<Employee>>> Handle()
    {
        var employees = await _repository.GetAllAsync();
        return employees.IsSuccess 
            ? Result.Success(employees.Value) 
            : Result.Failure<List<Employee>>(employees.Error);
    }
}