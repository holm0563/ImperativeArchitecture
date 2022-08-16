using System.Text;

namespace ExampleAppSimple.Models;

public class Echo
{
    public virtual string Message { get; set; } = "";
}

/// <summary>
///     Plain old class type (POCO)
/// </summary>
/// todo: check for interface (for all public methods)
public class Echo2
{
    private int? _repeat;

    public string? Header;

    /// <remarks>
    ///     The virtual key word identifies that this could have business logic associated
    ///     with this property when this class is extended.
    /// </remarks>
    public virtual string Message { get; set; } = "";

    /// <remarks>
    ///     This essentially adds business logic which for simplicity and extendability we will only define in services.
    /// </remarks>
    public int Repeat
    {
        get => _repeat ?? 1;
        set
        {
            if (value < 0)
                _repeat = null;
            else if (value > 10)
                _repeat = 10;
            else
                _repeat = value;
        }
    }

    public override string ToString()
    {
        var builder = new StringBuilder(Header);

        if (!string.IsNullOrWhiteSpace(Header))
            builder.AppendLine();

        for (var x = 0; x < Repeat; x++)
            builder.AppendLine(Message);

        return builder.ToString();
    }
}