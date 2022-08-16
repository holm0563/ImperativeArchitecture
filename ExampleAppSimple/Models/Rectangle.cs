namespace ExampleAppSimple.Models;

public class Rectangle : Shape
{
    public int Height;

    public int Width;

    public override void Draw()
    {
        Console.ForegroundColor = Color;

        for (var x = 0; x < Height; x++)
        {
            for (var y = 0; y < Width; y++)
                Console.Write(Image);

            Console.WriteLine();
        }

        Console.ResetColor();
    }
}