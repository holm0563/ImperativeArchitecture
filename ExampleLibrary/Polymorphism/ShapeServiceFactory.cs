using System;

namespace ExampleLibrary.Polymorphism;

/// <summary>
///     A service factory used to instantiate the right service based on the POCO type.
/// </summary>
public interface IShapeServiceFactory
{
    /// <summary>
    ///     Draw the shape.
    /// </summary>
    void Draw(Shape shape);
}

public class ShapeServiceFactory : IShapeServiceFactory
{
    // </inherit>
    public void Draw(Shape shape)
    {
        switch (shape)
        {
            case Rectangle rectangle:
                _rectangle.Draw(rectangle);
                return;
            case Square square:
                _square.Draw(square);
                break;
        }
    }

    #region Dependency Injection

    private readonly IShape<Square> _square;
    private readonly IShape<Rectangle> _rectangle;

    public ShapeServiceFactory(IShape<Square> square, IShape<Rectangle> rectangle)
    {
        //It could be simpler code was to use 
        _square = square ?? throw new ArgumentNullException(nameof(square));
        _rectangle = rectangle ?? throw new ArgumentNullException(nameof(rectangle));
    }

    #endregion
}