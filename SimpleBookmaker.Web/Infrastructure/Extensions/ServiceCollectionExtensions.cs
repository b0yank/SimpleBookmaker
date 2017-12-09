namespace SimpleBookmaker.Web.Infrastructure.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using SimpleBookmaker.Services.Contracts;
    using System.Linq;
    using System.Reflection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseServices(this IServiceCollection services)
        {
            Assembly.GetAssembly(typeof(IService))
                    .GetTypes()
                    .Where(ty => typeof(IService).IsAssignableFrom(ty) && ty.IsClass)
                    .ToList()
                    .ForEach(implementation =>
                    {
                        var contract = implementation.GetInterfaces().First();

                        services.AddTransient(contract, implementation);
                    });

            return services;
        }
    }
}
