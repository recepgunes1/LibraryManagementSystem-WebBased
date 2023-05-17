using System.Reflection;
using LMS.Data.Context;
using LMS.Entity.Entities;
using LMS.Service.Services.Abstracts;
using LMS.Service.Services.Concretes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace LMS.Service.Extensions;

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
                Name = "LMS_Cookies"
            };

            opt.LoginPath = new PathString("/Account/Auth/Login");
            opt.LogoutPath = new PathString("/Account/Auth/Logout");
            opt.AccessDeniedPath = new PathString("/Account/Auth/AccessDenied");
            opt.Cookie = cookieBuilder;
            opt.ExpireTimeSpan = TimeSpan.FromDays(16);
            opt.SlidingExpiration = true;
        });

        service.AddScoped<IUserService, UserService>();
        service.AddScoped<IAuthorService, AuthorService>();
        service.AddScoped<ICategoryService, CategoryService>();
        service.AddScoped<IPublisherService, PublisherService>();
        service.AddScoped<IBookService, BookService>();
        service.AddAutoMapper(Assembly.GetExecutingAssembly());
        return service;
    }
}
