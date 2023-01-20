using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SiteScrapPolling.Common.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection FromAssembly<TType>(this IServiceCollection services,
                                                         Action<Type> registration)
    {
        foreach (var type in AppDomain.CurrentDomain.GetAssemblies().Where(a => a.IsDynamic is false)
                                      .SelectMany(a => a.GetTypes())
                                      .Where(type => typeof(TType).IsAssignableFrom(type) &&
                                                     type is {IsInterface: false, IsAbstract: false}))
        {
            registration(type);
        }

        return services;
    }
}
