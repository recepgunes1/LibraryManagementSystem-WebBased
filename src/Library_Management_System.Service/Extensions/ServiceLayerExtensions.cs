using System.Reflection;
using Library_Management_System.Data.Context;
using Library_Management_System.Entity.Entities;
using Library_Management_System.Service.Services.Abstracts;
using Library_Management_System.Service.Services.Concretes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Library_Management_System.Service.Extensions;

public static class ServiceLayerExtensions
{
    public static IServiceCollection LoadServiceLayer(this IServiceCollection service)
    {
        service.AddIdentity<User, Role>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;

                opt.User.AllowedUserNameCharacters = "0123456789";
                opt.User.RequireUniqueEmail = true;

                opt.SignIn.RequireConfirmedPhoneNumber = false;
                opt.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<AppDbContext>();

        service.ConfigureApplicationCookie(opt =>
        {
            var cookieBuilder = new CookieBuilder
            {
                Name = "UdemyAppCookie"
            };

            opt.LoginPath = new PathString("/Account/Auth/Login");
            opt.LogoutPath = new PathString("/Account/Auth/Logout");
            opt.AccessDeniedPath = new PathString("/Account/Auth/AccessDenied");
            opt.Cookie = cookieBuilder;
            opt.ExpireTimeSpan = TimeSpan.FromDays(60);
            opt.SlidingExpiration = true;
        });

        service.AddScoped<IUserService, UserService>();
        service.AddAutoMapper(Assembly.GetExecutingAssembly());
        return service;
    }
}