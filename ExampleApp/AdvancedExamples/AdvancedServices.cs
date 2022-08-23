using ExampleApp;
using ExampleApp.AdvancedExamples.Echo;
using ExampleLibrary.Echo;
using ExampleLibrary.Polymorphism;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ExampleLibrary;

public static class AdvancedServices
{
    public static void AddAdvancedExamples(this IServiceCollection services)
    {
        services.AddSingleton<IAdvancedCharacter, SquareAdvancedService>();
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