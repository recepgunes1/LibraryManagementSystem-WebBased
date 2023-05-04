using Library_Management_System.Data.Context;
using Library_Management_System.Data.Repository;
using Library_Management_System.Data.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library_Management_System.Data.Extensions;

public static class DataLayerExtensions
{
    public static IServiceCollection LoadDataLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddDbContext<AppDbContext>(p => p.UseSqlServer(configuration.GetConnectionString("default")));
        return services;
    }
}