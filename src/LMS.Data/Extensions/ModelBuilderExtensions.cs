using LMS.Entity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LMS.Data.Extensions;

public static class ModelBuilderExtensions
{
    public static void LoadMappings(this ModelBuilder builder)
    {
        var roleStudent = new Role
        {
            Id = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            Name = "student",
            NormalizedName = "STUDENT"
        };
        var roleLecturer = new Role
        {
            Id = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            Name = "lecturer",
            NormalizedName = "LECTURER"
        };
        var roleAdmin = new Role
        {
            Id = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            Name = "admin",
            NormalizedName = "ADMIN"
        };

        var userAdmin = new User
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = "admin",
            LastName = "admin",
            UserName = "111111111",
            NormalizedUserName = "111111111",
            Email = "admin@admin.com",
            NormalizedEmail = "ADMIN@ADMIN.COM"
        };
        var ph = new PasswordHasher<User>();
        userAdmin.PasswordHash = ph.HashPassword(userAdmin, "Admin123.");

        builder.Entity<IdentityRoleClaim<string>>(p => p.ToTable("RoleClaims"));
        builder.Entity<Role>(p => p.ToTable("Roles"));
        builder.Entity<IdentityUserClaim<string>>(p => p.ToTable("UserClaims"));
        builder.Entity<IdentityUserLogin<string>>(p => p.ToTable("UserLogins"));
        builder.Entity<IdentityUserRole<string>>(p => p.ToTable("UserRoles"));
        builder.Entity<User>(p => p.ToTable("Users"));
        builder.Entity<IdentityUserToken<string>>(p => p.ToTable("UserTokens"));
        builder.Entity<Book>(p => p.HasIndex(i => i.Isbn).IsUnique());
        builder.Entity<Author>(p => p.HasIndex(i => i.FullName).IsUnique());

        builder.Entity<Role>().HasData(roleAdmin, roleLecturer, roleStudent);
        builder.Entity<User>().HasData(userAdmin);
        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = roleAdmin.Id,
            UserId = userAdmin.Id
        });
        builder.Entity<IdentityRoleClaim<string>>().HasData(new IdentityRoleClaim<string>
        {
            Id = 1,
            RoleId = roleStudent.Id,
            ClaimType = "MaxBooks",
            ClaimValue = "5"
        }, new IdentityRoleClaim<string>
        {
            Id = 2,
            RoleId = roleStudent.Id,
            ClaimType = "MaxDays",
            ClaimValue = "20"
        }, new IdentityRoleClaim<string>
        {
            Id = 3,
            RoleId = roleLecturer.Id,
            ClaimType = "MaxBooks",
            ClaimValue = "4"
        }, new IdentityRoleClaim<string>
        {
            Id = 4,
            RoleId = roleLecturer.Id,
            ClaimType = "MaxDays",
            ClaimValue = "12"
        });

        builder.Entity<User>(p =>
        {
            p.Ignore(i => i.EmailConfirmed);
            p.Ignore(i => i.PhoneNumber);
            p.Ignore(i => i.PhoneNumberConfirmed);
            p.Ignore(i => i.TwoFactorEnabled);
        });
    }
}