using BeerStore.Api.Data;
using BeerStore.Api.Data.EfCatalog;
using BeerStore.Api.Data.Mapping;
using BeerStore.Api.Data.Repositories;
using BeerStore.Api.Data.Repositories.EF;
using BeerStore.Api.Data.Services;
using BeerStore.Api.Data.Validators;
using BeerStore.Api.Middleware;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MySqlConnector;
var builder = WebApplication.CreateBuilder(args);

var ordersConn = builder.Configuration.GetConnectionString("beer_orders");
var catalogConn = builder.Configuration.GetConnectionString("beer_catalog");

builder.Services.AddScoped<IUnitOfWork>(sp => new UnitOfWork(ordersConn));
builder.Services.AddScoped<IOrderRepository>(sp => sp.GetRequiredService<IUnitOfWork>().Orders);

builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseMySql(catalogConn, ServerVersion.AutoDetect(catalogConn)));

builder.Services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("beer_catalog");
    var conn = new MySqlConnection(connectionString);
    conn.Open(); 
    return conn;
});
builder.Services.AddScoped<IDapperProductRepository>(sp =>
{
    var conn = new MySqlConnection(catalogConn);
    conn.Open();
    return new DapperProductRepository(conn);
});
builder.Services.AddScoped<IDapperCategoryRepository, DapperCategoryRepository>();

builder.Services.AddScoped<ICatalogService, CatalogService>();

builder.Services.AddAutoMapper(typeof(CatalogProfile).Assembly);
builder.Services.AddAutoMapper(typeof(ProductProfile).Assembly);
builder.Services.AddControllers().AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<ProductCreateDtoValidator>());
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BeerStore API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BeerStore API v1"));
}
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
