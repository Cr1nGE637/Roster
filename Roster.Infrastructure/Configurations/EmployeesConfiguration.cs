using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roster.Infrastructure.Entities;

namespace Roster.Infrastructure.Configurations;

public class EmployeesConfiguration :  IEntityTypeConfiguration<EmployeeEntity>
{
    public void Configure(EntityTypeBuilder<EmployeeEntity> builder)
    {
        builder.HasKey(e => new {e.FullName, e.DateOfBirth});
        
        builder.Property(e => e.FullName)
            .IsRequired();
        
        builder.Property(e => e.DateOfBirth)
            .IsRequired();
    }
}