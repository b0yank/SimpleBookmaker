namespace SimpleBookmaker.Web.Infrastructure.Extensions
{
    using Data;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Services.Infrastructure;
    using System.Threading.Tasks;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDefaultRoles(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var database = serviceScope.ServiceProvider.GetService<SimpleBookmakerDbContext>().Database;

                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                Task
                    .Run(async () =>
                    {
                        var roles = Roles.All();

                        foreach (var role in roles)
                        {
                            bool exists = await roleManager.RoleExistsAsync(role);
                            
                            if (!exists)
                            {
                                await roleManager.CreateAsync(new IdentityRole { Name = role });
                            }
                        }
                    })
                    .GetAwaiter()
                    .GetResult();
            }

            return app;
        }

        public static IApplicationBuilder UseDatabaseMigration(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<SimpleBookmakerDbContext>().Database.Migrate();
            }

            return app;
        }
    }
}
