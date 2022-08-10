using ExampleApp.Services.Echo;
using ExampleApp.Services.Polymorphism;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class Program
{
    public static void Main(string[] args)
    {
        //setup our DI
        var hostBuilder = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, builder) => { builder.SetBasePath(Directory.GetCurrentDirectory()); })
            .ConfigureServices(ConfigureDefaultServices)
            .ConfigureLogging((_, logging) =>
            {
                logging.AddDebug();
                logging.AddConsole();
            });

        // Pass in any argument or comment out this if statement to see the change in behavior.
        if (args.Length > 0)
            hostBuilder.ConfigureServices(ConfigureModifiedServices);

        var host = hostBuilder.Build();

        var echoService = host.Services.GetService<IEchoService>();

        EchoDemo(echoService);

        var shapeServiceFactory = host.Services.GetService<IShapeServiceFactory>();

        PolyDemo(shapeServiceFactory);
    }

    public static void EchoDemo(IEchoService? echoService)
    {
        var echo3 = new Echo
        {
            Header = "Echo",
            Message = "Testing",
            Repeat = 3
        };

        var echo = echo3 with { Repeat = 1 };

        Console.WriteLine(echoService?.ToString(echo3));
        Console.ReadKey();
        Console.WriteLine(echoService?.ToString(echo));
        Console.ReadKey();
    }

    public static void PolyDemo(IShapeServiceFactory? shapeServiceFactory)
    {
        var square = new Square
        {
            Length = 3,

            Image = 'X',
            Color = ConsoleColor.DarkCyan
        };

        var rectangle = new Rectangle
        {
            Width = 6,
            Height = 3,

            Color = ConsoleColor.DarkGreen
        };

        IList<Shape> demos = new List<Shape> { square, rectangle };

        foreach (var shape in demos)
        {
            Console.WriteLine($"Drawing {shape.GetType()}");
            shapeServiceFactory?.Draw(shape);
            Console.ReadKey();
        }
    }

    public static void ConfigureDefaultServices(HostBuilderContext context, IServiceCollection services)
    {
        services.AddSingleton<IEchoService, EchoService>();
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
    /// <param name="context"></param>
    /// <param name="services"></param>
    public static void ConfigureModifiedServices(HostBuilderContext context, IServiceCollection services)
    {
        services.Replace(new ServiceDescriptor(typeof(IEchoService),
            new EchoAdvancedService(
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