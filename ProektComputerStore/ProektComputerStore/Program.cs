using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProektComputerStore.Data;
using ProektComputerStore.Repositories.Interface;
using ProektComputerStore.Repositories.Repos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<StoreDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"));

});

builder.Services.AddScoped<ICategoryRepository , CategoryRepository>();
builder.Services.AddScoped<IProductRepository , ProductRepository>();

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
