using TaxCalculator.Api.Core.Interfaces;
using TaxCalculator.Api.Core.Services;
using TaxCalculator.Api.Data.Interfaces;
using TaxCalculator.Api.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ITaxCalculatorService, TaxCalculatorService>();
builder.Services.AddTransient<IFeeRepository, FeeRepository>();
builder.Services.AddTransient<IVehicleRepository, VehicleRepository>();

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
