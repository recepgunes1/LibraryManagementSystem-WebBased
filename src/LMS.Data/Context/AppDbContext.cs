using LMS.Core.Entity;
using LMS.Data.Extensions;
using LMS.Entity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LMS.Data.Context;

public sealed class AppDbContext : IdentityDbContext<User, Role, string>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Console.WriteLine("==============================================================================");
        Console.WriteLine($"Name:{Thread.CurrentThread.Name}-----Id:{Thread.CurrentThread.ManagedThreadId}");
        Console.WriteLine("==============================================================================");

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.RegisterAllEntities<EntityBase>();
        builder.LoadMappings();
    }
}