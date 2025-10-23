using BlockedCountries.API.BackgroundServices;
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
builder.Services.AddValidatorsFromAssemblyContaining<TemporalBlockRequestValidator>();

//builder.Services.AddFluentValidationClientsideAdapters();



// Dependency Injection setup

// repos
builder.Services.AddSingleton<IBlockedCountriesRepository, BlockedCountriesRepository>();
builder.Services.AddSingleton<ITemporalBlocksRepository, TemporalBlocksRepository>();
builder.Services.AddSingleton<IBlockedAttemptsLogRepository, BlockedAttemptsLogRepository>();

// services
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<ITemporalBlockService, TemporalBlockService>();
builder.Services.AddScoped<IBlockedAttemptsLogger, BlockedAttemptsLogger>();
builder.Services.AddHttpClient<IGeolocationService, GeolocationService>(client =>
{
    client.BaseAddress = new Uri("https://api.ipgeolocation.io/");
});



// background service 
builder.Services.AddHostedService<TemporalBlockCleanupService>();

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
