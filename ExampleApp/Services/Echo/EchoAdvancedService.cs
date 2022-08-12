using System.Text;

namespace ExampleApp.Services.Echo;

/// <summary>
///     Advanced example using decorator pattern to extend a service later.
/// </summary>
public class EchoServiceAdvancedService : IEchoService
{
    // </inherit>
    // Since interfaces are a requirement, documentation will not be required on public methods.
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
    /// <remarks>
    ///     Only arguments that will be allowed are interfaces that will be used for dependency injection.
    /// </remarks>
    /// <param name="originalService">The original service to decorate.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public EchoServiceAdvancedService(IEchoService originalService)
    {
        _originalService = originalService ?? throw new ArgumentNullException(nameof(originalService));
    }

    #endregion

    // No public properties or fields allowed
}