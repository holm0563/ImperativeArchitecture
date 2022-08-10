namespace ExampleApp.Services.Polymorphism;

/// <summary>
///     Plain old class type (POCO)
/// </summary>
public record Shape
{
    public ConsoleColor Color = ConsoleColor.Gray;

    public char Image = 'O';

    // With the traditional approach it makes it hard to extend later. 
    // you have to anticipate this class will be inherited so that you can define virtual or abstract methods
    //public virtual void Draw(){} // Not allowed
}