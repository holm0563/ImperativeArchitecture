﻿using System;

namespace ExampleLibrary.Polymorphism;

/// <summary>
///     A common shape service.
/// </summary>
public interface IShape<in T> where T : Shape
{
    /// <summary>
    ///     Draw the shape.
    /// </summary>
    /// <param name="shape"></param>
    void Draw(T shape);
}

public class SquareService : IShape<Square>
{
    // </inherit>
    public void Draw(Square shape)
    {
        Console.ForegroundColor = shape.Color;

        for (var x = 0; x < shape.Length; x++)
        {
            for (var y = 0; y < shape.Length; y++)
                Console.Write(shape.Image);

            Console.WriteLine();
        }

        Console.ResetColor();
    }
}

public class RectangleService : IShape<Rectangle>
{
    // </inherit>
    public void Draw(Rectangle shape)
    {
        Console.ForegroundColor = shape.Color;

        for (var x = 0; x < shape.Height; x++)
        {
            for (var y = 0; y < shape.Width; y++)
                Console.Write(shape.Image);

            Console.WriteLine();
        }

        Console.ResetColor();
    }
}