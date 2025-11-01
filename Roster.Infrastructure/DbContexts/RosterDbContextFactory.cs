using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Roster.Infrastructure.DbContexts;

public class RosterDbContextFactory : IDesignTimeDbContextFactory<RosterDbContext>
{
    public RosterDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<RosterDbContext>();
        
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Roster;Username=postgres;Password=postgres123");

        return new RosterDbContext(optionsBuilder.Options);
    }
}