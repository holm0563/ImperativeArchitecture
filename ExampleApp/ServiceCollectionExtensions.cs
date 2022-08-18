﻿using ExampleApp.Services.Polymorphism;
using Microsoft.Extensions.DependencyInjection;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Class", "ClassMissingInterface:ClassMissingInterface", Justification = "Not part of nuget library")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Class", "ClassMethodMissingInterface:ClassMethodMissingInterface", Justification = "Not part of nuget library")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Class", "DerivedClasses:DerivedClasses", Justification = "Not part of nuget library")]
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Gets the first of the specific service from the service collection by type.
    /// </summary>
    /// <typeparam name="T">The type of the service.</typeparam>
    /// <param name="services">The instance of the service collection.</param>
    /// <returns>The specific service found.</returns>
    public static T GetService<T>(this IServiceCollection services) where T : class
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        var matchingServices = services.GetServices<T>();
        
        return matchingServices?.FirstOrDefault() ?? throw new ArgumentNullException(nameof(T));
    }

    /// <summary>
    ///     Gets the collection of specific services from the service collection by type.
    /// </summary>
    /// <typeparam name="T">The type of services.</typeparam>
    /// <param name="services">The instance of the service collection.</param>
    /// <returns>The specific services found.</returns>
    public static IEnumerable<T>? GetServices<T>(this IServiceCollection services) where T : class
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        // Creates a service provider from the service collection and gets the collection of services based on type.
        var matchingServices = services.BuildServiceProvider()
            ?.GetServices<T>();

        return matchingServices;
    }
}