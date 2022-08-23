using ExampleLibrary;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ExampleApp;

internal class Program
{
    internal static void Main(string[] args)
    {
        //setup our DI
        var hostBuilder = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((_, builder) => { builder.SetBasePath(Directory.GetCurrentDirectory()); })
            .ConfigureServices(ConfigureDefaultServices)
            .ConfigureLogging((_, logging) =>
            {
                logging.AddSimpleConsole(o => o.IncludeScopes = true);
                logging.SetMinimumLevel(LogLevel.Trace);
            });

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("Show advanced demo? Y/N ");
        var advanced = Console.ReadKey();
        Console.ResetColor();
        Console.WriteLine();

        if (advanced.Key != ConsoleKey.N)
        {
            Console.WriteLine("Showing advanced demo:");
            hostBuilder.ConfigureServices(ConfigureModifiedServices);
        }
        else
        {
            Console.WriteLine("Showing basic demo:");
        }

        var host = hostBuilder.Build();

        using var serviceScope = host.Services.CreateScope();
        var services = serviceScope.ServiceProvider;

        var myService = services.GetRequiredService<Application>();
        myService.Run();
    }


    internal static void ConfigureDefaultServices(HostBuilderContext context, IServiceCollection services)
    {
        services.AddTransient<Application>();

        services.AddExampleLibrary();
    }

    /// <summary>
    ///     Example of being open to extensions, but closed for modifications (O in Solid principles)
    ///     This is very nice especially if we are using code as a nuget library and extending its functionality in another
    ///     library.
    /// </summary>
    internal static void ConfigureModifiedServices(HostBuilderContext context, IServiceCollection services)
    {
        services.AddAdvancedExamples();
    }
}