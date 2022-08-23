using ExampleLibrary.Echo;
using ExampleLibrary.Polymorphism;
using Microsoft.Extensions.DependencyInjection;

namespace ExampleLibrary;

public static class Services
{
    public static void AddExampleLibrary(this IServiceCollection services)
    {
        services.AddSingleton<IEchoService, EchoServiceService>();
        services.AddSingleton<IShape<Square>, SquareService>();
        services.AddSingleton<IShape<Rectangle>, RectangleService>();
        services.AddSingleton<IShapeServiceFactory, ShapeServiceFactory>();
    }
}