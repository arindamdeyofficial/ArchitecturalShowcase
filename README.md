# ArchitecturalShowcase
<h1>Problem Statement Taken</h1>
One simple grid in UI capable of CRUD such as Add,Edit,Delete,Update

Summary:
Very Simple example of WebApi implimenting Cloud Native Architecture having capability to handle 10000 concurrent user with degree of parallalism having capability of controlling Task and Thread.

Highlights:
With abstract Generics implimentation of BaseController we can have typed Factory e.g. ILogger
Can handle 10000 concurrent user
Ability to control over Task and Thread with cancellation token implimentation
Through nTier layer with ASP .Net Core dependency Injection implimenting multiple resolver
Cloud Native design with API plugability support
Used mini ORM like dapper
Used EntityFrameworkCore in asynchronus way with granular division of transaction [Challenge: DbContext doesn't support async and EF is not good for transaction specific]


Future Scope:

Impliment Distributed Saga to fit in big Microservices mesh
Devide repository to basic unit of work
Add Serilog implimentation in Sync
Token Authetication
SSO capability with Claims implimentation
MongoDB or any Document DB integration
Integrate Azure Cosmos



Design Pattern Used:
 Saga,AggregatorRoot[AR], CQRS[Command Query Responsibility Segregation], Repository, DI[Dependency Injection]

ORM tools:
EntityFramework Core[Code first] and Dapper

Other Tools:
Swagger, Automapper
