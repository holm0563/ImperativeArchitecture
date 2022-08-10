namespace ExampleApp.Services.Polymorphism;

/// <summary>
///     A shape service.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IShapeServiceFactory
{
    /// <summary>
    ///     Draw the shape.
    /// </summary>
    /// <param name="shape"></param>
    void Draw(Shape shape);
}

public class ShapeServiceFactory : IShapeServiceFactory
{
    // </inherit>
    public void Draw(Shape shape)
    {
        if (shape is Rectangle rectangle)
        {
            _rectangle.Draw(rectangle);
            return;
        }

        if (shape is Square square)
            _square.Draw(square);
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