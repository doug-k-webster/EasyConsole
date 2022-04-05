namespace EasyConsole;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class ConsoleProgramHost
{
    public async Task Execute<TStartPage>(string title, string[] args, Action<IServiceCollection>? serviceRegistrations = null)
        where TStartPage : Page
    {
        using var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
            {
                services.RegisterConsoleAppComponents();
                serviceRegistrations?.Invoke(services);
            })
            .Build();

        using var serviceScope = host.Services.CreateScope();
        var provider = serviceScope.ServiceProvider;

        var pages = provider.GetServices<Page>();
        var program = provider.GetRequiredService<ConsoleProgram>();
        await program.Run<TStartPage>(pages, title, true, CancellationToken.None);
        await host.RunAsync();
    }
}