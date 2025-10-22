using BeerStore.Api.Data;
using BeerStore.Api.Data.Repositories;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Connection strings
var ordersConn = builder.Configuration.GetConnectionString("OrdersDb");
var catalogConn = builder.Configuration.GetConnectionString("CatalogDb");

// Register UnitOfWork as scoped
builder.Services.AddScoped<IUnitOfWork>(sp => new UnitOfWork(ordersConn, catalogConn));

// Register repositories via UnitOfWork (UnitOfWork will provide concrete repos)
builder.Services.AddScoped<IOrderRepository>(sp => sp.GetRequiredService<IUnitOfWork>().Orders);
builder.Services.AddScoped<IProductRepository>(sp => sp.GetRequiredService<IUnitOfWork>().Products);
builder.Services.AddScoped<ICategoryRepository>(sp => sp.GetRequiredService<IUnitOfWork>().Categories);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BeerStore API", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BeerStore API v1"));

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
