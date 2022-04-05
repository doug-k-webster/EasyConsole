using Microsoft.Extensions.DependencyInjection;

namespace Demo;

using Demo.Pages;

using EasyConsole;

internal class Runner
{
    private static async Task Main(string[] args)
    {
        var host = new ConsoleProgramHost();
        
        await host.Execute<MainPage>("Demo App", args,
            services =>
            {
                services.AddSingleton<ISampleService,SampleService>();
            });
    }
}