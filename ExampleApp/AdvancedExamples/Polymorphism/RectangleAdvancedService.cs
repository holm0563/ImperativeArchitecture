namespace ExampleLibrary.Polymorphism;

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
        _advancedCharacter = advancedCharacter;
    }

    private readonly IAdvancedCharacter _advancedCharacter;

    #endregion
}