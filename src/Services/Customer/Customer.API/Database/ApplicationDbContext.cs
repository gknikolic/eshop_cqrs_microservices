using Customer.API.Database.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection;

namespace Customer.API.Database;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public DbSet<User> Users { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);

        builder.Entity<User>().Property(p => p.Initials).HasMaxLength(5);

        builder.HasDefaultSchema("identity");
    }
}