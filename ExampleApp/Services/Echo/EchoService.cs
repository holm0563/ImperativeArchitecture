using System.Text;
using Microsoft.Extensions.Logging;

namespace ExampleApp.Services.Echo;

/// <summary>
///     All Services must have interfaces with all public methods defined.
/// </summary>
public interface IEcho
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
public class EchoService : IEcho
{
    // </inherit>
    public string ToString(Echo echo)
    {
        // Other patterns suchs as overriding tostring on the class itself can lead to expensive refactors later.
        // For example if you need to add logging support you can't without dependency injection.
        _logger.Log(LogLevel.Debug, 0, "Method Called ToString");

        var builder = new StringBuilder(echo.Header);

        if (!string.IsNullOrWhiteSpace(echo.Header))
            builder.AppendLine();

        for (var x = 0; x < echo.Repeat; x++)
            builder.AppendLine(echo.Message);

        return builder.ToString();
    }

    #region Dependency Injection

    private readonly ILogger<EchoService> _logger;

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">The only parameters allowed are for dependency injection</param>
    /// <exception cref="ArgumentNullException"></exception>
    public EchoService(ILogger<EchoService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    #endregion

    // No public properties or fields allowed
}