namespace ExampleApp.Services.Polymorphism;

/// <summary>
///     A shape service.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IShape<Shape>
{
    /// <summary>
    ///     Draw the shape.
    /// </summary>
    /// <param name="shape"></param>
    void Draw(Shape shape);
}

public class SquareService : IShape<Square>
{
    // </inherit>
    public void Draw(Square shape)
    {
        Console.ForegroundColor = shape.Color;

        for (var x = 0; x < shape.Length; x++)
        {
            for (var y = 0; y < shape.Length; y++)
                Console.Write(shape.Image);

            Console.WriteLine();
        }

        Console.ResetColor();
    }
}

public class RectangleService : IShape<Rectangle>
{
    // </inherit>
    public void Draw(Rectangle shape)
    {
        Console.ForegroundColor = shape.Color;

        for (var x = 0; x < shape.Height; x++)
        {
            for (var y = 0; y < shape.Width; y++)
                Console.Write(shape.Image);

            Console.WriteLine();
        }

        Console.ResetColor();
    }
}