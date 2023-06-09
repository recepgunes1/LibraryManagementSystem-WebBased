using LMS.Data.Context;
using LMS.Data.Repository;
using LMS.Data.UnitOfWorks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LMS.Data.Extensions;

public static class DataLayerExtensions
{
    public static IServiceCollection LoadDataLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddDbContext<AppDbContext>(p =>
            p.UseSqlServer(configuration.GetConnectionString("default"))
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging());
        return services;
    }
}