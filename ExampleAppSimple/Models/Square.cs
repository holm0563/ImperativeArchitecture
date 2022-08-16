namespace ExampleAppSimple.Models;

public class Square : Shape
{
    /// <summary>
    ///     The size in all directions.
    /// </summary>
    public int Length;

    public override void Draw()
    {
        Console.ForegroundColor = Color;

        for (var x = 0; x < Length; x++)
        {
            for (var y = 0; y < Length; y++)
                Console.Write(Image);

            Console.WriteLine();
        }

        Console.ResetColor();
    }
}