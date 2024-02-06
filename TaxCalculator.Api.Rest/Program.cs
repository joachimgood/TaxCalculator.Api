using TaxCalculator.Api.Core.Interfaces;
using TaxCalculator.Api.Core.Services;
using TaxCalculator.Api.Data.Interfaces;
using TaxCalculator.Api.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ITaxCalculatorService, TaxCalculatorService>();
builder.Services.AddSingleton<IFeeRepository, FeeRepository>();
builder.Services.AddSingleton<IVehicleRepository, VehicleRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
