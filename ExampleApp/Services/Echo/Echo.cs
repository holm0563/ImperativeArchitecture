using System.ComponentModel.DataAnnotations;

namespace ExampleApp.Services.Echo;

/// <summary>
///     Plain old class type (POCO)
/// </summary>
/// <remarks>
///     Records will be the only type that allows fields.
///     Properties not be used to remove the temptation to add business logic that is not easily modified.
/// </remarks>
public record Echo
{
    public const int MaxRepeat = 10;

    public string? Header;
    public string Message = "";

    [Range(0, MaxRepeat)] public int Repeat = 1;


    // <summary>
    //     This is prohibited. Using Services and Dependency injection allow more flexibility.
    //     For example if this is a library, consumers of the library can use dependency injection with the decorator pattern to override or extend this easily.
    // </summary>
    //public override string ToString()
    //{
    //    return "Not Allowed";
    //}

    //Constructors would not be allowed preventing yet another area the business logic could live.
    //No methods are allowed  preventing yet another area the business logic could live.

    //No properties are allowed  preventing yet another area the business logic could live.
    //public string ToStringProperty => Message ?? "Not Allowed";
}