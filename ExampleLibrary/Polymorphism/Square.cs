namespace ExampleLibrary.Polymorphism;

/// <summary>
///     Plain old class type (POCO)
/// </summary>
public record Square : Shape
{
    /// <summary>
    ///     The size in all directions.
    /// </summary>
    public int Length;
}