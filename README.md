# Objective
Define an opinionated architecture standard for C# that focuses its scope based on modern patterns like SOA (service oriented architecture), DI (dependency injection), Statelessness. This improves productivity, reusability, and standardization for software development teams.

# Overview
When it comes to architecture C# is too robust. Its a multiuse tool that can be used for so many different use cases. This goes against the KISS philosophy (keep it simple stupid). When we are using C# for specific use cases like microservices, libraries, etc we want to use the "good parts" and be consistent. Teams struggle because
there are so many patterns available. It makes it difficult to reuse code, train junior programmers, and perform code reviews.

How many different ways can you store business logic? Is it best to use: properties, constructors, static methods, methods?
How many different ways can you define models? Is it best to use: structs, classes, records?
How should we define classes? Is it best to use: protected, private, override, virtual and abstract classes?

This project will define an opinionated way to do these things using service oriented architecture. This provides consistency and flexibility as a project grows.

Since the goal is to define good and consistant architecture other patterns will be recommended as well ranging from devops to recommended tooling.

Essentially we are defining a subset of C#. This is much easier than a new programming language. We get all of the benefits of the C# system and the consistency and faster time to market benefits of having a simpler and focused architecture for our task.

## When not to use a Service Oriented Architecture
* Small applications that will not be built into nuget packages or consumed by other teams
* Dependency injection is not needed (usually needed for unit tests)

## When to use a SOA
* When you want to be open for extensibility
* Needs to add robust logging (Hard to add later with things like nested static functions)
* When following Microsofts AddServices patterns
* Helps to avoid hiden state common in object oriented patterns

# Goals
* Define a subset of recommended C# language features to help teams consistently achieve SOA patterns.
  *  Document what patterns are allowed and the alternatives that are blocked
  *  Document the "whys" of the architectural decisions
* Build tooling to automate the recommended patterns
  *  Should be automatable and used in Devops
  *  Should provide instant feedback and build errors
  *  Ideally some of the patterns can be code generated
  *  Ideally provide refactoring tool tips to automatically convert code

# Futher Reading
Object Oriented Patterns dont work well in a microservices stateless world.
https://towardsdatascience.com/object-oriented-programming-is-dead-wait-really-db1f1f05cc44

Singleton AntiPattern
https://www.michaelsafyan.com/tech/design/patterns/singleton

