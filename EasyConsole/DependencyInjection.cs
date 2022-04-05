namespace EasyConsole;

using Microsoft.Extensions.DependencyInjection;

internal static class DependencyInjection
{
    internal static IServiceCollection RegisterConsoleAppComponents(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        var typesFromAssemblies =
            assemblies.SelectMany(a => a.DefinedTypes.Where(x => x.ImplementsPage() && x.IsClass && !x.IsAbstract));
        foreach (var type in typesFromAssemblies)
        {
            services.Add(new ServiceDescriptor(typeof(Page), type, ServiceLifetime.Singleton));
        }

        services.AddSingleton<ConsoleProgram,ConsoleProgram>();

        return services;
    }

    private static bool ImplementsPage(this Type t)
    {
        while (true)
        {
            if (t.BaseType == null)
            {
                return false;
            }

            if (t.BaseType == typeof(Page))
            {
                return true;
            }

            t = t.BaseType;
        }
    }
}