# Overview
When it comes to architecture C# is bloated. Its a multiuse tool that can be used for so many different use cases.
When trying to use it for specific use cases like microservices, libraries, etc we want to use the "good parts". Teams struggle because
there are so many patterns available. It makes it difficult to reuse code, train junior programmers, and perform code reviews.

How many different ways can you store business logic? In properties, constructors, static methods, methods, etc.

How many different ways can you define models? In structs, classes, records, etc.

This project will define an opinionated way to do these things using service oriented architecture. This provides consistency and flexibility as a project grows.

Other patterns are optional but since the goal is to define good and consistant architecture other patterns will be recommended as well.

## When not to use a Service Oriented Architecture
* Small applications that will not be built into nuget packages
* Dependency injection is not needed for unit tests

## When to use a SOA
* When you want to be open for extensibility
* Needs to add robust logging (Hard if code has a lot of static functions)
* When following Microsofts AddServices patterns
* Helps to avoid hiding state from Object oriented patterns

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

