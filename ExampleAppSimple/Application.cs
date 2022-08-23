using ExampleAppSimple.Models;

namespace ExampleAppSimple;

public static class Application
{
    internal static void Run()
    {
        EchoDemo();
        PolyDemo();
    }

    private static void EchoDemo()
    {
        var echo3 = new Echo { Header = "Echo", Message = "Testing", Repeat = 3 };
        var echo1 = new Echo
        {
            Header = echo3.Header,
            Message = echo3.Message,
            Repeat = 1
        };

        Console.WriteLine(echo3.ToString());
        Console.ReadKey();
        Console.WriteLine(echo1.ToString());
        Console.ReadKey();

        // Middle Developer Exercise:
        // We find out we have an unpredictable error we think is happening in the ToString method of Echo.
        // How should we introduce logging every time ToString is called? Show example code.

        // Now I want to change the behavior of ToString. Based on configuration I want to always print out the
        // message only once with another message saying how many times it should be repeated.
        // Show example code.
        // How could these be easier if our application was already using this method 100s of time within our application?
        // Would your solutions work if this was a Nuget package built by another team in your company?
    }

    private static void PolyDemo()
    {
        var square = new Square { Length = 3, Image = 'X', Color = ConsoleColor.DarkCyan };
        var rectangle = new Rectangle { Width = 6, Height = 3, Color = ConsoleColor.DarkGreen };
        var advancedRectangle = new RectangleAdvanced { Width = 6, Height = 3, Color = ConsoleColor.DarkGreen };
        IList<Shape> demos = new List<Shape> { square, rectangle, advancedRectangle };

        foreach (var shape in demos)
        {
            Console.WriteLine($"Drawing {shape.GetType()}");
            shape.Draw();
            Console.ReadKey();
        }

        // Middle Developer Exercise:
        // We find out we we want to now optionally draw to a file instead of the console window.
        // Show example code.
        // What patterns what you suggest to make this easier?
        // Would your solutions work if this was a Nuget package built by another team in your company?
    }
}