namespace ExampleAppSimple.Models;

/// <summary>
///     Plain old class type (POCO)
/// </summary>
/// <remarks>
///     Todo: Throw `Abstract classes are not allowed. Try using a shared interface`
/// </remarks>
public abstract class Shape
{
    public ConsoleColor Color = ConsoleColor.Gray;

    public char Image = 'O';

    /// <summary>
    ///     Draw the shape.
    /// </summary>
    /// <remarks>
    ///     Todo: Throw `Abstract methods are not allowed on a class. Try using a shared interface`
    /// </remarks>
    public abstract void Draw();
}