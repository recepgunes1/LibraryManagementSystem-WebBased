using LMS.Core.Entity;
using LMS.Data.Extensions;
using LMS.Entity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LMS.Data.Context;

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
        optionsBuilder.UseSqlServer(
            "Server=172.19.0.2,1433;Database=Library;User Id=SA;Password=Recep123.;TrustServerCertificate=True;MultipleActiveResultSets=True;");
    }
}