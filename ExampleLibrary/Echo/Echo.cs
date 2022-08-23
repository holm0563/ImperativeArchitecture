using System.ComponentModel.DataAnnotations;

namespace ExampleLibrary.Echo;

/// <summary>
///     Immutable Plain old class type (POCO)
/// </summary>
public record Echo
{
    public const int MaxRepeat = 10;

    public string? Header { get; init; }
    public string Message { get; init; } = "";

    [Range(0, MaxRepeat)] public int Repeat { get; init; } = 1;
}

public interface ITest
{
}

public class Test : ITest
{
    public Test(ITest echo)
    {
    }

    public Test(ITest echo, ITest echo2)
    {
    }
}