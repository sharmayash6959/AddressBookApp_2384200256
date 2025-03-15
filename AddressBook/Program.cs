using AutoMapper;
using BusinessLayer.Interface;
using BusinessLayer.MappingProfiles;
using BusinessLayer.Service;
using BusinessLayer.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ModelLayer.DTOs;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AddressBookProfile));
builder.Services.AddValidatorsFromAssemblyContaining<AddressBookValidator>();



// Access Configuration
var configuration = builder.Configuration;

// Configure Database Context
builder.Services.AddDbContext<AddressBookContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("RepositoryLayer")));

builder.Services.AddScoped<IAddressBookBL, AddressBookBL>();
builder.Services.AddScoped<IAddressBookRL, AddressBookRL>();
builder.Services.AddScoped<IMapper, Mapper>();
builder.Services.AddScoped<IValidator<AddressBookDTO>, AddressBookValidator>();

// Build and run the app
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
