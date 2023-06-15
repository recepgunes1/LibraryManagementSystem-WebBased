using System.Reflection;
using LMS.Data.Context;
using LMS.Entity.Entities;
using LMS.Service.ClaimProviders;
using LMS.Service.Helpers.EmailService;
using LMS.Service.Helpers.ImageService;
using LMS.Service.OptionModels;
using LMS.Service.Services.Abstracts;
using LMS.Service.Services.Concretes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LMS.Service.Extensions;

public static class ServiceLayerExtensions
{
    public static IServiceCollection LoadServiceLayer(this IServiceCollection service, IConfiguration configuration)
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
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        service.ConfigureApplicationCookie(opt =>
        {
            var cookieBuilder = new CookieBuilder
            {
                Name = "LMS_Cookies"
            };

            opt.LoginPath = new PathString("/Account/Auth/Login");
            opt.LogoutPath = new PathString("/Account/Auth/Logout");
            opt.AccessDeniedPath = new PathString("/Account/Auth/AccessDenied");
            opt.Events = new CookieAuthenticationEvents
            {
                OnRedirectToAccessDenied = context =>
                {
                    context.Response.Redirect(opt.AccessDeniedPath);
                    return Task.CompletedTask;
                }
            };
            opt.Cookie = cookieBuilder;
            opt.ExpireTimeSpan = TimeSpan.FromDays(16);
            opt.SlidingExpiration = true;
        });

        service.AddAuthorizationCore(opt =>
            opt.AddPolicy("BookBorrowingCheck",
                builder => builder.RequireAuthenticatedUser().RequireClaim("borrowable", "true")));

        service.AddScoped<IUserService, UserService>();
        service.AddScoped<IAuthorService, AuthorService>();
        service.AddScoped<ICategoryService, CategoryService>();
        service.AddScoped<ISettingsService, SettingsService>();
        service.AddScoped<IPublisherService, PublisherService>();
        service.AddScoped<IBookService, BookService>();
        service.AddScoped<IBorrowService, BorrowService>();
        service.AddScoped<IImageService, ImageService>();
        service.AddScoped<IEmailService, EmailService>();
        service.AddScoped<IClaimsTransformation, BookBorrowingClaimProvider>();

        service.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        
        service.AddAutoMapper(Assembly.GetExecutingAssembly());
        return service;
    }

    public static IApplicationBuilder MigrateDatabase(this IApplicationBuilder builder)
    {
        using (var scope = builder.ApplicationServices.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.Migrate();
        }
        return builder;
    }
}