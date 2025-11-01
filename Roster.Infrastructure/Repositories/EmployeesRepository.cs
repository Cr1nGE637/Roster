using System.Diagnostics;
using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using NpgsqlTypes;
using Roster.Application.Interfaces;
using Roster.Domain.Enums;
using Roster.Domain.Model;
using Roster.Infrastructure.DbContexts;
using Roster.Infrastructure.Entities;

namespace Roster.Infrastructure.Repositories;

public class EmployeesRepository : IEmployeesRepository
{
    private readonly RosterDbContext _dbContext;
    private readonly IMapper _mapper;

    public EmployeesRepository(RosterDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Result<List<Employee>>> GetAllAsync()
    {
        var employees = await _dbContext.Employees
            .AsNoTracking()
            .OrderBy(e => e.FullName)
            .ToListAsync();
        return Result.Success(_mapper.Map<List<Employee>>(employees));
    }

    public async Task<Result<List<Employee>>> GetEmployeesByNameAndGenderAsync()
    {
        var employees = await _dbContext.Employees
            .AsNoTracking()
            .Where(e => e.Gender == Gender.Male && e.FullName.StartsWith("F"))
            .ToListAsync();
        return Result.Success(_mapper.Map<List<Employee>>(employees));
    }
    
    public async Task<Result> AddRangeAsync(IEnumerable<Employee> employees)
    {
        try
        {
            var connectionString = _dbContext.Database.GetConnectionString();
            using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();

            using var writer = connection.BeginBinaryImport(@"
            COPY ""Employees"" (""FullName"", ""DateOfBirth"", ""Gender"") 
            FROM STDIN (FORMAT BINARY)");

            foreach (var emp in employees)
            {
                writer.StartRow();
                writer.Write(emp.FullName.ToString(), NpgsqlDbType.Text);
                writer.Write(emp.DateOfBirth.Value, NpgsqlDbType.Date);
                writer.Write((int)emp.Gender, NpgsqlDbType.Integer);
            }

            await writer.CompleteAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Bulk insert failed: {ex.Message}");
        }
    }
    public async Task<Result> AddAsync(Employee employee)
    {
        var employeeEntity = _mapper.Map<EmployeeEntity>(employee);
        await _dbContext.Employees.AddAsync(employeeEntity);
        await _dbContext.SaveChangesAsync();
        return Result.Success();
    }
    
}