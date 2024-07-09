using ASP.Abstraction;
using ASP.Data;
using ASP.Mapper;
using ASP.Repository;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.AddMemoryCache(x => x.TrackStatistics = true);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    container.RegisterType<ProductRepository>().As<IProductRepository>();
    container.RegisterType<ProductGroupRepository>().As<IProductGroupRepository>();
    container.Register(_ => new StorageContext(builder.Configuration.GetConnectionString("db"))).InstancePerDependency();
});


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

app.UseStaticFiles();
