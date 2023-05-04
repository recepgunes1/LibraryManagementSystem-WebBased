using Library_Management_System.Core.Entity;
using Library_Management_System.Data.Extensions;
using Library_Management_System.Entity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Data.Context;

public sealed class AppDbContext : IdentityDbContext<User, Role, string>
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.RegisterAllEntities<EntityBase>();
        builder.LoadMappings();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer("Server=172.17.0.2,1433;Database=Library;User Id=SA;Password=Recep123.;TrustServerCertificate=True;");
    }
}