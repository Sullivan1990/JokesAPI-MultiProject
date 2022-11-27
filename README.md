# JokesAPI Demonstration Project

- .NET 6
- ASP.NET Core WebAPI template
- CRUD methods around single question and answer jokes
- Swagger enabled in Development

### A project used to demonstrate a range of different tools available when developing REST API's in C#.

Features will be added to the list below as they are added to the repository.

#### Integration Testing
Tests added using Nunit, utilising the EFCore InMemoryDatabase to spin up tests - testing the repository service responsible for interacting with a database through EF Core

#### Expressions
Added Methods and endpoints that allow for passing in expressions to LINQ statements for interacting with a database using LINQ to Entity
Added detail around the working of these methods, including querying a property based on the string name of the property
