using AuthProject.API.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthProject.API.Data;

public sealed class DataContext : IdentityDbContext<ApplicationUser, IdentityRole<long>, long>
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        SeedRoles(builder);
    }

    private void SeedRoles(ModelBuilder builder)
    {
        builder.Entity<IdentityRole<long>>().HasData(
            new IdentityRole<long> { Id = 1, Name = "Member", NormalizedName = "MEMBER" },
            new IdentityRole<long> { Id = 2, Name = "Moderator", NormalizedName = "MODERATOR" },
            new IdentityRole<long> { Id = 3, Name = "Administrator", NormalizedName = "ADMINISTRATOR" }
        );
    }
}
