using ExampleApp.Services.Echo;
using ExampleApp.Services.Polymorphism;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ExampleApp;

public class Program
{
    public static void Main(string[] args)
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

        // Pass in any argument to see the change in behavior.
        // By default the launchSettings.json is configured to provide an argument.
        if (args.Length > 0)
            hostBuilder.ConfigureServices(ConfigureModifiedServices);

        var host = hostBuilder.Build();

        using var serviceScope = host.Services.CreateScope();
        var services = serviceScope.ServiceProvider;

        var myService = services.GetRequiredService<Application>();
        myService.Run();
    }


    public static void ConfigureDefaultServices(HostBuilderContext context, IServiceCollection services)
    {
        services.AddTransient<Application>();
        services.AddSingleton<IEchoService, EchoServiceService>();
        services.AddSingleton<IAdvancedCharacter, SquareAdvancedService>();
        services.AddSingleton<IShape<Square>, SquareService>();
        services.AddSingleton<IShape<Rectangle>, RectangleService>();
        services.AddSingleton<IShapeServiceFactory, ShapeServiceFactory>();
    }

    /// <summary>
    ///     Example of being open to extensions, but closed for modifications (O in Solid principles)
    ///     This is very nice especially if we are using code as a nuget library and extending its functionality in another
    ///     library.
    /// </summary>
    public static void ConfigureModifiedServices(HostBuilderContext context, IServiceCollection services)
    {
        services.Replace(new ServiceDescriptor(typeof(IEchoService),
            new EchoServiceAdvancedService(
                services.GetService<IEchoService>())
        ));

        services.Replace(new ServiceDescriptor(typeof(IShape<Square>),
            new SquareAdvancedService()
        ));

        services.Replace(new ServiceDescriptor(typeof(IShape<Rectangle>),
            new RectangleAdvancedService(
                services.GetService<IAdvancedCharacter>()
            )
        ));
    }
}