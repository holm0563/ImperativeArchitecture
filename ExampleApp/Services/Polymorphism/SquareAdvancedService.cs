namespace ExampleApp.Services.Polymorphism;

public interface IAdvancedCharacter
{
    /// <summary>
    ///     Get the edge character for a shape.
    /// </summary>
    /// <remarks>
    ///     This is still a dependency injected service allowing for consuming services to easily alter the behavior.
    /// </remarks>
    char? EdgeShape(int x, int y, int maxX, int maxY);
}

public class SquareAdvancedService : IShape<Square>, IAdvancedCharacter
{
    public char? EdgeShape(int x, int y, int maxX, int maxY)
    {
        if (y == 0 && x == 0)
            return '┌';
        if (y == maxY && x == maxX)
            return '┘';
        if (y == maxY && x == 0)
            return '└';
        if (y == 0 && x == maxX)
            return '┐';
        if (y == 0 || y == maxY)
            return '─';
        if (x == 0 || x == maxX)
            return '│';

        return null;
    }

    // </inherit>
    public void Draw(Square shape)
    {
        Console.ForegroundColor = shape.Color;

        for (var y = 0; y < shape.Length; y++)
        {
            for (var x = 0; x < shape.Length; x++)
            {
                var edgeShape = EdgeShape(x, y, shape.Length - 1, shape.Length - 1);

                Console.Write(edgeShape ?? shape.Image);
            }

            Console.WriteLine();
        }

        Console.ResetColor();
    }
}

public class RectangleAdvancedService : IShape<Rectangle>
{
    // </inherit>
    public void Draw(Rectangle shape)
    {
        Console.ForegroundColor = shape.Color;

        for (var y = 0; y < shape.Height; y++)
        {
            for (var x = 0; x < shape.Width; x++)
            {
                var edgeShape = _advancedCharacter.EdgeShape(x, y, shape.Width - 1, shape.Height - 1);

                Console.Write(edgeShape ?? shape.Image);
            }

            Console.WriteLine();
        }

        Console.ResetColor();
    }

    #region Dependency Injection

    public RectangleAdvancedService(IAdvancedCharacter advancedCharacter)
    {
        _advancedCharacter = advancedCharacter ?? throw new ArgumentNullException(nameof(advancedCharacter));
    }

    private readonly IAdvancedCharacter _advancedCharacter;

    #endregion
}