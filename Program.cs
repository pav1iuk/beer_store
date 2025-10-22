using BeerStore.Api.Data;
using BeerStore.Api.Data.EfCatalog;
using BeerStore.Api.Data.Mapping;
using BeerStore.Api.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Connection strings
var ordersConn = builder.Configuration.GetConnectionString("beer_orders");
var catalogConn = builder.Configuration.GetConnectionString("beer_catalog");

// Register UnitOfWork as scoped
builder.Services.AddScoped<IUnitOfWork>(sp => new UnitOfWork(ordersConn, catalogConn));

// Register repositories via UnitOfWork (UnitOfWork will provide concrete repos)
builder.Services.AddScoped<IOrderRepository>(sp => sp.GetRequiredService<IUnitOfWork>().Orders);
builder.Services.AddScoped<IProductRepository>(sp => sp.GetRequiredService<IUnitOfWork>().Products);
builder.Services.AddScoped<ICategoryRepository>(sp => sp.GetRequiredService<IUnitOfWork>().Categories);
builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("beer_catalog"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("beer_catalog"))));
builder.Services.AddAutoMapper(typeof(CatalogProfile).Assembly);
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
