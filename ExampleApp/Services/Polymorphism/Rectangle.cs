namespace ExampleApp.Services.Polymorphism;

/// <summary>
///     Plain old class type (POCO)
/// </summary>
public record Rectangle : Shape
{
    public int Height;

    public int Width;
}