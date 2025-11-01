using CSharpFunctionalExtensions;
using Roster.Domain.Model;

namespace Roster.Application.Interfaces;

public interface IEmployeesRepository
{
    Task<Result<List<Employee>>> GetAllAsync();
    Task<Result<List<Employee>>> GetEmployeesByNameAndGenderAsync();
    Task<Result> AddRangeAsync(IEnumerable<Employee> employees);
    Task<Result> AddAsync(Employee employee);
}