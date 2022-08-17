namespace ExampleAppSimple.Models;

/// <summary>
///     Plain old class type (POCO)
/// </summary>
public abstract class Shape
{
    public ConsoleColor Color = ConsoleColor.Gray;

    public char Image = 'O';

    /// <summary>
    ///     Draw the shape.
    /// </summary>
    public abstract void Draw();
}