using Microsoft.Extensions.DependencyInjection;
using Roster.Application.Services;
using Roster.Infrastructure.DbContexts;

namespace Roster.API;

public class AppRunner
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<string, Func<string[], Task<int>>> _commands;

    public AppRunner(
        CreateEmployeeCommandHandler createHandler,
        CreateListOfEmployeesCommandHandler createListHandler,
        GetAllEmployeesQueryHandler getAllHandler,
        GetEmployeesByNameAndGenderQueryHandler getByNameAndGenderHandler,
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _commands = new()
        {
            ["1"] = _ => HandleInitializeDatabase(),
            ["2"] = args => HandleCreateEmployee(createHandler, args),
            ["3"] = _ => HandleGetAllEmployees(getAllHandler),
            ["4"] = _ => HandleCreateListOfEmployees(createListHandler),
            ["5"] = _ => HandleGetEmployeesByNameAndGender(getByNameAndGenderHandler)
        };
    }

    private async Task<int> HandleGetEmployeesByNameAndGender(GetEmployeesByNameAndGenderQueryHandler getByNameAndGenderHandler)
    {
        var result = await getByNameAndGenderHandler.Handle();
        if (result.IsFailure)
        {
            Console.WriteLine(result.Error);
            return 1;
        }
        /*var employees = result.Value;
        foreach (var employee in employees)
        {
            Console.WriteLine($"{employee.FullName}, {employee.Gender}");
        }*/
        return 0;
    }

    public async Task<int> RunAsync(string[] args)
    {
        if (args.Length == 0 || !_commands.TryGetValue(args[0], out var command))
        {
            return 1;
        }

        return await command(args.Skip(1).ToArray());
    }

    private async Task<int> HandleCreateListOfEmployees(CreateListOfEmployeesCommandHandler createListHandler)
    {
        var result = await createListHandler.Handle();
        if (result.IsFailure)
        {
            Console.WriteLine(result.Error);
            return 1;
        }
        Console.WriteLine("Success");
        return 0;
    }
    
    private async Task<int> HandleGetAllEmployees(GetAllEmployeesQueryHandler queryHandler)
    {
        var result = await queryHandler.Handle();
        if (result.IsFailure)
        {
            Console.WriteLine(result.Error);
            return 1;
        }
        var employees = result.Value;
        foreach (var employee in employees)
        {
            Console.WriteLine($"{employee.FullName}, {employee.DateOfBirth}, {employee.Gender}, {employee.CalculateAge()}");
        }
        return 0;
    }

    private async Task<int> HandleCreateEmployee(CreateEmployeeCommandHandler createHandler, string[] args)
    {
        if (args.Length is < 3 or > 4)
        {
            Console.WriteLine("Only 3 args are supported");
            return 1;
        }

        var result = await createHandler.Handle(args[0], args[1], args[2]);
        if (result.IsFailure)
        {
            Console.WriteLine(result.Error);
            return -1;
        }
        
        Console.WriteLine("Success");
        return 0;
    }

    private async Task<int> HandleInitializeDatabase()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<RosterDbContext>();
        await context.Database.EnsureCreatedAsync();
        return 0;
    }
}