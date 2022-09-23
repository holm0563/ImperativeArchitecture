using ExampleAppSimple.Shared;

namespace ExampleAppSimple.Models;

public class RectangleAdvanced : Rectangle
{
    public override void Draw()
    {
        Console.ForegroundColor = Color;

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var edgeShape = AdvancedCharacterUtility.EdgeShape(x, y, Width - 1, Height - 1);

                Console.Write(edgeShape ?? Image);
            }

            Console.WriteLine();
        }

        Console.ResetColor();
    }
}