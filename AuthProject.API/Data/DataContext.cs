using AuthProject.API.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;

namespace AuthProject.API.Data;

public sealed class DataContext : IdentityDbContext<ApplicationUser, IdentityRole<long>, long>
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }
}