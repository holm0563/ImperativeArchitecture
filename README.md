# Objective
Define an opinionated architecture standard for C# libraries that creates modern and consistent architectural patterns. Enforce these patterns using C# analyzers.

# Overview
When it comes to architecture C# is too robust. Its a multiuse tool that can be used for so many different use cases. This goes against the KISS philosophy (keep it simple stupid). When we are using C# for specific use cases like microservices, libraries, etc we want to use the "good parts" and be consistent. Teams struggle because
there are so many patterns available. It makes it difficult to reuse code, train junior programmers, and perform code reviews.

How many different ways can you store business logic? Is it best to use: properties, constructors, static methods, methods?
How many different ways can you define models? Is it best to use: structs, classes, records?
How should we define classes? Is it best to use: protected, private, override, virtual and abstract classes?

This project will define an opinionated way to do these things using service oriented architecture. This provides consistency and flexibility as a project grows.

Ideally other patterns would be defined for other use cases.

Essentially we are defining a subset of C#. This is much easier than a new programming language. We get all of the benefits of the C# system and the consistency and faster time to market benefits of having a simpler and focused architecture for our task.

## When not to use a Service Oriented Architecture
* Small applications that will not be built into nuget packages or consumed by other teams
* Dependency injection is not needed (usually needed for unit tests)

## When to use a SOA
* When you want to be open for extensibility
* When you are building a library to be deployed to Nuget
* Project needs to add robust logging (Hard to add later with things like nested static functions)
* When following Microsofts AddServices patterns
* Helps to avoid hiden state common in object oriented patterns

## Analyzers
Analysis done by the extension to inforce an opinionated services oriented architecture in code.

### All Symbols
* NotPublic: `Warning` - Symbol '{0}' is not public. It is recommended to have most code public. Situations where you do not want to be open to extensibility should be very rare or live in another project outside your shared library.

### Records
Records are meant to signify data objects. These should be free of any business logic. The record type has a lot of advantages over class objects. It has built in shallow cloning and can be immutable.
https://www.c-sharpcorner.com/article/c-sharp-9-0-introduction-to-record-types/

* RecordWithMethod: `Error` - Record '{0}' contains a method named '{1}. Methods are restricted inside of records. To define business logic use classes. This helps keep business logic and data separate.'
* NotPublicInRecord: `Error` - Record '{0}' contains a non public member named '{1}'. Records are restricted to be plain old class objects. These can not have any logic inside of them. This helps keep business logic and data separate.
* InterfaceInRecord: `Error` - Record '{0}' contains a constructor with an interface parameter named '{1}'. Records are restricted to be plain old class objects. These do not have logic or dependency injection. This helps keep business logic and data separate.

### Classes
Classes are meant to define business logic. This should be completely free of state and use interfaces to provide extensibility.

* ClassWithData: `Error` - Class '{0}' contains a field or property named '{1}'. Non private fields and properties are restricted inside of classes. To define models use records. This helps keep business logic and data separate.
* ClassMethodMissingInterface: `Error` - Class '{0}' method '{1}' is missing a corresponding interface. All classes methods must use interfaces so that they are open to extension by other libraries.
* ConstructorNotDi: `Error` - Class '{0}' contains a constructor with a non interface parameter named '{1}'. Classes should be dependency injected. This gets overly complex if they have multiple constructors or constructors with other values.
* DerivedClasses: `Error` - Class '{0}' contains virtual, static, or override keywords. Instead of using derived classes use services. See the decorator pattern for more help. Services can be injected at runtime where derived and static classes can not providing better flexibility.

# Futher Reading
Object Oriented Patterns dont work well in a microservices stateless world.
https://towardsdatascience.com/object-oriented-programming-is-dead-wait-really-db1f1f05cc44

Singleton AntiPattern
https://www.michaelsafyan.com/tech/design/patterns/singleton

