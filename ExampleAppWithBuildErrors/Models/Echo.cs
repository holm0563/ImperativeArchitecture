using System.Text;

namespace ExampleAppWithBuildErrors.Models;

/// <summary>
///     Plain old class type (POCO)
/// </summary>
/// <remarks>
///     Todo: throw error `Identified a class with field values. Use records to define POCO types. These are easier to
///     clone`
/// </remarks>
public class Echo
{
    /// <remarks>
    ///     Todo: throw error `Private keywords is prohibited in POCO types`
    /// </remarks>
    private int? _repeat;

    public string? Header;

    /// <summary>
    ///     Todo: throw error `Properties not allowed in POCO types`
    /// </summary>
    /// <remarks>
    ///     The virtual key word identifies that this could have business logic associated
    ///     with this property when this class is extended.
    /// </remarks>
    public virtual string Message { get; set; } = "";

    /// <summary>
    ///     Todo: throw error `Properties not allowed in POCO types`
    /// </summary>
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