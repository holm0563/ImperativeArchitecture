namespace ExampleApp.Services.Echo;

/// <summary>
///     Plain old class type (POCO)
/// </summary>
public record Echo
{
    public string? Header;
    public string Message;
    public int Repeat = 1;


    /// <summary>
    ///     This is prohibited. Using Services and Dependency injection allow more flexibility.
    ///     For example if this is a library, consumers of the library can use dependency injection with the decorator pattern to override or extend this easily.
    /// </summary>
    /// <returns></returns>
    //public override string ToString()
    //{
    //    return "Not Allowed";
    //}

    //Constructors would not be allowed preventing yet another area the business logic could live.
    //No methods are allowed  preventing yet another area the business logic could live.

    //No properties are allowed  preventing yet another area the business logic could live.
    //public string ToStringProperty => "Not Allowed";
}