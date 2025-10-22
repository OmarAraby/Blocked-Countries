using BlockedCountries.API.Repositories.InMemory;
using BlockedCountries.API.Repositories.Interfaces;
using BlockedCountries.API.Services.Implementations;
using BlockedCountries.API.Services.Interfaces;
using BlockedCountries.API.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// FluentValidation
builder.Services.AddFluentValidationAutoValidation(); 
builder.Services.AddValidatorsFromAssemblyContaining<BlockCountryRequestValidator>();
//builder.Services.AddFluentValidationClientsideAdapters();



// Dependency Injection setup
builder.Services.AddSingleton<IBlockedCountriesRepository, BlockedCountriesRepository>();
builder.Services.AddScoped<ICountryService, CountryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
