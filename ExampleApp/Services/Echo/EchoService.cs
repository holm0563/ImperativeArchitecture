using System.Text;
using Microsoft.Extensions.Logging;

namespace ExampleApp.Services.Echo;

/// <summary>
///     All Services must have interfaces with all public methods defined.
/// </summary>
public interface IEchoService
{
    /// <summary>
    ///     Gets the full string to echo including the repeated content.
    /// </summary>
    /// <param name="echo">Echo POCO.</param>
    string ToString(Echo echo);
}

/// <summary>
///     Echo Service
/// </summary>
public class EchoServiceService : IEchoService
{
    // </inherit>
    public string ToString(Echo echo)
    {
        // Other patterns such as overriding tostring on the class itself can lead to expensive refactors later.
        // For example if you need to add logging support you can't without dependency injection.
        _logger.Log(LogLevel.Trace, 0, "Method Called ToString");

        var builder = new StringBuilder(echo.Header);

        if (!string.IsNullOrWhiteSpace(echo.Header))
            builder.AppendLine();

        var repeat = echo.Repeat > Echo.MaxRepeat ? Echo.MaxRepeat : echo.Repeat;
        for (var x = 0; x < repeat; x++)
            builder.AppendLine(echo.Message);

        return builder.ToString();
    }

    #region Dependency Injection

    private readonly ILogger<EchoServiceService> _logger;

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">The only parameters allowed are for dependency injection</param>
    /// <exception cref="ArgumentNullException"></exception>
    public EchoServiceService(ILogger<EchoServiceService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    #endregion

    // No public properties or fields allowed
}