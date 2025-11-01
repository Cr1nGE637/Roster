using Microsoft.EntityFrameworkCore;
using Roster.Infrastructure.Entities;

namespace Roster.Infrastructure.DbContexts;

public class RosterDbContext(DbContextOptions<RosterDbContext> options) :  DbContext(options)
{
    public DbSet<EmployeeEntity> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RosterDbContext).Assembly);
    }
    
}