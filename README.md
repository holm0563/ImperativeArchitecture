# Overview
When it comes to architecture C# is bloated. Its a multiuse tool that can be used for so many different use cases. This goes against the KISS philosophy (keep it simple stupid). When we are using C# for specific use cases like microservices, libraries, etc we want to use the "good parts" and be consistent. Teams struggle because
there are so many patterns available. It makes it difficult to reuse code, train junior programmers, and perform code reviews.

How many different ways can you store business logic? In properties, constructors, static methods, methods, etc.
How many different ways can you define models? In structs, classes, records, etc.
How should we define classes? When should we use protected, private, override, virtual and abstract classes?

This project will define an opinionated way to do these things using service oriented architecture. This provides consistency and flexibility as a project grows.

Other patterns are optional but since the goal is to define good and consistant architecture other patterns will be recommended as well.

Essentially we are defining a subset of C#. This is much easier than a new programming language. We get all of the benefits of the C# system and the consistency and faster time to market benefits of having a simpler and focused architecture for our task.

## When not to use a Service Oriented Architecture
* Small applications that will not be built into nuget packages
* Dependency injection is not needed for unit tests

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

# Futher Reading
Object Oriented Patterns dont work well in a microservices stateless world.
https://towardsdatascience.com/object-oriented-programming-is-dead-wait-really-db1f1f05cc44

Singleton AntiPattern
https://www.michaelsafyan.com/tech/design/patterns/singleton

