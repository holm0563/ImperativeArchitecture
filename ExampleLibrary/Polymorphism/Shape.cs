using System;

namespace ExampleLibrary.Polymorphism;

/// <summary>
///     Plain old class type (POCO)
/// </summary>
public record Shape
{
    public ConsoleColor Color = ConsoleColor.Gray;

    public char Image = 'O';
}