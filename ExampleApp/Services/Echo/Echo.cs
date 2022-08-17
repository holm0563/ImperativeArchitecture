using System.ComponentModel.DataAnnotations;

namespace ExampleApp.Services.Echo;

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