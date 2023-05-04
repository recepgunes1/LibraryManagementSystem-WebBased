using Library_Management_System.Entity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Data.Extensions;

public static class ModelBuilderExtensions
{
    public static void RegisterAllEntities<TBase>(this ModelBuilder builder)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(p => p.FullName!.Contains(nameof(Entity.Entities)));

        var types = assemblies.SelectMany(assembly => assembly.GetExportedTypes())
            .Where(c => c is { IsClass: true, IsAbstract: false, IsPublic: true } && typeof(TBase).IsAssignableFrom(c));

        foreach (var type in types)
        {
            Console.WriteLine(type.FullName);
            builder.Entity(type);
        }
    }

    public static void LoadMappings(this ModelBuilder builder)
    {
        builder.Entity<IdentityRoleClaim<string>>(p => p.ToTable("RoleClaims"));
        builder.Entity<Role>(p => p.ToTable("Roles"));
        builder.Entity<IdentityUserClaim<string>>(p => p.ToTable("UserClaims"));
        builder.Entity<IdentityUserLogin<string>>(p => p.ToTable("UserLogins"));
        builder.Entity<IdentityUserRole<string>>(p => p.ToTable("UserRoles"));
        builder.Entity<User>(p => p.ToTable("Users"));
        builder.Entity<IdentityUserToken<string>>(p => p.ToTable("UserTokens"));
        builder.Entity<Book>(p => p.HasIndex(i => i.Isbn).IsUnique());
        builder.Entity<Author>(p => p.HasIndex(i => i.FullName).IsUnique());

        builder.Entity<Role>().HasData(
            new Role() { Name = "student", NormalizedName = "STUDENT", ConcurrencyStamp = Guid.NewGuid().ToString() },
            new Role() { Name = "lecturer", NormalizedName = "LECTURER", ConcurrencyStamp = Guid.NewGuid().ToString() },
            new Role() { Name = "admin", NormalizedName = "ADMIN", ConcurrencyStamp = Guid.NewGuid().ToString() }
        );

        builder.Entity<User>(p =>
        {
            p.Ignore(i => i.EmailConfirmed);
            p.Ignore(i => i.PhoneNumber);
            p.Ignore(i => i.PhoneNumberConfirmed);
            p.Ignore(i => i.TwoFactorEnabled);
        });
        
    }
}