using Microsoft.EntityFrameworkCore;
using AuthProject.API.Data;

namespace AuthProject.API.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using DataContext context = scope.ServiceProvider.GetRequiredService<DataContext>();

        context.Database.Migrate();
    }
}
