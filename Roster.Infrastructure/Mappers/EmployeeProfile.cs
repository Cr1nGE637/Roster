using AutoMapper;
using Roster.Domain.Model;
using Roster.Domain.ValueObjects;
using Roster.Infrastructure.Entities;

namespace Roster.Infrastructure.Mappers;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<EmployeeEntity, Employee>()
            .ConstructUsing(src =>
                Employee.Create(FullName.Create(src.FullName).Value, DateOfBirth.Create(src.DateOfBirth), Employee.ValidateGender(src.Gender.ToString()).Value).Value);
        
        CreateMap<Employee, EmployeeEntity>()
            .ForMember(src => src.FullName, opt => opt.MapFrom(src => src.FullName.ToString()))
            .ForMember(src => src.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.Value))
            .ForMember(src => src.Gender, opt => opt.MapFrom(src => src.Gender));
    }
}