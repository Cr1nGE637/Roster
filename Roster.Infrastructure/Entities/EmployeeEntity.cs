using Roster.Domain.Enums;

namespace Roster.Infrastructure.Entities;

public class EmployeeEntity
{
    public string FullName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public Gender Gender { get; set; }
}