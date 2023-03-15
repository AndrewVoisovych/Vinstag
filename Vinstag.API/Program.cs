using Microsoft.AspNetCore.Mvc;
using Vinstag.API.DependencyInjection;
using Vinstag.API.Middleware;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
IConfiguration configuration = builder.Configuration;

// Add services to the container.
services.AddControllers(config => config.Filters.Add(new ProducesAttribute("application/json")));
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

//Enable Cors
services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => 
        options
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
        );
});

// Dev services
services
    .AddHttpClientConfiguration()
    .AddConfigurationOptions(configuration)
    .AddServices()
    .AddDataProviders();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options => 
    options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

app.UseMiddleware<ExceptionMiddleware>();
