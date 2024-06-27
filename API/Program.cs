using API.Extensions;
using API.Middleware;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddApplicationServices(builder.Configuration);   // Extension Method


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles(); //for images etc in wwwwroot(serving static content), api's new job other than dealing with http requests

app.UseCors("CorsPolicy");  // middleware for application services extensions cors service

app.UseAuthorization();             //All middleware including http request pipeline

app.MapControllers();

using var scope = app.Services.CreateScope();  
var services = scope.ServiceProvider;
var context = services.GetRequiredService<StoreContext>();  //Get two services from scope
var logger = services.GetRequiredService<ILogger<Program>>();  
try
{
    await context.Database.MigrateAsync();  // async applies pending migrations OR creates database if none exists when app starts
    await StoreContextSeed.SeedAsync(context);
}
catch (Exception ex)
{
    
    logger.LogError(ex, "An error occurred during migration");
}

app.Run();
