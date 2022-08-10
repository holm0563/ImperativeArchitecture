using System.Text;

namespace ExampleApp.Services.Echo;

/// <summary>
///     Advanced example using decorator pattern.
/// </summary>
public class EchoAdvancedService : IEchoService
{
    // </inherit>
    public string ToString(Echo echo)
    {
        var advancedEcho = echo with { Repeat = 1 };
        var builder = new StringBuilder();

        builder.Append(_originalService.ToString(advancedEcho));
        builder.AppendLine($"Repeat {echo.Repeat} times.");

        return builder.ToString();
    }

    #region Dependency Injection

    private readonly IEchoService _originalService;

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">The only parameters allowed are for dependency injection</param>
    /// <param name="originalService">The original service to decorate.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public EchoAdvancedService(IEchoService originalService)
    {
        _originalService = originalService ?? throw new ArgumentNullException(nameof(originalService));
    }

    #endregion

    // No public properties or fields allowed
}