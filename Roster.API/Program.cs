using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Roster.API;
using Roster.Application.Interfaces;
using Roster.Application.Services;
using Roster.Infrastructure.DbContexts;
using Roster.Infrastructure.Repositories;

var builder = Host.CreateApplicationBuilder();

builder.Services.AddDbContext<RosterDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("RosterDb")));
builder.Services.AddAutoMapper(cfg =>
    cfg.AddMaps(typeof(Roster.Infrastructure.Mappers.EmployeeProfile).Assembly));

builder.Services.AddScoped<IEmployeesRepository, EmployeesRepository>();
builder.Services.AddScoped<CreateEmployeeCommandHandler>();
builder.Services.AddScoped<CreateListOfEmployeesCommandHandler>();
builder.Services.AddScoped<GetAllEmployeesQueryHandler>();
builder.Services.AddScoped<GetEmployeesByNameAndGenderQueryHandler>();
builder.Services.AddScoped<AppRunner>();

var app = builder.Build();

var appRunner = app.Services.GetService<AppRunner>();

await appRunner?.RunAsync(args)!;
