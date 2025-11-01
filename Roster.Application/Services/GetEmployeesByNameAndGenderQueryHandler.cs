using System.Diagnostics;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Roster.Application.Interfaces;
using Roster.Domain.Model;

namespace Roster.Application.Services;

public class GetEmployeesByNameAndGenderQueryHandler
{
    private readonly IEmployeesRepository _repository;
    private readonly ILogger<GetEmployeesByNameAndGenderQueryHandler> _logger;

    public GetEmployeesByNameAndGenderQueryHandler(IEmployeesRepository repository, ILogger<GetEmployeesByNameAndGenderQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<List<Employee>>> Handle()
    {
        var timer = Stopwatch.StartNew();
        var employees = await _repository.GetEmployeesByNameAndGenderAsync();
        timer.Stop();
        _logger.LogInformation($"GetEmployeesByNameAndGenderQueryHandler: {timer.ElapsedMilliseconds} ms");
        return employees.IsSuccess 
            ? Result.Success(employees.Value) 
            : Result.Failure<List<Employee>>(employees.Error);
    }
}