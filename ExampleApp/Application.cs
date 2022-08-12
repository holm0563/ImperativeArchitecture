using ExampleApp.Services.Echo;
using ExampleApp.Services.Polymorphism;

namespace ExampleApp;

public class Application
{
    internal void Run()
    {
        EchoDemo();
        PolyDemo();
    }

    private void EchoDemo()
    {
        var echo3 = new Echo { Header = "Echo", Message = "Testing", Repeat = 3 };
        var echo = echo3 with { Repeat = 1 };

        Console.WriteLine(_echoService.ToString(echo3));
        Console.ReadKey();
        Console.WriteLine(_echoService.ToString(echo));
        Console.ReadKey();
    }

    private void PolyDemo()
    {
        var square = new Square { Length = 3, Image = 'X', Color = ConsoleColor.DarkCyan };

        var rectangle = new Rectangle { Width = 6, Height = 3, Color = ConsoleColor.DarkGreen };

        IList<Shape> demos = new List<Shape> { square, rectangle };

        foreach (var shape in demos)
        {
            Console.WriteLine($"Drawing {shape.GetType()}");
            _shapeServiceFactory?.Draw(shape);
            Console.ReadKey();
        }
    }

    #region Dependency Injection

    private readonly IShapeServiceFactory _shapeServiceFactory;
    private readonly IEchoService _echoService;

    public Application(IShapeServiceFactory shapeServiceFactory, IEchoService echoService)
    {
        _shapeServiceFactory = shapeServiceFactory ?? throw new ArgumentNullException(nameof(shapeServiceFactory));
        _echoService = echoService ?? throw new ArgumentNullException(nameof(echoService));
    }

    #endregion
}